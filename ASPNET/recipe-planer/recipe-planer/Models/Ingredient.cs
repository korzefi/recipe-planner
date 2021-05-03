using System;
namespace recipe_planer.Models
{
    public class Ingredient : IEquatable<Ingredient>
    {
        public string name;
        public double amount;
        public string unit;

        public Ingredient(string Name, double Amount, string Unit)
        {
            this.name = Name;
            this.amount = Amount;
            this.unit = Unit;
        }

        public static bool operator == (Ingredient LHS, Ingredient RHS)
        {
            return LHS.Equals(RHS);
        }

        public static bool operator !=(Ingredient LHS, Ingredient RHS)
        {
            return !(LHS.Equals(RHS));
        }

        public bool Equals(Ingredient other)
        {
            bool string_equal = this.name.Equals(other.name);
            bool unit_equal = this.unit.Equals(other.unit);

            return string_equal && unit_equal;
        }

        public override bool Equals(object other)
        {
            return Equals(other as Ingredient);
        }
    }
}
