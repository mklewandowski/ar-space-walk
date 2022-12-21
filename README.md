# Space Walk
An AR solar system space walk application built in Unity. This application uses GPS device locations to place 11 celestial objects (the Sun, the 8 planets, the asteroid belt, and Pluto) in AR (augmented reality) as a user walks along a path from a starting point. Object spacing is based on actual planetary distances from the Sun. The entire walk is scaled to about 106 meters.

## Gameplay
Press the Start button to begin the walk and place the Sun in AR. As you walk away from the Sun, your location is recorded via GPS location data. Notifications will appear when you encounter a celestial object such as a planet. You can stop, place the object in AR, and explore and investigate the object, or you can continue the walk.

![Space Walk gameplay](https://github.com/mklewandowski/ar-space-walk/blob/main/Assets/Images/gameplay.gif?raw=true)

![Space Walk gameplay](https://github.com/mklewandowski/ar-space-walk/blob/main/Assets/Images/screenshot.png?raw=true)

## Supported Platforms
This project is designed for use on both iOS and Android, but it has only been tested on Android.

## Running Locally
Use the following steps to run locally:
1. Clone this repo
2. Open repo folder using Unity 2021.3.16f1
3. Import TextMeshPro

## Project Structure
The project contains several scenes with different variations on celestial object placement. `ThresholdGroupPlacement` uses scaled relative distances, contains the asteroid belt, and uses a placement mechanism that is tied to GPS locations. This scene is recommended if you want to try out the app. The other scenes are worth taking a look at if you want to explore different options for triggering celestial object placement.

## Development
Setup steps to be able to include AR Foundation and build and deploy:
- Install AR Foundation located in the Package Manager under AR Foundation
- Install ARKit located in the Package Manager under AR Kit XR Plugin (required for iOS devices)
- Install ARCore located in the Package Manager under AR Core XR Plugin (required for Android devices)
- In Project Settings > XR Plug-in Management, set the Plug-in Provider on the Android tab to ARCore
- Ensure AR scenes contain an AR Session and AR Session Origin
- In Project Settings > Resolution and Presentation, disable Render Outside Safe Space
- For Android, in Project Settings > Other Settings, set Minimum API Level to Android API level 27 or higher (this is required to build for Android. We need a minimum of 24 for AR and 27 for the location services)
- For Android, in Project Settings > Other Settings, remove Vulkan from Graphics APIs (this is required to build for Android, need to uncheck Auto Graphics API first)
- For Android, in Project Settings > Other Settings, Set Scripting Backend to IL2CPP
- For Android, in Project Settings > Other Settings, Add ARM64 to Target Architectures

## GPS Precision
By default, Unity provides GPS positions as floats. For some applications this does not provide enough digits of precision to be useful (such as the location fidelity needed to perform a scaled solar system walk). To access the native GPS positions as doubles with more precision use this plugin:
https://assetstore.unity.com/packages/tools/localization/native-gps-plugin-ios-android-216027

### Importing NASA 3D Assets
3D assets from NASA are available on the NASA Solar System Expedition site under Resources:
https://solarsystem.nasa.gov/resources/

Models can be downloaded as .glb files which cannot be natively imported into Unity. Instead they can be converted to .obj files and textures which can be used in Unity using online conversion tools such as this one:
https://products.aspose.app/3d/extractor/glb

Or this one: https://anyconv.com/glb-to-obj-converter/

#### Working With Models
Some models may have difficulty working properly in AR or need special adjustments to work properly. Some notes on models:
- if a model shows as black or without the proper texture when scaled, try changing the scale factor on the model asset itself rather than the scale on the Game Object.
- in this project, I created a new material for each model and applied the associated texture in the albedo field. Check the smoothness and other material settings to ensure they align with the original asset material or with the desired rendering of the model.

## Development Tools
- Created using Unity 2021.3.16f1
- Code edited using Visual Studio Code
