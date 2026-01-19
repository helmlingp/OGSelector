using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OGSelector.Models;
using OGSelector.Services;

namespace OGSelector.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly JsonDownloadService _jsonDownloadService;
    private readonly RegistryService _registryService;

    [ObservableProperty]
    private ObservableCollection<BusinessUnit> businessUnits = new();

    [ObservableProperty]
    private ObservableCollection<RoleItem> roles = new();

    [ObservableProperty]
    private ObservableCollection<GeoItem> geos = new();

    [ObservableProperty]
    private BusinessUnit? selectedBusinessUnit;

    [ObservableProperty]
    private RoleItem? selectedRole;

    [ObservableProperty]
    private GeoItem? selectedGeo;

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string statusMessage = "Ready to load data";

    [ObservableProperty]
    private string jsonUrl = "";

    public MainViewModel()
    {
        _jsonDownloadService = new JsonDownloadService();
        _registryService = new RegistryService();
    }

    [RelayCommand]
    public async Task LoadData()
    {
        if (string.IsNullOrWhiteSpace(JsonUrl))
        {
            StatusMessage = "Please enter a JSON URL";
            return;
        }

        IsLoading = true;
        StatusMessage = "Loading data...";

        var data = await _jsonDownloadService.DownloadInputsDataAsync(JsonUrl);
        
        if (data != null)
        {
            BusinessUnits = new ObservableCollection<BusinessUnit>(data.BusinessUnits);
            Roles = new ObservableCollection<RoleItem>(data.Roles);
            Geos = new ObservableCollection<GeoItem>(data.Geos);
            StatusMessage = "Data loaded successfully";
        }
        else
        {
            StatusMessage = "Failed to load data";
        }

        IsLoading = false;
    }

    [RelayCommand]
    public void Submit()
    {
        if (SelectedBusinessUnit == null || SelectedRole == null || SelectedGeo == null)
        {
            StatusMessage = "Please select a Business Unit, Role, and Geography";
            return;
        }

        try
        {
            _registryService.SetRegistryKey("BUs", SelectedBusinessUnit.Name);
            _registryService.SetRegistryKey("Roles", SelectedRole.Name);
            _registryService.SetRegistryKey("Geos", SelectedGeo.Name);
            StatusMessage = "Selections saved to registry successfully";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving to registry: {ex.Message}";
        }
    }
}
