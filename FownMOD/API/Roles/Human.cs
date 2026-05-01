using PlayerRoles;
using PlayerRoles.Blood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Roles
{
    public class Human : Role
    {
        public Human(PlayerRoleBase roleBase) : base(roleBase)
        {
            this.Base = (HumanRole)roleBase;
        }
        public new HumanRole Base { get; set; }
        public BloodSettings BloodSettings
        {
            get => Base.BloodSettings;
        }
        public void WriteNickname(StringBuilder sb)
        {
            this.Base.WriteNickname(sb);
        }
        public bool AllowDisarming(Player detainer)
        {
            return this.Base.AllowDisarming(detainer.ReferenceHub);
        }
        public bool AllowUndisarming(Player player)
        {
            return this.Base.AllowUndisarming(player.ReferenceHub);
        }
        public int GetArmorEfficacy(HitboxType hitbox)
        {
            return this.Base.GetArmorEfficacy(hitbox);
        }
        
    }
}
