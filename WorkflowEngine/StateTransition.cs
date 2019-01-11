using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowEngine
{
    public abstract class StateTransitionBase
    {
        public State StartState { get; set; }
        public State EndState { get; set; }

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
    }
}
