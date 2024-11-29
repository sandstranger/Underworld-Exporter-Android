# Unofficial port of Ultima Underworld for Android
This is an Android port of Underworld Exporter project found at https://github.com/hankmorgan/UnderworldExporter 

# System Requirements

Underworld Exporter for Android has the following system requirements:

### Minimum
* Operating system: Android 6 or later
* Graphics: videocard, which supports GLES 3 render
* Memory: 1GB system RAM

### Recommended
* Operating system: Android 11 or later
* Graphics: videocard, which supports GLES 3 render
* Memory: 4GB system RAM or more

## Gamepads Support
Android version of this engine supports gamepads.

Gamepad works correctly only with hidden screen controls

Default controls:

Movement - Left Stick

Mouselook - Right Stick

Left mouse button click emulation - Right Shoulder

Right mouse button click emulation - Left Shoulder

Jump - Button South

Toggle mouselook - Button East

Cast selected spell - Right Trigger

Interaction modes - From Left Trigger To Dpad Buttons

Fly Up - Button North

Fly down - Button West

Track skill - Right Stick Press

Charge Attack - Hold down Right Shoulder

Release Attack - Release Right Shoulder

Escape - Button Start

## Keyboard and Mouse Support
This Android port should also support keyboard and mouse

Default keyboard and mouse controls:

Movement - WASD

Jump - Spare

Toggle mouselook* - E

Cast selected spell* - Q

Toggle fullscreen* - F  (some ui elements are draggable in fullscreen mode)

Interaction modes*  F1 to F6.

Fly Up* - R

Fly down* = V

Track* = T

Charge Attack - Hold down right mouse key

Release Attack - Release right mouse key.

## Building

To get an APK file, clone this repository, open the `Underworld-Exporter-Android` directory in Unity 6000.0.29f1 and run the project.

## Issues

1) Music is not included in binary files.

Users need to download it from here https://www.nexusmods.com/ultimaunderworldunity/mods/1

then copy this music to phone, and write path to it in config.ini, which is stored in the folder, where all game data files are stored.

https://github.com/sandstranger/Underworld-Exporter-Android/blob/android/Assets/StreamingAssets/Configs/config.ini#L49 

https://imgur.com/a/rElQLWy

2) New game is broken after loading any save, if you want to start new game after loading any save - use "Restart Game Scene" button

## Credits
This port based on hankmorgan sources - https://github.com/hankmorgan/UnderworldExporter 
