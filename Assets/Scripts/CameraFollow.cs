using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float Speed;

    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        _camera.orthographicSize = (Screen.height / 100f) / 2f;

        if (Target)
        {
            transform.position = Vector3.Lerp(transform.position, Target.position, Speed) + new Vector3(0, 0, -10f);
        }
    }
}
