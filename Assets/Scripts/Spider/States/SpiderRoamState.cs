using UnityEngine;

class SpiderRoamState : State
{
    private Vector2 _roamTarget;
    private readonly SpiderController _controller;
    private const float RoamSpeed = 0.01f;

    public SpiderRoamState(SpiderController controller)
    {
        _roamTarget = Vector2.zero;
        _controller = controller;

        AddTransition(Transition.CanSensePlayer, StateId.PersuePlayer);

        AddTransition(Transition.PlayerSighted, StateId.PersuePlayer);

        // set externally
        //AddTransition(Transition.CollidedWithPlayer, StateId.Attack);
    }

    public override StateId StateId
    {
        get { return StateId.Roam; }
    }

    public override void DoBeforeEnter()
    {
        _roamTarget = Vector2.zero;
    }

    public override void Act(GameObject self, GameObject player)
    {
        if (_roamTarget == Vector2.zero || Vector2.Distance(self.transform.position, _roamTarget) < 0.5)
        {
            NewRoam(self);
        }

        self.transform.position = Vector3.MoveTowards(self.transform.position, _roamTarget, RoamSpeed);
    }

    public override void Reason(GameObject self, GameObject player)
    {
        if (_controller.CheckIfPlayerIsVisible())
        {
            Debug.Log("Spider Roam: I can see the player");
            _controller.PerformTransition(Transition.PlayerSighted);
            return;
        }

        if (_controller.CanSensePlayer())
        {
            Debug.Log("Spider Roam: I can sense the player");
            _controller.PerformTransition(Transition.CanSensePlayer);
            return;
        }
    }

    private void NewRoam(GameObject self)
    {
        // TODO get a new direction
        // raycast in that direction
        // change look direction
        // move
        _controller.ChangeLookDirection();

        var hit = Physics2D.Raycast(self.transform.position, _controller.LookDirection);

        _roamTarget = hit.point;
    }
}

