using NLayer;
using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Networking;

namespace FMOD.Extensions
{
    public static class AudioMessageExtensions
    {
        public static AudioMessage ConvertMp3ToAudioMessage(string filePath, byte controllerId)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("MP3 file not found: " + filePath);
            }

            if (Path.GetExtension(filePath).ToLower() != ".mp3")
            {
                throw new System.ArgumentException("Only .mp3 format is supported");
            }

            using (var fileStream = File.OpenRead(filePath))
            using (var mpegDecoder = new MpegFile(fileStream))
            {
                float[] audioSamples = ReadAllSamples(mpegDecoder);
                byte[] audioData = FloatArrayToByteArray(audioSamples);

                AudioMessage message = new AudioMessage(controllerId, audioData, audioData.Length);

                return message;
            }
        }
        public static AudioMessage ConvertWavToAudioMessage(string filePath, byte controllerId)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("WAV file not found: " + filePath);
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                string riff = new string(reader.ReadChars(4));
                if (riff != "RIFF")
                {
                    throw new InvalidDataException("Not a valid WAV file");
                }

                reader.ReadInt32();

                string wave = new string(reader.ReadChars(4));
                if (wave != "WAVE")
                {
                    throw new InvalidDataException("Not a valid WAV file");
                }

                string fmt = new string(reader.ReadChars(4));
                if (fmt != "fmt ")
                {
                    throw new InvalidDataException("Invalid WAV format");
                }

                int fmtChunkSize = reader.ReadInt32();
                short audioFormat = reader.ReadInt16();
                short numChannels = reader.ReadInt16();
                int sampleRate = reader.ReadInt32();
                int byteRate = reader.ReadInt32();
                short blockAlign = reader.ReadInt16();
                short bitsPerSample = reader.ReadInt16();

                reader.BaseStream.Seek(fmtChunkSize - 16, SeekOrigin.Current);

                string dataChunk = new string(reader.ReadChars(4));
                if (dataChunk != "data")
                {
                    throw new InvalidDataException("WAV data chunk not found");
                }

                int dataSize = reader.ReadInt32();
                byte[] wavData = reader.ReadBytes(dataSize);

                AudioMessage message = new AudioMessage(controllerId, wavData, wavData.Length);
                return message;
            }
        }
        public static AudioMessage ConvertOggToAudioMessage(string filePath, byte controllerId)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("OGG file not found: " + filePath);
            }

            if (Path.GetExtension(filePath).ToLower() != ".ogg")
            {
                throw new System.ArgumentException("Only .ogg format is supported");
            }

            using (var vorbisReader = new VorbisReader(filePath))
            {
                if (vorbisReader.SampleRate != 48000)
                {
                    throw new InvalidDataException("Only 48000Hz sample rate is supported, current: " + vorbisReader.SampleRate + "Hz");
                }

                if (vorbisReader.Channels != 1)
                {
                    throw new InvalidDataException("Only mono channel is supported, current channels: " + vorbisReader.Channels);
                }

                float[] audioSamples = ReadAllSamples(vorbisReader);
                byte[] audioData = FloatArrayToByteArray(audioSamples);

                AudioMessage audioMessage = new AudioMessage(controllerId, audioData, audioData.Length);

                return audioMessage;
            }
        }
        public static float[] ReadAllSamples(MpegFile decoder)
        {
            int totalSamples = (int)(decoder.Length / sizeof(float));
            float[] samples = new float[totalSamples];

            int samplesRead = decoder.ReadSamples(samples, 0, totalSamples);

            if (samplesRead < totalSamples)
            {
                System.Array.Resize(ref samples, samplesRead);
            }

            return samples;
        }
        public static float[] ReadAllSamples(VorbisReader reader)
        {
            int totalSamples = (int)(reader.TotalSamples * reader.Channels);
            float[] samples = new float[totalSamples];

            int samplesRead = 0;
            float[] buffer = new float[4096];

            while (true)
            {
                int read = reader.ReadSamples(buffer, 0, buffer.Length);
                if (read == 0) break;

                System.Array.Copy(buffer, 0, samples, samplesRead, read);
                samplesRead += read;
            }

            if (samplesRead < totalSamples)
            {
                System.Array.Resize(ref samples, samplesRead);
            }

            return samples;
        }

        public static byte[] FloatArrayToByteArray(float[] floatArray)
        {
            byte[] byteArray = new byte[floatArray.Length * sizeof(float)];
            System.Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }
    }
}
