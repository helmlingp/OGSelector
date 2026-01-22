using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
// using Microsoft.Extensions.Configuration;
using OGSelector.Models;

namespace OGSelector.Services;

public class JsonDownloadService
{
    private readonly HttpClient _httpClient;

    public JsonDownloadService()
    {
        _httpClient = new HttpClient();
    }

    private bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    public async Task<InputsData?> LoadFromFileOrUrlAsync(string? url = null)
    {
        try
        {
            string? jsonUrl = null;

            // Check if URL is provided and is valid
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (IsValidUrl(url))
                {
                    jsonUrl = url;
                    Debug.WriteLine($"Valid jsonURL found: {jsonUrl}");
                }
                else
                {
                    Debug.WriteLine($"Invalid URL in jsonURL parameter: {url}");
                }
            }

            // Get the current exe directory
            var exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var inputsJsonPath = Path.Combine(exeDirectory, "inputs.json");

            // If jsonURL exists and is valid, download and save to inputs.json
            if (!string.IsNullOrWhiteSpace(jsonUrl))
            {
                try
                {
                    var response = await _httpClient.GetAsync(jsonUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        await File.WriteAllTextAsync(inputsJsonPath, jsonContent);
                        Debug.WriteLine($"Downloaded and saved JSON to: {inputsJsonPath}");
                    }
                    else
                    {
                        Debug.WriteLine($"Failed to download JSON: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error downloading JSON: {ex.Message}");
                }
            }

            // Now read inputs.json and deserialize to InputsData
            if (File.Exists(inputsJsonPath))
            {
                var jsonContent = await File.ReadAllTextAsync(inputsJsonPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<InputsData>(jsonContent, options);
                
                Debug.WriteLine($"Successfully loaded inputs.json from: {inputsJsonPath}");
                return data;
            }
            else
            {
                Debug.WriteLine($"inputs.json not found in: {exeDirectory}");
            }

            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during LoadFromFileOrUrlAsync: {ex.Message}");
            return null;
        }
    }
}
