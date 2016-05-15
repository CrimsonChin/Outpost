using UnityEngine;
using System.Collections;

public class Warp : MonoBehaviour 
{
	public Transform WarpTarget;

	IEnumerator OnTriggerEnter2D(Collider2D other)
	{
        if (other.gameObject.tag == "Player")
        {
		    ScreenFader sf = GameObject.FindGameObjectWithTag ("Fader").GetComponent<ScreenFader> ();

		    yield return StartCoroutine (sf.FadeToBlack());

		    other.gameObject.transform.position = WarpTarget.position;
		    Camera.main.transform.position = WarpTarget.position;

		    yield return StartCoroutine (sf.FadeToClear());
        }
    }

	void OnDrawGizmos() 
	{
		if (WarpTarget != null) 
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, WarpTarget.transform.position);
		}
	}
}