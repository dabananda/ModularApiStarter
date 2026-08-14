# Renaming Your Project

When you create a new repository from the `ModularApiStarter` template, everything — namespaces, `.csproj` files, the `.slnx`, folder names — still says `ModularApiStarter`. Two scripts are included to rename all of it in one pass: `rename-project.ps1` (PowerShell) and `rename-project.sh` (bash).

Pick whichever matches your environment. They do exactly the same thing.

---

## What the script does

1. **Updates file contents** — every occurrence of `ModularApiStarter` inside `.cs`, `.csproj`, `.slnx`, `.json`, and `.md` files is replaced with your new name. This covers namespaces, `using` statements, project references, the solution file, and config.
2. **Renames files and folders** — anything whose name contains `ModularApiStarter` (e.g. `ModularApiStarter.Api.csproj`, the `ModularApiStarter.Shared/` folder) is renamed to match. Renaming happens deepest-path-first, so a parent folder is never renamed out from under its own children.
3. **Skips build/tooling folders** — `bin/`, `obj/`, `.git/`, `.vs/`, `.idea/`, `node_modules/` are left alone.
4. **Prints a summary** of every file updated and every file/folder renamed, plus next-step reminders.

The script only touches files under the folder you point it at — it won't reach outside your project directory.

---

## Prerequisites

- **PowerShell script**: PowerShell 7+ (`pwsh`). Comes preinstalled on Windows; on macOS/Linux install via `brew install powershell` or your package manager.
- **Bash script**: any Linux/macOS shell, or WSL/Git Bash on Windows. No extra dependencies — just `bash`, `find`, `sed`, and `grep`, which are standard on all of these.

---

## Usage

Run the script from the root of your newly created project (the folder containing the `.slnx` file).

### PowerShell

```powershell
./rename-project.ps1 -NewName "YourProjectName"
```

Optional parameters:

```powershell
./rename-project.ps1 -NewName "YourProjectName" -OldName "ModularApiStarter" -Path "./path/to/repo"
```

| Parameter  | Required | Default              | Description                                  |
|------------|----------|----------------------|-----------------------------------------------|
| `-NewName` | Yes      | —                    | The name to rename the project to             |
| `-OldName` | No       | `ModularApiStarter`  | The name being replaced                       |
| `-Path`    | No       | `.` (current folder) | Root folder of the repo to run the rename in  |

### Bash

```bash
./rename-project.sh YourProjectName
```

Optional positional arguments (same order as above):

```bash
./rename-project.sh YourProjectName ModularApiStarter ./path/to/repo
```

If the script isn't executable yet:

```bash
chmod +x rename-project.sh
./rename-project.sh YourProjectName
```

---

## Naming conventions

Use PascalCase, optionally with dots for a namespace-style name — the same shape as `ModularApiStarter` itself:

- `TaskFlow`
- `Acme.Server`
- `InventoryHub.Api`

The script warns you (and asks for confirmation) if your new name contains characters that wouldn't be a valid .NET namespace/project name — spaces, special characters, or a name starting with a digit.

---

## Example run

```bash
$ ./rename-project.sh TaskFlow

Renaming 'ModularApiStarter' -> 'TaskFlow' under /repos/TaskFlow

Step 1/2: Updating file contents...
  updated: ModularApiStarter.slnx
  updated: ModularApiStarter.Api/Program.cs
  updated: ModularApiStarter.Shared/DependencyInjection.cs
  ... (35+ more files)
Updated content in 40 file(s).

Step 2/2: Renaming files and folders...
  renamed: ModularApiStarter.Api.csproj  ->  TaskFlow.Api.csproj
  renamed: ModularApiStarter.Shared.csproj  ->  TaskFlow.Shared.csproj
  renamed: ModularApiStarter.Api  ->  TaskFlow.Api
  renamed: ModularApiStarter.Shared  ->  TaskFlow.Shared
  renamed: ModularApiStarter.slnx  ->  TaskFlow.slnx
  ... (a few more)
Renamed 8 file(s)/folder(s).

Done. Next steps:
  1. Open the solution and confirm it builds: dotnet build
  2. Check appsettings.json / appsettings.Development.json for anything you want to adjust
  3. Update the repo name on GitHub if it still says 'ModularApiStarter'
```

---

## After running the script

1. **Build the solution** to confirm the rename didn't break anything:
   ```bash
   dotnet build
   ```
2. **Review `appsettings.json`** — the `Serilog.Properties.Application` value and any other project-specific settings.
3. **Rename the GitHub repository itself** (Settings → repository name), if it still shows the template's name.
4. **Delete the rename scripts** (`rename-project.ps1`, `rename-project.sh`) once you're done with them, or keep them around if you think you'll need to re-run a rename later.

---

## Troubleshooting

**"Target exists" skip messages during Step 2** — the script won't overwrite a file/folder that already exists at the destination name. This can happen if you run the script twice, or if a file with your new name already existed for another reason. Check that file manually.

**Script says a file couldn't be read in Step 1** — this is expected for binary files (if any end up in the repo) and is silently skipped; it's not an error.

**Want to undo a rename?** — the safest approach is `git checkout .` (if you haven't committed yet) or `git reset --hard` to your last commit before running the script again with the correct name.
