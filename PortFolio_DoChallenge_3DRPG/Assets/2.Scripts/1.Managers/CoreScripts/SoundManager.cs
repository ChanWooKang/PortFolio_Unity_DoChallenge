using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class SoundManager
{
    AudioSource[] audioSources = new AudioSource[(int)SoundType.Max_Cnt];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundName = System.Enum.GetNames(typeof(SoundType));
            for (int i = 0; i < soundName.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundName[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            audioSources[(int)SoundType.BGM].loop = true;
        }

    }

    public void Clear()
    {
        if (audioSources != null)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.clip = null;
                audioSource.Stop();
            }
        }

        if (_audioClips != null)
        {
            _audioClips.Clear();
        }

    }

    public void Play(string path, SoundType type = SoundType.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void Play(AudioClip audioClip, SoundType type = SoundType.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == SoundType.BGM)
        {

            AudioSource audioSource = audioSources[(int)SoundType.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();


        }
        else
        {
            AudioSource audioSource = audioSources[(int)SoundType.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClip(string path, SoundType type = SoundType.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == SoundType.BGM)
        {
            audioClip = Managers._resource.Load<AudioClip>(path);
        }
        else
        {

            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers._resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }

        }

        if (audioClip == null)
        {
            Debug.Log($"AudioClip Missing {path}");
        }

        return audioClip;
    }
}
