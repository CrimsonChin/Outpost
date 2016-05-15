using System;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    #region Fields

    public GameObject player;

    public float Speed = 0.02f;

    private FiniteStateMachine _fsm;
    private Animator _anim;

    #endregion

    // Use this for initialization
    void Start ()
    {
        _anim = GetComponent<Animator>();
        SetupStateMachine();
	}

    // Update is called once per frame
    void Update ()
    {
        _fsm.CurrentState.Asses(gameObject, player);
        _fsm.CurrentState.Act(gameObject, player);
	}

    private void SetupStateMachine()
    {
        State roam = new SpiderRoamState(this, _anim);
        roam.AddTransition(Transition.PlayerSighted, StateId.Persue);

        _fsm = new FiniteStateMachine();
        _fsm.AddState(roam);
    }

    public void SetTransition(Transition transition)
    {
        _fsm.PerformTransition(transition);
    }
}
