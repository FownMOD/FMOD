using AdminToys;
using FMOD.Enums;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoiceChat.Networking;

namespace FMOD.API.AdminToys
{
    public class Speaker : AdminToy
    {
        public override AdminToyType AdminToyType => AdminToyType.Speaker;
        public Speaker(SpeakerToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }
        public new SpeakerToy Base { get; set; }
        public float Volume
        {
            get
            {
                return this.Base.NetworkVolume;
            }
            set
            {
                this.Base.NetworkVolume = value;
            }
        }

        public bool IsSpatial
        {
            get
            {
                return this.Base.NetworkIsSpatial;
            }
            set
            {
                this.Base.NetworkIsSpatial = value;
            }
        }

        public float MaxDistance
        {
            get
            {
                return this.Base.NetworkMaxDistance;
            }
            set
            {
                this.Base.NetworkMaxDistance = value;
            }
        }

        public float MinDistance
        {
            get
            {
                return this.Base.NetworkMinDistance;
            }
            set
            {
                this.Base.NetworkMinDistance = value;
            }
        }

        public byte ControllerId
        {
            get
            {
                return this.Base.NetworkControllerId;
            }
            set
            {
                this.Base.NetworkControllerId = value;
            }
        }
        public static Speaker Create(Vector3 pos)
        {
            SpeakerToy speakerToy = new SpeakerToy();
            NetworkServer.Spawn(speakerToy.gameObject);
            speakerToy.NetworkPosition = pos;
            return (Speaker)AdminToy.Get(speakerToy);
        }
        public void Play(AudioMessage message)
        {
            foreach (Player player in Player.List)
            {
                player.Connection.Send<AudioMessage>(message);
            }
        }
        public void Play(byte[] samples, int? length = null)
        {
            Play(new AudioMessage(this.ControllerId, samples, length ?? samples.Length));
        }
    }
}
