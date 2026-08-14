#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Renames the ModularApiStarter template to a project name of your choice.

.DESCRIPTION
    Replaces every occurrence of the old name (in file contents, file names,
    and folder names) with your new project name, across the whole repo.
    Safe to run right after "Use this template" and cloning, before you've
    made any other changes.

.PARAMETER NewName
    The new project name, e.g. "Acme.Server" or "TaskFlowApi".
    Use PascalCase / dot-namespace style, same as the original
    "ModularApiStarter" — this becomes your root namespace.

.PARAMETER OldName
    The name to replace. Defaults to "ModularApiStarter". Only override this
    if you've already renamed things partially and are re-running the script.

.PARAMETER Path
    Root folder of the repo. Defaults to the current directory.

.EXAMPLE
    ./rename-project.ps1 -NewName "TaskFlow"

.EXAMPLE
    ./rename-project.ps1 -NewName "Acme.Server" -Path "C:\repos\my-new-project"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$NewName,

    [string]$OldName = "ModularApiStarter",

    [string]$Path = "."
)

$ErrorActionPreference = "Stop"

if ($NewName -eq $OldName) {
    Write-Host "New name is the same as the old name — nothing to do." -ForegroundColor Yellow
    exit 0
}

if ($NewName -notmatch '^[A-Za-z][A-Za-z0-9_.]*$') {
    Write-Host "Warning: '$NewName' contains characters that may not be valid in a .NET namespace/project name." -ForegroundColor Yellow
    $confirm = Read-Host "Continue anyway? (y/N)"
    if ($confirm -ne "y") { exit 1 }
}

$root = Resolve-Path $Path
Write-Host "Renaming '$OldName' -> '$NewName' under $root" -ForegroundColor Cyan

# Directories to skip entirely (build output, git internals, IDE folders).
$excludeDirs = @('bin', 'obj', '.git', '.vs', '.idea', 'node_modules')

function Test-Excluded($fullPath) {
    $relative = $fullPath.Substring($root.Path.Length).TrimStart('\', '/')
    $segments = $relative -split '[\\/]'
    foreach ($seg in $segments) {
        if ($excludeDirs -contains $seg) { return $true }
    }
    return $false
}

# --- Step 1: replace occurrences of OldName inside file contents ---
Write-Host "`nStep 1/2: Updating file contents..." -ForegroundColor Cyan

$allFiles = Get-ChildItem -Path $root -Recurse -File | Where-Object { -not (Test-Excluded $_.FullName) }
$editedCount = 0

foreach ($file in $allFiles) {
    try {
        $content = Get-Content -Raw -Path $file.FullName -ErrorAction Stop
    } catch {
        continue  # binary or unreadable file, skip
    }

    if ($null -ne $content -and $content.Contains($OldName)) {
        $newContent = $content.Replace($OldName, $NewName)
        Set-Content -Path $file.FullName -Value $newContent -NoNewline
        $editedCount++
        Write-Host "  updated: $($file.FullName.Substring($root.Path.Length).TrimStart('\','/'))"
    }
}

Write-Host "Updated content in $editedCount file(s)." -ForegroundColor Green

# --- Step 2: rename files and folders containing OldName ---
# Deepest paths first, so renaming a parent folder never orphans/duplicates
# a child whose new path was already created.
Write-Host "`nStep 2/2: Renaming files and folders..." -ForegroundColor Cyan

$allPaths = Get-ChildItem -Path $root -Recurse |
    Where-Object { -not (Test-Excluded $_.FullName) -and $_.Name.Contains($OldName) } |
    Sort-Object { $_.FullName.Split([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar).Count } -Descending

$renamedCount = 0

foreach ($item in $allPaths) {
    $newName = $item.Name.Replace($OldName, $NewName)
    $newFullPath = Join-Path $item.Directory.FullName $newName

    if (Test-Path $newFullPath) {
        Write-Host "  skip (target exists): $newFullPath" -ForegroundColor Yellow
        continue
    }

    Rename-Item -Path $item.FullName -NewName $newName
    $renamedCount++
    Write-Host "  renamed: $($item.Name)  ->  $newName"
}

Write-Host "Renamed $renamedCount file(s)/folder(s)." -ForegroundColor Green

Write-Host "`nDone. Next steps:" -ForegroundColor Cyan
Write-Host "  1. Open the solution and confirm it builds: dotnet build"
Write-Host "  2. Check appsettings.json / appsettings.Development.json for anything you want to adjust"
Write-Host "  3. Update the repo name on GitHub if it still says '$OldName'"
