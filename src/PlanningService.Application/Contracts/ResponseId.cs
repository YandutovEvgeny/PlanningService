namespace PlanningService.Application.Contracts;

public class ResponseId<T>
{
    public required T Id { get; set; }
}