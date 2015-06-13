using System;
using Tao.Sdl;
using System.IO;
using System.Collections.Generic;

static class Sound
{
    private static List<IntPtr> sounds;
    private static int channels;

    public static bool IsOn { get; set; }
    public static bool IsLoaded { get; set; }

    public static void Init(int freq, int channelsNumber, int bytes)
    {
        IsOn = true;
        IsLoaded = true;
        channels = channelsNumber;
        SdlMixer.Mix_OpenAudio(freq, (short)SdlMixer.MIX_DEFAULT_FORMAT, 
                                                   channelsNumber, bytes);
        SdlMixer.Mix_AllocateChannels(3);
        sounds = new List<IntPtr>();
        Load();
    }

    private static void Load()
    {
        List<string> files = new List<string>(
                    Directory.EnumerateFiles("Data/Sounds"));

        foreach (string file in files)
        {
            int fileNameIndex = file.LastIndexOf("\\");
            string fileName = file.Substring(fileNameIndex + 1);

            if (fileName.EndsWith(".wav"))
            {
                IntPtr sound = SdlMixer.Mix_LoadWAV("Data/Sounds/"+fileName);
                sounds.Add(sound);

                if (sound == IntPtr.Zero)
                {
                    Console.WriteLine("ERROR AT LOADING: " + fileName);
                }
            }
        }
    }

    public static void PlayWav(int pos, int channelPos, int repeats)
    {
        if (pos >= 0 && pos < sounds.Count && channelPos >= 1 
            && channelPos <= channels)
        {
            SdlMixer.Mix_PlayChannel(channelPos, sounds[pos], repeats);
            IsOn = true;
        }
    }

    public static void Stop(int channel)
    {
        IsOn = false;
        SdlMixer.Mix_HaltChannel(channel);
    }

    public static void Pause(int channel)
    {
        SdlMixer.Mix_Pause(channel);
    }

    public static void Resume(int channel)
    {
        SdlMixer.Mix_Resume(channel);
    }

    public static void SetVolume(int channel, int volume)
    {
        SdlMixer.Mix_Volume(channel,volume);
    }
    public static int BACKGROUND = 0;
    public static int POWERUP = 1;
    public static int STEP = 2;
}