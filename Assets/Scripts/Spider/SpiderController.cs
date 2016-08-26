using System;
using System.Collections.Generic;
using Assets.Scripts.Spider.States;
using UnityEngine;
using UnityEngine.VR;

public class SpiderController : MonoBehaviour
{
    #region Fields

    public GameObject Player;

    public float Speed = 0.002f;
    public float SenseRadius = 0.75f;
    public float AttackRadius = 0.3f;
    public float ViewRadius = 0.5f;

    private FiniteStateMachine _fsm;
    private Animator _animator;
    private Vector2 _lookingDirection;

    private const float SightDistance = 1.5f;

    public Vector2 LookDirection
    {
        get { return _lookingDirection; }
        set
        {
            _lookingDirection = value;
            SetAnimator(_lookingDirection);
        }
    }

    public Vector2 PlayerLastSightedLocation { get; set; }

    public Color DebugColor { get; set; }

    #endregion

    public void Awake()
    {
        DebugColor = Color.red;
    }

    public void Start()
    {
        _animator = GetComponent<Animator>();
        SetupStateMachine();
    }

    public void Update()
    {
        _fsm.CurrentState.Reason(gameObject, Player);
        _fsm.CurrentState.Act(gameObject, Player);
    }

    private void SetupStateMachine()
    {
        var roam = new SpiderRoamState(this);
        var alert = new SpiderAlertState(this);
        //var attack = new SpiderAttackState(this, _animator);
        var persue = new SpiderPersueState(this);

        _fsm = new FiniteStateMachine();
        var states = new State[] { roam, persue, alert };
        _fsm.AddStates(states);
    }

    public bool CanSensePlayer()
    {
        return Vector2.Distance(transform.position, Player.transform.position) < SenseRadius;
    }

    public bool IsPlayerWithinAttackingDistance()
    {
        return Vector2.Distance(transform.position, Player.transform.position) < AttackRadius;
    }

    public void ChangeLookDirection()
    {
        LookDirection = GetNewRandomDirection(LookDirection);
    }

    private void SetAnimator(Vector2 vector2)
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
        Debug.DrawRay(transform.position, LookDirection * SightDistance, Color.red);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, ViewRadius, LookDirection, SightDistance);

        return hit && hit.transform == Player.transform;
    }


    private Vector2 GetNewRandomDirection(Vector2 previousDirection)
    {
        var tick = (int)DateTime.UtcNow.Ticks;
        var random = new System.Random(tick);

        var possibleDirections = new List<Vector2>
        {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

        possibleDirections.Remove(previousDirection);

        var randomIndex = random.Next(0, possibleDirections.Count);
        return possibleDirections[randomIndex];
    }

    public void PerformTransition(Transition transition)
    {
        _fsm.PerformTransition(transition);
    }

    //public void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        _animator.SetBool("isAttacking", true);
    //        _fsm.PerformTransition(Transition.CollidedWithPlayer);
    //    }
    //}

    //public void OnCollisionExit2D(Collision2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        _animator.SetBool("isAttacking", false);
    //        _fsm.PerformTransition(Transition.PlayerEscaped);
    //    }
    //}

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, SenseRadius); 

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ViewRadius); 

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRadius); 
    }
}
