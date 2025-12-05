using PlayerRoles;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.PlayableScps.Scp173;
using PlayerRoles.Subroutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Roles
{
    public class Scp173Role : Role
    {
        public Scp173Role(PlayerRoles.PlayableScps.Scp173.Scp173Role roleBase) : base(roleBase)
        {
            this.Base = roleBase;
        }
        public new PlayerRoles.PlayableScps.Scp173.Scp173Role Base { get; }
        public SubroutineManagerModule SubroutineModule
        {
            get =>Base.SubroutineModule;
        }
        public ScpHudBase HudPrefab
        {
            get => Base.HudPrefab;
        }
        public Scp173AudioPlayer Scp173AudioPlayer
        {
            get => SubroutineModule.GetComponent<Scp173AudioPlayer>();
        }
        public Scp173BlinkTimer Scp173BlinkTimer
        {
            get =>SubroutineModule.GetComponent<Scp173BlinkTimer>();
        }
        public Scp173BreakneckSpeedsAbility Scp173BreakneckSpeedsAbility
        {
            get => SubroutineModule.GetComponent<Scp173BreakneckSpeedsAbility>();
        }
        public Scp173ChaseThemeProvider Scp173ChaseThemeProvider
        {
            get => SubroutineModule.GetComponent<Scp173ChaseThemeProvider>();
        }
        public Scp173ObserversTracker Scp173ObserversTracker
        {
            get => SubroutineModule.GetComponent<Scp173ObserversTracker>();
        }
    }
}
