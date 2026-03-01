# API Documentation

## Services

### ConfigurationService

Manages loading application configuration from `appsettings.json`.

#### Methods

##### `BuildConfiguration()`

Builds and returns an IConfiguration object from `appsettings.json`.

**Returns:**
- `IConfiguration`: Configuration object with all settings

**Behavior:**
1. Checks current working directory for `appsettings.json`
2. Falls back to application directory if not found in current directory
3. Exports configuration file from app directory to current directory if needed
4. Loads from environment variables as override layer
5. Returns built configuration object

**Example:**
```csharp
var config = ConfigurationService.BuildConfiguration();
var settings = config.GetSection("Settings").Get<Settings>();
```

---

### JsonDownloadService

Handles loading configuration data from local files or remote URLs.

#### Methods

##### `LoadFromFileOrUrlAsync(string? url = null)`

Loads `inputs.json` data from a URL or local file.

**Parameters:**
- `url` (string?, optional): URL to download JSON from. If null or invalid, uses local file.

**Returns:**
- `Task<InputsData?>`: Deserialized InputsData object, or null if loading fails

**Behavior:**
1. Validates the provided URL
2. If URL is valid, attempts to download JSON and save to local `inputs.json`
3. Reads local `inputs.json` file
4. Deserializes JSON into InputsData object
5. Returns null if file doesn't exist or deserialization fails

**Example:**
```csharp
var service = new JsonDownloadService();
var data = await service.LoadFromFileOrUrlAsync("https://example.com/inputs.json");
if (data != null)
{
    // Use the loaded data
}
```

---

### RegistryService

Provides read/write operations for Windows Registry.

#### Methods

##### `SetRegistryKey(string keyName, string value)`

Writes a string value to the configured registry path.

**Parameters:**
- `keyName` (string): Name of the registry value to set
- `value` (string): String value to write

**Registry Location:**
- Hive: `HKEY_LOCAL_MACHINE`
- View: 64-bit
- Path: Configured in `appsettings.json` (Settings.RegKeyPath)

**Example:**
```csharp
var service = new RegistryService();
service.SetRegistryKey("BUName", "Marketing");
```

##### `GetRegistryKey(string keyName)`

Reads a string value from the configured registry path.

**Parameters:**
- `keyName` (string): Name of the registry value to read

**Returns:**
- `string?`: The registry value as a string, or null if not found

**Example:**
```csharp
var service = new RegistryService();
var value = service.GetRegistryKey("BUName");
if (value != null)
{
    Console.WriteLine($"Business Unit: {value}");
}
```

##### `DeleteRegistryKeyPath()`

Deletes the entire configured registry path and all values.

**Example:**
```csharp
var service = new RegistryService();
service.DeleteRegistryKeyPath();
```

---

## ViewModels

### MainViewModel

The main view model that manages application state and user interactions.

#### Properties

##### Observable Collections

- **`BusinessUnits`** (`ObservableCollection<BusinessUnit>`): List of available business units
- **`Processes`** (`ObservableCollection<ProcessItem>`): List of available processes (if applicable)
- **`Roles`** (`ObservableCollection<RoleItem>`): List of available roles (if applicable)
- **`Geos`** (`ObservableCollection<GeoItem>`): List of available geographies (if applicable)

##### Selected Items

- **`SelectedBusinessUnit`** (`BusinessUnit?`): Currently selected business unit
- **`SelectedProcess`** (`ProcessItem?`): Currently selected process
- **`SelectedRole`** (`RoleItem?`): Currently selected role
- **`SelectedGeo`** (`GeoItem?`): Currently selected geography

##### UI State

- **`IsLoading`** (`bool`): Indicates if data is currently being loaded
- **`StatusMessage`** (`string`): Status or error message to display
- **`HasProcess`** (`bool`): Indicates if processes are available for selected BU
- **`HasRoles`** (`bool`): Indicates if roles are available for selected BU
- **`HasGeos`** (`bool`): Indicates if geographies are available for selected BU

#### Methods

##### `LoadDataAsync(string? jsonUrl = null)`

Loads data from JSON source and populates the collections.

**Parameters:**
- `jsonUrl` (string?, optional): URL to load JSON from

**Returns:**
- `Task`: Async task

**Effects:**
- Sets `IsLoading` to true during loading
- Populates `BusinessUnits` collection
- Sets availability flags (`HasProcess`, `HasRoles`, `HasGeos`)
- Updates `StatusMessage` with error details if loading fails

**Example:**
```csharp
var viewModel = new MainViewModel();
await viewModel.LoadDataAsync("https://example.com/inputs.json");
```

##### `Submit()` [RelayCommand]

Validates selections and writes them to the registry.

**Command Binding:**
```xml
<Button Command="{Binding SubmitCommand}" />
```

**Validation:**
- Ensures Business Unit is selected (required)
- Ensures Process is selected if processes are available
- Ensures Role is selected if roles are available
- Ensures Geography is selected if geographies are available
- Sets `StatusMessage` if validation fails

**Registry Keys Written:**
- `OGUuid`: UUID from selected business unit
- `OGid`: ID from selected business unit
- `OGName`: Name from selected business unit
- `BUName`: Display name of selected business unit
- `Process`: Selected process name (empty if N/A)
- `ProcessTagUuid`: UUID of selected process (empty if N/A)
- `Roles`: Selected role name (empty if N/A)
- `RolesTagUuid`: UUID of selected role (empty if N/A)
- `Geos`: Selected geography name (empty if N/A)
- `GeosTagUuid`: UUID of selected geography (empty if N/A)

