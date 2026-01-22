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
    "jsonURL": "",
    "Fullscreen": false,
    "AllowExit": true,
    "RegKeyPath": "HKEY_LOCAL_MACHINE\\SOFTWARE\\CUSTOMER"
  }
}
```

**Key Settings to Customize:**
- `jsonURL`: Set to your hosted inputs.json URL (or leave blank for local file)
- `Fullscreen`: Set to `true` for kiosk mode
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
      "businessUnit": "Display Name for Users"
    }
  ],
  "Roles": [
    {"roleName": "Executive"},
    {"roleName": "Manager"},
    {"roleName": "Employee"},
    {"roleName": "Contractor"}
  ],
  "Geos": [
    {"geoName": "North America"},
    {"geoName": "Europe"},
    {"geoName": "Asia Pacific"},
    {"geoName": "Latin America"}
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
   - File Type: EXE
   - Upload `OGSelector.exe`

2. **Add Configuration Files:**
   - Under "Files/Actions" tab
   - Add `appsettings.json` to be deployed alongside exe
   - Add `inputs.json` (if not using remote URL)
   - Add `logo.png` (if applicable)

3. **Configure Installation:**
   - Install Command: `OGSelector.exe` (if running immediately)
   - Or: Deploy files to `C:\Program Files\OGSelector\`
   - Install Context: System/Device

4. **Set Command-Line Arguments** (if using remote JSON):
   - Install Arguments: `"https://your-server.com/inputs.json"`

5. **Configure Smart Groups:**
   - Assign to appropriate device groups
   - Set to "Auto" install

6. **Optional: Run at Login:**
   - Create a Product Provisioning profile
   - Set as Mandatory application
   - Configure to run at user login

#### Microsoft Intune

1. **Create Win32 App:**
   - Package using Content Prep Tool
   - Upload `.intunewin` package

2. **Configure Installation:**
   - Install command: `OGSelector.exe`
   - Uninstall command: `cmd /c del /f OGSelector.exe`
   - Detection rules: File existence check

3. **Assign to Groups:**
   - Required assignment for enrollment devices
   - User or device context

#### SCCM/Configuration Manager

1. **Create Application:**
   - Deployment Type: Script Installer
   - Content Location: UNC path to files

2. **Configure Detection:**
   - Registry key detection
   - File system detection

3. **Deploy:**
   - Required deployment to device collections
   - Schedule installation during enrollment

### Method 3: Startup/Login Script

Deploy via Group Policy or login script:

```batch
@echo off
REM Copy files to local machine
xcopy "\\server\share\OGSelector\*" "C:\Program Files\OGSelector\" /Y /I /Q

REM Run the application
"C:\Program Files\OGSelector\OGSelector.exe"

REM Optional: Pass remote URL
REM "C:\Program Files\OGSelector\OGSelector.exe" "https://example.com/inputs.json"
```

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

3. **Deploy only the executable and appsettings.json**
   - Application downloads `inputs.json` on each run
   - Ensures users always get latest data

4. **Update Process:**
   - Update `inputs.json` on web server
   - All clients get updates automatically on next run

## Kiosk Mode Deployment

For enrollment kiosks or locked-down scenarios:

### Configuration
```json
{
  "Settings": {
    "Fullscreen": true,
    "AllowExit": false,
    "jsonURL": "https://config.company.com/inputs.json"
  }
}
```

### Windows Kiosk Setup

1. **Create a Kiosk User Account**
2. **Configure Windows Kiosk Mode:**
   - Settings > Accounts > Family & other users
   - Set up a kiosk
   - Choose OGSelector.exe as the kiosk app

3. **Alternative: Shell Launcher:**
   ```xml
   <ShellLauncherConfiguration>
     <Shell Shell="C:\Program Files\OGSelector\OGSelector.exe" />
   </ShellLauncherConfiguration>
   ```

4. **Auto-Restart on Exit:**
   - Configure with Task Scheduler
   - Monitor process and restart if closed

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
- BusinessUnit
- Role
- Geography
- uemUuid
- uemId
- uemName

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

2. **Registry Cleanup:**
   ```powershell
   Remove-ItemProperty -Path "HKLM:\SOFTWARE\CUSTOMER" -Name "BusinessUnit","Role","Geography","uemUuid","uemId","uemName"
   ```

3. **File Removal:**
   ```batch
   del "C:\Program Files\OGSelector\*" /F /Q
   rmdir "C:\Program Files\OGSelector"
   ```

## Support and Monitoring

### Logging

Application uses Debug output. To capture:

```powershell
# Run with output redirection
OGSelector.exe 2>&1 | Tee-Object -FilePath "C:\Logs\OGSelector.log"
```

### Monitoring Registry Keys

Use Workspace ONE Intelligence, Intune Reports, or custom scripts:

```powershell
$devices = Get-ADComputer -Filter * -Properties *
foreach ($device in $devices) {
    $regPath = "\\$($device.Name)\HKLM\SOFTWARE\CUSTOMER"
    # Check registry values
}
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
