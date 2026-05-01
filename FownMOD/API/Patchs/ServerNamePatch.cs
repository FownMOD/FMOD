using GameCore;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Patchs
{
    [HarmonyPatch(typeof(ServerConsole),nameof(ServerConsole.ReloadServerName))]
    public class ServerNamePatch
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            ServerConsole.ServerName = ConfigFile.ServerConfig.GetString("server_name", "ServerName");
            ServerConsole.ServerName += $"[FMOD{Other.FMODVersion.MainVersion}]";
            return false;
        }
    }
}
