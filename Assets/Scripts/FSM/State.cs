using System.Collections.Generic;
using UnityEngine;

abstract class State
{
    protected IDictionary<Transition, StateId> _map;

    public State()
    {
        _map = new Dictionary<Transition, StateId>();
    }

    public abstract StateId StateId { get; }

    public void AddTransition(Transition transition, StateId stateId)
    {
        if (_map.ContainsKey(transition))
        {
            Debug.Log("State Error: State " + stateId + " already has transition " + transition);
            return;
        }

        _map.Add(transition, stateId);
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

    public virtual void DoBeforeEnter() { }

    public virtual void DoBeforeExit() { }

    public virtual void OnCollisionEnter2D(Collision2D other) { }

    public virtual void OnCollisionExit2D(Collision2D other) { }

    public abstract void Reason(GameObject self, GameObject player);

    public abstract void Act(GameObject self, GameObject player);
}