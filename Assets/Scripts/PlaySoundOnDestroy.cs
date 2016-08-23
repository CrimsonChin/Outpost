using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnDestroy : MonoBehaviour
{
    public AudioClip[] AudioClips;

    void OnDestroy()
    {
        Debug.Log(gameObject.name);
        if (AudioClips != null && AudioClips.Any())
        {
            AudioClip clip = AudioClips[Random.Range(0, AudioClips.Length - 1)];
            AudioSource.PlayClipAtPoint(clip, Vector2.zero);
        }
    }
}
