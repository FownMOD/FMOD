using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using PlayerRoles.Spectating;
using PlayerRoles.Subroutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.Roles
{
    public class Scp079 : Role
    {
        public Scp079(PlayerRoleBase roleBase) : base(roleBase)
        {
            this.Base = (Scp079Role)roleBase;
        }
        public new Scp079Role Base { get; set; }
        public Vector3 CameraPosition
        {
            get => Base.CameraPosition;
        }
        public Scp079Camera CurrentCamera
        {
            get => Base.CurrentCamera;
        }
        public SubroutineManagerModule SubroutineModule => Base.SubroutineModule;
        public SpectatableModuleBase SpectatorModule => Base.SpectatorModule;
        public float CurrentAux
        {
            get
            {
                SubroutineModule.TryGetSubroutine<Scp079AuxManager>(out var subroutine);
                return subroutine.CurrentAux;
            }
            set
            {
                SubroutineModule.TryGetSubroutine<Scp079AuxManager>(out var subroutine);
                subroutine.CurrentAux = value;
            }
        }
        public float MaxAux
        {
            get
            {
                SubroutineModule.TryGetSubroutine<Scp079AuxManager>(out var subroutine);
                return subroutine.MaxAux;
            }
        }
        public int TotalExp
        {
            get
            {
                SubroutineModule.TryGetSubroutine<Scp079TierManager>(out var scp079Tier);
                return scp079Tier.TotalExp;
            }
            set
            {
                SubroutineModule.TryGetSubroutine<Scp079TierManager>(out var scp079Tier);
                scp079Tier.TotalExp = value;
            }
        }
        public int NextLevelNeedExp
        {
            get
            {
                SubroutineModule.TryGetSubroutine<Scp079TierManager>(out var scp079Tier);
                return scp079Tier.NextLevelThreshold;
            }
        }
        public int Level
        {
            get
            {
                SubroutineModule.TryGetSubroutine<Scp079TierManager>(out var scp079Tier);
                return scp079Tier.AccessTierIndex;
            }
        }
        public void AddExp(int ammo, Scp079HudTranslation reason, RoleTypeId subject = RoleTypeId.None)
        {
            SubroutineModule.TryGetSubroutine<Scp079TierManager>(out var scp079Tier);
            scp079Tier.ServerGrantExperience(ammo, reason, subject);
        }
    }
}
