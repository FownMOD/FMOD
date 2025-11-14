using AdminToys;
using FMOD.Enums;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoiceChat.Networking;

namespace FMOD.API.AdminToys
{
    public class Speaker : AdminToy
    {
        public override AdminToyType AdminToyType => AdminToyType.Speaker;

        public static new Dictionary<AdminToy, Speaker> List = new Dictionary<AdminToy, Speaker>();

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
            GameObject speakerObject = UnityEngine.Object.Instantiate(NetworkManager.singleton.spawnPrefabs.Find(p => p.GetComponent<SpeakerToy>() != null));
            SpeakerToy speakerToy = speakerObject.GetComponent<SpeakerToy>();

            if (speakerToy == null)
            {
                UnityEngine.Object.Destroy(speakerObject);
                return null;
            }

            NetworkServer.Spawn(speakerObject);
            speakerToy.NetworkPosition = pos;

            Speaker speaker = new Speaker(speakerToy);
            List.Add(AdminToy.Get(speakerToy), speaker);

            return speaker;
        }

        public static new Speaker Get(AdminToy adminToy)
        {
            if (List.ContainsKey(adminToy))
                return List[adminToy];
            return null;
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

        public void Destroy()
        {
            if (Base != null && Base.gameObject != null)
            {
                NetworkServer.Destroy(Base.gameObject);
            }

            if (List.ContainsValue(this))
            {
                var key = List.FirstOrDefault(x => x.Value == this).Key;
                if (key != null)
                    List.Remove(key);
            }
        }
    }
}