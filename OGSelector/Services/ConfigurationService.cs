using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace OGSelector.Services;

public class ConfigurationService
{
    public static IConfiguration BuildConfiguration()
    {
        // Get current working directory and application directory
        var currentDir = Directory.GetCurrentDirectory();
        var appDir = AppDomain.CurrentDomain.BaseDirectory;
        
        var currentDirSettingsPath = Path.Combine(currentDir, "appsettings.json");
        var appDirSettingsPath = Path.Combine(appDir, "appsettings.json");

        // Check if appsettings.json exists in current directory
        if (!File.Exists(currentDirSettingsPath) && File.Exists(appDirSettingsPath))
        {
            try
            {
                // Export/copy appsettings.json from app directory to current directory
                File.Copy(appDirSettingsPath, currentDirSettingsPath, overwrite: false);
                Debug.WriteLine($"Exported appsettings.json to: {currentDirSettingsPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to export appsettings.json: {ex.Message}");
            }
        }

        // Build configuration from current directory (or fall back to app directory)
        var builder = new ConfigurationBuilder();
        
        if (File.Exists(currentDirSettingsPath))
        {
            builder.AddJsonFile(currentDirSettingsPath);
            Debug.WriteLine($"Loading appsettings.json from: {currentDirSettingsPath}");
        }
        else if (File.Exists(appDirSettingsPath))
        {
            builder.AddJsonFile(appDirSettingsPath);
            Debug.WriteLine($"Loading appsettings.json from: {appDirSettingsPath}");
        }
        
        builder.AddEnvironmentVariables();
        return builder.Build();
    }
}
