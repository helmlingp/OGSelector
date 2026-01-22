# Contributing to OGSelector

Thank you for your interest in contributing to OGSelector! This document provides guidelines and instructions for contributing to the project.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [How to Contribute](#how-to-contribute)
- [Coding Standards](#coding-standards)
- [Testing](#testing)
- [Pull Request Process](#pull-request-process)

## Code of Conduct

This project aims to foster an inclusive and respectful environment. Please be considerate and professional in all interactions.

## Getting Started

### Prerequisites

- Windows 10/11
- .NET 10.0 SDK
- Visual Studio 2022 or VS Code with C# Dev Kit
- Git

### Fork and Clone

1. Fork the repository
2. Clone your fork:
   ```bash
   git clone https://github.com/YOUR_USERNAME/OGSelector.git
   cd OGSelector
   ```
3. Add upstream remote:
   ```bash
   git remote add upstream https://github.com/helmlingp/OGSelector.git
   ```

## Development Setup

### 1. Install Dependencies

```bash
dotnet restore
```

### 2. Build the Project

```bash
dotnet build
```

### 3. Run in Development Mode

Open in Visual Studio 2022:
- Open `OGSelector.sln`
- Press F5 to build and run with debugger

Or use VS Code:
- Open the project folder
- Press F5 (Run and Debug)

### 4. Watch for Changes (Optional)

```bash
dotnet watch run
```

## How to Contribute

### Reporting Bugs

Before creating bug reports, please check existing issues. When creating a bug report, include:

- **Description**: Clear description of the issue
- **Steps to Reproduce**: Detailed steps to reproduce the behavior
- **Expected Behavior**: What you expected to happen
- **Actual Behavior**: What actually happened
- **Environment**: 
  - OS version
  - .NET version
  - Application version
- **Screenshots**: If applicable
- **Logs**: Any error messages or debug output

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, include:

- **Clear title and description**
- **Use case**: Why this enhancement would be useful
- **Proposed solution**: How you envision this working
- **Alternatives considered**: Other solutions you've thought about
- **Additional context**: Screenshots, mockups, etc.

### Code Contributions

1. **Check existing issues** or create one to discuss your proposed changes
2. **Create a feature branch** from `main`:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Make your changes** following the coding standards
4. **Test your changes** thoroughly
5. **Commit your changes** with clear commit messages
6. **Push to your fork**:
   ```bash
   git push origin feature/your-feature-name
   ```
7. **Create a Pull Request**

## Coding Standards

### C# Style Guidelines

Follow Microsoft's C# coding conventions:

#### Naming Conventions

- **Classes, Methods, Properties**: PascalCase
  ```csharp
  public class BusinessUnit
  public void LoadDataAsync()
  public string BusinessUnitName { get; set; }
  ```

- **Private fields**: _camelCase with underscore prefix
  ```csharp
  private readonly JsonDownloadService _jsonDownloadService;
  ```

- **Local variables, parameters**: camelCase
  ```csharp
  var inputsData = await LoadData();
  public void SetValue(string keyName, string value)
  ```

- **Constants**: PascalCase
  ```csharp
  private const RegistryHive RootHive = RegistryHive.LocalMachine;
  ```

#### Code Organization

- One class per file
- File name should match class name
- Organize using statements (remove unused)
- Use `#region` sparingly, prefer smaller classes
- Order class members:
  1. Fields
  2. Constructors
  3. Properties
  4. Methods
  5. Nested types

#### Comments and Documentation

- Use XML documentation comments for public APIs:
  ```csharp
  /// <summary>
  /// Loads data from a JSON file or URL.
  /// </summary>
  /// <param name="url">Optional URL to load from</param>
  /// <returns>The loaded InputsData or null if failed</returns>
  public async Task<InputsData?> LoadFromFileOrUrlAsync(string? url = null)
  ```

- Use inline comments sparingly, prefer self-documenting code
- Explain "why" not "what" in comments

#### MVVM Pattern

- Use CommunityToolkit.Mvvm attributes:
  ```csharp
  [ObservableProperty]
  private bool isLoading;
  
  [RelayCommand]
  public void Submit()
  ```

- Keep ViewModels UI-agnostic
- Use Services for business logic
- Use Models for data structures only

#### Async/Await

- Always use `async`/`await` for asynchronous operations
- Name async methods with `Async` suffix
- Don't block on async code (no `.Result` or `.Wait()`)
  ```csharp
  public async Task LoadDataAsync()
  {
      var data = await _service.LoadFromFileOrUrlAsync();
  }
  ```

#### Error Handling

- Use try-catch for expected exceptions
- Log errors to Debug output
- Provide user-friendly error messages
  ```csharp
  try
  {
      // Operation
  }
  catch (Exception ex)
  {
      Debug.WriteLine($"Error: {ex.Message}");
      StatusMessage = "A user-friendly error message";
  }
  ```

### XAML Style Guidelines

#### Formatting

- Indent with 4 spaces
- One attribute per line for complex elements
- Align attributes vertically
  ```xml
  <Button Content="Submit"
          Command="{Binding SubmitCommand}"
          IsEnabled="{Binding !IsLoading}"
          HorizontalAlignment="Center" />
  ```

#### Naming

- Use x:Name in PascalCase for named elements
- Name controls descriptively:
  ```xml
  <ComboBox x:Name="BusinessUnitComboBox" />
  ```

#### Data Binding

- Use compiled bindings when possible (default in this project)
- Prefer MVVM pattern over code-behind
  ```xml
  <TextBlock Text="{Binding StatusMessage}" />
  ```

### Git Commit Messages

- Use present tense ("Add feature" not "Added feature")
- Use imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit first line to 72 characters
- Reference issues and pull requests after first line

**Example:**
```
Add remote JSON caching feature

- Implement cache expiration logic
- Add cache directory configuration
- Update documentation

Fixes #123
```

### Commit Types

Use conventional commit prefixes:

- `feat:` New feature
- `fix:` Bug fix
- `docs:` Documentation changes
- `style:` Code style changes (formatting, etc.)
- `refactor:` Code refactoring
- `test:` Adding or updating tests
- `chore:` Maintenance tasks

## Testing

### Manual Testing

Test checklist before submitting PR:

- [ ] Application builds without errors
- [ ] Application runs without crashes
- [ ] UI displays correctly
- [ ] All interactive elements work
- [ ] Data loads correctly (local and remote)
- [ ] Registry writes work correctly
- [ ] Error handling works as expected
- [ ] Full-screen mode works (if changed)
- [ ] Exit prevention works (if changed)

### Test Configuration

Create a test `appsettings.json`:

```json
{
  "UI": {
    "Headline": "Test Configuration",
    "Subtitle": "This is a test build",
    "Information": "Testing new features..."
  },
  "Settings": {
    "jsonURL": "",
    "Fullscreen": false,
    "AllowExit": true,
    "RegKeyPath": "HKEY_LOCAL_MACHINE\\SOFTWARE\\TEST"
  }
}
```

### Test Data

Create a test `inputs.json`:

```json
{
  "BUs": [
    {"uemUuid": "test-uuid-1", "uemId": "1001", "uemName": "Test OG 1", "businessUnit": "Test BU 1"},
    {"uemUuid": "test-uuid-2", "uemId": "1002", "uemName": "Test OG 2", "businessUnit": "Test BU 2"}
  ],
  "Roles": [
    {"roleName": "Test Role 1"},
    {"roleName": "Test Role 2"}
  ],
  "Geos": [
    {"geoName": "Test Geo 1"},
    {"geoName": "Test Geo 2"}
  ]
}
```

## Pull Request Process

### Before Submitting

1. **Update documentation** if needed
2. **Test thoroughly** on Windows 10 and 11
3. **Update README.md** with any new features
4. **Check code style** compliance
5. **Verify no merge conflicts** with main branch

### PR Description Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix (non-breaking change fixing an issue)
- [ ] New feature (non-breaking change adding functionality)
- [ ] Breaking change (fix or feature causing existing functionality to not work as expected)
- [ ] Documentation update

## Testing
Describe testing performed:
- Test scenario 1
- Test scenario 2

## Checklist
- [ ] Code follows project style guidelines
- [ ] Self-review performed
- [ ] Comments added for complex code
- [ ] Documentation updated
- [ ] No new warnings generated
- [ ] Manual testing completed

## Screenshots (if applicable)
Add screenshots of UI changes

## Related Issues
Fixes #(issue number)
```

### Review Process

1. Maintainers will review your PR
2. Address any requested changes
3. Once approved, your PR will be merged
4. Your contribution will be credited

### After Merge

1. Pull the latest main branch:
   ```bash
   git checkout main
   git pull upstream main
   ```
2. Delete your feature branch:
   ```bash
   git branch -d feature/your-feature-name
   ```

## Project Structure

Understanding the codebase:

```
OGSelector/
├── Models/                 # Data models (POCOs)
│   └── InputsData.cs      # BU, Role, Geo models
├── Services/              # Business logic services
│   ├── JsonDownloadService.cs
│   └── RegistryService.cs
├── ViewModels/            # MVVM ViewModels
│   └── MainViewModel.cs
├── Views/                 # UI Views (XAML)
│   ├── MainView.axaml
│   ├── MainView.axaml.cs
│   ├── MainWindow.axaml
│   └── MainWindow.axaml.cs
├── Assets/                # Resources (Lottie animations)
├── App.axaml              # Application resources
├── App.axaml.cs           # Application lifecycle
├── Program.cs             # Entry point
└── appsettings.json       # Configuration
```

## Areas for Contribution

### High Priority

- Unit tests for Services
- Integration tests
- Localization/internationalization support
- Logging framework integration
- Configuration validation

### Medium Priority

- Additional animation assets
- Theme customization options
- CLI argument improvements
- Performance optimizations

### Low Priority

- Additional UI themes
- Export/import settings
- Diagnostic tools
- Statistics/analytics

## Questions?

- Open an issue for questions
- Tag with "question" label
- Maintainers will respond

## Recognition

Contributors will be recognized in:
- README.md Contributors section
- Release notes
- Git commit history

Thank you for contributing to OGSelector!
