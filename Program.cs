using WorkflowEngine.Models;
using WorkflowEngine.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var workflowService = new WorkflowService();

// 1. Create a new workflow definition
app.MapPost("/workflow-definition", (WorkflowDefinition def) =>
{
    var result = workflowService.AddDefinition(def);
    return result.Success ? Results.Ok(result.Message) : Results.BadRequest(result.Message);
});

// 2. Start a new instance
app.MapPost("/start-instance/{definitionId}", (string definitionId) =>
{
    var instance = workflowService.StartInstance(definitionId);
    return instance != null ? Results.Ok(instance) : Results.BadRequest("Invalid workflow definition or no initial state.");
});

// 3. Execute action on instance
app.MapPost("/execute-action/{instanceId}/{actionId}", (string instanceId, string actionId) =>
{
    var result = workflowService.ExecuteAction(instanceId, actionId);
    return result.Success ? Results.Ok(result.Message) : Results.BadRequest(result.Message);
});

// 4. Get instance details
app.MapGet("/instance/{id}", (string id) =>
{
    var instance = workflowService.GetInstance(id);
    return instance != null ? Results.Ok(instance) : Results.NotFound("Instance not found.");
});

// 5. Optional: list all workflow definitions
app.MapGet("/workflow-definitions", () =>
{
    return Results.Ok(workflowService.GetAllDefinitions());
});


app.Run();


