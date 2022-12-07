# About ImageGrabbber Project

## Description

This is a self practice project about using usb camere device for grabb and display image.

## Required Environment

* Windows OS
* .netcore = 3.1
* C# language version >= 10.0

## Required Packages

* MaterialDesignThemes >= 4.6.0
* Prism.Unity >= 8.197
* System.Drawing.Common = 6.0.0
* AFore.Viedo.DirectShow = 2.2.5

## Project Structure

In this section, shows the project structure(*namespace level*), and some simply introduce about each project.

<pre>
+-- IamgeGrabber.sln
|   +-- ImageGrabber.Application
|       +-- Models
|       +-- ViewModels
|       +-- Views
|   +-- ImageGrabber.Core
|       +-- CameraModule
|       +-- CommonInterface
|   +-- ImageGrabber.Debug
|   +-- ImageGrabber.Wpf
|       +-- Converters
|       +-- Extensions
|       +-- Styles
|       +-- Utility
+-- .gitignore
+-- README.md
</pre>

* **ImageGrabber.Application** is the executable project with interactive UI interface.

* **ImageGrabber.Core** is the core project which contains the camera control module.

* **ImageGrabber.Wpf** is a wpf base library which contains style sheets, converters and some extensions class to support xaml development.

* **ImageGrabber.Debug** is a console application for debug or test some ideas.
