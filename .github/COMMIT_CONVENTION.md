
# Commit Message Conventions for Maptor Repository

This repository follows the **Conventional Commits** standard to maintain consistent, descriptive, and automation-friendly commit messages.

---

## ✅ Format
```
<type>(<scope>): <summary>
```
- **type** → The nature of the change (see allowed types below)
- **scope** → The specific project or module affected
- **summary** → A short description in **imperative mood** (max 72 characters)

Example:
```
refactor(jab-common): rename namespaces for consistency
```

---

## ✅ Allowed Types

| Type       | Description                                                                |
|------------|----------------------------------------------------------------------------|
| **feat**   | A new feature                                                             |
| **fix**    | A bug fix                                                                 |
| **docs**   | Documentation only                                                        |
| **style**  | Code style changes (whitespace, formatting), no logic impact              |
| **refactor** | Code restructuring without changing functionality                       |
| **perf**   | Performance improvements                                                  |
| **test**   | Adding or updating tests                                                  |
| **chore**  | Maintenance tasks (build, tooling, dependencies)                          |
| **build**  | Changes to the build system or external dependencies                      |
| **ci**     | Continuous integration or deployment configuration changes                |

---

## ✅ Scopes

Use the **module/project name** or **feature area** as scope:

- `jab-common` → **IRI.Maptor.Jab.Common**
- `tile-services` → Tile provider system
- `layers` → FeatureLayer, RasterLayer, GridLayer
- `cartography`
- `localization`
- `presenters`
- `office-formats`
- `helpers`

Examples:
```
feat(tile-services): add Google Hybrid provider
fix(layers): resolve crash when adding empty feature set
```

---

## ✅ Commit Examples

### Feature:
```
feat(tile-services): add support for Bing Road layer
```

### Bug Fix:
```
fix(cartography): correct color ramp scaling on high zoom levels
```

### Refactor:
```
refactor(jab-common): rename namespaces to align with new conventions
```

### Documentation:
```
docs(readme): add FeatureLayer usage example
```

### Chore:
```
chore(deps): update WriteableBitmapEx to 1.7.1
```

### Test:
```
test(helpers): add unit tests for color blending helper
```

---

## ✅ Breaking Changes

If a commit **introduces a breaking change**, include a footer with `BREAKING CHANGE:` followed by details.

Example:
```
refactor(layers): remove obsolete RenderAsync method

BREAKING CHANGE: RenderAsync is replaced by RenderAsyncEx. Update all usages.
```

---

## ✅ Best Practices

✔ Use **imperative mood** in summary (e.g., “add”, not “added”)  
✔ Keep the summary under **72 characters**  
✔ Always specify **scope** where applicable  
✔ Add a **body** if needed to explain context  
✔ Reference issues in the footer using `Closes #<issue-number>`  

---

## ✅ Tools for Enforcement

- **Commitizen** – Interactive CLI for writing conventional commits  
- **Husky + Commitlint** – Git hooks for commit message linting  
- **Conventional Changelog** – Generates changelogs automatically from commit history  

---

Happy committing! 🚀
