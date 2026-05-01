using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

namespace FMOD.API.ServerSpecific
{
    public class liderSetting:SSServerSpecifiSetting
    {
        public liderSetting(SSSliderSetting sk):base(sk)
        {
            this.Base = sk;
        }

        public new SSSliderSetting Base { get; }
        public float DefaultValue
        {
            get => Base.DefaultValue;
        }
        public float SyncFloatValue
        {
            get => Base.SyncFloatValue;
            set => Base.SyncFloatValue = value;
        }
        public bool SyncDragging
        {
            get => Base.SyncDragging;
            set => Base.SyncDragging = value;
        }
        public float MinValue
        {
            get => Base.MinValue;
            set => Base.SendSliderUpdate(value, MaxValue, Integer, Base.FinalDisplayFormat, Base.FinalDisplayFormat);
        }
        public float MaxValue
        {
            get => Base.MaxValue;
            set => Base.SendSliderUpdate(MinValue, value, Integer, Base.FinalDisplayFormat, Base.FinalDisplayFormat);
        }
        public bool Integer
        {
            get => Base.Integer;
            set => Base.SendSliderUpdate(MinValue, MaxValue, value, Base.FinalDisplayFormat, Base.FinalDisplayFormat);
        }
    }
}
