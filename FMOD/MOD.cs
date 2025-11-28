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
        public abstract Type ConfigType {  get; }
        public virtual void OnEnabled() { }
        public virtual void OnDisable() { }
        public Events.Handlers.Player PlayerEvent;
        public object Config
        {
            get
            {
                if (ConfigType == null)
                    return null;

                return Activator.CreateInstance(ConfigType);
            }
        }
    }
}
