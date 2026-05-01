using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.FMOD
{
    public abstract class IMOD<TConfig>:MOD
    {
        public new Type ConfigType
        {
            get
            {
                return Config.GetType();
            }
        }
        public new TConfig Config { get; }
    }
}
