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
}
