namespace Helium.Pages.Projects.Shared
{
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
