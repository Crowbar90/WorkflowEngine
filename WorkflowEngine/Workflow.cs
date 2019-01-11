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
                throw new InvalidOperationException("Invalid next state.");

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

    public static class WorkflowStateExtensions
    {
        public static bool BelongsToWorkflow(this State state, Workflow workflow) =>
            workflow.Transitions.Any(t => Equals(t.StartState, state) || Equals(t.EndState, state));

        public static bool HasNextStateInWorkflow(this State state, Workflow workflow)
        {
            if (!state.BelongsToWorkflow(workflow))
                throw new InvalidOperationException("State does not belong to workflow.");

            return workflow.Transitions.Any(t => Equals(t.StartState, state));
        }

        public static bool HasPreviousStateInWorkflow(this State state, Workflow workflow)
        {
            if (!state.BelongsToWorkflow(workflow))
                throw new InvalidOperationException("State does not belong to workflow.");

            return workflow.Transitions.Any(t => Equals(t.EndState, state));
        }

        public static bool IsEndStateInWorkflow(this State state, Workflow workflow)
        {
            return !state.HasNextStateInWorkflow(workflow) && state.HasPreviousStateInWorkflow(workflow);
        }

        public static State StartingState(this Workflow workflow)
        {
            try
            {
                var result = workflow.Transitions
                    .Select(t => t.StartState)
                    .Distinct()
                    .SingleOrDefault(s => s.IsStartState);

                if (result is null)
                {
                    throw new InvalidOperationException("Workflow has no starting state.");
                }
                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Workflow has more than one starting state.");
            }
        }
    }
}
