using UnityEngine;

public class AudioManager : MonoBehaviour 
{
    public AudioSource MusicSource;
    public AudioSource SfxSource;

    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    static AudioManager()
    {
        Instance = null;
    }

    public static AudioManager Instance { get; private set; }

    public void Awake() 
	{
		if (Instance != null && Instance != this) 
		{
			Destroy(gameObject);
		}
		else 
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
    }
	
	public void PlaySound(AudioClip clip)
    { 
		SfxSource.clip = clip;
        SfxSource.Play();
	}

    public void PlayRandomSound(AudioClip[] audioClips)
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        SfxSource.pitch = randomPitch;
        SfxSource.clip = audioClips[randomIndex];
        SfxSource.Play();        
    }
}
