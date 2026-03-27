using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace FMOD.API.ServerSpecific
{
    public class KeybindSetting:SSServerSpecifiSetting
    {
        public KeybindSetting(SSKeybindSetting sk):base(sk)
        {
            this.Base = (SSKeybindSetting)sk;
        }

        public new SSKeybindSetting Base { get; }
        public KeyCode AssignedKeyCode
        {
            get => Base.AssignedKeyCode;
        }
        public KeyCode SuggestedKey
        {
            get => Base.SuggestedKey;
        }
        public bool IsPressed
        {
            get => Base.SyncIsPressed;
        }
    }
}
