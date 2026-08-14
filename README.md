# ModularApiStarter

A batteries-included, modular ASP.NET Core Web API starter template — the boring 80% (architecture, validation, logging, error handling, Swagger auth) already wired up, so you can jump straight into business logic.

Built as a **GitHub template repository**: click "Use this template", rename, and start building.

---

## ✨ What's included

- **Modular architecture** — self-contained feature modules implementing `IModule`, registered with a single line in `Program.cs`. No giant tangled project.
- **CQRS-lite pipeline** — `ISender`, `IRequestHandler<TRequest, TResponse>`, and pluggable `IPipelineBehavior<,>` (e.g. validation) instead of a heavyweight mediator library.
- **FluentValidation**, wired automatically into the request pipeline via `ValidationBehavior<,>` — add a validator, get validation for free.
- **Consistent `Result<T>` pattern** for success/failure responses, mapped to proper HTTP status codes by `BaseController.Handle()`.
- **Centralized exception handling** via `ExceptionHandlingMiddleware` — no scattered try/catch blocks.
- **Serilog** — console + rolling daily file sinks, fully config-driven from `appsettings.json`, with request logging and startup/shutdown crash logging.
- **Swagger UI with JWT Bearer auth support** — an "Authorize" button ready to accept `Bearer <token>` (add actual JWT validation per your auth provider).
- **Sample module (`ModularApiStarter.Modules.Greeting`)** — a working, dependency-free example (in-memory store, no DB needed) showing the full shape: entity, command, query, validator, handler, controller, and module registration.

---

## 📁 Project structure

```
ModularApiStarter/
├── ModularApiStarter.Api/                    # Host project — Program.cs, Swagger, Serilog, appsettings
├── ModularApiStarter.Shared/                 # Cross-cutting: abstractions, Result<T>, BaseController, middleware
│   ├── Abstraction/                          # IModule, ISender, IRequest, IRequestHandler, IPipelineBehavior
│   ├── Behaviors/                            # ValidationBehavior<,>
│   ├── Common/                                # Result<T>, BaseController, BaseEntity, ExceptionType, AppSettings
│   ├── Middlewares/                          # ExceptionHandlingMiddleware
│   └── Extensions/                           # AddRequestHandlers, AddValidators, AddPipelineBehaviors
└── ModularApiStarter.Modules.Greeting/       # Sample module — copy this shape for your own modules
    ├── Entities/
    ├── Features/
    │   ├── CreateGreeting/                   # Command + Validator + Handler
    │   └── GetGreetings/                     # Query + Handler
    ├── Controllers/
    └── GreetingModule.cs                     # IModule implementation
```

---

## 🚀 Getting started

1. **Use this template** on GitHub (or clone directly).
2. Restore and run:
   ```bash
   dotnet restore
   dotnet run --project ModularApiStarter.Api
   ```
3. Open Swagger UI (shown in the console output, typically `https://localhost:XXXX/swagger`) and try the sample `Greetings` endpoints:
   - `POST /api/v1/greetings` — body: `{ "name": "Your Name" }`
   - `GET /api/v1/greetings`

---

## 🧩 Adding your own module

1. Create a new class library project, e.g. `ModularApiStarter.Modules.YourFeature`, referencing `ModularApiStarter.Shared`.
2. Mirror the sample module's shape:
   ```
   Entities/
   Features/<UseCase>/<Command|Query>.cs, <Validator>.cs, <Handler>.cs
   Controllers/<Name>Controller.cs
   YourFeatureModule.cs   → implements IModule
   ```
3. In `RegisterModule`, register your handlers/validators and add the assembly as an MVC application part:
   ```csharp
   var assembly = typeof(YourFeatureModule).Assembly;
   services.AddRequestHandlers(assembly);
   services.AddValidators(assembly);
   services.AddControllers().AddApplicationPart(assembly);
   ```
4. Register it in `ModularApiStarter.Api/Program.cs`:
   ```csharp
   var modules = new IModule[] { new GreetingModule(), new YourFeatureModule() };
   ```
5. Remove the sample `Greeting` module once you no longer need it as a reference (delete the project, its `ProjectReference`, and the `new GreetingModule()` line).

---

## ⚙️ Configuration

`AppSettings` currently expects a SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "SqlServer": "your-connection-string-here"
  }
}
```

Serilog is configured in `appsettings.json` under the `Serilog` section (console + rolling file sinks, minimum level, enrichers). `appsettings.Development.json` overrides the minimum level to `Debug`.

> **Note:** `appsettings.json` is currently listed in `.gitignore` (to avoid committing secrets). If you keep non-secret config like the `Serilog` section there, either remove it from `.gitignore` or move that config into a file that *is* tracked (e.g. `appsettings.Development.json`).

Swagger UI's "Authorize" button accepts a bearer token, but no JWT validation middleware is configured yet — add `AddAuthentication().AddJwtBearer(...)` for your identity provider when you're ready to enforce auth.

---

## 🤝 Contributing

This template is intentionally opinionated and still evolving.

- Found something that could be cleaner? Open an issue.
- Have an improvement? Submit a PR.
- Disagree with an architectural choice? Start a discussion — different opinions make this better.

---

## ⭐ Support

If this saves you the "ugh, not this setup again" afternoon:

- **Star** the repo
- **Fork** it and make it your own
- **Share** it with a .NET dev who reinvents this wheel every project
