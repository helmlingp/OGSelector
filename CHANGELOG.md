# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Comprehensive project documentation
  - README.md with full project overview
  - API_DOCUMENTATION.md with technical API details
  - DEPLOYMENT.md with deployment strategies
  - CONTRIBUTING.md with contribution guidelines
  - CHANGELOG.md for tracking changes

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

[Unreleased]: https://github.com/helmlingp/OGSelector/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/helmlingp/OGSelector/releases/tag/v1.0.0
