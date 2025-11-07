using FMOD.API.Patchs;
using Hints;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.CustHint
{
    public class ABHint
    {
        public static Dictionary<uint, ABHint> List = new Dictionary<uint, ABHint>();

        public uint ID { get; set; }
        public string Content { get; set; }
        public XCoordinateType XCoordinateType { get; set; }
        public YCoordinateType YCoordinateType { get; set; }
        public float Duration { get; set; }
        public Func<string> AutoText { get; set; }

        private CoroutineHandle _autoUpdateCoroutine;
        private bool _isAutoUpdating = false;

        public float XCoordinate
        {
            get
            {
                switch (XCoordinateType)
                {
                    case XCoordinateType.Middle:
                        return 500f;
                    case XCoordinateType.Right:
                        return 800f;
                    case XCoordinateType.Left:
                        return 200f;
                    case XCoordinateType.None:
                    default:
                        return 500f;
                }
            }
        }

        public float YCoordinate
        {
            get
            {
                switch (YCoordinateType)
                {
                    case YCoordinateType.Top:
                        return 20f;
                    case YCoordinateType.Middle:
                        return 500f;
                    case YCoordinateType.Bottom:
                        return 1000f;
                    case YCoordinateType.None:
                    default:
                        return 500f;
                }
            }
        }

        public ABHint(uint id, string content,
                 XCoordinateType xType = XCoordinateType.Middle,
                 YCoordinateType yType = YCoordinateType.Middle,
                 float duration = 10f)
        {
            this.ID = id;
            this.Content = content;
            this.XCoordinateType = xType;
            this.YCoordinateType = yType;
            this.Duration = duration;

            if (!List.ContainsKey(id))
            {
                List.Add(id, this);
                HintDisplayPatch.RegisterABHintID(id);
            }
        }

        private TextHint CreateTextHint()
        {
            return new TextHint(Content, CreateParameters(), CreateEffects(), Duration);
        }

        private HintParameter[] CreateParameters()
        {
            var parameters = new List<HintParameter>();
            parameters.Add(new PositionHintParameter(XCoordinate, YCoordinate));
            return parameters.ToArray();
        }

        private static HintEffect[] CreateEffects()
        {
            return new HintEffect[]
            {
                HintEffectPresets.FadeIn(0.5f),
                HintEffectPresets.TrailingPulseAlpha(0.7f, 1f, 0.5f)
            };
        }
        private TextHint CreateEmptyHint()
        {
            return new TextHint("", CreateParameters(), CreateEffects(), 0.1f);
        }
        public void StartAutoUpdate(NetworkConnection connection = null)
        {
            if (_isAutoUpdating || AutoText == null)
                return;

            _isAutoUpdating = true;
            _autoUpdateCoroutine = Timing.RunCoroutine(AutoUpdateCoroutine(connection));
        }

        public void StopAutoUpdate()
        {
            if (!_isAutoUpdating)
                return;

            _isAutoUpdating = false;
            Timing.KillCoroutines(_autoUpdateCoroutine);
        }

        private IEnumerator<float> AutoUpdateCoroutine(NetworkConnection connection)
        {
            while (_isAutoUpdating)
            {
                yield return Timing.WaitForSeconds(1f);

                try
                {
                    string newText = AutoText.Invoke();
                    if (newText != Content)
                    {
                        Content = newText;
                        if (connection != null && connection.isReady)
                        {
                            SendToPlayer(connection);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"AutoText委托执行错误: {ex}");
                }
            }
        }

        public static ABHint GetHint(uint id)
        {
            return List.ContainsKey(id) ? List[id] : null;
        }

        public static void RemoveHint(uint id)
        {
            ABHint hint = GetHint(id);
            if (hint != null)
            {
                hint.StopAutoUpdate();
                List.Remove(id);
                HintDisplayPatch.UnregisterABHintID(id);
            }
        }

        public void UpdateContent(string newContent)
        {
            this.Content = newContent;
        }

        public void UpdatePosition(XCoordinateType xType, YCoordinateType yType)
        {
            this.XCoordinateType = xType;
            this.YCoordinateType = yType;
        }

        public void SendToPlayer(NetworkConnection connection)
        {
            if (connection != null && connection.isReady)
            {
                var textHint = CreateTextHint();
                var hintMessage = new HintMessage(textHint);
                connection.Send(hintMessage);
            }
        }
        public void SendToPlayer(ReferenceHub referenceHub)
        {
            if (referenceHub.IsHost)
            {
                SendToPlayer(connection: referenceHub.connectionToClient);
            }
            SendToPlayer(connection: referenceHub.connectionToServer);
        }
        public void RemoveFromPlayer(NetworkConnection connection)
        {
            if (connection != null && connection.isReady)
            {
                var emptyHint = CreateEmptyHint();
                var hintMessage = new HintMessage(emptyHint);
                connection.Send(hintMessage);
            }
        }
        public static void RemoveHintFromPlayer(uint id, NetworkConnection connection)
        {
            ABHint hint = GetHint(id);
            if (hint != null)
            {
                hint.RemoveFromPlayer(connection);
            }
        }
    }
    public enum YCoordinateType
    {
        Top,
        Middle,
        Bottom,
        None
    }
    public enum XCoordinateType
    {
        Right,
        Middle,
        Left,
        None
    }
}
