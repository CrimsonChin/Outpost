using UnityEngine;
using System.Collections;

public class DeadzoneCamera : MonoBehaviour 
{
	public Transform Target;

	public float Speed = 0.01f;

	private Camera _camera;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public float radius = 2;

    public float Top, Bottom, Left, Right;

	// Use this for initialization
	void Start() 
	{
		_camera = GetComponent<Camera> ();
        SetDeadzone();
    }
	
	// Update is called once per frame
	void Update() 
	{
        Vector3 pos = Target.transform.position;
        if (pos.y > Top || pos.y < Bottom || pos.x > Right || pos.y < Left)
        {
            SetDeadzone();
            StartCoroutine(MoveCamera(Target));          
        }
    }

    private void SetDeadzone()
    {
        Top = Target.transform.position.y + radius;
        Bottom = Target.transform.position.y - radius;
        Right = Target.transform.position.x + radius;
        Left = Target.transform.position.x - radius;
    }

    IEnumerator MoveCamera(Transform pos)
    {
        Vector3 targetPosition = pos.TransformPoint(new Vector3(0, 0, -10f));
        transform.position = targetPosition;
        yield return 0;
    }

    private void OldCode()
    {
        //_camera.orthographicSize = (Screen.height / 100f) / 4f;

        if (Target)
        {
            Vector3 targetPosition = Target.TransformPoint(new Vector3(0, 0, -10f));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            //transform.position = Vector3.Lerp(transform.position, Target.position, Speed) + new Vector3(0, 0, -10f);
        }
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(Target.transform.position, new Vector3(radius, radius));
    }
*/

}
