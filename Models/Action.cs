namespace WorkflowEngine.Models;

public class Action
{
    public string Id { get; set; }                 // Unique name (e.g. "submit", "approve")
    public bool Enabled { get; set; } = true;      // Whether this transition is active
    public List<string> FromStates { get; set; }   // The states from which this action is allowed
    public string ToState { get; set; }            // The state this action moves to
}