# Quick Start Guide

## Setup

1. **Navigate to project directory**:
   ```bash
   cd /Users/helmlingp/GitHub.work/OGSelector
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

## Configuration

### Using Your Own JSON File

1. Host your JSON file on a web server or use a publicly accessible URL
2. Ensure the JSON follows this structure:
   ```json
   {
     "BUs": [{"Display": "...", "Name": "..."}],
     "Roles": [{"Display": "...", "Name": "..."}],
     "Geos": [{"Display": "...", "Name": "..."}]
   }
   ```

3. In the app, paste your URL and click "Load Data"

### Testing Locally

For local testing without a server:

1. Update the `JsonUrl` constant in `Services/JsonDownloadService.cs`:
   ```csharp
   private const string JsonUrl = "file:///path/to/inputs.json";
   ```

2. Or use the sample file included at `Resources/Raw/sample_inputs.json`

## Verify Registry Keys

After making selections, verify the registry keys were created:

### Using Registry Editor (Windows)
1. Press `Win + R` and type `regedit`
2. Navigate to: `HKEY_LOCAL_MACHINE\SOFTWARE\CUSTOMER`
3. You should see three string values:
   - `BUs`
   - `Roles`
   - `Geos`

### Using PowerShell
```powershell
Get-ItemProperty -Path "HKLM:\SOFTWARE\CUSTOMER" -Name "BUs"
Get-ItemProperty -Path "HKLM:\SOFTWARE\CUSTOMER" -Name "Roles"
Get-ItemProperty -Path "HKLM:\SOFTWARE\CUSTOMER" -Name "Geos"
```

## Troubleshooting

### "Failed to load data"
- Verify the JSON URL is correct and accessible
- Check network connectivity
- Ensure the JSON format matches the expected structure

### Registry keys not being created
- Verify you have administrator privileges
- Check Windows UAC settings
- Ensure the SOFTWARE\CUSTOMER registry path exists (the app will create it if it doesn't)

### Application won't start
- Ensure .NET 8.0 SDK is installed: `dotnet --version`
- Clear the bin/obj folders: `dotnet clean`
- Restore packages: `dotnet restore`

## Project Architecture

### MVVM Pattern
- **Views**: XAML files (MainPage.xaml, AppShell.xaml)
- **ViewModels**: MainViewModel.cs handles business logic and property binding
- **Models**: InputsData.cs defines data structures

### Services
- **JsonDownloadService**: Fetches and deserializes JSON from remote or local sources
- **RegistryService**: Handles Windows Registry operations

### Key Technologies
- .NET 8.0
- MAUI (Multi-platform App UI)
- MVVM Toolkit for reactive programming
- Windows Registry API (via Microsoft.Win32)

## Development Notes

- The app uses data binding for automatic UI updates
- Registry operations happen immediately upon selection change
- The UI is responsive and shows loading indicators during data fetching
- All logging goes to Debug output (View > Output > Debug in Visual Studio)
