using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Loader
{
    public class LabConfig
    {
        public bool IsEnabled { get; set; } = true;
        [Description("在运行FMOD的插件时也会运行LabAPI的插件")]
        public bool LoadLabPlugin { get; set; } = true;
    }
}
