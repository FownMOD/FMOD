using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

namespace FMOD.API.ServerSpecific
{
    public class TwoButtonsSetting:SSServerSpecifiSetting
    {
        public TwoButtonsSetting(SSTwoButtonsSetting st):base(st)
        {
            this.Base = st;
        }

        public new SSTwoButtonsSetting Base { get; }
        public string OptionA
        {
            get => Base.OptionA;
            set => Base.SendTwoButtonUpdate(value, OptionB);
        }
        public string OptionB
        {
            get => Base.OptionB;
            set => Base.SendTwoButtonUpdate(OptionA, value);
        }
        public bool PressIsA
        {
            get => Base.SyncIsA;
        }
        public bool PressIsB
        {
            get => Base.SyncIsB;
        }
    }
}
