using AdminToys;
using FMOD.Enums;
using LabApi.Features.Audio;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoiceChat.Codec.Enums;
using VoiceChat.Networking;

namespace FMOD.API.AdminToys
{
    public class Speaker : AdminToy
    {
        public override AdminToyType AdminToyType => AdminToyType.Speaker;

        public new SpeakerToy Base { get; set; }

        public Speaker(SpeakerToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }

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
            var sp = FPrefabsManger.Spawn(pos, PrefabType.SpeakerToy);
            var speaker = sp.gameObject.AddComponent<SpeakerToy>();
            return new Speaker(speaker);
        }

        public void Play(AudioMessage message)
        {
            foreach (Player player in Player.List)
            {
                if (player.Connection != null)
                {
                    player.Connection.Send(message);
                }
            }
        }

        public void Play(byte[] samples, int? length = null)
        {
            Play(new AudioMessage(this.ControllerId, samples, length ?? samples.Length));
        }

        public void Play(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogError($"Audio file not found: {filePath}");
                return;
            }

            AudioMessage audioMessage = Extensions.AudioMessageExtensions.ConvertOggToAudioMessage(filePath, ControllerId);
            this.Play(audioMessage);
        }

        public void Play(string filePath, Enums.AudioType audioType)
        {
            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogError($"Audio file not found: {filePath}");
                return;
            }

            switch (audioType)
            {
                case Enums.AudioType.Ogg:
                    this.Play(filePath);
                    break;
                case Enums.AudioType.Mp3:
                    AudioMessage audioMessage = Extensions.AudioMessageExtensions.ConvertMp3ToAudioMessage(filePath, ControllerId);
                    this.Play(audioMessage);
                    break;
                case Enums.AudioType.Wav:
                    AudioMessage message = Extensions.AudioMessageExtensions.ConvertWavToAudioMessage(filePath, ControllerId);
                    this.Play(message);
                    break;
                default:
                    this.Play(filePath);
                    break;
            }
        }

        public void Stop()
        {
            AudioMessage stopMessage = new AudioMessage(this.ControllerId, new byte[0], 0);
            Play(stopMessage);
        }

    }
}