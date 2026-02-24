$source = $PSScriptRoot
$destination = "$env:ProgramData\Airwatch"

if (!(Test-Path $destination)) {
    New-Item -Path $destination -ItemType Directory -Force
}

Copy-Item -Path "$source\*" -Destination $destination -Recurse -Force

Start-Process "$destination\OGSelector.exe" -Wait
