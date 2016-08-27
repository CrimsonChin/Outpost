using System.Collections.Generic;
using UnityEngine;

abstract class State
{
    #region Fields

    protected IDictionary<Transition, StateId> _map;

    #endregion

    #region Ctor

    public State()
    {
        _map = new Dictionary<Transition, StateId>();
    }

    #endregion

    #region Members

    public abstract StateId StateId { get; }

    #endregion

    #region Methods

    public void AddTransition(Transition transition, StateId state)
    {
        if (_map.ContainsKey(transition))
        {
            Debug.Log("FSMState Error: State " + state + " already has transition " + transition);
            return;
        }

        _map.Add(transition, state);
    }

    public void RemoveTransiton(Transition transition)
    {
        if (_map.ContainsKey(transition))
        {
            _map.Remove(transition);
        }
    }

    public StateId GetOutputStateId(Transition transition)
    {
        if (_map.ContainsKey(transition))
        {
            return _map[transition];
        }

        return StateId.NullState;
    }

    #endregion

    #region Virtual Methods

    public virtual void DoBeforeEnter()
    {
    }

    public virtual void DoBeforeExit()
    {
    }

    public virtual void OnCollisionEnter2D(Collision2D other) { }

    public virtual void OnCollisionExit2D(Collision2D other) { }

    #endregion

    #region Abstract Methods 

    public abstract void Reason(GameObject self, GameObject player);

    public abstract void Act(GameObject self, GameObject player);

    #endregion
}