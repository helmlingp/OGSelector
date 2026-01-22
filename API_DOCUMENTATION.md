# API Documentation

## Services

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
service.SetRegistryKey("BusinessUnit", "Marketing");
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
var value = service.GetRegistryKey("BusinessUnit");
if (value != null)
{
    Console.WriteLine($"Business Unit: {value}");
}
```

---

## ViewModels

### MainViewModel

The main view model that manages application state and user interactions.

#### Properties

##### Observable Collections

- **`BusinessUnits`** (`ObservableCollection<BusinessUnit>`): List of available business units
- **`Roles`** (`ObservableCollection<RoleItem>`): List of available roles
- **`Geos`** (`ObservableCollection<GeoItem>`): List of available geographies

##### Selected Items

- **`SelectedBusinessUnit`** (`BusinessUnit?`): Currently selected business unit
- **`SelectedRole`** (`RoleItem?`): Currently selected role
- **`SelectedGeo`** (`GeoItem?`): Currently selected geography

##### UI State

- **`IsLoading`** (`bool`): Indicates if data is currently being loaded
- **`StatusMessage`** (`string`): Status or error message to display
- **`HasError`** (`bool`): Indicates if an error has occurred

#### Methods

##### `LoadDataAsync(string? jsonUrl = null)`

Loads data from JSON source and populates the collections.

**Parameters:**
- `jsonUrl` (string?, optional): URL to load JSON from

**Returns:**
- `Task`: Async task

**Effects:**
- Sets `IsLoading` to true during loading
- Populates `BusinessUnits`, `Roles`, and `Geos` collections
- Sets `HasError` to true if loading fails
- Updates `StatusMessage` with error details

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
- Ensures all three selections (BusinessUnit, Role, Geo) are made
- Sets `StatusMessage` if validation fails

**Registry Keys Written:**
- `BusinessUnit`: Selected business unit name
- `Role`: Selected role name
- `Geography`: Selected geography name
- `uemUuid`: UUID from selected business unit
- `uemId`: ID from selected business unit
- `uemName`: Name from selected business unit

---

## Models

### InputsData

Root container for all configuration data.

**Properties:**
- `BusinessUnits` (`List<BusinessUnit>`): List of business units
- `Roles` (`List<RoleItem>`): List of roles
- `Geos` (`List<GeoItem>`): List of geographies

**JSON Mapping:**
```json
{
  "BUs": [...],
  "Roles": [...],
  "Geos": [...]
}
```

---

### BusinessUnit

Represents an organizational group/business unit.

**Properties:**
- `UemUuid` (string): Unique identifier for the OG
- `UemId` (string): Numeric identifier
- `UemName` (string): Name in UEM system
- `BusinessUnitName` (string): User-facing name
- `Display` (string): Computed property for UI binding (returns BusinessUnitName)

**JSON Example:**
```json
{
  "uemUuid": "a1b2c3d4-e5f6-7890",
  "uemId": "1001",
  "uemName": "Marketing OG",
  "businessUnit": "Marketing"
}
```

---

### RoleItem

Represents a user role.

**Properties:**
- `RoleName` (string): Name of the role
- `Display` (string): Computed property for UI binding (returns RoleName)

**JSON Example:**
```json
{
  "roleName": "Manager"
}
```

---

### GeoItem

Represents a geographic location.

**Properties:**
- `GeoName` (string): Name of the geography
- `Display` (string): Computed property for UI binding (returns GeoName)

**JSON Example:**
```json
{
  "geoName": "North America"
}
```

---

## Configuration Classes

### Settings (in MainWindow.axaml.cs)

Application settings loaded from `appsettings.json`.

**Properties:**
- `jsonURL` (string): URL to download inputs.json from
- `Fullscreen` (bool): Enable full-screen mode
- `AllowExit` (bool): Allow user to close the application
- `RegKeyPath` (string): Full registry path (e.g., "HKEY_LOCAL_MACHINE\\SOFTWARE\\CUSTOMER")

---

### UI (in MainView.axaml.cs)

UI text configuration loaded from `appsettings.json`.

**Properties:**
- `Headline` (string): Main heading text
- `Subtitle` (string): Subtitle text
- `Information` (string): Informational message
- `ErrorHeadline` (string): Error heading
- `ErrorSubtitle` (string): Error subtitle
- `ErrorInformation` (string): Error message details

---

## Events and Lifecycle

### Application Startup

1. `Program.Main()` called
2. Avalonia app builder configured
3. `App.Initialize()` called
4. `App.OnFrameworkInitializationCompleted()` creates MainWindow
5. `MainWindow` constructor:
   - Creates MainViewModel
   - Configures window state (fullscreen, exit prevention)
   - Calls `InitializeAsync()`
6. `InitializeAsync()`:
   - Checks for command-line arguments
   - Loads jsonURL from settings if no argument
   - Calls `viewModel.LoadDataAsync()`

### Data Loading

1. `LoadDataAsync()` called
2. `JsonDownloadService.LoadFromFileOrUrlAsync()` invoked
3. If URL provided:
   - Download JSON
   - Save to local `inputs.json`
4. Read local `inputs.json`
5. Deserialize to InputsData
6. Populate ViewModel collections
7. UI updates via data binding

### User Submission

1. User selects all three dropdowns
2. User clicks Submit button
3. `SubmitCommand` executed
4. Validation performed
5. If valid:
   - `RegistryService.SetRegistryKey()` called for each value
   - Application exits (if AllowExit is true)
6. If invalid:
   - StatusMessage updated
   - User notified to complete selections
