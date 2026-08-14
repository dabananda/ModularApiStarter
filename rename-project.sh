#!/usr/bin/env bash
#
# Renames the ModularApiStarter template to a project name of your choice.
#
# Usage:
#   ./rename-project.sh <NewName> [OldName] [Path]
#
# Examples:
#   ./rename-project.sh TaskFlow
#   ./rename-project.sh Acme.Server ModularApiStarter ./my-new-project
#
set -euo pipefail

NEW_NAME="${1:?Usage: ./rename-project.sh <NewName> [OldName] [Path]}"
OLD_NAME="${2:-ModularApiStarter}"
ROOT="${3:-.}"

if [ "$NEW_NAME" = "$OLD_NAME" ]; then
  echo "New name is the same as the old name — nothing to do."
  exit 0
fi

if ! [[ "$NEW_NAME" =~ ^[A-Za-z][A-Za-z0-9_.]*$ ]]; then
  echo "Warning: '$NEW_NAME' contains characters that may not be valid in a .NET namespace/project name."
  read -r -p "Continue anyway? (y/N) " confirm
  [ "$confirm" = "y" ] || exit 1
fi

cd "$ROOT"
ROOT="$(pwd)"
echo "Renaming '$OLD_NAME' -> '$NEW_NAME' under $ROOT"

EXCLUDE_ARGS=( -path "*/bin/*" -o -path "*/obj/*" -o -path "*/.git/*" -o -path "*/.vs/*" -o -path "*/.idea/*" -o -path "*/node_modules/*" )

echo
echo "Step 1/2: Updating file contents..."
edited=0
while IFS= read -r -d '' file; do
  if grep -qlF "$OLD_NAME" "$file" 2>/dev/null; then
    sed -i.bak "s/${OLD_NAME//./\\.}/${NEW_NAME}/g" "$file" && rm -f "$file.bak"
    edited=$((edited + 1))
    echo "  updated: ${file#$ROOT/}"
  fi
done < <(find "$ROOT" -type f \( "${EXCLUDE_ARGS[@]}" \) -prune -o -type f -print0)
echo "Updated content in $edited file(s)."

echo
echo "Step 2/2: Renaming files and folders..."
renamed=0
# Deepest paths first, so renaming a parent directory never orphans/duplicates
# a child whose new path was already created.
while IFS= read -r path; do
  dir=$(dirname "$path")
  base=$(basename "$path")
  newbase="${base//$OLD_NAME/$NEW_NAME}"
  newpath="$dir/$newbase"

  if [ "$path" = "$newpath" ]; then
    continue
  fi

  if [ -e "$newpath" ]; then
    echo "  skip (target exists): $newpath"
    continue
  fi

  mv "$path" "$newpath"
  renamed=$((renamed + 1))
  echo "  renamed: $base  ->  $newbase"
done < <(find "$ROOT" \( "${EXCLUDE_ARGS[@]}" \) -prune -o -name "*${OLD_NAME}*" -print | awk '{ print length, $0 }' | sort -rn | cut -d' ' -f2-)

echo "Renamed $renamed file(s)/folder(s)."

echo
echo "Done. Next steps:"
echo "  1. Open the solution and confirm it builds: dotnet build"
echo "  2. Check appsettings.json / appsettings.Development.json for anything you want to adjust"
echo "  3. Update the repo name on GitHub if it still says '$OLD_NAME'"
