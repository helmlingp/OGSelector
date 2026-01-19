# OG Selector - MAUI App

A .NET 8 MAUI application for selecting Business Units, Roles, and Geographies from a JSON configuration file, with automatic Windows Registry integration.

## Features

- **JSON Download**: Download configuration data from a remote JSON source
- **Dropdown Selections**: Three dropdowns for BUs (Business Units), Roles, and Geos (Geographies)
- **Registry Integration**: Automatically writes selections to Windows Registry at `HKLM\SOFTWARE\CUSTOMER`
- **Real-time Updates**: Registry values update immediately upon selection
- **Cross-platform UI**: Works on Windows with MAUI framework

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code with C# support
- Windows 10/11 (for registry functionality)

## Project Structure

```
OGSelector/
├── Models/
│   └── InputsData.cs          # Data models for JSON deserialization
├── Services/
│   ├── JsonDownloadService.cs # JSON download functionality
│   └── RegistryService.cs     # Windows Registry access
├── ViewModels/
│   └── MainViewModel.cs       # MVVM ViewModel with property binding
├── MainPage.xaml              # Main UI layout
├── MainPage.xaml.cs           # Code-behind
├── App.xaml                   # Application resources
├── AppShell.xaml              # Shell navigation
├── MauiProgram.cs             # Dependency injection setup
└── OGSelector.csproj          # Project configuration
```

## Building and Running

### From Command Line

```bash
# Restore NuGet packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

### From Visual Studio

1. Open the solution in Visual Studio 2022
2. Set `OGSelector` as the startup project
3. Select the target platform (Windows)
4. Press F5 to run

## JSON Format

The app expects a JSON file with the following structure:

```json
{
  "BUs": [
    {
      "Display": "Display Name",
      "Name": "RegistryValue"
    }
  ],
  "Roles": [
    {
      "Display": "Display Name",
      "Name": "RegistryValue"
    }
  ],
  "Geos": [
    {
      "Display": "Display Name",
      "Name": "RegistryValue"
    }
  ]
}
```

- **Display**: The text shown in the UI dropdown
- **Name**: The value stored in the Windows Registry

## Usage

1. Launch the application
2. Enter the URL to your JSON configuration file in the text field
3. Click "Load Data" to fetch and parse the JSON
4. Select values from the three dropdowns (BU, Role, Geography)
5. Selected values automatically update the Windows Registry at:
   - `HKLM\SOFTWARE\CUSTOMER\BUs`
   - `HKLM\SOFTWARE\CUSTOMER\Roles`
   - `HKLM\SOFTWARE\CUSTOMER\Geos`

## Registry Keys

The application creates/updates the following registry keys:

| Key | Value | Example |
|-----|-------|---------|
| `HKLM\SOFTWARE\CUSTOMER\BUs` | Selected BU Name | `NA` |
| `HKLM\SOFTWARE\CUSTOMER\Roles` | Selected Role Name | `Admin` |
| `HKLM\SOFTWARE\CUSTOMER\Geos` | Selected Geo Name | `US` |

## Dependencies

- `Microsoft.Maui.Controls` - MAUI framework
- `CommunityToolkit.Mvvm` - MVVM pattern support
- `System.Text.Json` - JSON deserialization (built-in)

## Notes

- Registry operations require appropriate Windows permissions
- The app stores selections in the local machine registry (HKLM)
- Internet connection is required to download the JSON file
- For testing, a sample JSON file is included at `Resources/Raw/sample_inputs.json`

## Future Enhancements

- [ ] Local caching of downloaded JSON
- [ ] Settings for registry path customization
- [ ] Error logging and telemetry
- [ ] Configuration file for default URL
- [ ] Support for additional platforms (macOS, Linux via WSL)

## License

This project is provided as-is for internal use.
