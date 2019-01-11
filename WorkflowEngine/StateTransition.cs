using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowEngine
{
    public abstract class StateTransitionBase
    {
        public State StartState { get; set; }
        public State EndState { get; set; }
        protected abstract bool CanBePerformed(out string errorMessage);
        protected abstract void InnerPerform();

        public void Perform()
        {
            if (!CanBePerformed(out var error))
                throw new InvalidOperationException($"Transition cannot be performed: {error}");

            InnerPerform();
        }
    }
}
