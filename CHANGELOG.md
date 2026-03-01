# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.1.5] - 2026-03-02

### Added
- **Process Selection**: New Process dropdown for additional organizational structure
  - ProcessItem model with processName and processUuid fields
  - Process selection in submission flow
  - Registry keys for process storage (Process, ProcessTagUuid)
- **Enhanced Data Structure**: Nested Role, Geo, and Process items within each Business Unit
  - Each Business Unit now contains its own Roles, Geos, and Process lists
  - UUIDs for all items (Roles, Geos, Processes) for tag-based identification
- **ConfigurationService**: New service for centralized configuration management
  - Loads appsettings.json from current or application directory
  - Auto-exports configuration to current directory
  - Environment variable override support
- **Updated Registry Keys**: New registry structure matching UEM requirements
  - OGUuid, OGid, OGName for organizational group identification
  - BUName for business unit display name
  - RolesTagUuid, GeosTagUuid, ProcessTagUuid for tag-based identification
  - Delete existing registry keys on Submit, prior to writing new values
- **Install & Uninstall scripts**: Provide install and uninstall powershell scripts
- **Block Windows Key**: Block Windows key to prevent minimise of the app
- **Versioning**: Add versioning

### Changed
- **Data Model Structure**: Roles, Geos, and Process are now nested under BusinessUnit
  - Old flat structure replaced with hierarchical organization
  - Each BU has independent role/geo/process options
- **Registry Output**: Expanded from 6 to 10 registry keys
  - Added Process-related keys
  - Renamed some keys for clarity (Role → Roles, Geography → Geos, etc.)
  - Added UUID fields for all selections
- **MainViewModel**: Enhanced to handle optional Process/Role/Geo selections
  - Added HasProcess, HasRoles, HasGeos flags
  - Dynamic visibility based on available data
  - Improved validation logic for optional selections
- **Documentation**: Completely updated for new schema
  - README.md updated with nested data structure
  - API_DOCUMENTATION.md recreated from scratch
  - DEPLOYMENT.md updated with new registry keys
  - Updated JSON schema examples throughout

### Fixed
- Registry key cleanup now properly deletes entire path tree
- Improved error handling in configuration loading
- Better validation for optional selection fields

### Technical Details
- ProcessItem class added to Models
- RegistryService.DeleteRegistryKeyPath() method updated
- MainViewModel selection and validation logic enhanced
- All documentation synchronized with implementation

## [1.0.0] - 2026-01-22

### Added
- Initial release of OGSelector
- Avalonia-based cross-platform UI framework
- Business Unit, Role, and Geography selection interface
- Windows Registry integration for storing selections
- Remote JSON configuration loading from URLs
- Local JSON file fallback support
- Customizable UI text through appsettings.json
- Custom logo support (PNG/SVG)
- Full-screen kiosk mode
- Exit prevention mode for controlled environments
- Lottie animation support for enhanced UX
- Self-contained single-file deployment option
- .NET 10.0 support

### Features
- Dynamic data loading from local or remote JSON sources
- MVVM architecture using CommunityToolkit.Mvvm
- Searchable combo boxes for easy selection
- Error handling and user feedback
- Configuration management via JSON
- Registry service for reading/writing Windows Registry
- Command-line argument support for JSON URL

### UI Components
- Responsive main view with three dropdown selectors
- Loading animations
- Error state display
- Submit button with validation
- Custom branding support

### Configuration
- appsettings.json for application settings
- inputs.json for data configuration
- Support for environment variables
- Registry path customization

### Services
- JsonDownloadService: Handles data loading from files or URLs
- RegistryService: Manages Windows Registry operations

### Technical Details
- Target Framework: .NET 10.0
- Platform: Windows x64
- UI Framework: Avalonia 11.3.11
- MVVM Framework: CommunityToolkit.Mvvm 8.2.2
- Animation: Avalonia.Skia.Lottie 11.0.0
- Configuration: Microsoft.Extensions.Configuration

## Release Notes

### Version 1.0.0 - Initial Release

This is the first stable release of OGSelector, a tool designed to help organizations manage device enrollment by allowing users to select their Business Unit, Role, and Geography. The application integrates with Unified Endpoint Management (UEM) systems by storing the selections in the Windows Registry.

**Key Features:**
- Simple, user-friendly interface
- Flexible configuration options
- Remote and local data sources
- Kiosk mode support
- Full Windows Registry integration

**Use Cases:**
- Device enrollment processes
- Organizational group assignment
- Self-service device configuration
- Kiosk-based device setup

**Deployment Options:**
- Self-contained single executable
- Framework-dependent deployment
- UEM/MDM distribution (Workspace ONE, Intune, SCCM)
- Manual distribution

**For more information, see:**
- [README.md](README.md) - Full documentation
- [DEPLOYMENT.md](DEPLOYMENT.md) - Deployment guide
- [API_DOCUMENTATION.md](API_DOCUMENTATION.md) - Technical details

---

## Version History Template

### [X.Y.Z] - YYYY-MM-DD

#### Added
- New features added

#### Changed
- Changes to existing functionality

#### Deprecated
- Features that will be removed in future versions

#### Removed
- Features that have been removed

#### Fixed
- Bug fixes

#### Security
- Security fixes and improvements

---

## Upgrade Notes

### From Future Versions

*Upgrade notes will be added as new versions are released*

---

## Breaking Changes

### Future Versions

*Breaking changes will be documented here*

---

## Migration Guide

### Future Migrations

*Migration guides will be added as needed*

---

[Unreleased]: https://github.com/helmlingp/OGSelector/compare/v1.1.0...HEAD
[1.1.0]: https://github.com/helmlingp/OGSelector/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/helmlingp/OGSelector/releases/tag/v1.0.0
