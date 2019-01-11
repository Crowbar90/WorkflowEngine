using System;

namespace WorkflowEngine
{
    public class State : IEquatable<State>
    {
        public string Code { get; set; }
        public bool IsStartState { get; set; }

        public State(string code) : this(code, false) { }

        public State(string code, bool isStartState)
        {
            Code = code;
            IsStartState = isStartState;
        }

        /// <inheritdoc />
        public bool Equals(State other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Code, other.Code);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((State) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Code != null ? Code.GetHashCode() : 0;
        }
    }
}
