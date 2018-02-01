namespace Helium24.Models
{
    /// <summary>
    /// Defines an equation created for use in partial views.
    /// </summary>
    public class Equation
    {
        public Equation(string equationText, string title)
        {
            this.EquationText = $"$${equationText}$$";
            this.Title = title;
        }

        public string EquationText { get; }

        public string Title { get; }
    }
}
