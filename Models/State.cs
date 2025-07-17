namespace WorkflowEngine.Models;

public class State
{
    public string Id { get; set; }           // Unique name for the state (e.g. "draft")
    public bool IsInitial { get; set; }      // True only for the starting state
    public bool IsFinal { get; set; }        // True only for the end state
    public bool Enabled { get; set; } = true;// You can turn states on/off
}