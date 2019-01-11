using System;

namespace WorkflowEngine
{
    public class State
    {
        public string Code { get; set; }
        public bool IsStartState { get; set; }

        public State(string code, bool isStartState = false)
        {
            Code = code;
            IsStartState = isStartState;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(State other)
        {
            return string.Equals(Code, other.Code);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (Code != null ? Code.GetHashCode() : 0);
        }
    }
}
