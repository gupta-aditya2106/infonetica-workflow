using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

public class WorkflowService
{
    // Store definitions and instances in memory
    private readonly Dictionary<string, WorkflowDefinition> definitions = new();
    private readonly Dictionary<string, WorkflowInstance> instances = new();

    // 🔧 Create a new workflow definition
    public (bool Success, string Message) AddDefinition(WorkflowDefinition def)
    {
        if (definitions.ContainsKey(def.Id))
            return (false, "Workflow already exists.");

        // Validate: must have exactly one initial state
        var initialStates = def.States.Count(s => s.IsInitial);
        if (initialStates != 1)
            return (false, "There must be exactly one initial state.");

        // Check for duplicate state IDs
        if (def.States.Select(s => s.Id).Distinct().Count() != def.States.Count)
            return (false, "Duplicate state IDs are not allowed.");

        // Check for duplicate action IDs
        if (def.Actions.Select(a => a.Id).Distinct().Count() != def.Actions.Count)
            return (false, "Duplicate action IDs are not allowed.");

        // Check that all referenced states in actions exist
        var stateIds = def.States.Select(s => s.Id).ToHashSet();

        foreach (var action in def.Actions)
        {
            foreach (var fromState in action.FromStates)
            {
                if (!stateIds.Contains(fromState))
                    return (false, $"Invalid fromState '{fromState}' in action '{action.Id}'.");
            }

            if (!stateIds.Contains(action.ToState))
                return (false, $"Invalid toState '{action.ToState}' in action '{action.Id}'.");
        }

        // ✅ Passed all validations
        definitions[def.Id] = def;
        return (true, "Workflow definition added.");
    }

    // 🔧 Start a new instance of a workflow
    public WorkflowInstance? StartInstance(string workflowId)
    {
        if (!definitions.TryGetValue(workflowId, out var def))
            return null;

        var initial = def.States.FirstOrDefault(s => s.IsInitial && s.Enabled);
        if (initial == null)
            return null;

        var instance = new WorkflowInstance
        {
            Id = Guid.NewGuid().ToString(),
            WorkflowDefinitionId = workflowId,
            CurrentStateId = initial.Id
        };

        instances[instance.Id] = instance;
        return instance;
    }

    // 🔧 Execute an action on an instance (move it to next state)
    public (bool Success, string Message) ExecuteAction(string instanceId, string actionId)
    {
        if (!instances.TryGetValue(instanceId, out var instance))
            return (false, "Instance not found.");

        var def = definitions[instance.WorkflowDefinitionId];

        var action = def.Actions.FirstOrDefault(a => a.Id == actionId && a.Enabled);
        if (action == null)
            return (false, "Action not found or disabled.");

        if (!action.FromStates.Contains(instance.CurrentStateId))
            return (false, $"Action '{actionId}' is not valid from state '{instance.CurrentStateId}'.");

        var targetState = def.States.FirstOrDefault(s => s.Id == action.ToState);
        if (targetState == null || !targetState.Enabled)
            return (false, "Target state is invalid or disabled.");

        var currentState = def.States.First(s => s.Id == instance.CurrentStateId);
        if (currentState.IsFinal)
            return (false, "Cannot execute action on a final state.");

        // ✅ Transition allowed — apply the change
        instance.CurrentStateId = targetState.Id;
        instance.History.Add((action.Id, DateTime.UtcNow));

        return (true, "Action executed successfully.");
    }

    // 🔧 Get an instance’s current state and history
    public WorkflowInstance? GetInstance(string instanceId)
    {
        return instances.GetValueOrDefault(instanceId);
    }

    // Optional: Get all definitions (for listing in UI)
    public IEnumerable<WorkflowDefinition> GetAllDefinitions()
    {
        return definitions.Values;
    }
}
