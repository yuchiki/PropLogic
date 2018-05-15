namespace PropLogic.Tests {
    using System;
    using Xunit;
    using static TestUtil;

    public class PropositionTest {
        // This is public because of XUnit testing
        public static object[][] Props {
            get {
                var (a, b, c, d, e) = TupleMap(MakeVar, ("a", "b", "c", "d", "e"));
                return new[] {
                    new object[] { a * b * c * d * e, "a&b&c&d&e" }
                    , new object[] { a + b * c, "a|b&c" }
                    , new object[] { a ^ b ^ c, "(a->b)->c" }
                    , new object[] { a     ^ (b ^ c), "a->b->c" }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Props))]
        public void CreatePropositionCorrectly(Prop p, string s) => Assert.Equal(p.ToString(), s);
    }

    public static class TestUtil {
        public static PVar MakeVar(string s) => new PVar(s);

        public static (T, T)    TupleMap<TS, T>(Func<TS, T> f, (TS, TS)     s) => (f(s.Item1), f(s.Item2));
        public static (T, T, T) TupleMap<TS, T>(Func<TS, T> f, (TS, TS, TS) s) => (f(s.Item1), f(s.Item2), f(s.Item3));

        public static (T, T, T, T) TupleMap<TS, T>(Func<TS, T> f, (TS, TS, TS, TS) s) =>
            (f(s.Item1), f(s.Item2), f(s.Item3), f(s.Item4));

        public static (T, T, T, T, T) TupleMap<TS, T>(Func<TS, T> f, (TS, TS, TS, TS, TS) s) =>
            (f(s.Item1), f(s.Item2), f(s.Item3), f(s.Item4), f(s.Item5));

        public static (T, T, T, T, T, T) TupleMap<TS, T>(Func<TS, T> f, (TS, TS, TS, TS, TS, TS) s) =>
            (f(s.Item1), f(s.Item2), f(s.Item3), f(s.Item4), f(s.Item5), f(s.Item6));

        public static (T, T, T, T, T, T, T) TupleMap<TS, T>(Func<TS, T> f, (TS, TS, TS, TS, TS, TS, TS) s) => (
            f(s.Item1), f(s.Item2), f(s.Item3), f(s.Item4), f(s.Item5), f(s.Item6), f(s.Item7));

        public static (T, T, T, T, T, T, T, T) TupleMap<TS, T>(Func<TS, T> f, (TS, TS, TS, TS, TS, TS, TS, TS) s) => (
            f(s.Item1), f(s.Item2), f(s.Item3), f(s.Item4), f(s.Item5), f(s.Item6), f(s.Item7), f(s.Item8));
    }
}
