set-location -path src\benday.presidents.webui

dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context PresidentsDbContext
