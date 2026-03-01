# Deployment Guide

## Prerequisites

### Build Environment
- Windows 10/11
- .NET 10.0 SDK
- Visual Studio 2022 (optional) or VS Code with C# Dev Kit

### Target Environment
- Windows 10/11 (64-bit)
- Administrator privileges for registry operations
- Internet connectivity (optional, for remote JSON loading)

## Build Process

### Option 1: Using the Build Script

The easiest way to build a release version:

```batch
build_release.bat
```

This script:
1. Publishes a self-contained single-file executable
2. Includes all native libraries
3. Copies the resulting executable to your desktop

### Option 2: Manual Build

```batch
dotnet publish -c Release -r win-x64 ^
  /p:PublishSingleFile=true ^
  /p:IncludeNativeLibrariesForSelfExtract=true ^
  --self-contained true
```

The output will be located at:
```
OGSelector\bin\Release\net10.0\win-x64\publish\
```

### Option 3: Framework-Dependent Build

If .NET 10.0 runtime is already installed on target machines:

```batch
dotnet publish -c Release -r win-x64 ^
  /p:PublishSingleFile=true ^
  --self-contained false
```

This produces a smaller executable but requires .NET 10.0 runtime on the target machine.

## Deployment Package

### Required Files

Create a deployment folder with these files:

```
OGSelector/
├── OGSelector.exe          # Main executable
├── appsettings.json        # Configuration file
├── inputs.json             # Data file (if not using remote URL)
└── logo.png                # Optional: Company logo
```

### Optional Files

- `logo.png` or `logo.svg`: Custom company logo (placed in same directory as exe)
- `README.md`: User documentation

## Configuration

### 1. Edit appsettings.json

Before deployment, customize the configuration:

```json
{
  "UI": {
    "Headline": "Device configuration",
    "Subtitle": "Select your Business Unit, Role, and Geography",
    "Information": "Placing your device in the correct Business Unit helps us configure it properly.",
    "ErrorHeadline": "Uh oh!",
    "ErrorSubtitle": "We've encountered an error",
    "ErrorInformation": "An error occurred during the configuration process for this device. \n\nPlease contact your IT Support."
  },
  "Settings": {
    "jsonURL": "https://config.company.com/inputs.json",
    "Fullscreen": true,
    "AllowExit": false,
    "RegKeyPath": "HKEY_LOCAL_MACHINE\\SOFTWARE\\CUSTOMER"
  }
}
```

**Key Settings to Customize:**
- `jsonURL`: Set to your hosted inputs.json URL (or leave blank for local file)
- `Fullscreen`: Set to `true` for kiosk / lockdown mode
- `AllowExit`: Set to `false` to prevent users from closing
- `RegKeyPath`: Update to match your organization's registry structure
- `UI.*`: Customize all user-facing text

### 2. Prepare inputs.json

Create or download your `inputs.json` file:

```json
{
  "BUs": [
    {
      "uemUuid": "uuid-value-here",
      "uemId": "1001",
      "uemName": "OG Name in UEM",
      "businessUnit": "Display Name for Users",
      "Roles": [
        {"roleName": "Executive", "roleUuid": "role-uuid-1"},
        {"roleName": "Manager", "roleUuid": "role-uuid-2"},
        {"roleName": "Employee", "roleUuid": "role-uuid-3"},
        {"roleName": "Contractor", "roleUuid": "role-uuid-4"}
      ],
      "Geos": [
        {"geoName": "North America", "geoUuid": "geo-uuid-1"},
        {"geoName": "Europe", "geoUuid": "geo-uuid-2"},
        {"geoName": "Asia Pacific", "geoUuid": "geo-uuid-3"},
        {"geoName": "Latin America", "geoUuid": "geo-uuid-4"}
      ],
      "process": [
        {"processName": "Process 1", "processUuid": "process-uuid-1"},
        {"processName": "Process 2", "processUuid": "process-uuid-2"}
      ]
    }
  ]
}
```

### 3. Add Custom Logo (Optional)

