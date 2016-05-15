using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour 
{
	Animator _anim;
	bool _isFading = false;

	void Start () 
	{
		_anim = GetComponent<Animator> ();
	}

	public IEnumerator FadeToClear()
	{
		_isFading = true;
		_anim.Play ("FadeIn");

		while (_isFading) 
		{
			yield return null;
		}
	}

	public IEnumerator FadeToBlack()
	{
		_isFading = true;
		_anim.Play ("FadeOut");
		
		while (_isFading) 
		{
			yield return null;
		}
	}

	void AnimationComplete()
	{
		_isFading = false;
	}
}
