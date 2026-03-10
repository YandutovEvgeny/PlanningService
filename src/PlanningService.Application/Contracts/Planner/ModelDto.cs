using PlanningService.Application.Contracts.Planner;

namespace PlanningService.WebHost.Contracts.Planner
{
    public class ModelDto
    {
        public required List<string> Data { get; set; }
        public required List<ColumnMetadata> Metadata { get; set; }
    }
}
