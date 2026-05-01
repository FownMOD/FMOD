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
using static PlayerStatsSystem.DamageHandlerBase;

namespace FMOD.API.DamageHandles
{
    public abstract class DamageBase
    {
        public DamageBase(DamageHandlerBase damageHandler)
        {
            this.Base = damageHandler;
        }
        public string DeathScreenText
        {
            get => Base.DeathScreenText;
        }
        public CassieAnnouncement CassieDeathAnnouncement
        {
            get => Base.CassieDeathAnnouncement;
        }
        public DamageHandlerBase Base { get; }
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
