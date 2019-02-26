using System.Collections;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace WorkflowEngine.Test
{
    public class StateTests
    {
        private static readonly State StateAImplicitFalse = new State("CODEA");
        private static readonly State StateAExplicitFalse = new State("CODEA", false);
        private static readonly State StateAExplicitTrue = new State("CODEA", true);
        private static readonly State StateBImplicitFalse = new State("CODEB");
        private static readonly State StateBExplicitFalse = new State("CODEB", false);
        private static readonly State StateBExplicitTrue = new State("CODEB", true);

        [Theory]
        [ClassData(typeof(NotStartStatesGenerator))]
        public void IsStartStateIsFalse(State state)
        {
            state.IsStartState.ShouldBe(false);
        }

        [Theory]
        [ClassData(typeof(StartStatesGenerator))]
        public void IsStartStateIsTrue(State state)
        {
            state.IsStartState.ShouldBe(true);
        }

        [Theory]
        [ClassData(typeof(EqualStatesGenerator))]
        public void SameCodeStatesAreEqual(State stateA, State stateB)
        {
            stateA.ShouldBe(stateB);
            stateA.GetHashCode().ShouldBe(stateB.GetHashCode());
            stateA.Equals(stateB).ShouldBe(true);
        }

        [Theory]
        [ClassData(typeof(NotEqualStatesGenerator))]
        public void DifferentCodeStatesAreNotEqual(State stateA, State stateB)
        {
            stateA.ShouldNotBe(stateB);
            stateA.Equals(stateB).ShouldBe(false);
        }

        [Theory]
        [ClassData(typeof(AllStatesGenerator))]
        public void SameReferenceStatesAreEqual(State stateA)
        {
            stateA.ShouldBe(stateA);
            stateA.Equals((object)stateA).ShouldBe(true);
        }

        [Theory]
        [ClassData(typeof(AllStatesGenerator))]
        public void StatesAreNotEqualToNull(State state)
        {
            // ReSharper disable once RedundantCast
            state.Equals((State)null).ShouldBe(false);
            state.Equals((object)null).ShouldBe(false);
        }

        #region Test data generators
        
        public class AllStatesGenerator : IEnumerable<object[]>
        {
            /// <inheritdoc />
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {StateAImplicitFalse};
                yield return new object[] {StateAExplicitFalse};
                yield return new object[] {StateAExplicitTrue};
                yield return new object[] {StateBImplicitFalse};
                yield return new object[] {StateBExplicitFalse};
                yield return new object[] {StateBExplicitTrue};
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class NotStartStatesGenerator : IEnumerable<object[]>
        {
            /// <inheritdoc />
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {StateAImplicitFalse};
                yield return new object[] {StateAExplicitFalse};
                yield return new object[] {StateBImplicitFalse};
                yield return new object[] {StateBExplicitFalse};
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class StartStatesGenerator : IEnumerable<object[]>
        {
            /// <inheritdoc />
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {StateAExplicitTrue};
                yield return new object[] {StateBExplicitTrue};
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class EqualStatesGenerator : IEnumerable<object[]>
        {
            /// <inheritdoc />
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {StateAImplicitFalse, StateAExplicitFalse};
                yield return new object[] {StateAImplicitFalse, StateAExplicitTrue};
                yield return new object[] {StateAExplicitFalse, StateAExplicitTrue};
                yield return new object[] {StateBImplicitFalse, StateBExplicitFalse};
                yield return new object[] {StateBImplicitFalse, StateBExplicitTrue};
                yield return new object[] {StateBExplicitFalse, StateBExplicitTrue};
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class NotEqualStatesGenerator : IEnumerable<object[]>
        {
            /// <inheritdoc />
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {StateAImplicitFalse, StateBImplicitFalse};
                yield return new object[] {StateAImplicitFalse, StateBImplicitFalse};
                yield return new object[] {StateAImplicitFalse, StateBExplicitFalse};
                yield return new object[] {StateAExplicitFalse, StateBImplicitFalse};
                yield return new object[] {StateAExplicitFalse, StateBImplicitFalse};
                yield return new object[] {StateAExplicitFalse, StateBExplicitFalse};
                yield return new object[] {StateAExplicitTrue, StateBImplicitFalse};
                yield return new object[] {StateAExplicitTrue, StateBImplicitFalse};
                yield return new object[] {StateAExplicitTrue, StateBExplicitFalse};
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion
    }
}
