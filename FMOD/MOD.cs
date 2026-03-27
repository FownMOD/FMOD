using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.FMOD
{
    public abstract class MOD
    {
        public abstract string Name { get; }
        public abstract string Author { get; }
        public abstract Version Version { get; }
        public Type ConfigType {  get; }
        public abstract void OnEnabled();
        public abstract void OnDisable();
        public Object Config
        {
            get => Activator.CreateInstance(ConfigType);
        }
    }
}
