using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowEngine
{
    public class Workflow
    {
        private State _currentState;

        public State CurrentState => _currentState ?? this.StartingState();

        public IEnumerable<StateTransitionBase> Transitions { get; set; } = new List<StateTransitionBase>();

        public Dictionary<State, IEnumerable<StateTransitionBase>> AvailableTransitionsFromState =>
            Transitions.GroupBy(t => t.StartState).ToDictionary(g => g.Key, g => g.AsEnumerable());

        public IEnumerable<StateTransitionBase> AvailableTransitionsFromCurrentState =>
            CurrentState.HasNextStateInWorkflow(this)
                ? AvailableTransitionsFromState[CurrentState]
                : new StateTransitionBase[] { };

        public IEnumerable<State> PossibleNextStates =>
            AvailableTransitionsFromCurrentState.Select(t => t.EndState);

        public void MoveToState(State nextState)
        {
            var transition = AvailableTransitionsFromCurrentState.SingleOrDefault(t => Equals(t.EndState, nextState));

            if (transition is null)
            {
                throw new InvalidOperationException("Invalid next state.");
            }

            try
            {
                transition.Perform();
                _currentState = nextState;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Exception thrown while performing transition. See InnerException for more details.", e);
            }
        }
    }
}
