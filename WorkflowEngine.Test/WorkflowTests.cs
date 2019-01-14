using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace WorkflowEngine.Test
{
    public class WorkflowTests
    {
        private readonly Workflow _workflow;

        public WorkflowTests()
        {
            _workflow = TestDummies.DummyWorkflow;
        }

        [Fact]
        public void OnInitializationCurrentStateIsStartingState()
        {
            _workflow.CurrentState.ShouldBe(TestDummies.DummyStates["INPUTSTATE"]);
        }

        [Theory]
        [ClassData(typeof(AvailableTransitionsTestDataGenerator))]
        public void AvailableTransitionsAreCorrect(State startState, IEnumerable<StateTransitionBase> expectedTransitions)
        {
            _workflow.AvailableTransitionsFromState[startState].ShouldBe(expectedTransitions, true);
        }
    }
}
