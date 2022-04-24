using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Helium.Pages.Projects
{
    public class SimulationProjectsModel : PageModel
    {
        private const string Area = "SimulationProjects";

        public static List<Card> GitHubCards = new List<Card>()
        {
            new Card(Area, "DPC++ Experiments", "https://github.com/GuMiner/DPC-experiments", "/img/simulation-projects/DPCExperiments.png",
                new DateTime(2021, 2, 1), new[] { Tag.Software, Tag.Simulation }),
        };

        public static Card BeamFlowModelCard = new Card(
            Area, "Beam Flow", "SimulationProjects/BeamFlow", "/img/simulation-projects/BeamFlow.png",
            new DateTime(2011, 12, 1), new[] { Tag.Simulation, Tag.Software });
        public static Card FieldSimulatorModelCard = new Card(
            Area, "Field Simulator", "SimulationProjects/FieldSimulator", "/img/simulation-projects/FieldSimulator.png",
            new DateTime(2009, 1, 1), new[] { Tag.Simulation, Tag.Software });
        public static Card FluxSimModelCard = new Card(
            Area, "Flux Sim", "SimulationProjects/FluxSim", "/img/simulation-projects/FluxSim.png",
            new DateTime(2014, 8, 1), new[] { Tag.Simulation, Tag.Software });
        public static Card SimulatorModelCard = new Card(
            Area, "Simulator", "SimulationProjects/Simulator", "/img/simulation-projects/Simulator.png",
            new DateTime(2015, 4, 1), new[] { Tag.Simulation, Tag.Software });
        public static Card VectorFlowModelCard = new Card(
            Area, "Vector Flow", "SimulationProjects/VectorFlow", "/img/simulation-projects/VectorFlow.png",
            new DateTime(2010, 6, 1), new[] { Tag.Simulation, Tag.Software });

        public static List<Card> Cards = typeof(SimulationProjectsModel).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(field => field.FieldType == typeof(Card)).Select(field => field.GetValue(null) as Card).Select(card => card!)
            .Where(card => card.Area.Equals(Area))
            .Union(GitHubCards)
            .OrderByDescending(card => card.Date)
            .ToList();

        public void OnGet()
        {

        }
    }
}