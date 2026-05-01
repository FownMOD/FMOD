using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.CASSIE
{
    public class StartingArgs: System.EventArgs,IFMODEvent
    {
        public StartingArgs(string customAnnouncement)
        {
            Content = customAnnouncement;
            IsAllow = true;
        }
        public string Content { get; set; }
        public bool IsAllow { get; set; } = true;
    }
}
