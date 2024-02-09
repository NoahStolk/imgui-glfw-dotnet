# Changelog

This library uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.3.0

### Changed

- Changed namespace to `ImGuiGlfw`.

## 0.2.0

### Added

- Added the following members to the `GlfwInput` class:
  - `IReadOnlyList<MouseButton> MouseButtonsChanged`
  - `bool IsMouseButtonPressed(MouseButton button)`
  - `bool IsMouseButtonReleased(MouseButton button)`
  - `bool IsKeyPressed(Keys key)`
  - `bool IsKeyReleased(Keys key)`

## 0.1.0

- Initial release