Place a `logo.png` file in the deployment folder. Supported formats:
- PNG (recommended)
- SVG

Recommended dimensions: 200x50 pixels (or similar aspect ratio)

## Deployment Methods

### Method 1: Manual Distribution

1. Build the release package
2. Create a ZIP file with all required files
3. Distribute to target machines
4. Extract to a permanent location (e.g., `C:\Program Files\OGSelector\`)
5. Create shortcuts as needed

### Method 2: UEM/MDM Distribution

#### Workspace ONE UEM

1. **Create Application Package:**
   - Package Type: Public/Internal Application
   - File Type: ZIP
   - Name: `OGSelector.ZIP`
   - Add `appsettings.json` to be deployed alongside exe
   - Add `inputs.json` (if not using remote URL)
   - Add `logo.png` (if applicable)
   - Add `uem-install.ps1`
   - Add `uem-ininstall.ps1`

3. **Configure Installation:**
   - Install Command: `powershell.exe -ep bypass -file ./uem-install.ps1`
   - Uninstall command: `powershell.exe -ep bypass -file ./uem-uninstall.ps1`
   - Detection rules: File Exists: `%PROGRAMDATA%\Airwatch\OGSelector.exe`
   - Install Context: USER
   - Admin Privileges: YES

3. **Assign to Groups:**
   - Required assignment for enrollment devices
   - User or device context

### Method 4: Remote JSON Hosting

Host `inputs.json` on a web server for centralized management:

1. **Upload inputs.json to web server**
   - Must be accessible via HTTPS
   - No authentication required (or use public-readable location)

2. **Configure appsettings.json:**
   ```json
   {
     "Settings": {
       "jsonURL": "https://config.company.com/ogselector/inputs.json"
     }
   }
   ```

3. **Deploy without the inputs.json**
   - Application downloads `inputs.json` on each run
   - Ensures users always get latest data

4. **Update Process:**
   - Update `inputs.json` on web server
   - All clients get updates automatically on next run

## Post-Deployment Verification

### 1. Test Installation

Run the executable and verify:
- [ ] Application launches successfully
- [ ] UI displays correctly with custom text
- [ ] Logo appears (if configured)
- [ ] Dropdowns populate with data
- [ ] Selections can be made
- [ ] Submit button works

### 2. Verify Registry Writes

After submission, check registry:

```powershell
Get-ItemProperty -Path "HKLM:\SOFTWARE\CUSTOMER"
```

Expected keys:
- `OGUuid`: UEM UUID from selected Business Unit
- `OGid`: UEM ID from selected Business Unit
- `OGName`: UEM Name from selected Business Unit
- `BUName`: Display name of selected Business Unit
- `Roles`: Selected Role name (empty if no role available)
- `RolesTagUuid`: UUID of selected Role (empty if no role)
- `Geos`: Selected Geography name (empty if no geo available)
- `GeosTagUuid`: UUID of selected Geography (empty if no geo)
- `Process`: Selected Process name (empty if no process available)
- `ProcessTagUuid`: UUID of selected Process (empty if no process)

### 3. Test Remote Loading (if applicable)

```batch
OGSelector.exe "https://your-server.com/inputs.json"
```

Verify:
- [ ] JSON downloads successfully
- [ ] Data populates in UI
- [ ] Local `inputs.json` created in app directory

### 4. Test Full-Screen Mode

Set `Fullscreen: true` and verify application launches full-screen.

### 5. Test Exit Prevention

Set `AllowExit: false` and verify:
- [ ] Close button is disabled or closing is prevented
- [ ] Application cannot be alt-F4'd
- [ ] Task Manager can still force-close (as expected)

## Troubleshooting Deployment

### Application won't start
- **Cause:** Missing .NET runtime
- **Solution:** Use self-contained build or install .NET 10.0 runtime

### Data not loading
- **Cause:** inputs.json not found or invalid JSON
- **Solution:** 
  - Verify file exists in same directory as exe
  - Validate JSON syntax
  - Check file permissions

### Registry keys not created
- **Cause:** Insufficient permissions
- **Solution:**
  - Run as Administrator
  - Deploy via system context in UEM
  - Adjust registry path permissions

### Remote JSON not downloading
- **Cause:** Network/firewall issues
- **Solution:**
  - Test URL in browser
  - Check corporate proxy settings
  - Verify SSL certificate validity
  - Add firewall exceptions

### Custom logo not appearing
- **Cause:** File not found or wrong format
- **Solution:**
  - Verify filename is exactly `logo.png` or `logo.svg`
  - Place in same directory as executable
  - Check file is not corrupted

## Update Strategy

### Updating the Application

1. **Build new version**
2. **Test in development environment**
3. **Update version number in project file**
4. **Deploy to test group**
5. **Rollout to production**

### Updating Configuration Only

Update `appsettings.json` without rebuilding:
- Use UEM to push updated file
- Or update via script/file distribution

### Updating Data Only

**Local File Method:**
- Distribute new `inputs.json` via UEM
- Replace file on target machines

**Remote URL Method:**
- Update `inputs.json` on web server
- All clients get updates automatically
- No deployment needed

## Security Considerations

1. **Registry Permissions:**
   - Ensure service account has write access to registry path
   - Use least-privilege principle

2. **Remote JSON:**
   - Use HTTPS only
   - Consider authentication if data is sensitive
   - Validate SSL certificates

3. **Executable Signing:**
   - Sign the executable with code signing certificate
   - Prevents SmartScreen warnings
   - Establishes trust

4. **File Permissions:**
   - Deploy to protected folder (e.g., Program Files)
   - Prevent users from modifying configuration
   - Read-only permissions for standard users

## Rollback Plan

If issues occur:

1. **Immediate Rollback:**
   - Remove application assignment in UEM
   - Push previous version

2. **Uninstall:**
   - Run the `uem-uninstall.ps1` script

   ```powershell
   $patterns = '*.json','OGSelector.exe'
   Get-ChildItem -Recurse -File | Where-Object { $name = $_.Name;   $patterns | Where-Object { $name -like $_ }} | Remove-Item -Force
   ```

   - Remove Registry Entries

   ```powershell
   Remove-ItemProperty -Path "HKLM:\SOFTWARE\CUSTOMER" -Name "OGUuid","OGid","OGName","BUName","Roles","RolesTagUuid","Geos","GeosTagUuid","Process","ProcessTagUuid" -Force -ErrorAction SilentlyContinue
   ```

## Support and Monitoring

### Logging

Application uses Debug output. To capture:

```powershell
# Run with output redirection
OGSelector.exe 2>&1 | Tee-Object -FilePath "C:\Logs\OGSelector.log"
```

### Monitoring Registry Keys

Use Workspace ONE UEM Sensors:

```powershell
# Description: Read and return registry value
# Execution Context: SYSTEM
# Execution Architecture: EITHER64OR32BIT
# Return Type: STRING

# Define the registry key variables
$keyPath = "HKLM:\SOFTWARE\CUSTOMER"
$valueName = "OGUuid"

# Check if the registry key path exists
if (Test-Path $keyPath) {
    # Get the value of the registry value
    $value = Get-ItemProperty -Path $keyPath -Name $valueName -ErrorAction SilentlyContinue
    # Check if the value is not null (i.e., the value exists)
    if ($value -ne $null) {
        return $value.$valueName
    }
    else { return "" }
}
else { return "" }

```

### User Feedback

Monitor helpdesk tickets for:
- Launch failures
- Data loading issues
- Submission errors
- UI/UX feedback

## Best Practices

1. **Test thoroughly** in development environment
2. **Pilot with small group** before full rollout
3. **Use remote JSON** for easier updates
4. **Monitor registry writes** to ensure success
5. **Provide user training** or documentation
6. **Have rollback plan** ready
7. **Sign the executable** for production use
8. **Version control** your configuration files
9. **Document customizations** specific to your environment
10. **Schedule regular reviews** of BU/Role/Geo data
