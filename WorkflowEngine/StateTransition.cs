using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowEngine
{
    public abstract class StateTransitionBase
    {
        public State StartState { get; }
        public State EndState { get; }

        protected StateTransitionBase(State startState, State endState)
        {
            StartState = startState;
            EndState = endState;
        }

        protected virtual bool CanBePerformed(out string errorMessage)
        {
            errorMessage = null;
            return true;
        }

        protected virtual void InnerPerform() { }

        public void Perform()
        {
            if (!CanBePerformed(out var error))
                throw new InvalidOperationException($"Transition cannot be performed: {error}");

            InnerPerform();
        }

        protected bool Equals(StateTransitionBase other)
        {
            return Equals(StartState, other.StartState) && Equals(EndState, other.EndState);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((StartState != null ? StartState.GetHashCode() : 0) * 397) ^ (EndState != null ? EndState.GetHashCode() : 0);
            }
        }
    }
}
