using System;

namespace WorkflowEngine
{
    public class StateTransitionBase : IEquatable<StateTransitionBase>
    {
        public State StartState { get; }
        public State EndState { get; }

        public StateTransitionBase(State startState, State endState)
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

        /// <inheritdoc />
        public bool Equals(StateTransitionBase other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(StartState, other.StartState) && Equals(EndState, other.EndState);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return GetType().IsInstanceOfType(obj) && Equals((StateTransitionBase) obj);
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
