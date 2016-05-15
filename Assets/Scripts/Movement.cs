using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour 
{
    public float speed = 1.5f;
	private Rigidbody2D _rigidbody;
	private Animator _anim;

    public Vector2 FacingDirection = Vector2.up;

	void Start () 
	{
		_rigidbody = GetComponent<Rigidbody2D> ();
		_anim = GetComponent<Animator>();
    }
	
	void Update () 
	{
        Vector2 movementVector = GetMovementVector();

        if (movementVector != Vector2.zero)
		{
            FacingDirection = movementVector;

            _anim.SetBool("isWalking", true);
			_anim.SetFloat("inputX", movementVector.x);
			_anim.SetFloat("inputY", movementVector.y);
		}
		else 
		{
			_anim.SetBool("isWalking", false);
		}

		_rigidbody.MovePosition(_rigidbody.position + movementVector * Time.deltaTime * speed);
	}

    private Vector2 GetMovementVector()
    {
        Vector2 movementVector = Vector2.zero;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x != 0)
        {
            movementVector = new Vector2(x, 0);
        }
        else if (y != 0)
        {
            movementVector = new Vector2(0, y);
        }

        return movementVector;
    }
}
