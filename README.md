# KeyboardHeatmapPro

A Windows desktop application for tracking and visualizing keyboard usage patterns through an interactive heatmap.

## ⚠️ Important Notice

**This application is currently in an incomplete/buggy state and has the following limitations:**
- The application cannot start properly due to initialization issues
- Many features are not fully implemented
- The codebase contains several bugs that prevent normal operation
- This is a work-in-progress prototype

## Features (Planned)

- Real-time keyboard activity tracking
- Visual heatmap display showing key usage frequency
- Statistics including:
  - Total keys pressed
  - Keys per minute
  - Session duration
  - Most/least used keys
- Customizable heatmap color schemes
- Data export functionality (CSV/JSON)
- Session management with pause/resume capabilities

## Prerequisites

- Windows 10 or later
- .NET Framework or .NET Core (check .csproj for specific version)
- Visual Studio 2019 or later (for development)

## Project Structure

```
KeyboardHeatmapPro/
├── Controls/          # Custom WPF controls
├── Converters/        # Value converters for data binding
├── Helpers/           # Utility classes
├── Models/            # Data models
├── Resources/         # Application resources and assets
├── Services/          # Business logic and services
├── ViewModels/        # MVVM ViewModels
├── Views/             # WPF Views
├── App.xaml           # Application entry point
├── MainWindow.xaml    # Main application window
└── KeyboardHeatmapPro.csproj  # Project file
```

## Building the Project

1. Clone the repository
2. Open `KeyboardHeatmapPro.csproj` in Visual Studio
3. Restore NuGet packages
4. Build the solution (Ctrl+Shift+B)

## Known Issues

- **Application fails to start**: There are initialization errors preventing the application from launching
- **Incomplete features**: Most functionality is not fully implemented
- **UI bugs**: Various UI elements may not render or function correctly
- **Data persistence**: Settings and statistics may not save properly

## Development Status

This project is currently **abandoned/incomplete**. The codebase serves as a reference implementation for a keyboard heatmap tracker but requires significant work to become functional.

## License

No license specified.

## Contributing

As this project is in a non-functional state, contributions to fix the core issues would be needed before adding new features.
