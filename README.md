# OGSelector

A Windows desktop application that allows users to select their Business Unit (BU), Role, and Geography to configure their device and assign it to the appropriate Organizational Group (OG) in a UEM system.

## Overview

OGSelector is an Avalonia-based .NET application designed to streamline device enrollment and organization placement in enterprise environments. The application presents users with a clean interface to select their organizational details, which are then stored in the Windows Registry for device management purposes.

## Features

- **User-Friendly Interface**: Modern, responsive UI built with Avalonia UI framework
- **Dynamic Data Loading**: Supports loading configuration data from local files or remote URLs
- **Customizable Branding**: Supports custom logos and UI text through configuration
- **Full-Screen Mode**: Optional kiosk-style full-screen mode for enrollment scenarios
- **Exit Prevention**: Configurable option to prevent users from closing the application
- **Registry Integration**: Stores selected values in Windows Registry for downstream consumption
- **Animated Feedback**: Lottie animations for loading states and user feedback

## Requirements

- Windows 10/11 (64-bit)
- .NET 10.0 Runtime (or self-contained deployment)
- Administrator privileges (for registry write operations)

## Project Structure

```
OGSelector/
├── Assets/                      # Lottie animation files
│   ├── dots.json
│   ├── footer.json
│   ├── glass.json
│   ├── lost.json
│   ├── spinner.json
│   ├── stick.json
│   └── tick.json
├── Models/
│   └── InputsData.cs           # Data models for BUs, Roles, and Geos
├── Services/
│   ├── JsonDownloadService.cs  # Handles loading data from file/URL
│   └── RegistryService.cs      # Windows Registry operations
├── ViewModels/
│   └── MainViewModel.cs        # Main application view model (MVVM)
├── Views/
│   ├── MainView.axaml          # Main UI layout
│   ├── MainView.axaml.cs       # Main view code-behind
│   ├── MainWindow.axaml        # Application window
│   └── MainWindow.axaml.cs     # Window initialization
├── App.axaml                    # Application resource definitions
├── App.axaml.cs                # Application entry point
├── Program.cs                  # Main program entry
├── appsettings.json            # Application configuration
└── OGSelector.csproj           # Project file
```

## Configuration

### appsettings.json

The application is configured via `appsettings.json`:

```json
{
  "UI": {
    "Headline": "Device configuration",
    "Subtitle": "Select your Business Unit, Role, and Geography",
    "Information": "Placing your device in the correct Business Unit helps us configure it properly.",
    "ErrorHeadline": "Uh oh!",
    "ErrorSubtitle": "We've encountered an error",
    "ErrorInformation": "An error occurred during the configuration process..."
  },
  "Settings": {
    "jsonURL": "",
    "Fullscreen": true,
    "AllowExit": false,
    "RegKeyPath": "HKEY_LOCAL_MACHINE\\SOFTWARE\\CUSTOMER"
  }
}
```

**Configuration Options:**

- **UI Section**: Customizable text for all user-facing messages
- **jsonURL**: Optional URL to download `inputs.json` from a remote location (or leave blank for local file)
- **Fullscreen**: Set to `true` for kiosk / lockdown mode
- **AllowExit**: Set to `false` to prevent users from closing the application
- **RegKeyPath**: Registry path where selections will be stored

### inputs.json

The application requires an `inputs.json` file that defines available Business Units, Roles, and Geographies:

```json
  "BUs": [
    {
      "uemUuid": "uuid-value-here",
      "uemId": "1001",
      "uemName": "OG Name in UEM",
      "businessUnit": "Display Name for Users",
      "Roles": [
        {"roleName": "Executive", "roleUuid": "role-uuid-1"},
        {"roleName": "Manager", "roleUuid": "role-uuid-2"},
        {"roleName": "Employee", "roleUuid": "role-uuid-3"},
        {"roleName": "Contractor", "roleUuid": "role-uuid-4"}
      ],
      "Geos": [
        {"geoName": "North America", "geoUuid": "geo-uuid-1"},
        {"geoName": "Europe", "geoUuid": "geo-uuid-2"},
        {"geoName": "Asia Pacific", "geoUuid": "geo-uuid-3"},
        {"geoName": "Latin America", "geoUuid": "geo-uuid-4"}
      ],
      "process": [
        {"processName": "Process 1", "processUuid": "process-uuid-1"},
        {"processName": "Process 2", "processUuid": "process-uuid-2"}
      ]
    }
  ]
}
```

**Data Structure:**
- **BUs**: List of Business Units with UEM metadata and nested options
  - `uemUuid`: Unique identifier for the organizational group
  - `uemId`: Numeric ID for the organizational group
  - `uemName`: Display name in UEM system
  - `businessUnit`: User-facing name shown in the UI
  - **Roles**: List of available roles for this BU
    - `roleName`: Display name for the role
    - `roleUuid`: Unique identifier for the role
  - **Geos**: List of geographic locations for this BU
    - `geoName`: Display name for the geography
    - `geoUuid`: Unique identifier for the geography
  - **process**: List of processes for this BU
    - `processName`: Display name for the process
    - `processUuid`: Unique identifier for the process

