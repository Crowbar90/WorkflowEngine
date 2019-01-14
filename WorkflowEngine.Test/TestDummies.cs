using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowEngine.Test
{
    public static class TestDummies
    {
        public static Dictionary<string, State> DummyStates = new[]
            {
                new State("INPUTSTATE", true),
                new State("MIDSTATE_A"),
                new State("MIDSTATE_B"),
                new State("ENDSTATE_A"),
                new State("ENDSTATE_B"),
                new State("EXTSTATE")
            }
            .ToDictionary(s => s.Code);

        public static Dictionary<string, StateTransitionBase> DummyTransitions = new[]
            {
                new DummyTransition(DummyStates["INPUTSTATE"], DummyStates["MIDSTATE_A"], true),
                new DummyTransition(DummyStates["INPUTSTATE"], DummyStates["MIDSTATE_B"], true),
                new DummyTransition(DummyStates["MIDSTATE_A"], DummyStates["ENDSTATE_A"], true),
                new DummyTransition(DummyStates["MIDSTATE_A"], DummyStates["INPUTSTATE"], true),
                new DummyTransition(DummyStates["MIDSTATE_B"], DummyStates["ENDSTATE_B"], false),
                new DummyTransition(DummyStates["MIDSTATE_B"], DummyStates["INPUTSTATE"], true)
            }
            .ToDictionary(t => $"{t.StartState.Code}_{t.EndState.Code}", t => t as StateTransitionBase);

        public static Workflow DummyWorkflow = new Workflow
        {
            Transitions = DummyTransitions.Values
        };
    }

    internal class DummyTransition : StateTransitionBase
    {
        private readonly bool _passing;

        /// <inheritdoc />
        public DummyTransition(State startState, State endState, bool passing) : base(startState, endState)
        {
            _passing = passing;
        }

        protected override bool CanBePerformed(out string errorMessage)
        {
            errorMessage = _passing ? null : "ERROR";
            return _passing;
        }

        protected override void InnerPerform() { }
    }

    public class AvailableTransitionsTestDataGenerator : IEnumerable<object[]>
    {
        /// <inheritdoc />
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                TestDummies.DummyStates["INPUTSTATE"],
                new []
                {
                    TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"],
                    TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"]
                }
            };
            
            yield return new object[]
            {
                TestDummies.DummyStates["MIDSTATE_A"],
                new []
                {
                    TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"],
                    TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"]
                }
            };
            
            yield return new object[]
            {
                TestDummies.DummyStates["MIDSTATE_B"],
                new []
                {
                    TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"],
                    TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"]
                }
            };
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
