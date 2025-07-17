namespace WorkflowEngine.Models;

public class WorkflowInstance
{
    public string Id { get; set; }                  // Unique ID for this instance
    public string WorkflowDefinitionId { get; set; }// Which workflow definition this belongs to
    public string CurrentStateId { get; set; }      // The current state

    // Action history: records what actions were taken + timestamps
    public List<(string ActionId, DateTime Timestamp)> History { get; set; } = new();
}