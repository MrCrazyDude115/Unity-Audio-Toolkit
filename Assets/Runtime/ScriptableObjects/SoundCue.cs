/*
 * Audio Toolkit
 * Created by Maged Atta (2026)
 * GitHub: https://github.com/MrCrazyDude115
 *
 * Please retain the original author credit.
 * Modifications and contributions are welcome.
 */

using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(
    fileName = "SoundCue",
    menuName = "Audio Toolkit/Sound Cue"
)]

public class SoundCue : ScriptableObject
{
    [Header("Audio")]
    [Tooltip("The Audio Resource that will play. Can be an individual sound file or an Audio Random Container.")]
    [SerializeField] AudioResource audioResource;
    [Tooltip("Optional Audio Mixer Group this cue will use when played.")]
    [SerializeField] AudioMixerGroup outputMixerGroup;

    [Header("Playback")]
    [Tooltip("Sets the overall volume of the sound.")]
    [SerializeField][Range(0f, 1f)] float volume = 1f;
    [Tooltip("Use this to slow down or speed up the sound.")]
    [SerializeField][Range(0.1f, 3f)] float pitch = 1f;
    [SerializeField] bool loop;

    [Header("Spatial Settings")]
    [Tooltip("0.0 = 2D, 1.0 = 3D. Unity supports values in between, and as such, I'm giving the designer the option to use the in-between spatial blending if they choose to.")]
    [SerializeField][Range(0f, 1f)] float spatialBlend = 1f;

    [Tooltip("Inside this distance, the sound is at full volume. Attenuation starts past this point.")]
    [SerializeField] float minDistance = 1f;
    [Tooltip("Past this distance, the sound has reached the end of its attenuation curve.")]
    [SerializeField] float maxDistance = 50f;

    [Tooltip("Determines how a 3D sound gets quieter over distance. Logarithmic Rolloff decreases quickly at first, then more gradually. Linear Rolloff decreases at a constant rate, before going silent at Max Distance. CUSTOM IS CURRENTLY UNSUPPORTED BY THIS AUDIO TOOLKIT.")]
    [SerializeField] AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

    //Read-only public properties
    public AudioResource AudioResource => audioResource;
    public AudioMixerGroup OutputMixerGroup => outputMixerGroup;

    public float Volume => volume;
    public float Pitch => pitch;
    public bool Loop => loop;

    public float SpatialBlend => spatialBlend;
    public float MinDistance => minDistance;
    public float MaxDistance => maxDistance;
    public AudioRolloffMode RolloffMode => rolloffMode;

    private void OnValidate()
    {
        minDistance = Mathf.Max(0f, minDistance);
        maxDistance = Mathf.Max(minDistance, maxDistance);
    }
}
