using System.Collections.Generic;
using UnityEngine;

class FiniteStateMachine
{
    #region Fields

    private readonly IDictionary<StateId, State> _stateByStateId;

    #endregion

    #region Ctor

    public FiniteStateMachine()
    {
        _stateByStateId = new Dictionary<StateId, State>();
    }

    #endregion

    #region Members

    public State CurrentState { get; private set; }

    #endregion

    #region Methods

    public void AddStates(IEnumerable<State> states)
    {
        foreach (var state in states)
        {
            AddState(state);
        }
    }

    public void AddState(State state)
    {
        if (_stateByStateId.ContainsKey(state.StateId))
        {
            Debug.Log("State Machine already contains the state " + state.StateId);
            return;
        }

        if (_stateByStateId.Count == 0)
        {
            CurrentState = state;
        }

        _stateByStateId.Add(state.StateId, state);
    }

    public void RemoveState(State state)
    {
        RemoveState(state.StateId);
    }

    public void RemoveState(StateId stateId)
    {
        if (!_stateByStateId.ContainsKey(stateId))
        {
            Debug.Log("State " + stateId + " not found.");
            return;
        }

        _stateByStateId.Remove(stateId);
    }

    public void PerformTransition(Transition transition)
    {
        StateId outputState = CurrentState.GetOutputStateId(transition);
        if (outputState == StateId.NullState)
        {
            Debug.Log("No output state found for transition " + transition + " on state " + CurrentState.StateId);
            return;
        }

        State state;
        if (!_stateByStateId.TryGetValue(outputState, out state))
        {
            Debug.Log("State not found for state " + CurrentState.StateId);
            return;
        }

        CurrentState.DoBeforeExit();
        CurrentState = state;
        CurrentState.DoBeforeEnter();
    }

    #endregion
}