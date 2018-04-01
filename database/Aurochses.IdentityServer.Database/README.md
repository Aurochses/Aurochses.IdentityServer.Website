# Database

### How to use this?
Add migration for context:
```
dotnet ef migrations add Initial -c:BaseContext
```
Other migrations:
```
dotnet ef migrations remove -c:BaseContext

dotnet ef database update
dotnet ef database update 0 -c:BaseContext
```

### ASPNETCORE_ENVIRONMENT
set ASPNETCORE_ENVIRONMENT=Development
echo %ASPNETCORE_ENVIRONMENT%
