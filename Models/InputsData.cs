using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace OGSelector.Models;

public class InputsData
{
    [JsonPropertyName("BUs")]
    public List<BusinessUnit> BusinessUnits { get; set; } = new();

    [JsonPropertyName("Roles")]
    public List<RoleItem> Roles { get; set; } = new();

    [JsonPropertyName("Geos")]
    public List<GeoItem> Geos { get; set; } = new();
}

public class BusinessUnit
{
    [JsonPropertyName("Display")]
    public string Display { get; set; } = string.Empty;

    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;
}

public class RoleItem
{
    [JsonPropertyName("Display")]
    public string Display { get; set; } = string.Empty;

    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;
}

public class GeoItem
{
    [JsonPropertyName("Display")]
    public string Display { get; set; } = string.Empty;

    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;
}
