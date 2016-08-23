using System;
using UnityEngine;

public class Movement : MonoBehaviour 
{
    public float Speed = 1.5f;
	private Rigidbody2D _rigidbody;
	private Animator _animator;

    public Vector2 FacingDirection = Vector2.up;

    public void Start () 
	{
		_rigidbody = GetComponent<Rigidbody2D> ();
		_animator = GetComponent<Animator>();
    }

    public void Update () 
	{
        var movementVector = GetMovementVector();

        if (movementVector != Vector2.zero)
		{
            FacingDirection = movementVector;

            _animator.SetBool("isWalking", true);
			_animator.SetFloat("inputX", movementVector.x);
			_animator.SetFloat("inputY", movementVector.y);
		}
		else 
		{
			_animator.SetBool("isWalking", false);
		}

		_rigidbody.MovePosition(_rigidbody.position + movementVector * Time.deltaTime * Speed);
	}

    private static Vector2 GetMovementVector()
    {
        var x = Input.GetAxisRaw("Horizontal");
        if (Math.Abs(x) > 0)
        {
            return new Vector2(x, 0);
        }

        var y = Input.GetAxisRaw("Vertical");
        return Math.Abs(y) > 0 ? new Vector2(0, y) : Vector2.zero;
    }
}
