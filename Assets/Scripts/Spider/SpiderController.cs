using System;
using System.Collections.Generic;
using Assets.Scripts.Spider.States;
using UnityEngine;
using UnityEngine.VR;
using Assets.Scripts.Enums;

public class SpiderController : MonoBehaviour
{
    #region Fields

    public GameObject Player;
    public float SightDistance = 2f;
    public float Speed = 0.002f;
    public float SenseRadius = 1f;
    public float ViewRadius = 0.5f;
    public float AttackRadius = 0.25f;

    private FiniteStateMachine _stateMachine;
    private Animator _animator;
    private Vector2 _facingDirection;

    public Vector2 FacingDirection
    {
        get { return _facingDirection; }
        set
        {
            _facingDirection = value;
            SetAnimator(_facingDirection);
        }
    }

    public Vector2 PlayerLastSightedLocation { get; set; }

    #endregion

    public void Awake()
    {
    }

    public void Start()
    {
        _animator = GetComponent<Animator>();
        SetupStateMachine();
    }

    public void Update()
    {
        _stateMachine.CurrentState.Reason(gameObject, Player);
        _stateMachine.CurrentState.Act(gameObject, Player);
    }

    private void SetupStateMachine()
    {
        var roam = new SpiderRoamState(this);
        var alert = new SpiderAlertState(this);
        var attack = new SpiderAttackState(this, _animator);
        var persue = new SpiderPersueState(this);

        _stateMachine = new FiniteStateMachine();
        var states = new State[] { roam, persue, alert, attack };
        _stateMachine.AddStates(states);
    }

    public bool CanSensePlayer()
    {
        var distance = Vector2.Distance(transform.position, Player.transform.position);
        return Math.Round(distance, 2) <= SenseRadius;
    }

    public void ChangeLookDirection()
    {
        FacingDirection = GetNewRandomDirection(FacingDirection);
    }

    public void SetAnimator(Vector2 vector2)
    {
        if (_animator == null)
        {
            return;
        }

        _animator.SetFloat("inputX", vector2.x);
        _animator.SetFloat("inputY", vector2.y);
    }

    public bool CheckIfPlayerIsVisible()
    {
        const float offset1 = 0.25f;
        const float offset2 = -0.25f;

        Vector3 ray1 = new Vector2(transform.position.x, transform.position.y);
        Vector3 ray2 = new Vector2(transform.position.x, transform.position.y);

        if (FacingDirection.x == 0)
        {
            ray1.x += offset1;
            ray2.x += offset2;
        }
        else
        {
            ray1.y += offset1;
            ray2.y += offset2;
        }

        var rayLength = FacingDirection * SightDistance;

        Debug.DrawRay(transform.position, FacingDirection * (SightDistance + 0.5f), Color.red);
        Debug.DrawRay(ray1, rayLength, Color.yellow);
        Debug.DrawRay(ray2, rayLength, Color.blue);

        const int playerLayerMask = 1 << (int)Layer.Player;
        var hit1 = Physics2D.Raycast(transform.position, FacingDirection, SightDistance + 0.5f, playerLayerMask);
        var hit2 = Physics2D.Raycast(ray1, FacingDirection, SightDistance, playerLayerMask);
        var hit3 = Physics2D.Raycast(ray2, FacingDirection, SightDistance, playerLayerMask);

        if (hit1.transform != null)
        {
            PlayerLastSightedLocation = hit1.transform.position;
            Debug.Log("HIT ONE");
            return true;
        }

        if (hit2.transform != null)
        {
            Debug.Log("HIT TWO");
            PlayerLastSightedLocation = hit2.transform.position;
            return true;
        }

        if (hit3.transform != null)
        {
            Debug.Log("HIT THREE");
            PlayerLastSightedLocation = hit3.transform.position;
            return true;
        }

        //var offset2 = 1.5f;
        //Debug.DrawRay(new Vector2(transform.position.x * offset2, transform.position.y), FacingDirection * (SightDistance + ViewRadius), Color.red);

        //RaycastHit2D ray = Physics2D.Raycast(transform.position, LookDirection, SightDistance, 9);
        //if (ray != null && ray.transform == Player.transform)
        //{
        //    Debug.Log("RAY HIT");
        //}

        // circle cast starts
        //RaycastHit2D hit = Physics2D.CircleCast(transform.position, ViewRadius, FacingDirection, SightDistance);

        //var hitPlayer = hit && hit.transform == Player.transform;
        //if (hitPlayer)
        //{
        //    PlayerLastSightedLocation = hit.transform.position;
        //}
        return false;
    }

    public bool CanAttackPlayer()
    {
        var col = Physics2D.OverlapCircle(transform.position, AttackRadius, 1 << (int)Layer.Player);
        return col != null && col.name == "Player";
    }

    private static Vector2 GetNewRandomDirection(Vector2 removeDirection)
    {
        var tick = (int)DateTime.UtcNow.Ticks;
        var random = new System.Random(tick);

        var possibleDirections = new List<Vector2>
        {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

        possibleDirections.Remove(removeDirection);

        var randomIndex = random.Next(0, possibleDirections.Count);
        return possibleDirections[randomIndex];
    }

    public void PerformTransition(Transition transition)
    {
        _stateMachine.PerformTransition(transition);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        _stateMachine.CurrentState.OnCollisionEnter2D(other);
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        _stateMachine.CurrentState.OnCollisionExit2D(other);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, SenseRadius);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, ViewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}
