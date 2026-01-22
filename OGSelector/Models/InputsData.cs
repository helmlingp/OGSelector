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
    [JsonPropertyName("uemUuid")]
    public string UemUuid { get; init; } = string.Empty;

    [JsonPropertyName("uemId")]
    public string UemId { get; init; } = string.Empty;

    [JsonPropertyName("uemName")]
    public string UemName { get; init; } = string.Empty;

    [JsonPropertyName("businessUnit")]
    public string BusinessUnitName { get; init; } = string.Empty;

    // Display property for UI binding
    public string Display => BusinessUnitName;
}

public class RoleItem
{
    [JsonPropertyName("roleName")]
    public string RoleName { get; init; } = string.Empty;

    // Display property for UI binding
    public string Display => RoleName;
}

public class GeoItem
{
    [JsonPropertyName("geoName")]
    public string GeoName { get; init; } = string.Empty;

    // Display property for UI binding
    public string Display => GeoName;
}
