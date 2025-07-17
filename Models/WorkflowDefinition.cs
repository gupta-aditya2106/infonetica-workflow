namespace WorkflowEngine.Models;

public class WorkflowDefinition
{
    public string Id { get; set; }                 // Unique ID of this definition (e.g. "approvalFlow")
    public List<State> States { get; set; } = new();
    public List<Action> Actions { get; set; } = new();
}