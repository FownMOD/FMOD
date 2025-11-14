using FMOD.API.AdminToys;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.SSAudio
{
    public class AudioPlayerBase:MonoBehaviour
    {
        public static Dictionary<string, AudioPlayerBase> List = new Dictionary<string, AudioPlayerBase>();
        public static Dictionary<AudioPlayerBase, Speaker> Speakers = new Dictionary<AudioPlayerBase, Speaker>();
        
        public static bool TryGet(string name, out AudioPlayerBase player)
        {
            AudioPlayerBase audioPlayerBase = List[name];
            if (audioPlayerBase == null) { player = null; return false; }
            player = audioPlayerBase;
            return true;
        }
        public string Name
        {
            get
            {
                return base.name;
            }
            set
            {
                base.name = value;
            }
        }
        public Vector3 Position
        {
            get
            {
                return gameObject.transform.position;
            }
            set
            {
                gameObject.transform.position = value;
            }
        }
        public float Volume
        {
            get
            {
                return Speaker.Volume;
            }
            set
            {
                Speaker.Volume = value;
            }
        }
        public bool IsPlaying { get; set; } = false;
        public Speaker Speaker { get; set; }
        public static AudioPlayerBase Create(string audioName)
        {
            if (List.ContainsKey(audioName))
            {
                Debug.LogWarning($"AudioPlayerBase with name '{audioName}' already exists!");
                return List[audioName];
            }

            GameObject gameObject = new GameObject(audioName);
            AudioPlayerBase player = gameObject.AddComponent<AudioPlayerBase>();
            player.Name = audioName;

            List.Add(audioName, player);
            return player;
        }
        public void AddOrCreateSpeaker(Speaker speaker)
        {
            this.Speaker = speaker;
            Speakers.Add(this, speaker);
        }
        public void Stop()
        {
            this.Destroy(true);
        }
        public void Play(string filepath, Enums.AudioType audioType)
        {
            if (Speaker!= null)
            {
                IsPlaying = false;
                Log.Debug("Speaker未绑定");
                return;
            }
            Speaker.Play(filepath, audioType);
        }
        public void Destroy(bool DestroySpeaker = false)
        {
            GameObject.Destroy(gameObject);
            GameObject.Destroy(this);
            if(!DestroySpeaker)
            {
                if (Speaker != null)
                {
                    GameObject.Destroy(Speaker.Base.gameObject);
                }
            }
            Log.Debug($"[{this.Name}]播放器销毁成功");
        }
    }
}
