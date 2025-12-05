using FMOD.Enums;
using Footprinting;
using Mirror;
using PlayerRoles.PlayableScps.Scp1507;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.PlayableScps.Scp939;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace FMOD.API.DamageHandles
{
    public class DamageBase : DamageHandlerBase
    {
        public override string RagdollInspectText { get; }

        public override string DeathScreenText { get; }

        public override string ServerLogsText { get; }

        public override string ServerMetricsText { get; }
        public HitboxType HitboxType
        {
            get
            {
                return As<FirearmDamageHandler>().Hitbox;
            }
            set
            {
                As<FirearmDamageHandler>().Hitbox = value;
            }
        }
        public override CassieAnnouncement CassieDeathAnnouncement { get; }

        public override HandlerOutput ApplyDamage(ReferenceHub ply)
        {
            throw new NotImplementedException();
        }
        public DamageHandlerBase Base { get; }
        public RecontainmentDamageHandler RecontainmentDamageHandlerBase { get; }
        public StandardDamageHandler StandardDamageHandlerBase { get; }
        public CustomReasonDamageHandler CustomReasonDamageHandlerBase { get; }
        public WarheadDamageHandler WarheadDamageHandlerBase { get; }
        public ExplosionDamageHandler ExplosionDamageHandlerBase { get; }
        public FirearmDamageHandler FirearmDamageHandlerBase { get; }
        public AttackerDamageHandler AttackerDamageHandlerBase { get; }
        public DisruptorDamageHandler DisruptorDamageHandlerBase { get; }
        public CustomReasonFirearmDamageHandler CustomReasonFirearmDamageHandlerBase { get; }
        public ScpDamageHandler ScpDamageHandlerBase { get; }
        public Scp096DamageHandler Scp096DamageHandlerBase { get; }
        public Scp049DamageHandler Scp049DamageHandlerBase { get; }
        public Scp3114DamageHandler Scp3114DamageHandlerBase { get; }
        public Scp018DamageHandler Scp018DamageHandlerBase { get; set; }
        public PlayerStats PlayerStats {  get; }
        public MicroHidDamageHandler MicroHidDamageHandlerBase { get; }
        public JailbirdDamageHandler JailbirdDamageHandlerBase { get; }
        public UniversalDamageHandler UniversalDamageHandlerBase { get; }
        public DeathTranslation Translations {  get; }

        public T As<T>() where T : DamageHandlerBase
        {
            return this.Base as T;
        }
        public T BaseAs<T>() where T : DamageHandlerBase
        {
            return this as T;
        }
        public bool BaseIs<T>(out T param) where T : DamageHandlerBase
        {
            param = default(T);
            T t = this as T;
            if (t == null)
            {
                return false;
            }
            param = t;
            return true;
        }

        public Player Target { get; }
        public Player Attacker { get;}
        public float AbsorbedAhpDamage
        {
            get
            {
                StandardDamageHandler standardDamageHandler;
                if (!BaseIs<StandardDamageHandler>(out standardDamageHandler))
                {
                    return 0f;
                }
                return standardDamageHandler.AbsorbedAhpDamage;
            }
        }
        public Footprint TargetFootprint
        {
            get
            {
                return Target.Footprint;
            }
        }

        public virtual float Damage
        {
            get
            {
                StandardDamageHandler standardDamageHandler;
                if (!BaseIs<StandardDamageHandler>(out standardDamageHandler))
                {
                    return 0f;
                }
                return standardDamageHandler.Damage;
            }
            set
            {
                StandardDamageHandler standardDamageHandler;
                if (BaseIs<StandardDamageHandler>(out standardDamageHandler))
                {
                    standardDamageHandler.Damage = value;
                }
            }
        }

        private DamageType damageType;
        public DamageType Type
        {
            get
            {
                if (this.damageType != DamageType.Unknown)
                {
                    return this.damageType;
                }
                DamageHandlerBase @base = this.Base;
                if (@base is CustomReasonDamageHandler)
                {
                    return DamageType.Custom;
                }
                if (@base is WarheadDamageHandler)
                {
                    return DamageType.Warhead;
                }
                if (@base is ExplosionDamageHandler)
                {
                    return DamageType.Explosion;
                }
                if (@base is Scp018DamageHandler)
                {
                    return DamageType.Scp018;
                }
                if (@base is RecontainmentDamageHandler)
                {
                    
                    return DamageType.Recontainment;
                }
                if (@base is MicroHidDamageHandler)
                {
                    return DamageType.MicroHid;
                }
                if (@base is DisruptorDamageHandler)
                {
                    return DamageType.ParticleDisruptor;
                }
                if (@base is Scp939DamageHandler)
                {
                    return DamageType.Scp939;
                }
                if (@base is JailbirdDamageHandler)
                {
                    return DamageType.Jailbird;
                }
                if (@base is Scp1507DamageHandler)
                {
                    return DamageType.Scp1507;
                }
                if (@base is Scp956DamageHandler)
                {
                    return DamageType.Scp956;
                }
                if (@base is SnowballDamageHandler)
                {
                    return DamageType.SnowBall;
                }
                Scp3114DamageHandler scp3114DamageHandler = @base as Scp3114DamageHandler;
                DamageType result;
                if (scp3114DamageHandler != null)
                {
                    switch (scp3114DamageHandler.Subtype)
                    {
                        case Scp3114DamageHandler.HandlerType.Slap:
                            result = DamageType.Scp3114;
                            break;
                        case Scp3114DamageHandler.HandlerType.Strangulation:
                            result = DamageType.Strangled;
                            break;
                        case Scp3114DamageHandler.HandlerType.SkinSteal:
                            result = DamageType.Scp3114;
                            break;
                        default:
                            result = DamageType.Unknown;
                            break;
                    }
                    return result;
                }
                if (DeathScreenText == DeathTranslations.Poisoned.DeathscreenTranslation)
                {
                    return DamageType.Poison;
                }
                if (DeathScreenText == DeathTranslations.Scp207.DeathscreenTranslation)
                {
                    return DamageType.Scp207;
                }
                if (DeathScreenText == DeathTranslations.PocketDecay.DeathscreenTranslation)
                {
                    return DamageType.PocketDimension;
                }
                if (DeathScreenText == DeathTranslations.Asphyxiated.DeathscreenTranslation)
                {
                    return DamageType.Asphyxiation;
                }    
                if (DeathScreenText == DeathTranslations.Bleeding.DeathscreenTranslation)
                {
                    return DamageType.Bleeding;
                }
                if (DeathScreenText == DeathTranslations.CardiacArrest.DeathscreenTranslation)
                {
                    return DamageType.CardiacArrest;
                }
                if (DeathScreenText == DeathTranslations.Crushed.DeathscreenTranslation)
                {
                    return DamageType.Crushed;
                }
                if (DeathScreenText == DeathTranslations.Decontamination.DeathscreenTranslation)
                {
                    return DamageType.Decontamination;
                }
                if (DeathScreenText == DeathTranslations.Falldown.DeathscreenTranslation)
                {
                    return DamageType.Falldown;
                }
                if (DeathScreenText == DeathTranslations.Recontained.DeathscreenTranslation)
                {
                    return DamageType.Recontainment;
                }
                if (DeathScreenText == DeathTranslations.MarshmallowMan.DeathscreenTranslation)
                {
                    return DamageType.Marshmallow;
                }
                if (DeathScreenText == DeathTranslations.FriendlyFireDetector.DeathscreenTranslation)
                {
                    return DamageType.FriendlyFireDetector;
                }
                return DamageType.Unknown;
            }
        }
    }
}
