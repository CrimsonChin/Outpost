using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float Speed;
    Camera cam;

    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        cam.orthographicSize = (Screen.height / 100f) / 2f;

        if (Target)
        {
            transform.position = Vector3.Lerp(transform.position, Target.position, Speed) + new Vector3(0, 0, -10f);
        }
	}
}
