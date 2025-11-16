# Copilot Instructions for Vortex

## Project Overview
Vortex is a cross-platform C# implementation of WebGPU, designed for use with MAUI, Avalonia, and Blazor. It provides a 3D rendering engine focused on performance and ease of integration.
Vortex maintains its own WebGPU implementation based on the official specification (https://www.w3.org/TR/webgpu/) to ensure consistent, standards-compliant behavior across platforms.

## Project Structure
- **Vortex.Common**: Shared utilities, helpers, and common types (e.g., math libraries, asset loaders). No platform-specific code.
- **Vortex.Gl**: Core WebGPU implementation. Handles device creation, adapters, shaders, pipelines, buffers, textures, and rendering logic. Use WebGPU APIs via C# bindings.
- **Vortex.Controls**: Platform-agnostic controls for embedding a 3D WebGPU canvas. Include:
    - `WebGpuCanvas`: Base control for rendering.
    - Adapters for MAUI (`MauiWebGpuView`), Avalonia (`AvaloniaWebGpuView`), and Blazor (`BlazorWebGpuComponent`).
- **Vortex.Examples**: Demonstration projects showing usage. Include:
    - Basic 3D canvas setup.
    - Loading and rendering an .OBJ file.

## Key Features to Implement
- **Default 3D Canvas**: Initialize WebGPU context, create swap chain, handle rendering loop. Support optional FPS counter in top-left corner (toggle via property).
- **Cross-Platform Compatibility**: Ensure code runs on Windows, macOS, Linux, iOS, Android, and Web via respective frameworks.
- **Asset Loading**: In examples, implement .OBJ loader in Vortex.Common. Parse vertices, normals, textures; render using Vortex.Gl.

## Coding Guidelines
- Use C# 14+ features.
- Follow async patterns for initialization and rendering.
- Handle errors gracefully (e.g., device lost).
- Use dependency injection for controls.
- Write unit tests in Vortex.tests.
- Document public APIs with XML comments.

## Initial Implementation Steps
1. Set up WebGPU bindings in Vortex.Gl.
2. Create basic canvas in Vortex.Controls.
3. Add FPS counter logic.
4. In Vortex.Examples, load sample .OBJ and render rotating model.

Adhere to this structure for all code generation.