using Microsoft.Win32;

namespace OGSelector.Services;

public class RegistryService
{
    private const string RegistryPath = @"SOFTWARE\CUSTOMER";
    private const RegistryHive RootHive = RegistryHive.LocalMachine;

    public void SetRegistryKey(string keyName, string value)
    {
        try
        {
            using (var key = RegistryKey.OpenBaseKey(RootHive, RegistryView.Registry64))
            using (var subKey = key.OpenSubKey(RegistryPath, writable: true) ?? key.CreateSubKey(RegistryPath))
            {
                subKey?.SetValue(keyName, value, RegistryValueKind.String);
                Debug.WriteLine($"Registry key set: {keyName} = {value}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error setting registry key: {ex.Message}");
        }
    }

    public string? GetRegistryKey(string keyName)
    {
        try
        {
            using (var key = RegistryKey.OpenBaseKey(RootHive, RegistryView.Registry64))
            using (var subKey = key.OpenSubKey(RegistryPath))
            {
                var value = subKey?.GetValue(keyName) as string;
                return value;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error reading registry key: {ex.Message}");
            return null;
        }
    }
}
