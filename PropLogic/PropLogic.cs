namespace PropLogic {
    using System;
    using Interpretation = System.Collections.Generic.HashSet<string>;
    using BinOp = System.Func<bool, bool, bool>;

    public abstract class Prop {
        public int AssocRank { get; private set; }

        protected Prop(int assocRank) => AssocRank = assocRank;

        public abstract bool Interpret(Interpretation interpretation);

        public static Prop operator +(Prop p1, Prop p2) => new POr(p1, p2);
        public static Prop operator *(Prop p1, Prop p2) => new PAnd(p1, p2);
        public static Prop operator ~(Prop p)           => new PNeg(p);
        public static Prop operator ^(Prop p1, Prop p2) => new PImpl(p1, p2);

        protected static string Paren(Prop p, bool isParened) => isParened ? $"({p})" : $"{p}";
    }

    public class PVar : Prop {
        private string Name { get; set; }
        public PVar(string name) : base(0) => Name = name;

        public override bool Interpret(Interpretation interpretation) => interpretation.Contains(Name);

        public override string ToString() => Name;
    }

    public class PNeg : Prop {
        private readonly Prop _p;
        public PNeg(Prop p) : base(1) => _p = p;

        public override bool Interpret(Interpretation interpretation) => !_p.Interpret(interpretation);

        public override string ToString() => $"!{Paren(_p, _p.AssocRank > AssocRank)}";
    }

    public abstract class BinProp : Prop {
        private readonly Prop   _left, _right;
        private readonly BinOp  _operation;
        private          bool   IsLeftAssoc { get; set; }
        private          string Symbol      { get; set; }

        protected BinProp(Prop left, Prop right, Func<bool, bool, bool> operation, string symbol, bool isLeftAssoc
            , int              assocRank) : base(assocRank) => (_left, _right, _operation, Symbol, IsLeftAssoc) =
            (left, right, operation, symbol, isLeftAssoc);

        public override bool Interpret(Interpretation interpretation) =>
            _operation(_left.Interpret(interpretation), _right.Interpret(interpretation));

        public override string ToString()           => $"{ParenLeft(_left)}{Symbol}{ParenRight(_right)}";
        private         string ParenLeft(Prop    p) => IsLeftAssoc ? AssocSide(p) : NonAssocSide(p);
        private         string ParenRight(Prop   p) => IsLeftAssoc ? NonAssocSide(p) : AssocSide(p);
        private         string AssocSide(Prop    p) => Paren(p, p.AssocRank > AssocRank);
        private         string NonAssocSide(Prop p) => Paren(p, p.AssocRank >= AssocRank);
    }

    public class PAnd : BinProp {
        public PAnd(Prop l, Prop r) : base(l, r, (a, b) => a && b, "&", true, 2) {}
    }

    public class POr : BinProp {
        public POr(Prop l, Prop r) : base(l, r, (a, b) => a || b, "|", true, 3) {}
    }

    public class PImpl : BinProp {
        public PImpl(Prop l, Prop r) : base(l, r, (a, b) => !a || b, "->", false, 4) {}
    }
}
