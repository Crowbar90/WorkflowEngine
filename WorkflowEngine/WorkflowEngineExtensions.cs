using System;
using System.Linq;

namespace WorkflowEngine
{
    public static class WorkflowEngineExtensions
    {
        public static bool BelongsToWorkflow(this State state, Workflow workflow) =>
            workflow.Transitions.Any(t => Equals(t.StartState, state) || Equals(t.EndState, state));

        public static bool HasNextStateInWorkflow(this State state, Workflow workflow)
        {
            if (!state.BelongsToWorkflow(workflow))
                throw new InvalidOperationException("State does not belong to workflow.");

            return workflow.Transitions.Any(t => t.StartState.Equals(state));
        }

        public static bool HasPreviousStateInWorkflow(this State state, Workflow workflow)
        {
            if (!state.BelongsToWorkflow(workflow))
            {
                throw new InvalidOperationException("State does not belong to workflow.");
            }

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
