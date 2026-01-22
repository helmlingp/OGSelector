using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
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
    private string statusMessage = "";

    [ObservableProperty]
    private string jsonUrl = "";

    [ObservableProperty]
    private bool hasError = false;

    public MainViewModel()
    {
        _jsonDownloadService = new JsonDownloadService();
        _registryService = new RegistryService();
    }

    public async Task LoadDataAsync(string? jsonUrl = null)
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Loading data...";

            var inputsData = await _jsonDownloadService.LoadFromFileOrUrlAsync(jsonUrl);

            if (inputsData != null)
            {
                BusinessUnits = new ObservableCollection<BusinessUnit>(inputsData.BusinessUnits);
                Roles = new ObservableCollection<RoleItem>(inputsData.Roles);
                Geos = new ObservableCollection<GeoItem>(inputsData.Geos);
                HasError = false;
                StatusMessage = "";
                System.Diagnostics.Debug.WriteLine("Data loaded successfully into ViewModel");
            }
            else
            {
                // inputs.json not found - show error
                HasError = true;
                StatusMessage = "";
                System.Diagnostics.Debug.WriteLine("Failed to load inputs.json - showing error panel");
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            StatusMessage = $"Error loading data: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"Error in LoadDataAsync: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
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
            _registryService.SetRegistryKey("OG", SelectedBusinessUnit.UemName);
            _registryService.SetRegistryKey("BU", SelectedBusinessUnit.BusinessUnitName);
            _registryService.SetRegistryKey("Roles", SelectedRole.RoleName);
            _registryService.SetRegistryKey("Geos", SelectedGeo.GeoName);
            StatusMessage = "Selections saved to registry successfully";
            
            // Close the application after successful submission
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving to registry: {ex.Message}";
        }
    }

    [RelayCommand]
    public void CloseApp()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}
