using AFK;
using AnimatorLayerManagement;
using Audio;
using CameraShaking;
using CentralAuth;
using CommandSystem;
using CommandSystem.Commands.RemoteAdmin.Dms;
using CustomPlayerEffects;
using Discord;
using FMOD.API.CustHint;
using FMOD.API.Interface;
using FMOD.API.Roles;
using FMOD.API.ServerSpecific;
using FMOD.API.SSAudio;
using FMOD.Events.Handlers;
using FMOD.Extensions;
using Footprinting;
using Hints;
using InventorySystem;
using InventorySystem.Disarming;
using InventorySystem.GUI;
using InventorySystem.Items;
using InventorySystem.Items.Usables;
using InventorySystem.Items.Usables.Scp330;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using Mirror;
using Org.BouncyCastle.Cms;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers;
using PlayerRoles.Voice;
using PlayerStatsSystem;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoiceChat;
using static RoundPlayerHistory;
using static RoundSummary;
using IVoiceRole = PlayerRoles.Voice.IVoiceRole;

namespace FMOD.API
{
    public class Player
    {
        public static List<ReferenceHub> ReferenceHubs = ReferenceHub.AllHubs.ToList();
        public static List<Player> List = new List<Player>();
        public static Dictionary<GameObject, Player> Dictionary { get; } = new Dictionary<GameObject, Player>();
        public static List<Player> RemoteAdmins => Player.List.Where(x => x.RemoteAdminAccess).ToList();
        public static int DummyCount => ReferenceHub.GetPlayerCount(ClientInstanceMode.Dummy);
        public static Dictionary<string, Player> UserIdsCache { get; } = new Dictionary<string, Player>(20);
        public Player(ReferenceHub referenceHub)
        {
            ReferenceHub = referenceHub;
        }
        public static Player Get(RoleTypeId roleTypeId)
        {
            return List.FirstOrDefault(x => x.ReferenceHub.roleManager.CurrentRole.RoleTypeId == roleTypeId);
        }
        public static Player Get(int Id)
        {
            return Get(ReferenceHubs.First(x => x.PlayerId == Id));
        }
        public static Player Get(string args)
        {
            Player result;
            try
            {
                Player player;
                int id;
                if (string.IsNullOrWhiteSpace(args))
                {
                    result = null;
                }
                else if (Player.UserIdsCache.TryGetValue(args, out player) && player.IsConnected)
                {
                    result = player;
                }
                else if (int.TryParse(args, out id))
                {
                    result = Player.Get(id);
                }
                else
                {
                    if (args.EndsWith("@steam") || args.EndsWith("@discord") || args.EndsWith("@northwood") || args.EndsWith("@offline"))
                    {
                        using (Dictionary<GameObject, Player>.ValueCollection.Enumerator enumerator = Player.Dictionary.Values.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Player player2 = enumerator.Current;
                                if (player2.UserId == args)
                                {
                                    player = player2;
                                    break;
                                }
                            }
                            goto IL_150;
                        }
                    }
                    int num = 31;
                    string text = args.ToLower();
                    foreach (Player player3 in Player.Dictionary.Values)
                    {
                        if (player3.IsConnected && player3.Nickname != null && player3.Nickname.ToLower().Contains(args.ToLower()))
                        {
                            int num2 = player3.Nickname.Length - text.Length;
                            if (num2 < num)
                            {
                                num = num2;
                                player = player3;
                            }
                        }
                    }
                IL_150:
                    if (player != null)
                    {
                        Player.UserIdsCache[player.UserId] = player;
                    }
                    result = player;
                }
            }
            catch (Exception arg)
            {
                Log.Error(string.Format("{0}.{1} error: {2}", typeof(Player).FullName, "Get", arg));
                result = null;
            }
            return result;
        }
        public static Player Get(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }
            Player result;
            if (Player.Dictionary.TryGetValue(gameObject, out result))
            {
                return result;
            }
            if (Player.UnverifiedPlayers.TryGetValue(gameObject, out result))
            {
                return result;
            }
            ReferenceHub referenceHub;
            if (ReferenceHub.TryGetHub(gameObject, out referenceHub))
            {
                return new Player(referenceHub);
            }
            return null;
        }
        public static Player Get(ICommandSender commandSender)
        {
            CommandSender Sender = commandSender as CommandSender;
            return Get(Sender.SenderId);
        }
        public static Player Get(CommandSender commandSender)
        {
            return Get(commandSender.SenderId);
        }
        public static Player Get(Footprint footprint)
        {
            return Get(footprint.Hub);
        }

        public static Player Get(ReferenceHub referenceHub)
        {
            return Get(referenceHub.gameObject);
        }
        public ReferenceHub ReferenceHub;
        public Footprint Footprint
        {
            get => ReferenceHub.GetComponent<Footprint>();
        }
        public Transform Transform => ReferenceHub.transform;
        public Role Role
        {
            get
            {
                Roles.Role role = new Role(this.ReferenceHub.roleManager.CurrentRole);
                return role;
            }
        }
        public void SetRole(RoleTypeId newrole, RoleSpawnFlags roleSpawnFlags = RoleSpawnFlags.None, RoleChangeReason roleChangeReason = RoleChangeReason.None)
        {
            this.ReferenceHub.roleManager.ServerSetRole(newrole, roleChangeReason, roleSpawnFlags);
        }
        public int Id => ReferenceHub.PlayerId;
        public string UserId => ReferenceHub.authManager.UserId;
        public GameObject GameObject
        {
            get => ReferenceHub.gameObject;
        }
        public virtual Vector3 Position
        {
            get
            {
                return FpcModule.Position;
            }
            set
            {
                this.ReferenceHub.TryOverridePosition(value);
            }
        }
        public bool IsDummy
        {
            get
            {
                return this.ReferenceHub.authManager.InstanceMode == ClientInstanceMode.Dummy;
            }
        }

        public Quaternion Quaternion => GameObject.transform.rotation;
        public Room CurrentRoom => Room.GetRoom(Position);
        public Inventory Inventory => ReferenceHub.inventory;
        public void Hurt(DamageHandlerBase damageHandlerBase)
        {
            this.ReferenceHub.playerStats.DealDamage(damageHandlerBase);
        }
        public void Hurt(float damage)
        {
            Hurt(new UniversalDamageHandler(damage, DeathTranslations.Unknown));
        }
        public void Kill()
        {
            this.Hurt(new UniversalDamageHandler(-1f, DeathTranslations.Unknown, null));
        }
        public void Kill(string msg)
        {
            this.Hurt(new CustomReasonDamageHandler(msg));
        }
        public float Health
        {
            get
            {
               return ReferenceHub.playerStats.GetModule<HealthStat>().CurValue;
            }
            set
            {
                ReferenceHub.playerStats.GetModule<HealthStat>().CurValue = value;
            }
        }
        public float MaxHealth
        {
            get
            {
                return this.ReferenceHub.playerStats.GetModule<HealthStat>().MaxValue;
            }
            set
            {
                this.ReferenceHub.playerStats.GetModule<HealthStat>().MaxValue = value;
            }
        }


        public void AddItem(ItemType itemType)
        {
            ReferenceHub.inventory.ServerAddItem(itemType, InventorySystem.Items.ItemAddReason.AdminCommand);
        }
        public API.Items.Item AddItem(ItemType itemType, InventorySystem.Items.ItemAddReason reason)
        {
            LabApi.Features.Wrappers.Item item = LabApi.Features.Wrappers.Item.Get(ReferenceHub.inventory.ServerAddItem(itemType, InventorySystem.Items.ItemAddReason.AdminCommand));
            return API.Items.Item.Get(item.Base);
        }
        public RoundPlayerHistory.PlayerHistoryLog GetData()
        {
            return RoundPlayerHistory.singleton.GetData(Id);
        }
        public void DisableAllEffects()
        {
            StatusEffectBase[] allEffects = this.ReferenceHub.playerEffectsController.AllEffects;
            for (int i = 0; i < allEffects.Length; i++)
            {
                allEffects[i].IsEnabled = false;
            }
        }
        public void RemoveItem(ItemBase item)
        {
            this.Inventory.ServerRemoveItem(item.ItemSerial, null);
        } 
        public void RemoveItem(ItemType itemType)
        {
            RemoveItem(itemType.GetItemBase());
        }
        public string Nickname
        {
            get
            {
                return this.ReferenceHub.nicknameSync.MyNick;
            }
            set
            {
                this.ReferenceHub.nicknameSync.MyNick = value;
            }
        }
        public Team Team => ReferenceHub.roleManager.CurrentRole.Team;
        public Vector3 Scale => GameObject.transform.localScale;
        public VoiceModuleBase VoiceModule
        {
            get
            {
                IVoiceRole voiceRole = this.Role.Base as IVoiceRole;
                if (voiceRole == null)
                {
                    return null;
                }
                return voiceRole.VoiceModule;
            }
        }
        public VoiceChatChannel VoiceChannel
        {
            get
            {
                VoiceModuleBase voiceModule = this.VoiceModule;
                if (voiceModule == null)
                {
                    return VoiceChatChannel.None;
                }
                return voiceModule.CurrentChannel;
            }
            set
            {
                if (VoiceModule is Interface.IVoiceRole voiceRole)
                {
                    voiceRole.VoiceChatChannel = value;
                }
            }
        }
        public uint NetworkId => ReferenceHub.characterClassManager.netId;
        public FacilityZone Zone => CurrentRoom.Zone;
        public string DisplayName => ReferenceHub.nicknameSync.DisplayName;
        public string IP => ReferenceHub.characterClassManager.connectionToClient.address;
        public IReadOnlyCollection<API.Items.Item> Items { get; }
        public Transform Camera => ReferenceHub.PlayerCameraReference;
        public bool IsNorthwoodStaff => ReferenceHub.authManager.NorthwoodStaff;
        public UserGroup UserGroup
        {
            get
            {
                return ReferenceHub.serverRoles.Group;
            }
            set
            {
                ReferenceHub.serverRoles.SetGroup(value);
            }
        }

        public void Kick(string MSG)
        {
            BanPlayer.KickUser(ReferenceHub, MSG);
        }
        public void Kick()
        {
            Kick(null);
        }
        public void Ban(string msg, long time)
        {
            BanPlayer.BanUser(ReferenceHub, msg, time);
        }
        public bool RemoteAdminAccess => ReferenceHub.serverRoles.RemoteAdmin;
        public PlayerPermissions RemoteAdminPermissions => (PlayerPermissions)this.ReferenceHub.serverRoles.Permissions;
        public PlayerCommandSender Sender
        {
            get
            {
                ReferenceHub.queryProcessor.TryGetSender(out var sender);
                return sender;
            }
        }
        public float Stamina
        {
            get
            {
                return ReferenceHub.playerStats.GetModule<StaminaStat>().CurValue;

            }
            set
            {
                ReferenceHub.playerStats.GetModule<StaminaStat>().CurValue = value;
            }
        }
        public float MaxStamina
        {
            get
            {
                return ReferenceHub.playerStats.GetModule<StaminaStat>().MaxValue;
            }
            set
            {
                ReferenceHub.playerStats.GetModule<StaminaStat>().MaxValue = value;
            }
        }
        public IEnumerable<StatusEffectBase> ActiveEffects
        {
            get
            {
                return from effect in this.ReferenceHub.playerEffectsController.AllEffects
                       where effect.Intensity > 0
                       select effect;
            }
        }
        public bool EnableEffect(StatusEffectBase statusEffect, byte intensity, float duration = 0f, bool addDurationIfActive = false)
        {
            if (statusEffect == null)
            {
                return false;
            }
            statusEffect.ServerSetState(intensity, duration, addDurationIfActive);
            return statusEffect != null && statusEffect.IsEnabled;
        }
        public bool EnableEffect(StatusEffectBase statusEffect, float duration = 0f, bool addDurationIfActive = false)
        {
            return this.EnableEffect(statusEffect, 1, duration, addDurationIfActive);
        }
        public StatusEffectBase EnableEffect(string effectName, float duration = 0f, bool addDurationIfActive = false)
        {
            return this.EnableEffect(effectName, 1, duration, addDurationIfActive);
        }
        public StatusEffectBase EnableEffect(string effectName, byte intensity, float duration = 0f, bool addDurationIfActive = false)
        {
            return this.ReferenceHub.playerEffectsController.ChangeState(effectName, intensity, duration, addDurationIfActive);
        }
        public bool IsHasItem(ItemType itemType) => Items.Any(x => x.Type == itemType);
        public bool IsHasItem(API.Items.Item item) => Items.Contains(item);
        public void Heal(float amount, bool overrideMaxHealth = false)
        {
            if (!overrideMaxHealth)
            {
                this.ReferenceHub.playerStats.GetModule<HealthStat>().ServerHeal(amount);
                return;
            }
            Health += amount;
        }
        public Vector3 Velocity
        {
            get
            {
                return this.ReferenceHub.GetVelocity();
            }
        }
        public bool IsCuffed
        {
            get
            {
                return this.Inventory.IsDisarmed();
            }
        }
        public bool IsJumping
        {
            get
            {
                IFpcRole fpcRole = this.Role.Base as IFpcRole;
                return fpcRole != null && fpcRole.FpcModule.Motor.JumpController.IsJumping;
            }
        }
        public bool IsHost
        {
            get
            {
                return this.ReferenceHub.isLocalPlayer;
            }
        }

        public bool IsAlive
        {
            get
            {
                return !this.IsDead;
            }
        }

        public bool IsDead
        {
            get
            {
                return Role.RoleTypeId == RoleTypeId.Spectator;
            }
        }
        public bool IsCH => Team == Team.ChaosInsurgency;
        public bool IsMTF => Team == Team.FoundationForces;
        public bool IsSCP => Team == Team.SCPs;
        public bool IsClassD => Role.RoleTypeId == RoleTypeId.ClassD;
        public bool IsScientist => Role.RoleTypeId == RoleTypeId.Scientist;
        public bool IsHuman
        {
            get
            {
                bool B = Role.RoleTypeId != RoleTypeId.Spectator && Team != Team.SCPs;
                return B;
            }
        }
        public bool IsBypassModeEnabled
        {
            get
            {
                return this.ReferenceHub.serverRoles.BypassMode;
            }
            set
            {
                this.ReferenceHub.serverRoles.BypassMode = value;
            }
        }
        public EmotionPresetType Emotion
        {
            get
            {
                return EmotionSync.GetEmotionPreset(this.ReferenceHub);
            }
            set
            {
                this.ReferenceHub.ServerSetEmotionPreset(value);
            }
        }
        public bool IsMuted
        {
            get
            {
                return VoiceChatMutes.QueryLocalMute(this.UserId, false);
            }
            set
            {
                if (value)
                {
                    this.VoiceChatMuteFlags |= VcMuteFlags.LocalRegular;
                    return;
                }
                this.VoiceChatMuteFlags &= ~VcMuteFlags.LocalRegular;
            }
        }

        public bool IsGlobalMuted
        {
            get
            {
                return VoiceChatMutes.IsMuted(ReferenceHub) && this.VoiceChatMuteFlags.HasFlag(VcMuteFlags.GlobalRegular);
            }
            set
            {
                if (value)
                {
                    this.VoiceChatMuteFlags |= VcMuteFlags.GlobalRegular;
                    return;
                }
                this.VoiceChatMuteFlags &= ~VcMuteFlags.GlobalRegular;
            }
        }

        public bool IsIntercomMuted
        {
            get
            {
                return VoiceChatMutes.QueryLocalMute(this.UserId, true);
            }
            set
            {
                if (value)
                {
                    this.VoiceChatMuteFlags |= VcMuteFlags.LocalIntercom;
                    return;
                }
                this.VoiceChatMuteFlags &= ~VcMuteFlags.LocalIntercom;
            }
        }
        public VcMuteFlags VoiceChatMuteFlags
        {
            get
            {
                return VoiceChatMutes.GetFlags(this.ReferenceHub);
            }
            set
            {
                VoiceChatMutes.SetFlags(this.ReferenceHub, value);
            }
        }
        public string AuthenticationToken
        {
            get
            {
                return this.ReferenceHub.authManager.GetAuthToken();
            }
        }
        public bool IsNPC
        {
            get
            {
                return this.ReferenceHub.IsDummy;
            }
        }
        public bool HasCustomName
        {
            get
            {
                return this.ReferenceHub.nicknameSync.HasCustomName;
            }
        }
        public string CustomName
        {
            get
            {
                return this.ReferenceHub.nicknameSync.Network_displayName ?? this.Nickname;
            }
            set
            {
                this.ReferenceHub.nicknameSync.Network_displayName = value;
            }
        }
        public PlayerInfoArea InfoArea
        {
            get
            {
                return this.ReferenceHub.nicknameSync.Network_playerInfoToShow;
            }
            set
            {
                this.ReferenceHub.nicknameSync.Network_playerInfoToShow = value;
            }
        }
        public string CustomInfo
        {
            get
            {
                return this.ReferenceHub.nicknameSync.Network_customPlayerInfoString;
            }
            set
            {
                string str;
                if (!NicknameSync.ValidateCustomInfo(value, out str))
                {
                    Log.Error("Could not set CustomInfo for " + this.Nickname + ". Reason: " + str);
                }
                this.InfoArea = (string.IsNullOrEmpty(value) ? (this.InfoArea & ~PlayerInfoArea.CustomInfo) : (this.InfoArea |= PlayerInfoArea.CustomInfo));
                this.ReferenceHub.nicknameSync.Network_customPlayerInfoString = value;
            }
        }
        public bool DoNotTrack
        {
            get
            {
                return this.ReferenceHub.authManager.DoNotTrack;
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.GameObject != null;
            }
        }
        public bool Invisible { get; set; } = false;
        public bool IsWhitelisted
        {
            get
            {
                return WhiteList.IsWhitelisted(this.UserId);
            }
        }
        internal static ConditionalWeakTable<GameObject, Player> UnverifiedPlayers { get; } = new ConditionalWeakTable<GameObject, Player>();
        public bool IsOverwatchEnabled
        {
            get
            {
                return this.ReferenceHub.serverRoles.IsInOverwatch;
            }
            set
            {
                this.ReferenceHub.serverRoles.IsInOverwatch = value;
            }
        }
        public T GetComponent<T>()
        {
            return GameObject.GetComponent<T>();
        }
        public bool IsNoclipPermitted
        {
            get
            {
                return FpcNoclip.IsPermitted(this.ReferenceHub);
            }
            set
            {
                if (value)
                {
                    FpcNoclip.PermitPlayer(this.ReferenceHub);
                    return;
                }
                FpcNoclip.UnpermitPlayer(this.ReferenceHub);
            }
        }
        public bool IsSpeaking
        {
            get
            {
                IVoiceRole voiceRole = Role.Base as IVoiceRole;
                return voiceRole != null && voiceRole.VoiceModule.ServerIsSending;
            }
        }
        public void SendHint(string text, float duration = 3f)
        {
            this.SendHint(text, new HintParameter[]
            {
        new StringHintParameter(string.Empty)
            }, null, duration);
        }
        public void SendHint(string text, HintEffect[] effects, float duration = 3f)
        {
            this.ReferenceHub.hints.Show(new TextHint(text, new HintParameter[]
            {
        new StringHintParameter(string.Empty)
            }, effects, duration));
        }
        public void SendHint(string text, HintParameter[] parameters, HintEffect[] effects = null, float duration = 3f)
        {
            HintDisplay hints = this.ReferenceHub.hints;
            HintParameter[] parameters2;
            if (!parameters.IsEmpty<HintParameter>())
            {
                parameters2 = parameters;
            }
            else
            {
                (parameters2 = new HintParameter[1])[0] = new StringHintParameter(string.Empty);
            }
            hints.Show(new TextHint(text, parameters2, effects, duration));
        }
        public NetworkConnection Connection
        {
            get
            {
                if (!this.IsHost)
                {
                    return this.ReferenceHub.networkIdentity.connectionToClient;
                }
                return this.ReferenceHub.networkIdentity.connectionToServer;
            }
        }
        public void SendBroadcast(string msg, ushort time)
        {
            Broadcast.Singleton.TargetAddElement(this.Connection, msg, time, Broadcast.BroadcastFlags.Normal);
        }
        public void ClearInventory()
        {

            foreach (var item in Items.ToArray())
            {
                item.UnSpawn();
            }
        }
        public PlayerHistoryLog PlayerHistory
        {
            get
            {
                PlayerHistoryLog pllog = new PlayerHistoryLog();
                foreach (PlayerHistoryLog log in RoundPlayerHistory.singleton.historyLogs)
                {
                    if (log.PlayerId == Id)
                    {
                        pllog = log;
                    }
                }
                return pllog;
            }
        }
        public DamagedAudio DamagedAudio
        {
            get
            {
                return ReferenceHub.GetComponent<Audio.DamagedAudio>();
            }
        }
        public AnimatorLayerManager AnimatorLayerManager
        {
            get
            {
                return ReferenceHub.GetComponent<AnimatorLayerManager>();
            }
        }
        public void AFKKick()
        {
            AFKManager.AddPlayer(ReferenceHub);
        }
        public AmmoElement AmmoElement
        {
            get
            {
                return ReferenceHub.GetComponent<AmmoElement>();
            }
        }
        public EmotionPresetType EmotionPresetType
        {
            get
            {
                return EmotionSync.GetEmotionPreset(ReferenceHub);
            }
            set
            {
                EmotionSync.ServerSetEmotionPreset(ReferenceHub, value);
            }
        }
        public float WalkSpeed
        {
            get
            {
                return FpcModule.WalkSpeed;
            }
            set
            {
                FpcModule.WalkSpeed = value;
            }
        }
        public float JumpSpeed
        {
            get
            {
                return FpcModule.JumpSpeed;
            }
            set
            {
                FpcModule.JumpSpeed = value;
            }

        }
        public float SprintSpeed
        {
            get
            {
                return FpcModule.SprintSpeed;
            }
            set
            {
                FpcModule.SprintSpeed = value;
            }
        }
        public Vector2 LookRotation
        {
            get
            {
                return new Vector2(FpcMouseLook.CurrentVertical, FpcMouseLook.CurrentHorizontal);
            }
            set => ReferenceHub.TryOverrideRotation(value);
        }
        public FpcMouseLook FpcMouseLook
        {
            get
            {
                return FpcModule.MouseLook;
            }
        }
        public FirstPersonMovementModule FpcModule
        {
            get
            {
                IFpcRole fpcRole = ReferenceHub.roleManager.CurrentRole as IFpcRole;
                return fpcRole.FpcModule;
            }
        }
        public Vector3 Gravity
        {
            get
            {
                return FpcModule.Motor.GravityController.Gravity;
            }

            set
            {
                FpcModule.Motor.GravityController.Gravity = value;
            }
        }
        public Faction Faction => Team.GetFaction();
        public void AddCustHint(ABHint hint)
        {
            hint.SendToPlayer(ReferenceHub);
        }
        public void RemoveCustHint(ABHint hint)
        {
            hint.RemoveFromPlayer(Connection);
        }
    }
}
