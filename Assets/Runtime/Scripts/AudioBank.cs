/*
 * Audio Toolkit
 * Created by Maged Atta (2026)
 * GitHub: https://github.com/MrCrazyDude115
 *
 * Please retain the original author credit.
 * Modifications and contributions are welcome.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioBank : MonoBehaviour
{
    [Serializable]
    public class SoundEntry
    {
        [Tooltip("Use this identifier when referencing the sound in code or events.")]
        [SerializeField] string identifier;
        [SerializeField] SoundCue cue;
        [SerializeField] bool playOnStart;

        //Read-only properties
        public string Identifier => identifier;
        public SoundCue Cue => cue;
        public bool PlayOnStart => playOnStart;

        public void StandardizeIdentifier()
        {
            if (identifier == null)
            {
                identifier = string.Empty;
                return;
            }

            identifier = identifier.Trim();
        }
    }

    //A dictionary is similar to a list but it stores things in pairs and can be accessed by a specific key or string, rather than a position [0], [1], etc.. like an array.
    //Stores active looping sound cues to be stopped manually later with Stop()
    Dictionary<string, AudioSource> activeLoopingSources = new Dictionary<string, AudioSource>();

    [SerializeField] List<SoundEntry> entries = new List<SoundEntry>();

    private void Start()
    {
        foreach (SoundEntry entry in entries)
        {
            if (entry == null)
            {
                continue;
            }

            if (entry.PlayOnStart)
            {
                Play(entry.Identifier);
            }
        }
    }

    private void OnValidate()
    {
        HashSet<string> identifiers = new HashSet<string>();

        foreach (SoundEntry entry in entries)
        {
            if (entry != null)
            {
                entry.StandardizeIdentifier();

                if (string.IsNullOrEmpty(entry.Identifier))
                {
                    continue;
                }

                if (entry.Identifier.Contains(" "))
                {
                    Debug.LogWarning("Sound Cue identifier contains spaces: " + entry.Identifier, this);
                    continue;
                }

                if (!identifiers.Add(entry.Identifier))
                {
                    Debug.LogWarning("Duplicate Sound Cue identifier: " + entry.Identifier, this);
                }
            }
        }
    }

    //This is to stop looping sounds from being left in the scene with no way to destroy them if the AudioBank is destroyed before they are manually stopped.
    private void OnDestroy()
    {
        foreach (AudioSource source in activeLoopingSources.Values)
        {
            if (source != null)
            {
                Destroy(source.gameObject);
            }
        }
    }

    public SoundCue GetCue(string identifier)
    {
        foreach (SoundEntry entry in entries)
        {
            if (entry == null)
            {
                continue;
            }

            if (entry.Identifier == identifier)
            {
                if (entry.Cue == null)
                {
                    Debug.LogWarning("No SoundCue assigned to " + identifier + "." + " No sound will play.", this);
                    return null;
                }

                if (entry.Cue.AudioResource == null)
                {
                    Debug.LogWarning("No AudioResource assigned to SoundCue " + identifier + "." + " No sound will play.", this);
                    return null;
                }

                return entry.Cue;
            }
        }

        Debug.LogWarning("No identifier " + identifier + " was found. Make sure you didn't misspell it.", this);
        return null;
    }

    private AudioSource CreateAudioSource(SoundCue cue, Transform parent)
    {
        if (cue == null)
        {
            return null;
        }

        GameObject audioObject = new GameObject("Sound Cue: " + cue.name);

        audioObject.transform.SetParent(parent);
        audioObject.transform.localPosition = Vector3.zero;

        AudioSource source = audioObject.AddComponent<AudioSource>();

        ConfigureAudioSource(source, cue);

        return source;
    }

    private AudioSource CreateAudioSourceAtPosition(SoundCue cue, Vector3 position)
    {
        if (cue == null)
        {
            return null;
        }

        GameObject audioObject = new GameObject("Sound Cue: " + cue.name);

        audioObject.transform.position = position;
        //audioObject.transform.localPosition = Vector3.zero;

        AudioSource source = audioObject.AddComponent<AudioSource>();

        ConfigureAudioSource(source, cue);

        return source;
    }

    //Overload
    public void Play(string identifier, Transform emitter)
    {
        SoundCue cue = GetCue(identifier);

        if (cue == null)
        {
            return;
        }

        if (emitter == null)
        {
            emitter = transform;
        }

        if (cue.Loop && activeLoopingSources.TryGetValue(identifier, out AudioSource existingSource))
        {
            if (existingSource != null)
            {
                return;
            }

            activeLoopingSources.Remove(identifier);
        }

        AudioSource source = CreateAudioSource(cue, emitter);

        if (source == null)
        {
            return;
        }

        source.Play();

        if (cue.Loop)
        {
            activeLoopingSources.Add(identifier, source);
        }
        else
        {
            AudioSourceCleanup cleanup = source.gameObject.AddComponent<AudioSourceCleanup>();
        }
    }

    public void Play(string identifier)
    {
        Play(identifier, transform);
    }

    public void PlayAtPosition(string identifier, Vector3 position)
    {

        SoundCue cue = GetCue(identifier);

        if (cue == null)
        {
            return;
        }

        if (cue.Loop && activeLoopingSources.TryGetValue(identifier, out AudioSource existingSource))
        {
            if (existingSource != null)
            {
                return;
            }

            activeLoopingSources.Remove(identifier);
        }

        AudioSource source = CreateAudioSourceAtPosition(cue, position);

        if (source == null)
        {
            return;
        }

        source.Play();

        if (cue.Loop)
        {
            activeLoopingSources.Add(identifier, source);
        }
        else
        {
            AudioSourceCleanup cleanup = source.gameObject.AddComponent<AudioSourceCleanup>();
        }
    }

    public void Stop(string identifier)
    {
        if (activeLoopingSources.TryGetValue(identifier, out AudioSource source))
        {
            if (source != null)
            {
                source.Stop();
                Destroy(source.gameObject);
            }

            activeLoopingSources.Remove(identifier);
        }
    }

    //Helper Methods

    //Fill the audio source's fields from the cue
    private void ConfigureAudioSource(AudioSource source, SoundCue cue)
    {
        source.resource = cue.AudioResource;
        source.outputAudioMixerGroup = cue.OutputMixerGroup;
        source.volume = cue.Volume;
        source.pitch = cue.Pitch;
        source.loop = cue.Loop;
        source.spatialBlend = cue.SpatialBlend;
        source.minDistance = cue.MinDistance;
        source.maxDistance = cue.MaxDistance;
        source.rolloffMode = cue.RolloffMode;
    }
}