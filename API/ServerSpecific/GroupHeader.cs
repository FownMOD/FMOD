using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;
namespace FMOD.API.ServerSpecific
{
    public class GroupHeader : SSServerSpecifiSetting
    {
        public GroupHeader(ServerSpecificSettingBase serverSpecificSettingBase) : base(serverSpecificSettingBase)
        {
            this.Base = (SSGroupHeader)serverSpecificSettingBase;
        }
        public new SSGroupHeader Base { get; }
    }
}
