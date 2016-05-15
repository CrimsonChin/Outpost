using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnDestroy : MonoBehaviour
{
    public AudioClip[] audioClips;

    void OnDestroy()
    {
        Debug.Log(gameObject.name);
        if (audioClips != null && audioClips.Any())
        {
            AudioClip clip = audioClips[Random.Range(0, audioClips.Length - 1)];
            AudioSource.PlayClipAtPoint(clip, Vector2.zero);
        }
    }
}
