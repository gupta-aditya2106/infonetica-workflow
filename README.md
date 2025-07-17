# Infonetica – Configurable Workflow Engine (State-Machine API)

This is a minimal backend service built with .NET 8 and C# to manage configurable workflows using state machines.

---

## Features

- Define workflows with states and transitions (actions)
- Start workflow instances from definitions
- Execute actions to move between states with validation
- View current state and action history of any instance
- Minimal API structure with in-memory storage

---

## How to Run

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Run Locally

```bash
git clone https://github.com/gupta-aditya2106/infonetica-workflow.git
cd infonetica-workflow

dotnet build

dotnet run

You should see output like:
Now listening on: http://localhost:5283

Swagger UI allows you to:
Test all API endpoints
Send JSON payloads
View live responses without Postman or external tools


Use the /workflow-definition endpoint to create a new workflow (sample JSON is provided below in this README). Then:

1. Start a new instance using /start-instance/{definitionId}
2. Perform state transitions with /execute-action/{instanceId}/{actionId}
3. Check the instance status using /instance/{id}


Use this in the /workflow-definition POST endpoint:

{
  "Id": "leave-approval",
  "States": [
    { "Id": "draft", "IsInitial": true, "IsFinal": false, "Enabled": true },
    { "Id": "managerApproved", "IsInitial": false, "IsFinal": false, "Enabled": true },
    { "Id": "approved", "IsInitial": false, "IsFinal": true, "Enabled": true },
    { "Id": "rejected", "IsInitial": false, "IsFinal": true, "Enabled": true }
  ],
  "Actions": [
    { "Id": "submit", "Enabled": true, "FromStates": ["draft"], "ToState": "managerApproved" },
    { "Id": "approve", "Enabled": true, "FromStates": ["managerApproved"], "ToState": "approved" },
    { "Id": "reject", "Enabled": true, "FromStates": ["managerApproved"], "ToState": "rejected" }
  ]
}

WorkflowEngine/
├── Models/              # Data classes (State, Action, etc.)
├── Services/            # Core logic for workflow execution
├── Program.cs           # Main API entry point
└── README.md


