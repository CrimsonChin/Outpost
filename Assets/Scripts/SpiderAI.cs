using UnityEngine;
using System.Collections.Generic;
using System;

public class SpiderAI : MonoBehaviour
{
    public float sightDistance = 3f;

    public Transform player;

    public SpiderState state;

    public float timeLooking = 6f;
    public float timeSpentLooking = 0;
    public float lookTimeInDirection = 1.5f;
    public float timeSpentLookingInDirection = 0;

    public float spideySenseRange = 0.75f;

    private Vector2 lookDirection;
    private Animator anim;

    public float followSpeed = 0.02f;

    public Vector2 roamingTarget = Vector2.zero;
    public Vector2 playerLastSightedLocation = Vector2.zero;

    private Color debugColor = Color.green;

    private float attackTime;

    public AudioClip[] OnDestroySounds;
    private bool _isShuttingDown = false;

    void Start()
    {
        anim = GetComponent<Animator>();

        player = GameObject.Find("Player").transform;

        DecideState();
    }

    void Update()
    {
        switch (state)
        {
            case SpiderState.Wait:
                debugColor = Color.green;
                Wait();
                break;
            case SpiderState.Alert:
                debugColor = Color.yellow;
                Alert();
                break;
            case SpiderState.Pursue:
                debugColor = Color.red;
                Persue();
                break;
            case SpiderState.Attack:
                debugColor = Color.red;
                Attack();
                break;
            default: //EnemyState.Roam
                debugColor = Color.green;
                Roam();
                break;
        }
    }
 
    private void NewRoam()
    {
        lookDirection = GetRandomDirection();
        SetAnimator();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDirection);
        if (!hit)
        {
            NewRoam();
        }
        else
        {
            roamingTarget = hit.point;
        }
    }

    private void Roam()
    {
        // Roam asserts
        // --------------------------------------------------------------------------------------------

        CanSensePlayer();

        // have we reached our destination?
        if (roamingTarget == Vector2.zero || Vector2.Distance(transform.position, roamingTarget) < 0.5)
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
            playerLastSightedLocation = player.transform.position;
            state = SpiderState.Pursue;
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, roamingTarget, followSpeed);
    }

    private void NewLookDirection()
    {
        Vector2 newDirection = GetRandomDirection();
        
        if (newDirection == lookDirection)
        {
            NewLookDirection();
        }

        lookDirection = GetRandomDirection();

        timeSpentLookingInDirection = lookTimeInDirection;

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
        // if we cants ee the enemy after x mins giveup
        timeSpentLooking -= Time.deltaTime;
        if (timeSpentLooking <= 0)
        {
            // Pick a new roaming direction and go there
            state = SpiderState.Roam;
            NewRoam();
        }

        // change the direction we're looking in
        if (timeSpentLookingInDirection >= 0)
        {
            timeSpentLookingInDirection -= Time.deltaTime;
        }
        else
        {
            NewLookDirection();
        }

        // can we see the player?
        bool isPlayerVisible = LookForPlayer();
        if (isPlayerVisible)
        {
            playerLastSightedLocation = player.transform.position;
            state = SpiderState.Pursue;
            return;
        }
    }

    private void Attack()
    {
        if (player == null)
        {
            anim.SetBool("isAttacking", false);
            DecideState();
        }

        if (attackTime <= 0)
        {
            DamagePlayer();
            attackTime += 30;
        }
        else
        {
            attackTime--;
        }
    }

    private void DamagePlayer()
    {
        Destroyable destroyablePlayer = player.GetComponent<Destroyable>();
        if (destroyablePlayer != null)
        {
            destroyablePlayer.Damage(1);
        }

        if (player == null)
        {
            DecideState();
        }
    }

    private void Persue()
    {
         bool isPlayerVisible = LookForPlayer();
        if (isPlayerVisible)
        {
            playerLastSightedLocation = player.transform.position;
        }

        Vector2 distance = transform.position - player.transform.position;
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            lookDirection = distance.x > 0 ? Vector2.left : Vector2.right;
        }
        else
        {
            lookDirection = distance.y > 0 ? Vector2.down : Vector2.up;
        }

        transform.position = Vector3.MoveTowards(transform.position, playerLastSightedLocation, followSpeed * 2f);
        SetAnimator();

        // Arrived at target but the player isn't there!
        if (Vector2.Distance(transform.position, playerLastSightedLocation) <= 0)
        {
            timeSpentLooking = timeLooking;
            state = SpiderState.Alert;
            return;
        }

        CanSensePlayer();
    }

    private void Wait()
    {
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isAttacking", true);
            state = SpiderState.Attack;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isAttacking", false);
            DecideState();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere(transform.position, spideySenseRange);
    }

    private void DecideState()
    {
        state = SpiderState.Roam;
    }

    private void LookAtPlayer()
    {
        float x = player.transform.position.x - transform.position.x;
        float y = player.transform.position.y - transform.position.y;

        lookDirection = new Vector2(x, y);
        SetAnimator();
    }

    private bool LookForPlayer()
    {
        if (player == null)
        {
            return false;
        }
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, 15) * lookDirection) * (sightDistance - 2), debugColor);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, -15) * lookDirection) * (sightDistance - 2), debugColor);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, 10) * lookDirection) * (sightDistance - 1), debugColor);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, -10) * lookDirection) * (sightDistance - 1), debugColor);
        Debug.DrawRay(transform.position, lookDirection * sightDistance, debugColor);

        // could use a circle cast?
        RaycastHit2D hitCentre = Physics2D.Raycast(transform.position, lookDirection, sightDistance);
        RaycastHit2D hitLeft20 = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, 15) * lookDirection, sightDistance - 1);
        RaycastHit2D hitRight20 = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -15) * lookDirection, sightDistance - 1);
        RaycastHit2D hitLeft45 = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, 10) * lookDirection, sightDistance - 2);
        RaycastHit2D hitRight45 = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -10) * lookDirection, sightDistance - 2);

        IList<RaycastHit2D> hits = new List<RaycastHit2D>
        {
            hitCentre, hitLeft20, hitRight20, hitLeft45, hitRight45
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
        return hit && hit.transform == player.transform;
    }

    private void SetAnimator()
    {
        if (anim == null)
        {
            return;
        }
        anim.SetFloat("inputX", lookDirection.x);
        anim.SetFloat("inputY", lookDirection.y);
    }

    private void CanSensePlayer()
    {
        // Can sense player
        if (player == null)
        {
            return;
        }

        if (Vector2.Distance(transform.position, player.transform.position) < spideySenseRange)
        {
            state = SpiderState.Pursue;
            return;
        }
    }

    void OnDestroy()
    {
        if (_isShuttingDown = !Application.isLoadingLevel)
        {
            AudioManager.Instance.PlayRandomSound(OnDestroySounds);
        }        
    }

    void OnApplicationQuit()
    {
        _isShuttingDown = true;
    }
}
