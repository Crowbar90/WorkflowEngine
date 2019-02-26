using Shouldly;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace WorkflowEngine.Test
{
    public class StateTransitionTests
    {
        private static readonly StateTransitionBase FirstDefaultStateTransition = new StateTransitionBase(TestDummies.DummyStates["INPUTSTATE"], TestDummies.DummyStates["MIDSTATE_A"]);
        private static readonly StateTransitionBase FirstPassingStateTransition = new DummyTransition(TestDummies.DummyStates["INPUTSTATE"], TestDummies.DummyStates["MIDSTATE_A"], true);
        private static readonly StateTransitionBase FirstFailingStateTransition = new DummyTransition(TestDummies.DummyStates["INPUTSTATE"], TestDummies.DummyStates["MIDSTATE_A"], false);

        [Theory]
        [ClassData(typeof(PassingTransitionsGenerator))]
        public void PerformableTransitionDoesNotThrowExceptions(StateTransitionBase transition)
        {
            Should.NotThrow(() => transition.Perform());
        }

        [Theory]
        [ClassData(typeof(FailingTransitionsGenerator))]
        public void NotPerformableTransitionThrowsException(StateTransitionBase transition)
        {
            Should.Throw<InvalidOperationException>(() => transition.Perform(), () => "Transition cannot be performed: ERROR");
        }

        [Theory]
        [ClassData(typeof(EqualTransitionsGenerator))]
        public void SameStatesTransitionsAreEqual(StateTransitionBase transitionA, StateTransitionBase transitionB)
        {
            transitionA.ShouldBe(transitionB);
            transitionA.Equals((object)transitionB).ShouldBe(true);
        }

        [Theory]
        [ClassData(typeof(NotEqualTransitionsGenerator))]
        public void DifferentStatesTransitionsAreNotEqual(StateTransitionBase transitionA, StateTransitionBase transitionB)
        {
            transitionA.ShouldNotBe(transitionB);
            transitionA.Equals((object)transitionB).ShouldBe(false);
        }

        [Theory]
        [ClassData(typeof(AllTransitionsGenerator))]
        public void SameReferenceTransitionsAreEqual(StateTransitionBase transitionA)
        {
            transitionA.ShouldBe(transitionA);
            transitionA.Equals((object)transitionA).ShouldBe(true);
        }

        [Theory]
        [ClassData(typeof(AllTransitionsGenerator))]
        public void TransitionsAreNotEqualToNull(StateTransitionBase transition)
        {
            // ReSharper disable once RedundantCast
            transition.Equals((StateTransitionBase)null).ShouldBe(false);
            transition.Equals((object)null).ShouldBe(false);
        }


        #region Test data generators

        public class AllTransitionsGenerator : IEnumerable<object[]>
        {
            /// <inheritdoc />
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { FirstDefaultStateTransition };
                yield return new object[] { FirstPassingStateTransition };
                yield return new object[] { FirstFailingStateTransition };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class EqualTransitionsGenerator : IEnumerable<object[]>
        {
            /// <inheritdoc />
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { FirstDefaultStateTransition, FirstPassingStateTransition };
                yield return new object[] { FirstDefaultStateTransition, FirstFailingStateTransition };
                yield return new object[] { FirstPassingStateTransition, FirstFailingStateTransition };
                yield return new object[] { FirstDefaultStateTransition, TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { FirstPassingStateTransition, TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { FirstFailingStateTransition, TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"], TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"], TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"], TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"], TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class NotEqualTransitionsGenerator : IEnumerable<object[]>
        {
            /// <inheritdoc />
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { FirstDefaultStateTransition, TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { FirstDefaultStateTransition, TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { FirstDefaultStateTransition, TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
                yield return new object[] { FirstDefaultStateTransition, TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
                yield return new object[] { FirstDefaultStateTransition, TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
                yield return new object[] { FirstPassingStateTransition, TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { FirstPassingStateTransition, TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { FirstPassingStateTransition, TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
                yield return new object[] { FirstPassingStateTransition, TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
                yield return new object[] { FirstPassingStateTransition, TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
                yield return new object[] { FirstFailingStateTransition, TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { FirstFailingStateTransition, TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { FirstFailingStateTransition, TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
                yield return new object[] { FirstFailingStateTransition, TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
                yield return new object[] { FirstFailingStateTransition, TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"], TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"], TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"], TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"], TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"], TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"], TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"], TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"], TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"], TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"], TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"], TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"], TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"], TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"], TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"], TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"], TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"], TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"], TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"], TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"], TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"], TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class FailingTransitionsGenerator : IEnumerable<object[]>
        {
            /// <inheritdoc />
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { FirstFailingStateTransition };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_ENDSTATE_B"] };
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class PassingTransitionsGenerator : IEnumerable<object[]>
        {
            /// <inheritdoc />
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { FirstDefaultStateTransition };
                yield return new object[] { FirstPassingStateTransition };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["INPUTSTATE_MIDSTATE_B"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_ENDSTATE_A"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_A_INPUTSTATE"] };
                yield return new object[] { TestDummies.DummyTransitions["MIDSTATE_B_INPUTSTATE"] };
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        #endregion
    }
}