**Example:**
```csharp
// Via XAML binding
<Button Command="{Binding SubmitCommand}" Content="Submit" />
```

---

## Models

### InputsData

Root container for all configuration data.

**Properties:**
- `BusinessUnits` (`List<BusinessUnit>`): List of business units

**JSON Mapping:**
```json
{
  "BUs": [...]
}
```

---

### BusinessUnit

Represents an organizational group/business unit with associated options.

**Properties:**
- `UemUuid` (string): Unique identifier for the OG
- `UemId` (string): Numeric identifier
- `UemName` (string): Name in UEM system
- `BusinessUnitName` (string): User-facing name
- `Roles` (`List<RoleItem>`): Available roles for this BU
- `Geos` (`List<GeoItem>`): Available geographies for this BU
- `Process` (`List<ProcessItem>`): Available processes for this BU
- `Display` (string): Computed property for UI binding (returns BusinessUnitName)

**JSON Example:**
```json
{
  "uemUuid": "a1b2c3d4-e5f6-7890",
  "uemId": "1001",
  "uemName": "Marketing OG",
  "businessUnit": "Marketing",
  "Roles": [...],
  "Geos": [...],
  "process": [...]
}
```

---

### RoleItem

Represents a user role.

**Properties:**
- `RoleName` (string): Name of the role
- `RoleTagUuid` (string): UUID of the role tag
- `Display` (string): Computed property for UI binding (returns RoleName)

**JSON Example:**
```json
{
  "roleName": "Manager",
  "roleUuid": "role-uuid-12345"
}
```

---

### GeoItem

Represents a geographic location.

**Properties:**
- `GeoName` (string): Name of the geography
- `GeoTagUuid` (string): UUID of the geography tag
- `Display` (string): Computed property for UI binding (returns GeoName)

**JSON Example:**
```json
{
  "geoName": "North America",
  "geoUuid": "geo-uuid-12345"
}
```

---

### ProcessItem

Represents a process or workflow.

**Properties:**
- `ProcessName` (string): Name of the process
- `ProcessTagUuid` (string): UUID of the process tag
- `Display` (string): Computed property for UI binding (returns ProcessName)

**JSON Example:**
```json
{
  "processName": "Onboarding",
  "processUuid": "process-uuid-12345"
}
```

---

## Configuration Classes

### Settings

Application settings loaded from `appsettings.json` under the `Settings` section.

**Properties:**
- `jsonURL` (string): URL to download inputs.json from (empty for local file)
- `Fullscreen` (bool): Enable full-screen mode for kiosk scenarios
- `AllowExit` (bool): Allow user to close the application
- `RegKeyPath` (string): Full registry path (e.g., "HKEY_LOCAL_MACHINE\\SOFTWARE\\CUSTOMER")

**JSON Example:**
```json
{
  "Settings": {
    "jsonURL": "https://example.com/inputs.json",
    "Fullscreen": true,
    "AllowExit": false,
    "RegKeyPath": "HKEY_LOCAL_MACHINE\\SOFTWARE\\CUSTOMER"
  }
}
```

---

### UI

UI text configuration loaded from `appsettings.json` under the `UI` section.

**Properties:**
- `Headline` (string): Main heading text
- `Subtitle` (string): Subtitle text
- `Information` (string): Informational message
- `ErrorHeadline` (string): Error heading
- `ErrorSubtitle` (string): Error subtitle
- `ErrorInformation` (string): Error message details

**JSON Example:**
```json
{
  "UI": {
    "Headline": "Device Configuration",
    "Subtitle": "Select your Business Unit, Role, and Geography",
    "Information": "Placing your device in the correct Business Unit...",
    "ErrorHeadline": "Uh oh!",
    "ErrorSubtitle": "We've encountered an error",
    "ErrorInformation": "An error occurred during configuration..."
  }
}
```

---

## Events and Lifecycle

### Application Startup

1. `Program.Main()` called
2. Avalonia app builder configured with OGSelector as the app
3. `.Startup` event triggers, then `.FrameworkInitializationCompleted`
4. `App.OnFrameworkInitializationCompleted()` creates MainWindow
5. `MainWindow` constructor:
   - Creates MainViewModel
   - Loads configuration via ConfigurationService
   - Configures window properties (fullscreen, exit prevention)
   - Calls `InitializeAsync()`
6. `InitializeAsync()`:
   - Checks for command-line arguments containing JSON URL
   - Falls back to `jsonURL` from appsettings.json
   - Calls `viewModel.LoadDataAsync(url)`
7. MainViewModel begins loading and populating UI collections

### Data Loading

1. User provides URL via argument or configuration, or local file is used
2. `JsonDownloadService.LoadFromFileOrUrlAsync()` invoked
3. If URL provided:
   - Validates URL format
   - Downloads JSON from URL
   - Saves to local `inputs.json` for caching
4. Reads local `inputs.json`
5. Deserializes JSON to InputsData object
6. Populates BusinessUnits collection
7. Sets availability flags for Process/Role/Geo based on data
8. UI updates automatically via data binding

### User Submission

1. User selects Business Unit (required)
2. User selects Process (if available)
3. User selects Role (if available)
4. User selects Geography (if available)
5. User clicks Submit button
6. `SubmitCommand` executed
7. Validation performed:
   - All required fields checked
   - StatusMessage set if validation fails
8. If valid:
   - Registry service deletes previous registry path
   - Registry keys written for each selected item
   - Status message confirms success
   - Application closes (if AllowExit is true)
9. If invalid:
   - StatusMessage updated with validation error
   - Application remains open for user correction
