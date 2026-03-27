using FMOD.Extensions;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoiceChat;
using VoiceChat.Networking;

namespace FMOD.API.FAudioPlayer
{
    public class FDummyPlayer
    {
        public FDummyPlayer(Player Speaker,string PlayerName,string File,VoiceChatChannel voiceChatChannel,bool IsLoop)
        {
            if(!Speaker.IsDummy)
            {
                this.Speaker = null;
            }
            this.Speaker = Speaker;
            this.MusicFile = File;
            this.IsSpeak = false;
            this.VoiceChatChannel = voiceChatChannel;
            this.IsLoop = IsLoop;
            this.Speaker.Nickname = PlayerName;
        }
        public string MusicFile { get; set; }
        public bool IsSpeak = false;
        public string AudioPlayerName
        {
            get => Speaker.Nickname;
            set => Speaker.Nickname = value;
        }
        public VoiceChatChannel VoiceChatChannel { get; set; }
        public Player Speaker { get; set; }
        public bool IsLoop = false;
        public VoiceMessage VoiceMessage;
        public CoroutineHandle CoroutineHandle;
        public float AudioLength;
        public float elapsedTime = 0f;
        public bool DestroyForFinish = true;
        public void Play()
        {
            if(IsSpeak==true)
            {
                Log.Error($"[{Speaker.Nickname}]无法使用，因为正在播放其他音频");
            }
            if (Path.GetExtension(MusicFile).ToLower()==".ogg")
            {
                this.VoiceMessage = VoiceMessageExtensions.ConvertOggToVoiceMessage(MusicFile, Speaker, VoiceChatChannel);
            }
            if(Path.GetExtension(MusicFile).ToLower()==".mp3")
            {
                this.VoiceMessage = VoiceMessageExtensions.ConvertMp3ToVoiceMessage(MusicFile, Speaker, VoiceChatChannel);
            }
            if(Path.GetExtension(MusicFile).ToLower()==".wav")
            {
                this.VoiceMessage = VoiceMessageExtensions.ConvertWavToVoiceMessage(MusicFile,Speaker, VoiceChatChannel);
            }
            foreach (ReferenceHub hub in ReferenceHub.AllHubs)
            {
                hub.connectionToClient.Send(VoiceMessage);
            }
            this.IsSpeak = true;
            AudioLength = GetAudioLength(MusicFile);
            if(CoroutineHandle.IsRunning)
            {
                Timing.KillCoroutines(CoroutineHandle);
            }

        }
        public IEnumerator<float> CheckPlayFinish()
        {
            yield return Timing.WaitForSeconds(1f);
            while (this.Speaker != null)
            {
                if (IsSpeak == false)
                {
                    yield break;
                }
                if (elapsedTime < AudioLength)
                {
                    elapsedTime += Time.deltaTime;
                    yield return Timing.WaitForSeconds(1f);
                }
                Finish(DestroyForFinish);
                yield break;
            }
        }
        private void Finish(bool WillDestroy)
        {
            elapsedTime = 0f;
            this.IsSpeak = false;
            this.AudioLength = 0;
            if (IsLoop == true)
            {
                Timing.CallDelayed(0.3f, () =>
                {
                    Play();
                });
            }
            byte[] emptyData = new byte[0];
            VoiceMessage voiceMessage = new VoiceMessage(this.Speaker.ReferenceHub, VoiceChatChannel.None, emptyData, 0, false);
            this.VoiceMessage = voiceMessage;
            foreach (ReferenceHub hub in ReferenceHub.AllHubs)
            {
                hub.connectionToClient.Send(voiceMessage);
            }
            if (CoroutineHandle.IsRunning)
            {
                Timing.KillCoroutines(CoroutineHandle);
            }
            if(WillDestroy == true)
            {
                Destroy();
            }
        }
        public float GetAudioLength(string MusicFile)
        {
            if (!File.Exists(MusicFile))
                return 0f;
            using (var vorbisReader = new NVorbis.VorbisReader(MusicFile))
            {
                double lengthInSeconds = (double)vorbisReader.TotalSamples / (vorbisReader.Channels * vorbisReader.SampleRate);
                return (float)lengthInSeconds;
            }
        }
        public void Stop(bool IsDestroy = false)
        {
            if(IsDestroy==true)
            {
                Destroy();
            }
            byte[] emptyData = new byte[0];
            VoiceMessage voiceMessage = new VoiceMessage(this.Speaker.ReferenceHub, VoiceChatChannel.None, emptyData, 0, false);
            this.VoiceMessage = voiceMessage;
            this.IsSpeak = false;
            foreach (ReferenceHub hub in ReferenceHub.AllHubs)
            {
                hub.connectionToClient.Send(voiceMessage);
            }
        }
        public void Destroy()
        {
            ReferenceHub.AllHubs.Remove(Speaker.ReferenceHub);
            Player.List.Remove(Speaker);
            NetworkServer.Destroy(Speaker.GameObject);
            this.Speaker = null;
        }
    }
}
