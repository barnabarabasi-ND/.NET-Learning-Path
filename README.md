# Branching Strategy

This repository follows a simple Git branching strategy to keep development organized and ensure that production code remains stable.

## Branches

### `main`
- Contains the stable, production-ready code.
- Direct commits are **not allowed**.
- Changes can only be merged through a Pull Request.
- All required checks (build, tests, etc.) must pass before merging.

### `develop`
- The main integration branch for ongoing development.
- All completed features are merged into `develop`.
- Direct commits are discouraged; use feature branches instead.

### Feature Branches

Every new task or feature should be developed in its own branch created from `develop`.

#### Branch naming convention

```
feature/<initials>/<short-description>
```

Examples:

```
feature/js/login-page
feature/ab/add-validation
feature/mp/update-documentation
feature/jd/create-api-endpoint
```

Where:
- `<initials>` are the developer's initials (e.g., `js` for John Smith).
- `<short-description>` is a concise, lowercase description of the feature using hyphens.

### Development Workflow

1. Pull the latest changes from `develop`.

   ```bash
   git checkout develop
   git pull origin develop
   ```

2. Create your feature branch.

   ```bash
   git checkout -b feature/js/login-page
   ```

3. Implement your changes within your personal folder.

4. Commit your work regularly using meaningful commit messages.

5. Push your branch.

   ```bash
   git push origin feature/js/login-page
   ```

6. Open a Pull Request targeting `develop`.

7. After approval and successful build checks, merge the Pull Request.

------

# Repository Structure

Each team member should work **only within their own folder**.

At the root of the repository, every developer must create a folder using their **first and last name** (or agreed naming convention).

Example:

```
/
├── Alice-Smith/
│   ├── ProjectA/
│   └── Exercises/
│
├── Bob-Jones/
│   ├── ProjectA/
│   └── Exercises/
│
├── Charlie-Brown/
│   └── ProjectA/
│
└── README.md
```

## Rules

- Create your personal folder before starting development.
- Store all your work inside your own folder.
- Do **not** modify files in another developer's folder unless explicitly requested.
- Shared documentation (such as this README) should remain in the repository root.

---

# Pull Request Requirements

When opening a Pull Request:

- Build the solution successfully.
- Ensure all automated checks pass.
- Resolve any merge conflicts.
- Request at least one code review.
- Merge only after approval.

---

# Summary

- `main` → Production-ready code.
- `develop` → Integration branch for ongoing development.
- `feature/*` → Individual feature branches created from `develop`.
- Every developer works exclusively in their own personal folder.
- All changes are integrated through Pull Requests.
