# Implementation Details & Examples

## How Selections Update Registry

### The Data Flow

1. **User selects from dropdown** → XAML Picker control
2. **SelectedItem binding** → Triggers ViewModel property
3. **Property changed event** → OnSelectedChanged handler
4. **RegistryService.SetRegistryKey()** → Writes to HKLM\SOFTWARE\CUSTOMER

### Code Example

In `MainViewModel.cs`:

```csharp
// This property is decorated with [ObservableProperty]
[ObservableProperty]
private BusinessUnit? selectedBusinessUnit;

// When selection changes, this method is automatically called
partial void OnSelectedBusinessUnitChanged(BusinessUnit? value)
{
    if (value != null)
    {
        // Automatically set registry key
        _registryService.SetRegistryKey("BUs", value.Name);
    }
}
```

The **MVVM Toolkit** generates the property change notification automatically, making this very clean.

## JSON Deserialization Example

The `JsonDownloadService` automatically maps JSON to your C# models:

```json
{
  "BUs": [
    {
      "Display": "North America",
      "Name": "NA"
    }
  ]
}
```

Maps to:

```csharp
var data = JsonSerializer.Deserialize<InputsData>(jsonContent, options);
// data.BusinessUnits[0].Display == "North America"
// data.BusinessUnits[0].Name == "NA"
```

The `[JsonPropertyName]` attributes in the models handle the property name mapping.

## Registry Operations

### Writing a Value

```csharp
_registryService.SetRegistryKey("BUs", "NA");
// Creates/updates: HKLM\SOFTWARE\CUSTOMER\BUs = "NA"
```

### Reading a Value

```csharp
var value = _registryService.GetRegistryKey("BUs");
// Returns: "NA"
```

### Registry Key Structure

```
HKEY_LOCAL_MACHINE
└── SOFTWARE
    └── CUSTOMER (created automatically if doesn't exist)
        ├── BUs (REG_SZ) = "NA"
        ├── Roles (REG_SZ) = "Admin"
        └── Geos (REG_SZ) = "US"
```

## MVVM Pattern Usage

The app uses the **CommunityToolkit.Mvvm** library which provides:

### Observable Properties
```csharp
[ObservableProperty]
private ObservableCollection<BusinessUnit> businessUnits = new();
// Generates: public ObservableCollection<BusinessUnit> BusinessUnits property
```

### Commands
```csharp
[RelayCommand]
public async Task LoadData()
{
    // Command accessible as: LoadDataCommand
}
```

### Change Notifications
```csharp
partial void OnSelectedBusinessUnitChanged(BusinessUnit? value)
{
    // Auto-called when SelectedBusinessUnit changes
}
```

## Data Binding Examples

### One-way Binding (Display only)
```xaml
<Label Text="{Binding StatusMessage}" />
```

### Two-way Binding (Input)
```xaml
<Entry Text="{Binding JsonUrl}" />
```

### Item Binding (Collections)
```xaml
<Picker ItemsSource="{Binding BusinessUnits}"
        SelectedItem="{Binding SelectedBusinessUnit}"
        ItemDisplayBinding="{Binding Display}" />
```

### Conditional Visibility
```xaml
<ActivityIndicator IsRunning="{Binding IsLoading}"
                   IsVisible="{Binding IsLoading}" />
```

## Adding More Dropdowns

If you need to add additional selections:

1. **Add to JSON model** - Update `InputsData.cs`:
```csharp
[JsonPropertyName("NewField")]
public List<NewItem> NewItems { get; set; } = new();
```

2. **Add to ViewModel** - Update `MainViewModel.cs`:
```csharp
[ObservableProperty]
private ObservableCollection<NewItem> newItems = new();

[ObservableProperty]
private NewItem? selectedNewItem;

partial void OnSelectedNewItemChanged(NewItem? value)
{
    if (value != null)
        _registryService.SetRegistryKey("NewField", value.Name);
}
```

3. **Add to UI** - Update `MainPage.xaml`:
```xaml
<VerticalStackLayout Spacing="8">
    <Label Text="New Field:" Style="{StaticResource LabelStyle}" FontAttributes="Bold" />
    <Picker Title="Select an Item"
            ItemsSource="{Binding NewItems}"
            SelectedItem="{Binding SelectedNewItem}"
            ItemDisplayBinding="{Binding Display}" />
</VerticalStackLayout>
```

4. **Update JSON download** - In `MainViewModel.LoadData()`:
```csharp
NewItems = new ObservableCollection<NewItem>(data.NewItems);
```

## Customizing Registry Path

If you need a different registry location:

1. **Edit `RegistryService.cs`**:
```csharp
private const string RegistryPath = @"SOFTWARE\MYCOMPANY\MYAPP";
private const RegistryHive RootHive = RegistryHive.LocalMachine;
```

You can also use:
- `RegistryHive.CurrentUser` for HKCU
- `RegistryHive.LocalMachine` for HKLM (current)
- `RegistryHive.ClassesRoot` for HKCR

## Error Handling Strategy

The app uses try-catch blocks with logging:

```csharp
try
{
    // Attempt operation
    subKey?.SetValue(keyName, value, RegistryValueKind.String);
}
catch (Exception ex)
{
    // Log error without crashing
    Debug.WriteLine($"Error: {ex.Message}");
    // Optionally update UI: StatusMessage = "Error occurred"
}
```

All errors are logged to the Debug output and displayed to the user as status messages.

## Testing the Implementation

### Test 1: JSON Loading
1. Provide a valid JSON URL
2. Click "Load Data"
3. Verify dropdowns populate

### Test 2: Selection & Registry
1. Select item from dropdown
2. Open Registry Editor: `regedit`
3. Navigate to `HKLM\SOFTWARE\CUSTOMER`
4. Verify registry key was updated

### Test 3: Error Handling
1. Enter invalid URL
2. Click "Load Data"
3. Verify error message appears

### Test 4: Persistence
1. Load data and make selections
2. Close and reopen app
3. Registry values should still exist (but UI won't remember selections)

## Performance Notes

- **JSON Download**: Async/await prevents UI blocking
- **Registry Operations**: Synchronous but very fast (< 10ms)
- **Binding**: Data binding handles updates efficiently
- **Collections**: Using `ObservableCollection<T>` for dynamic binding support
