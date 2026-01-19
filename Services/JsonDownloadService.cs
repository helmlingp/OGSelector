using System.Text.Json;
using OGSelector.Models;

namespace OGSelector.Services;

public class JsonDownloadService
{
    private readonly HttpClient _httpClient;
    private const string JsonUrl = "https://your-server.com/inputs.json"; // Update with your actual URL

    public JsonDownloadService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<InputsData?> DownloadInputsDataAsync(string? url = null)
    {
        try
        {
            var targetUrl = url ?? JsonUrl;
            var response = await _httpClient.GetAsync(targetUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Failed to download JSON: {response.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<InputsData>(content, options);
            
            return data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error downloading JSON: {ex.Message}");
            return null;
        }
    }

    public async Task<InputsData?> LoadFromFileOrUrlAsync(string? url = null)
    {
        // If URL is provided, use it
        if (!string.IsNullOrWhiteSpace(url))
        {
            return await DownloadInputsDataAsync(url);
        }

        // Try to load from input.json in current directory
        var localPath = Path.Combine(Directory.GetCurrentDirectory(), "input.json");
        if (File.Exists(localPath))
        {
            try
            {
                var content = await File.ReadAllTextAsync(localPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<InputsData>(content, options);
                Debug.WriteLine($"Loaded data from local file: {localPath}");
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading local JSON file: {ex.Message}");
            }
        }

        return null;
    }
}
