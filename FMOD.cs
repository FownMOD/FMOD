using FMOD.API;
using FMOD.Other;
using HarmonyLib;
using LabApi.Loader.Features.Plugins;
using Mirror;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD
{
    public class FMODConfig
    {
        public bool IsEnabled { get; set; } = true;
        [Description("FMOD的总目录")]
        public string FMODFile { get; set; } = Paths.BaseDir;
        
    }
    public class FMOD : Plugin<FMODConfig>
    {
        public override string Name => "FMOD";

        public override string Description => "SCP SL Plugin Loader - FOMD";

        public override string Author => "灰";

        public override Version Version => new Version(FMODVersion.MainVersion.ToString());

        public override Version RequiredApiVersion => new Version(1,0);
        public override void Enable()
        {
            Harmony harmony = new Harmony($"{Name}.{Version}");
            harmony.PatchAll();
            Log.CustomInfo($"使用Harmony进行事件注册", UnityEngine.Color.blue);
            try
            {
                Log.CustomInfo($"===>{Name}.{Version}注册成功<===", UnityEngine.Color.blue);
            }
            catch(Exception e)
            {
                Log.Error($"注册失败");
                Log.Error(e.Message.ToString());
            }
            Paths.GenerateFoldersAndFiles(Server.Port);
            Load.LoadAllPlugins(Server.Port);
            Permissions.Initialize();
            Log.Debug($"欢迎使用FMOD");
            Log.Debug($"{LogMsg.FMOD}");
            Log.Debug($"一共加载了{Load.GetLoadedPlugins().Count}个插件");
        }
        public override void Disable()
        {
            Load.DisableAllPlugins();
            Log.CustomInfo($"FMOD已被禁用", UnityEngine.Color.red);
            Log.Debug($"{LogMsg.FMOD}");
        }
    }
}
