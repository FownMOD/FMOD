using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

namespace FMOD.API.ServerSpecific
{
    public abstract class SSServerSpecifiSetting
    {
        public SSServerSpecifiSetting(ServerSpecificSettingBase serverSpecificSettingBase)
        {
            this.Base = serverSpecificSettingBase;
        }
        public SSServerSpecifiSetting()
        {

        }
        public ServerSpecificSettingBase Base { get; }
        public int SettingId
        {
            get => Base.SettingId;
        }
        public string Label
        {
            get => Base.Label;
            set => Base.SendLabelUpdate(value);
        }
        public string Description
        {
            get => Base.HintDescription;
            set => Base.SendHintUpdate(value);
        }
        public ServerSpecificSettingBase OriginalDefinition
        {
            get => Base.OriginalDefinition;
        }
        public byte CollectionId
        {
            get => Base.CollectionId;
        }
        public void SendTo(Player player)
        {
            ServerSpecificSettingsSync.SendToPlayer(player.ReferenceHub,new ServerSpecificSettingBase[] {this.Base});
        }
        public override string ToString()
        {
            return Base.ToString();
        }
    }
}
