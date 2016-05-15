using System.Collections.Generic;
using UnityEngine;

    class FiniteStateMachine
    {
        #region Fields

        IList<State> _states;

        #endregion

        #region Ctor

        public FiniteStateMachine()
        {
            _states = new List<State>();
        }

        #endregion

        #region Members

        public State CurrentState { get; private set; }

        #endregion

        #region Methods

        public void AddState(State state)
        {
            if (_states.Contains(state))
            {
                Debug.Log("State Machine already contains the state " + state.StateId);
                return;
            }

            if (_states.Count == 0)
            {
                CurrentState = state;
            }

            _states.Add(state);
        }

        public void DeleteState(State state)
        {
            if (!_states.Contains(state))
            {
                Debug.Log("State " + state.StateId + " not found.");
                return;
            }

            _states.Remove(state);
        }

        public void PerformTransition(Transition transition)
        {
            StateId outputState = CurrentState.GetOutputStateId(transition);
            if (outputState == StateId.NullState)
            {
                Debug.Log("No output state found for transition " + transition + " on state " + CurrentState.StateId);
            }

            foreach (State state in _states)
            {
                if (state.StateId == outputState)
                {
                    CurrentState.DoBeforeExit();

                    CurrentState = state;

                    CurrentState.DoBeforeEnter();
                    break;
                }
            }
        }

        #endregion
    }