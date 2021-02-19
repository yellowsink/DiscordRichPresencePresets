# DiscordRichPresencePresets
Discord Rich Presence Presets, is a program written in .NET that adds a custom presence on Discord, with the ability to change presets.
For Example you can set a preset for sleeping, e p i c l y  gaming, doing homework etc.
# Usage
## Windows Stable (WPF)
Grab a release from the [releases](https://github.com/cainy-a/DiscordRichPresencePresets/releases), or just get the [latest one](https://github.com/cainy-a/DiscordRichPresencePresets/releases/latest).
Extract the zip, then run the `.exe`.  
If you're on Linux or macOS, you can't use the stable build. Check out [MoltenCore](https://github.com/MoltenCoreDev)'s [python version](https://github.com/MoltenCoreDev/DiscordRPPresets) or the linux compatible beta!
## Windws, Linux, macOS beta (AvaloniaUI)
**⚠ THIS VERSION IS IN BETA AND MAY HAVE BUGS. PLEASE MAKE SURE TO OPEN ISSUES IF YOU FIND THEM!!! It is only tested on Linux. ⚠**  
Grab a release from the [releases](https://github.com/cainy-a/DiscordRichPresencePresets/releases) which is tagged as `avalonia-v_._` instead of `v_._`.
Extract the zip for your platform, then run the `.exe`, `.app`, or Linux binary.
# Preview
This is a (very slightly less than last time) BETA™ preview.  
![Beta](https://drawing-some.femboy.art/a0762Ca.gif)
# Compiling
## Install requirements
### All versions
You will need the **.NET 5** SDK to build this.
Get it from [here](https://dotnet.microsoft.com/download).
### WPF version
YOU NEED TO BE ON WINDOWS!!!! (.NET 5 is cross-platform, but WPF is not.)
### AvaloniaUI version
You need to accept that this version is in beta at the moment. That's it. ¯\\\_(ツ)\_/¯
## Build it
### Just build it
Assuming you are in the repo root, run the following:
```
dotnet build
```
### Build and run it
#### WPF
Assuming you are in the repo root, run the following:
```
cd DiscordRichPresencePresets.Wpf
dotnet run
```
#### AvaloniaUI
Assuming you are in the repo root, run the following:
```
cd DiscordRichPresencePresets.Avalonia
dotnet run
```
### Publish a release
#### WPF
Assuming you are in the repo root, run the following:
```
cd DiscordRichPresencePresets.Wpf
dotnet publish --self-contained false -r win-x64
```
#### AvaloniaUI
#### WPF
Assuming you are in the repo root, run the following:
```
cd DiscordRichPresencePresets.Avalonia
dotnet publish --self-contained false -r win-x64
dotnet publish --self-contained false -r linux-x64
dotnet publish --self-contained false -r osx-x64
```