## Usage

### Running the Application

1. **Standard mode:**
  - Place `inputs.json` and `appsettings.json` in the same directory as `OGSelector.exe`
  - Run `OGSelector.exe`

2. **With remote JSON URL:**
   ```
   OGSelector.exe "https://example.com/inputs.json"
   ```

### User Workflow

1. Application launches and displays the main interface
2. User selects their Business Unit from the dropdown
3. User selects their Process from the dropdown
4. User selects their Role from the dropdown
5. User selects their Geography from the dropdown
6. User clicks the "Submit" button
7. Selected values are written to Windows Registry

### Registry Output

After submission, the following registry keys are created:

**Location:** `HKEY_LOCAL_MACHINE\SOFTWARE\CUSTOMER` (or as configured)

**Keys:**
- `uemUuid`: UUID of the selected organizational group
- `uemId`: ID of the selected organizational group
- `uemName`: Name of the organizational group in UEM
- `BusinessUnit`: Selected business unit name
- `roleName`: Selected role name
- `roleUuid`: UUID of the Tag matching the selected role
- `geoName`: Selected georgaphy name
- `geoUuid`: UUID of the Tag matching the selected geography
- `processName`: Selected process name
- `processUuid`: UUID of the Tag matching the selected process

## Building the Application

### Prerequisites

- .NET 10.0 SDK
- Visual Studio 2022 or VS Code with C# extension

### Development Build

```bash
dotnet build
```

### Release Build (Self-Contained)

Use the provided batch script:

```bash
build_release.bat
```

Or manually:

```bash
dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true
```

The executable will be located at:
```
OGSelector\bin\Release\net10.0\win-x64\publish\OGSelector.exe
```

## Customization

### Custom Logo

Place a `logo.png` file in the same directory as `OGSelector.exe`. The application will automatically display it.

### UI Customization

Modify the `UI` section in `appsettings.json` to customize all user-facing text without recompiling.

### Styling

Avalonia XAML files can be modified to change the appearance:
- [Views/MainView.axaml](OGSelector/Views/MainView.axaml) - Main UI layout
- [Views/MainWindow.axaml](OGSelector/Views/MainWindow.axaml) - Window properties
- [App.axaml](OGSelector/App.axaml) - Global styles and resources

## Architecture

The application follows the **MVVM (Model-View-ViewModel)** pattern:

- **Models** (`InputsData.cs`): Define data structures for Business Units, Roles, and Geos
- **Views** (`.axaml` files): Define the UI layout and appearance
- **ViewModels** (`MainViewModel.cs`): Handle business logic and data binding
- **Services**: 
  - `ConfigurationService.cs`: Manages building application configuration based on `appsettings.json`
  - `JsonDownloadService`: Manages data loading from files or URLs
  - `RegistryService`: Handles Windows Registry operations

### Key Components

1. **ConfigurationService**
   - Loads application settings from `appsettings.json` using Microsoft.Extensions.Configuration
   - Searches for configuration files in current directory first, then falls back to application directory
   - Exports `appsettings.json` from app directory to current directory if not found locally

2. **JsonDownloadService**
   - Validates and downloads JSON from URLs
   - Falls back to local `inputs.json` if URL fails
   - Deserializes JSON into data models

3. **RegistryService**
   - Reads/writes to Windows Registry (64-bit view)
   - Uses configured registry path from settings
   - Handles errors gracefully

4. **MainViewModel**
   - Manages observable collections for UI binding
   - Handles user selection state
   - Coordinates data loading and submission

## Dependencies

- **Avalonia 11.3.11**: Cross-platform UI framework
- **Avalonia.Skia.Lottie**: Lottie animation support
- **CommunityToolkit.Mvvm**: MVVM helpers and attributes
- **AvaloniaSearchableComboBox**: Enhanced ComboBox controls
- **Microsoft.Extensions.Configuration**: Configuration management

## Troubleshooting

### Application won't start
- Ensure .NET 10.0 runtime is installed
- Check that `appsettings.json` is present
- Run from Command Prompt to see error messages

### Data not loading
- Verify `inputs.json` exists in the application directory
- Check JSON syntax is valid
- Review `jsonURL` setting if using remote loading
- Check Debug output for detailed error messages

### Registry keys not created
- Run the application as Administrator
- Verify the `RegKeyPath` in `appsettings.json` is correct
- Check Windows Event Viewer for access denied errors

### Can't close the application
- This is by design if `AllowExit` is set to `false`
- Use Task Manager to force close if needed
- Modify `appsettings.json` and set `AllowExit` to `true`

## Development

### Debug Build

1. Open `OGSelector.sln` in Visual Studio
2. Press F5 to build and debug
3. Or use VS Code with C# Dev Kit

### Running Tasks

Use the VS Code task runner or execute:

```bash
dotnet build
```

### Testing

The application outputs debug information to the Debug console. Enable detailed logging by running in Debug configuration.

## License

Copyright @helmlingp

## Author

Created by helmlingp

## Version History

See commit history for changes and updates.
