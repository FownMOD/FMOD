using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

namespace FMOD.API.ServerSpecific
{
    public class DropdownSetting : SSServerSpecifiSetting
    {
        public DropdownSetting(ServerSpecificSettingBase serverSpecificSettingBase) : base(serverSpecificSettingBase)
        {
            this.Base = (SSDropdownSetting)serverSpecificSettingBase;
        }
        public DropdownSetting(SSDropdownSetting @base) : base(@base)
        {
            Base = @base;
        }

        public new SSDropdownSetting Base { get; }
        public string[] Options
        {
            get => Base.Options;
            set => Base.SendDropdownUpdate(value);
        }
        public string SyncSelectionText
        {
            get => Base.SyncSelectionText;
        }
        public int SyncSelectionIndexValidated
        {
            get => Base.SyncSelectionIndexValidated;
        }

    }
}
