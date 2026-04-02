$csvPath = 'C:\Users\Phil\Desktop\new-27-Mar-2026-02-31-46_BusSelectTagV1.0.csv'
$jsonPaths = @(
    'C:\Users\Phil\source\OGSelector\OGSelector\bin\Debug\net10.0\inputs.json'
)

$rows = Import-Csv -Path $csvPath

function Get-FirstNonEmptyValue {
    param(
        [Parameter(Mandatory = $true)] [object[]] $SourceRows,
        [Parameter(Mandatory = $true)] [string] $PropertyName
    )

    return ($SourceRows |
        Select-Object -ExpandProperty $PropertyName |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
        Select-Object -First 1)
}

$newBUs = @(
    $rows |
    Where-Object { -not [string]::IsNullOrWhiteSpace($_.BusinessName) } |
    Group-Object -Property BusinessName |
    ForEach-Object {
        $groupRows = $_.Group

        $uemName = Get-FirstNonEmptyValue -SourceRows $groupRows -PropertyName 'OGname'
        $uemId = Get-FirstNonEmptyValue -SourceRows $groupRows -PropertyName 'IGID'
        $uemUuid = Get-FirstNonEmptyValue -SourceRows $groupRows -PropertyName 'OGUUID'

        $geos = @(
            $groupRows |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_.Region) -or -not [string]::IsNullOrWhiteSpace($_.RegionUUID) } |
            Select-Object @{Name = 'geoName'; Expression = { $_.Region } }, @{Name = 'geoUuid'; Expression = { $_.RegionUUID } } |
            Sort-Object geoName, geoUuid -Unique
        )

        $roles = @(
            $groupRows |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_.Role) -or -not [string]::IsNullOrWhiteSpace($_.RoleUUID) } |
            Select-Object @{Name = 'roleName'; Expression = { $_.Role } }, @{Name = 'roleUuid'; Expression = { $_.RoleUUID } } |
            Sort-Object roleName, roleUuid -Unique
        )

        $processes = @(
            $groupRows |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_.Process) -or -not [string]::IsNullOrWhiteSpace($_.ProcessUUID) } |
            Select-Object @{Name = 'processName'; Expression = { $_.Process } }, @{Name = 'processUuid'; Expression = { $_.ProcessUUID } } |
            Sort-Object processName, processUuid -Unique
        )

        [PSCustomObject]@{
            uemUuid = if ([string]::IsNullOrWhiteSpace($uemUuid)) { '' } else { $uemUuid }
            uemId = if ([string]::IsNullOrWhiteSpace($uemId)) { '' } else { $uemId }
            uemName = if ([string]::IsNullOrWhiteSpace($uemName)) { '' } else { $uemName }
            businessUnit = $_.Name
            Geos = $geos
            Roles = $roles
            Process = $processes
        }
    }
)

foreach ($jsonPath in $jsonPaths) {
    if (-not (Test-Path $jsonPath)) {
        Write-Output "Skipped (not found): $jsonPath"
        continue
    }

    $json = Get-Content -Raw -Path $jsonPath | ConvertFrom-Json

    if ($json.PSObject.Properties.Name -contains 'BUs') {
        $json.BUs = $newBUs
    }
    else {
        $json | Add-Member -NotePropertyName 'BUs' -NotePropertyValue $newBUs
    }

    $json | ConvertTo-Json -Depth 100 | Set-Content -Path $jsonPath -Encoding UTF8
    Write-Output "Updated: $jsonPath (BUs replaced with $($newBUs.Count) business units)"
}