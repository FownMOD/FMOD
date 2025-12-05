using Hints;
using Mirror;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.CustHint
{
    public class PlayerHintController:NetworkBehaviour
    {
        public static PlayerHintController LocalPlayer { get; private set; }

        private static readonly Dictionary<int, PlayerHintController> _controllers = new Dictionary<int, PlayerHintController>();

        private readonly Dictionary<int, ActiveHintInfo> _activeHints = new Dictionary<int, ActiveHintInfo>();

        private int _nextHintId = 1;

        private ReferenceHub _referenceHub;
        private HintDisplay _hintDisplay;

        private void Awake()
        {
            _referenceHub = GetComponent<ReferenceHub>();
            _hintDisplay = GetComponent<HintDisplay>();

            if (_hintDisplay == null)
            {
                Debug.LogError("PlayerHintController requires HintDisplay component!");
            }
        }

        private void Start()
        {
            _controllers[_referenceHub.PlayerId] = this;

            if (isLocalPlayer)
            {
                LocalPlayer = this;
            }

            if (isServer)
            {
                InitializeServer();
            }
        }

        private void OnDestroy()
        {
            ClearAllHints();
            _controllers.Remove(_referenceHub.PlayerId);

            if (isLocalPlayer)
            {
                LocalPlayer = null;
            }
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;

            UpdateActiveHints();
        }

        public int ShowHint(CustomHint hint, bool isPersistent = false)
        {
            if (hint == null)
                return -1;

            int hintId = _nextHintId++;

            if (isServer)
            {
                ServerShowHint(hintId, hint, isPersistent);
            }
            else if (isClient)
            {
                CmdShowHint(hintId, hint, isPersistent);
            }

            return hintId;
        }

        public void UpdateHintText(int hintId, string newText)
        {
            if (!_activeHints.ContainsKey(hintId))
                return;

            if (isServer)
            {
                ServerUpdateHintText(hintId, newText);
            }
            else if (isClient)
            {
                CmdUpdateHintText(hintId, newText);
            }
        }

        public void RemoveHint(int hintId)
        {
            if (!_activeHints.ContainsKey(hintId))
                return;

            if (isServer)
            {
                ServerRemoveHint(hintId);
            }
            else if (isClient)
            {
                CmdRemoveHint(hintId);
            }
        }

        public void ClearAllHints()
        {
            if (isServer)
            {
                ServerClearAllHints();
            }
            else if (isClient)
            {
                CmdClearAllHints();
            }
        }

        public bool HasHint(int hintId)
        {
            return _activeHints.ContainsKey(hintId);
        }

        [Server]
        private void InitializeServer()
        {
        }

        [Server]
        private void ServerShowHint(int hintId, CustomHint hint, bool isPersistent)
        {
            var hintInfo = new ActiveHintInfo
            {
                HintId = hintId,
                OriginalHint = hint,
                StartTime = (float)NetworkTime.time,
                IsPersistent = isPersistent,
                DisplayText = hint.GetDisplayText(0f)
            };

            _activeHints[hintId] = hintInfo;
            RpcShowHint(hintId, hint, isPersistent);
            Debug.Log($"Server: Show hint {hintId} to player {_referenceHub.nicknameSync.Network_displayName}");
        }

        [Server]
        private void ServerUpdateHintText(int hintId, string newText)
        {
            if (_activeHints.TryGetValue(hintId, out var hintInfo))
            {
                hintInfo.DisplayText = newText;
                RpcUpdateHintText(hintId, newText);
            }
        }

        [Server]
        private void ServerRemoveHint(int hintId)
        {
            if (_activeHints.Remove(hintId))
            {
                RpcRemoveHint(hintId);
            }
        }

        [Server]
        private void ServerClearAllHints()
        {
            var hintIds = new List<int>(_activeHints.Keys);
            foreach (int hintId in hintIds)
            {
                ServerRemoveHint(hintId);
            }
        }

        [Command]
        private void CmdShowHint(int hintId, CustomHint hint, bool isPersistent)
        {
            ServerShowHint(hintId, hint, isPersistent);
        }

        [Command]
        private void CmdUpdateHintText(int hintId, string newText)
        {
            ServerUpdateHintText(hintId, newText);
        }

        [Command]
        private void CmdRemoveHint(int hintId)
        {
            ServerRemoveHint(hintId);
        }

        [Command]
        private void CmdClearAllHints()
        {
            ServerClearAllHints();
        }

        [ClientRpc]
        private void RpcShowHint(int hintId, CustomHint hint, bool isPersistent)
        {
            if (!isLocalPlayer)
                return;

            var hintInfo = new ActiveHintInfo
            {
                HintId = hintId,
                OriginalHint = hint,
                StartTime = (float)NetworkTime.time,
                IsPersistent = isPersistent,
                DisplayText = hint.GetDisplayText(0f)
            };

            _activeHints[hintId] = hintInfo;

            if (_hintDisplay != null)
            {
                _hintDisplay.Show(hint);
            }
        }

        [ClientRpc]
        private void RpcUpdateHintText(int hintId, string newText)
        {
            if (!isLocalPlayer)
                return;

            if (_activeHints.TryGetValue(hintId, out var hintInfo))
            {
                hintInfo.DisplayText = newText;
            }
        }

        [ClientRpc]
        private void RpcRemoveHint(int hintId)
        {
            if (!isLocalPlayer)
                return;

            _activeHints.Remove(hintId);
        }

        private void UpdateActiveHints()
        {
            float currentTime = Time.time;
            var hintsToRemove = new List<int>();

            foreach (var kvp in _activeHints)
            {
                var hintInfo = kvp.Value;
                float elapsed = currentTime - hintInfo.StartTime;
                float totalDuration = hintInfo.OriginalHint.ShowTime * hintInfo.OriginalHint.DurationScalar;
                float progress = Mathf.Clamp01(elapsed / totalDuration);

                if (hintInfo.OriginalHint.AutoText != null)
                {
                    try
                    {
                        string newText = hintInfo.OriginalHint.AutoText(progress);
                        if (newText != hintInfo.DisplayText)
                        {
                            hintInfo.DisplayText = newText;
                            OnHintTextUpdated?.Invoke(hintInfo.HintId, newText);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error in AutoText delegate for hint {hintInfo.HintId}: {ex.Message}");
                    }
                }

                if (!hintInfo.IsPersistent && progress >= 1.0f)
                {
                    hintsToRemove.Add(kvp.Key);
                }
            }

            foreach (int hintId in hintsToRemove)
            {
                _activeHints.Remove(hintId);
                OnHintExpired?.Invoke(hintId);
            }
        }

        public static PlayerHintController GetController(int playerId)
        {
            _controllers.TryGetValue(playerId, out var controller);
            return controller;
        }

        public static PlayerHintController GetController(ReferenceHub hub)
        {
            if (hub == null)
                return null;

            return GetController(hub.PlayerId);
        }

        public static void BroadcastHint(CustomHint hint)
        {
            foreach (var controller in _controllers.Values)
            {
                if (controller != null)
                {
                    controller.ShowHint(hint);
                }
            }
        }

        public static void BroadcastHintToTeam(CustomHint hint, Team team)
        {
            foreach (var controller in _controllers.Values)
            {
                if (controller != null &&
                    controller._referenceHub.roleManager != null &&
                    controller._referenceHub.roleManager.CurrentRole.Team == team)
                {
                    controller.ShowHint(hint);
                }
            }
        }

        public delegate void HintTextUpdatedDelegate(int hintId, string newText);
        public delegate void HintExpiredDelegate(int hintId);

        public event HintTextUpdatedDelegate OnHintTextUpdated;
        public event HintExpiredDelegate OnHintExpired;

        private class ActiveHintInfo
        {
            public int HintId;
            public CustomHint OriginalHint;
            public float StartTime;
            public bool IsPersistent;
            public string DisplayText;
        }
    }
}
