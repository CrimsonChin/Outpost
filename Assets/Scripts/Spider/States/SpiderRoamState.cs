using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class SpiderRoamState : State
{
    Vector2 _roamTarget;
    private SpiderController _controller;
    private Animator _anim;

    public SpiderRoamState(SpiderController controller, Animator animator)
    {
        _roamTarget = Vector2.zero;
        _controller = controller;
        _anim = animator;
    }

    public override StateId StateId
    {
        get
        {
            return StateId.Roam;
        }
    }

    private void NewRoam(GameObject self)
    {
        Vector2 newPosition = UnityEngine.Random.insideUnitCircle * 10;

        Debug.DrawLine(self.transform.position, newPosition, Color.black);
        RaycastHit2D hit = Physics2D.Linecast(self.transform.position, new Vector2(newPosition.x, newPosition.y));
        if (!hit)
        {
            NewRoam(self);
        }
        else
        {
            _roamTarget = hit.point;
        }
    }

    private void SetAnimator()
    {
        _anim.SetFloat("inputX", _roamTarget.x);
        _anim.SetFloat("inputY", _roamTarget.y);
    }

    public override void Act(GameObject self, GameObject player)
    {
        Debug.DrawLine(self.transform.position, _roamTarget);
        if (_roamTarget == Vector2.zero || Vector2.Distance(self.transform.position, _roamTarget) < 1)
        {
            NewRoam(self);
            return;
        }

        SetAnimator();
        self.transform.position = Vector3.MoveTowards(self.transform.position, _roamTarget, _controller.Speed);

        ////RaycastHit2D hit = LookAhead();
        //// if (IsEnemyInLineOfSight(hit))
        //// {
        ////     isEnemySighted = true;
        //// }
    }

    public override void Asses(GameObject self, GameObject player)
    {
        if (Vector2.Distance(self.transform.position, player.transform.position) < 1.5)
        {
            _controller.SetTransition(Transition.SensePlayer);
            return;
        }
    }
}

