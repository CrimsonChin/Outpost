using UnityEngine;
using System.Collections.Generic;
using System;

public class SpiderAI : MonoBehaviour
{
    public float SightDistance = 3f;

    public Transform Player;

    public SpiderState State;

    public float TimeLooking = 6f;
    public float TimeSpentLooking;
    public float LookTimeInDirection = 1.5f;
    public float TimeSpentLookingInDirection;

    public float SpideySenseRange = 0.75f;

    private Vector2 _lookDirection;
    private Animator _animator;

    public float FollowSpeed = 0.02f;

    public Vector2 RoamingTarget = Vector2.zero;
    public Vector2 PlayerLastSightedLocation = Vector2.zero;

    private Color _debugColor = Color.green;

    private float _attackTime;

    public AudioClip[] OnDestroySounds;

    private bool _isShuttingDown;

    public void Start()
    {
        _animator = GetComponent<Animator>();

        Player = GameObject.Find("Player").transform;

        DecideState();
    }

    public void Update()
    {
        switch (State)
        {
            case SpiderState.Wait:
                _debugColor = Color.green;
                Wait();
                break;
            case SpiderState.Alert:
                _debugColor = Color.yellow;
                Alert();
                break;
            case SpiderState.Pursue:
                _debugColor = Color.red;
                Persue();
                break;
            case SpiderState.Attack:
                _debugColor = Color.red;
                Attack();
                break;
            default: //EnemyState.Roam
                _debugColor = Color.green;
                Roam();
                break;
        }
    }

    private void NewRoam()
    {
        _lookDirection = GetRandomDirection();
        SetAnimator();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _lookDirection);

