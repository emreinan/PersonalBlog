using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<App_Admin>("AppAdmin");
builder.AddProject<App_Client>("AppClient");
builder.AddProject<App_Auth_Api>("AppAuthApi");
builder.AddProject<App_Data_Api>("AppDataApi");
builder.AddProject<App_File_Api>("AppFileApi");

builder.Build().Run();
