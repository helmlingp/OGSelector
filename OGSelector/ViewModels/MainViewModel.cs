using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    private ObservableCollection<ProcessItem> processes = new();

    [ObservableProperty]
    private BusinessUnit? selectedBusinessUnit;

    [ObservableProperty]
    private RoleItem? selectedRole;

    [ObservableProperty]
    private GeoItem? selectedGeo;

    [ObservableProperty]
    private ProcessItem? selectedProcess;

    [ObservableProperty]
    private bool hasProcess = false;

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
                
                SelectedBusinessUnit = BusinessUnits.FirstOrDefault();
                Roles = new ObservableCollection<RoleItem>(inputsData.Roles);
                Geos = new ObservableCollection<GeoItem>(inputsData.Geos);
                Processes = new ObservableCollection<ProcessItem>();
                HasProcess = Processes.Count > 0;
                // SelectedProcess = null;
                SelectedProcess = Processes.FirstOrDefault();
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

        if (HasProcess && SelectedProcess == null)
        {
            StatusMessage = "Please select a Process";
            return;
        }

        try
        {
            _registryService.SetRegistryKey("OGUuid", SelectedBusinessUnit.UemUuid);
            _registryService.SetRegistryKey("OGid", SelectedBusinessUnit.UemId);
            _registryService.SetRegistryKey("OGName", SelectedBusinessUnit.UemName);
            _registryService.SetRegistryKey("BUName", SelectedBusinessUnit.BusinessUnitName);
            if (SelectedProcess != null)
            {
                _registryService.SetRegistryKey("Process", SelectedProcess.ProcessName);
            }
            else
            {
                _registryService.SetRegistryKey("Process", string.Empty);
            }
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

    partial void OnSelectedBusinessUnitChanged(BusinessUnit? value)
    {
        Processes = new ObservableCollection<ProcessItem>(value?.Process ?? new List<ProcessItem>());
        HasProcess = Processes.Count > 0;
        SelectedProcess = null;
    }
}