        RoamingTarget = hit.point;
    }

    private void Roam()
    {
        // Roam asserts
        // --------------------------------------------------------------------------------------------

        CanSensePlayer();

        // have we reached our destination?
        if (RoamingTarget == Vector2.zero || Vector2.Distance(transform.position, RoamingTarget) < 0.5)
        {
            NewRoam();
            return;
        }

        // Roam act
        // --------------------------------------------------------------------------------------------
        // Player sighted
        bool isPlayerVisible = LookForPlayer();
        if (isPlayerVisible)
        {
            PlayerLastSightedLocation = Player.transform.position;
            State = SpiderState.Pursue;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, RoamingTarget, FollowSpeed);
    }

    private void NewLookDirection()
    {
        Vector2 newDirection = GetRandomDirection();

        if (newDirection == _lookDirection)
        {
            NewLookDirection();
        }

        _lookDirection = GetRandomDirection();

        TimeSpentLookingInDirection = LookTimeInDirection;

        SetAnimator();
    }

    private Vector2 GetRandomDirection()
    {
        int tick = (int)DateTime.UtcNow.Ticks;
        System.Random rand = new System.Random(tick);

        Vector2 direction = new Vector2();
        switch (rand.Next(0, 4))
        {
            case 0:
                direction = Vector2.up;
                break;
            case 1:
                direction = Vector2.down;
                break;
            case 2:
                direction = Vector2.left;
                break;
            case 3:
                direction = Vector2.right;
                break;
        }

        return direction;
    }

    private void Alert()
    {
        // before enter transition
        // setup all the timers
        CanSensePlayer();

        // how long have we spent looking?
        // if we cant see the enemy after x mins giveup
        TimeSpentLooking -= Time.deltaTime;
        if (TimeSpentLooking <= 0)
        {
            // Pick a new roaming direction and go there
            State = SpiderState.Roam;
            NewRoam();
        }

        // change the direction we're looking in
        if (TimeSpentLookingInDirection >= 0)
        {
            TimeSpentLookingInDirection -= Time.deltaTime;
        }
        else
        {
            NewLookDirection();
        }

        // can we see the player?
        bool isPlayerVisible = LookForPlayer();
        if (isPlayerVisible)
        {
            PlayerLastSightedLocation = Player.transform.position;
            State = SpiderState.Pursue;
        }
    }

    private void Attack()
    {
        if (Player == null)
        {
            _animator.SetBool("isAttacking", false);
            DecideState();
        }

        if (_attackTime <= 0)
        {
            DamagePlayer();
            _attackTime += 30;
        }
        else
        {
            _attackTime--;
        }
    }

    private void DamagePlayer()
    {
        Destroyable destroyablePlayer = Player.GetComponent<Destroyable>();
        if (destroyablePlayer != null)
        {
            destroyablePlayer.Damage(1);
        }

        if (Player == null)
        {
            DecideState();
        }
    }

    private void Persue()
    {
        bool isPlayerVisible = LookForPlayer();
        if (isPlayerVisible)
        {
            PlayerLastSightedLocation = Player.transform.position;
        }

        Vector2 distance = transform.position - Player.transform.position;
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            _lookDirection = distance.x > 0 ? Vector2.left : Vector2.right;
        }
        else
        {
            _lookDirection = distance.y > 0 ? Vector2.down : Vector2.up;
        }

        transform.position = Vector3.MoveTowards(transform.position, PlayerLastSightedLocation, FollowSpeed * 2f);
        SetAnimator();

        // Arrived at target but the player isn't there!
        if (Vector2.Distance(transform.position, PlayerLastSightedLocation) <= 0)
        {
            TimeSpentLooking = TimeLooking;
            State = SpiderState.Alert;
            return;
        }

        CanSensePlayer();
    }

    private void Wait()
    {
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _animator.SetBool("isAttacking", true);
            State = SpiderState.Attack;
        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _animator.SetBool("isAttacking", false);
            DecideState();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = _debugColor;
        Gizmos.DrawWireSphere(transform.position, SpideySenseRange);
    }

    private void DecideState()
    {
        State = SpiderState.Roam;
    }


    private bool LookForPlayer()
    {
        if (Player == null)
        {
            return false;
        }

        /*
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, 15) * _lookDirection) * (SightDistance - 2), _debugColor);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, -15) * _lookDirection) * (SightDistance - 2), _debugColor);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, 10) * _lookDirection) * (SightDistance - 1), _debugColor);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, -10) * _lookDirection) * (SightDistance - 1), _debugColor);
        Debug.DrawRay(transform.position, _lookDirection * SightDistance, _debugColor);
        */
        Debug.DrawRay(transform.position, _lookDirection * SightDistance, _debugColor);

        // could use a circle cast?
        RaycastHit2D hitCentre = Physics2D.Raycast(transform.position, _lookDirection, SightDistance);

        /*
        RaycastHit2D hitLeft20 = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, 15) * _lookDirection, SightDistance - 1);
        RaycastHit2D hitRight20 = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -15) * _lookDirection, SightDistance - 1);
        RaycastHit2D hitLeft45 = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, 10) * _lookDirection, SightDistance - 2);
        RaycastHit2D hitRight45 = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -10) * _lookDirection, SightDistance - 2);
        */
        RaycastHit2D[] hits = {
            hitCentre //, hitLeft20, hitRight20, hitLeft45, hitRight45
        };

        return ContainsPlayerHit(hits);
    }

    private bool ContainsPlayerHit(IEnumerable<RaycastHit2D> hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (HitPlayer(hit))
            {
                return true;
            }
        }

        return false;
    }

    private bool HitPlayer(RaycastHit2D hit)
    {
        return hit && hit.transform == Player.transform;
    }

    private void SetAnimator()
    {
        if (_animator == null)
        {
            return;
        }
        _animator.SetFloat("inputX", _lookDirection.x);
        _animator.SetFloat("inputY", _lookDirection.y);
    }

    private void CanSensePlayer()
    {
        // Can sense player
        if (Player == null)
        {
            return;
        }

        if (Vector2.Distance(transform.position, Player.transform.position) < SpideySenseRange)
        {
            State = SpiderState.Pursue;
        }
    }

    public void OnDestroy()
    {
        if (!_isShuttingDown)
        {
            AudioManager.Instance.PlayRandomSound(OnDestroySounds);
        }
    }

    public void OnApplicationQuit()
    {
        _isShuttingDown = true;
    }
}
