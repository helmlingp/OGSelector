# OGSelector - Project Summary

## Overview
A .NET 8 MAUI application designed to download JSON configuration data and allow users to select Business Units, Roles, and Geographies from dropdown menus, with automatic Windows Registry integration.

## What Was Created

### Core Application Files
1. **App.xaml / App.xaml.cs** - Application entry point and resource definitions
2. **AppShell.xaml / AppShell.xaml.cs** - Shell-based navigation (single page app)
3. **MainPage.xaml / MainPage.xaml.cs** - Main UI with dropdown selections and status display
4. **MauiProgram.cs** - Dependency injection and service configuration

### Data Models (Models/)
- **InputsData.cs** - Models for:
  - `InputsData` - Root container for all data
  - `BusinessUnit` - BU with Display and Name properties
  - `RoleItem` - Role with Display and Name properties
  - `GeoItem` - Geography with Display and Name properties

### Services (Services/)
- **JsonDownloadService.cs** - Handles downloading and deserializing JSON from a URL
- **RegistryService.cs** - Manages Windows Registry read/write operations to `HKLM\SOFTWARE\CUSTOMER`

### ViewModels (ViewModels/)
- **MainViewModel.cs** - MVVM implementation using CommunityToolkit.Mvvm
  - Collections for BUs, Roles, and Geos
  - Selected item properties
  - LoadData command for fetching JSON
  - Automatic registry key updates on selection change

### Configuration & Documentation
- **OGSelector.csproj** - Project file with NuGet package references
- **README.md** - Comprehensive documentation
- **QUICKSTART.md** - Setup and testing instructions
- **.gitignore** - Git ignore rules for Visual Studio projects
- **sample_inputs.json** - Example JSON format and data

## How It Works

### User Flow
1. User enters a JSON URL and clicks "Load Data"
2. JsonDownloadService fetches and parses the JSON
3. UI populates three dropdown menus with BUs, Roles, and Geos
4. User selects values from each dropdown
5. ViewModel detects selection changes via property notifications
6. RegistryService automatically writes selected values to Windows Registry

### Registry Integration
When a user makes a selection, the following registry keys are created/updated:
```
HKLM\SOFTWARE\CUSTOMER
├── BUs = "selected_bu_name"
├── Roles = "selected_role_name"
└── Geos = "selected_geo_name"
```

The **Name** property from JSON becomes the registry value, while **Display** is shown to the user.

## JSON Format

The application expects JSON in this format:

```json
{
  "BUs": [
    {"Display": "North America", "Name": "NA"},
    {"Display": "Europe", "Name": "EU"}
  ],
  "Roles": [
    {"Display": "Administrator", "Name": "Admin"},
    {"Display": "User", "Name": "User"}
  ],
  "Geos": [
    {"Display": "United States", "Name": "US"},
    {"Display": "United Kingdom", "Name": "UK"}
  ]
}
```

## Key Features Implemented

✅ **JSON Download** - Downloads and deserializes JSON from configurable URL
✅ **Three Dropdowns** - BUs, Roles, Geos with proper binding
✅ **Registry Integration** - Writes selections to HKLM\SOFTWARE\CUSTOMER
✅ **Real-time Updates** - Registry keys update immediately upon selection
✅ **MVVM Architecture** - Clean separation of concerns with data binding
✅ **Loading States** - Shows loading indicator during data fetch
✅ **Error Handling** - User-friendly error messages
✅ **Status Messages** - Displays current state and feedback
✅ **Selection Display** - Shows current selections in summary panel
✅ **Modern UI** - Dark/light theme support with proper styling

## Technology Stack

- **.NET 8.0** - Latest .NET framework
- **MAUI** - Multi-platform App UI framework
- **MVVM Toolkit** - CommunityToolkit.Mvvm for reactive programming
- **Windows Registry API** - Microsoft.Win32.Registry
- **XAML** - XML-based UI definition language
- **C# 12** - Latest C# language features

## File Structure

```
OGSelector/
├── Models/
│   └── InputsData.cs
├── Services/
│   ├── JsonDownloadService.cs
│   └── RegistryService.cs
├── ViewModels/
│   └── MainViewModel.cs
├── Resources/
│   └── Raw/
│       └── sample_inputs.json
├── App.xaml
├── App.xaml.cs
├── AppShell.xaml
├── AppShell.xaml.cs
├── MainPage.xaml
├── MainPage.xaml.cs
├── MauiProgram.cs
├── OGSelector.csproj
├── README.md
├── QUICKSTART.md
└── .gitignore
```

## Running the Application

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code with C# support
- Windows 10/11 (for registry functionality)

### Commands
```bash
cd /Users/helmlingp/GitHub.work/OGSelector
dotnet restore
dotnet run
```

Or open in Visual Studio 2022 and press F5.

## Customization Points

### JSON URL
Update the default URL in `Services/JsonDownloadService.cs`:
```csharp
private const string JsonUrl = "https://your-server.com/inputs.json";
```

### Registry Path
Change the registry path in `Services/RegistryService.cs`:
```csharp
private const string RegistryPath = @"SOFTWARE\YOUR_PATH";
```

### UI Styling
Modify colors and styles in `App.xaml`:
```xaml
<Color x:Key="PrimaryColor">#512BD4</Color>
```

## Testing

1. **Load Test Data**: Use the sample JSON at `Resources/Raw/sample_inputs.json`
2. **Verify Registry**: Use Registry Editor (regedit) or PowerShell to verify keys
3. **Test Selection**: Select items and verify registry updates immediately
4. **Test Error Handling**: Enter invalid URLs to test error messages

## Next Steps

To use this in production:

1. **Host JSON file** - Place your JSON on a web server or CDN
2. **Update JSON URL** - Set the correct URL in JsonDownloadService
3. **Test with your data** - Verify with your actual JSON structure
4. **Deploy** - Build and distribute the executable

## Support

For questions or modifications, refer to:
- `README.md` - Full documentation
- `QUICKSTART.md` - Setup and troubleshooting
- Code comments throughout the project for implementation details
