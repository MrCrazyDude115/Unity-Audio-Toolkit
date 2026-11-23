/*
 * Audio Toolkit
 * Created by Maged Atta (2026)
 * GitHub: https://github.com/MrCrazyDude115
 *
 * Please retain the original author credit.
 * Modifications and contributions are welcome.
 */

using UnityEngine;
using System.Collections;

public class AudioSourceCleanup : MonoBehaviour
{
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        StartCoroutine(CleanupWhenFinished(source));
    }

    private IEnumerator CleanupWhenFinished(AudioSource source)
    {
        while (source != null && source.isPlaying)
        {
            yield return null;
        }

        if (source != null)
        {
            Destroy(source.gameObject);
        }
    }
}
