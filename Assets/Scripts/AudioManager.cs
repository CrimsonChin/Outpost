using UnityEngine;

public class AudioManager : MonoBehaviour 
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

	private static AudioManager instance = null;

	public static AudioManager Instance 
	{
		get { return instance; }
	}

	void Awake() 
	{
		if (instance != null && instance != this) 
		{
			Destroy(gameObject);
			return;
		}
		else 
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
    }
	
	public void PlaySound(AudioClip clip)
    { 
		sfxSource.clip = clip;
        sfxSource.Play();
	}

    public void PlayRandomSound(AudioClip[] audioClips)
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxSource.pitch = randomPitch;
        sfxSource.clip = audioClips[randomIndex];
        sfxSource.Play();        
    }
}
