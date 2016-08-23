using System;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    #region Fields

    public GameObject Player;

    public float Speed = 0.02f;

    private FiniteStateMachine _fsm;
    private Animator _animator;

    #endregion

    // Use this for initialization
    public void Start ()
    {
        _animator = GetComponent<Animator>();
        SetupStateMachine();
	}

    // Update is called once per frame
    public void Update ()
    {
        _fsm.CurrentState.Asses(gameObject, Player);
        _fsm.CurrentState.Act(gameObject, Player);
	}

    private void SetupStateMachine()
    {
        State roam = new SpiderRoamState(this, _animator);
        roam.AddTransition(Transition.PlayerSighted, StateId.Persue);

        _fsm = new FiniteStateMachine();
        _fsm.AddState(roam);
    }

    public void SetTransition(Transition transition)
    {
        _fsm.PerformTransition(transition);
    }
}
