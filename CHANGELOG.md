# Changelog

This library uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.5.0

### Added

- Added `ImGuiController.CreateDefaultFont` method to create a default font.

### Changed

- `ImGuiController` no longer creates a default font in the constructor. You must now call the `CreateDefaultFont` method to create a default font, or implement your own font creation logic. 

## 0.4.0

### Changed

- `GlfwInput` is now an instance class. This allows for multiple instances of `GlfwInput` to be used in the same application. For example, you may want to use a different instance for game physics and ImGui rendering.
- An instance of `GlfwInput` now needs to be passed to the `ImGuiController` constructor. 

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
