using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

namespace FMOD.API.ServerSpecific
{
    public class Button : SSServerSpecifiSetting
    {
        public Button(ServerSpecificSettingBase serverSpecificSettingBase) : base(serverSpecificSettingBase)
        {
            this.Base = (SSButton)serverSpecificSettingBase;
        }
        public Button(SSButton sb):base(sb)
        {
            this.Base = sb;
        }
        public new SSButton Base { get; }
        public string ButtonText
        {
            get => Base.ButtonText;
            set => Base.SendButtonUpdate(value,this.HoldTime);
        }
        public float HoldTime
        {
            get => Base.HoldTimeSeconds;
        }
    }
}
