using System;
namespace recipe_planer.Models
{
    public class Ingredient : IEquatable<Ingredient>
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }

        public Ingredient(string Name, double Amount, string Unit)
        {
            this.Name = Name;
            this.Amount = Amount;
            this.Unit = Unit;
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
            bool string_equal = this.Name.Equals(other.Name);
            bool unit_equal = this.Unit.Equals(other.Unit);

            return string_equal && unit_equal;
        }

        public override bool Equals(object other)
        {
            return Equals(other as Ingredient);
        }
    }
}
