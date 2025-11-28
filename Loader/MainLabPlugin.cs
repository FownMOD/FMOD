using FMOD.API;
using HarmonyLib;
using LabApi.Loader.Features.Plugins;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Loader
{
    public class MainLabPlugin : Plugin<LabConfig>
    {
        public override string Name => "FMOD";

        public override string Description => "SCP:SL插件加载器";

        public override string Author => "灰";

        public override Version Version => Other.FMODVersion.MainVersion;

        public override Version RequiredApiVersion => new Version(LabApi.Features.LabApiProperties.CompiledVersion);
        public Harmony MainHarmony;
        public override void Enable()
        {
            MainHarmony = new Harmony("fmod.harmony.Patch");
            MainHarmony.PatchAll();
            Paths.GenerateFoldersAndFiles(Server.Port);
            Log.Debug($"使用Harmony进行补丁事件");
            Log.Debug($"{Other.LogMsg.FMOD}");
            Log.Debug($"欢迎使用FMOD插件加载器");
            Load.LoadAllMod(Config.LoadLabPlugin);
        }
        public override void Disable()
        {
            MainHarmony.UnpatchAll();
        }
    }
}
