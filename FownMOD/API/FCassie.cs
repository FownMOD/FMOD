using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassie;
using FMOD.API.DamageHandles;

namespace FMOD.API
{
    public class FCassie
    {
        public static CassieTtsPayload Translation(string customAnnouncement, string Translation, bool playNosy = true)
        {
            CassieTtsPayload cassieTtsPayload = new CassieTtsPayload(customAnnouncement, Translation, playNosy);
            cassieTtsPayload.PlaySubtitleMessage();
            return cassieTtsPayload;
        }
        public static CassieTtsPayload Send(string customAnnouncement, bool playNosy = true)
        {
            CassieTtsPayload cassieTtsPayload = new CassieTtsPayload(customAnnouncement, playNosy);
            cassieTtsPayload.PlaySubtitleMessage();
            return cassieTtsPayload;
        }
        public static void SendScpTerminationAnnouncement(Player scp, DamageBase damageBase)
        {
            Cassie.CassieScpTerminationAnnouncement.AnnounceScpTermination(scp.ReferenceHub, damageBase.Base);
        }
    }
}
