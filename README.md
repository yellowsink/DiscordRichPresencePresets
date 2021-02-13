# DiscordRichPresencePresets
Discord Rich Presence Presets, is a program written in .NET that adds a custom presence on Discord, with the ability to change presets.
For Example you can set a preset for sleeping, e p i c l y  gaming, doing homework etc.
# Usage
Grab a release from the [releases](https://github.com/cainy-a/DiscordRichPresencePresets/releases), or just get the [latest one](https://github.com/cainy-a/DiscordRichPresencePresets/releases/latest).
Extract the zip, then run the `.exe`.
If you're on Linux, you can't use this. Check out [MoltenCore](https://github.com/MoltenCoreDev)'s [python version](https://github.com/MoltenCoreDev/DiscordRPPresets)!
# Preview
This is a (very slightly less than last time) BETA™ preview.  
![Beta](https://drawing-some.femboy.art/a0762Ca.gif)
# Compiling
## Install requirements
You will need the **.NET 5** SDK to build this. NOTE THAT YOU NEED TO BE ON WINDOWS!!!! (.NET 5 is cross-platform, but WPF is not.)  
Get it from [here](https://dotnet.microsoft.com/download).
## Build it
### Just build it
Assuming you are in the repo root, run the following:
```
dotnet build
```
### Build and run it
Assuming you are in the repo root, run the following:
```
cd DiscordRichPresencePresets
dotnet run
```
### Publish a release
Assuming you are in the repo root, run the following:
```
dotnet publish --self-contained false -r win-x86
```
