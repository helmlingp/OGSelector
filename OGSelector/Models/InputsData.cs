using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Diagnostics;

namespace OGSelector.Models;

public class InputsData
{
    [JsonPropertyName("BUs")]
    public List<BusinessUnit> BusinessUnits { get; set; } = new();

    [JsonPropertyName("SmartGroupTags")]
    public List<SmartGroupTags> SmartGroupTags { get; set; } = new();

    [JsonPropertyName("NetScopeSmartgroups")]
    public List<string> NetScopeSmartgroups { get; set; } = new();
}

public class SmartGroupTags
{
    [JsonPropertyName("SmartGroupName")]
    public string SmartGroupName { get; init; } = string.Empty;

    [JsonPropertyName("TagUUID")]
    public string TagUUID { get; init; } = string.Empty;
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

    [JsonPropertyName("Geos")]
    public List<GeoItem> Geos { get; init; } = new();

    [JsonPropertyName("Roles")]
    public List<RoleItem> Roles { get; init; } = new();

    [JsonPropertyName("Process")]
    public List<ProcessItem> Process { get; init; } = new();

    // Display property for UI binding
    public string Display => BusinessUnitName;
}

public class ProcessItem
{
    [JsonPropertyName("processName")]
    public string ProcessName { get; init; } = string.Empty;

    [JsonPropertyName("processUuid")]
    public string ProcessTagUuid { get; init; } = string.Empty;

    // Display property for UI binding
    public string Display => ProcessName;
}

public class RoleItem
{
    [JsonPropertyName("roleName")]
    public string RoleName { get; init; } = string.Empty;

    [JsonPropertyName("roleUuid")]
    public string RoleTagUuid { get; init; } = string.Empty;

    // Display property for UI binding
    public string Display => RoleName;
}

public class GeoItem
{
    [JsonPropertyName("geoName")]
    public string GeoName { get; init; } = string.Empty;

    [JsonPropertyName("geoUuid")]
    public string GeoTagUuid { get; init; } = string.Empty;

    // Display property for UI binding
    public string Display => GeoName;
}
