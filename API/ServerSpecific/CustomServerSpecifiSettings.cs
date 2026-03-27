using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

namespace FMOD.API.ServerSpecific
{
    public abstract class CustomServerSpecifiSettings
    {
        public abstract List<SSServerSpecifiSetting> Settings { get; set; }
        public abstract void RegisterEvents();
        public abstract void UnregisterEvents();
        public void SendToPlayer(Player Tagrt)
        {
            List<ServerSpecificSettingBase> sbl = new List<ServerSpecificSettingBase>();
            foreach (var item in Settings)
            {
                sbl.Add(item.Base);
            }
            foreach (var item in sbl)
            {
                ServerSpecificSettingsSync.SendToPlayer(Tagrt.ReferenceHub, new ServerSpecificSettingBase[] { item });
            }
        }
    }
}
