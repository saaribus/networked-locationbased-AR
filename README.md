# networked-locationbased-AR

For the full Documentation and detailed help setting up the project please read the included PDF: Documentation_LocationbasedMultiplayerAR.pdf

This includes the Unity Setup for locationbased AR, with the ADDITIONAL features of 1) a switch between the larger scale navigation with GPS and the more detailed tracking via ARFoundation and 2) the implementation of the mirror library. This is made for theater pieces in public spaces. With building for Android, WebGL and Windows you can have three parties playing in the same virtual space but being at 3 different locations in the physical world.

# Before Downloading Package
Before you can successfully download the package you have to create a new empty Unity Project (with the Base 3D Template and Universal Render Pipeline). I used Unity Editor Version 2021.3.ff LTS. Also make sure that you set your project up for ARFoundation. Meaning you have to import (via Unity Package Manager) the following External Packages:
  - ARFoundation 4.2.9
  - ARCore XR Plugin
  - ARFoundation Extensions v. 1.40.0 via Github: https://github.com/google-ar/arcore-unity-extensions
  - Mirror 83.2.0 via Github: https://github.com/MirrorNetworking/Mirror

Then import this package.

Open the DemoScene_GPSARMultiplayer to see how the elements correlate. 

# Setup in Unity
You need to create a new URPAsset (with Universal Renderer) and in the newly created Renderer you need to add the RenderFeature "ARBackground". In the project settings > Graphics > set the reference to newly created Universal Render Pipeline Asset. Now all the pink should be gone.

# mirror Setup/Test
In the DemoScene press play and "Host", you are now connected in your local network and can:
- walk around with "w", "a", "s", "d"
- look around with your mouse
- you can click on the radio, it will make a sound that is networked, so if other players connect they here the same sound simultaneously
- you can leave a text note by entering text in the Input field and pressing play (others in the network can see this note. Note: only the owner of the message can delete it, buy clicking on the upper right green area)
- you can walk into the POI and objects will be presented once you enter it (when in the editor or Windows/WebGL), when you are on Android and walk into the POI the GPS-AR-Switch is automatically called, so the camera switches from the GPS Cam (larger scale navigation) to the AR Cam (detailed Tracking with ARFOundation). When you leave the POI (literally walk out of it) the Cameras will switch back. This can of course be triggered whenever you want, but for demonstration purposes it whenever you walk into the POI (on Android).

for the basic understanding of the mirror library visit their documentation: https://mirror-networking.gitbook.io/docs/

# GPS-AR Setup
1) You can and should customize the map, using Open Street Maps, for the area you want people to walk around in. On https://www.openstreetmap.org/ select an area and export it as an .osm file. In order to import it to your unity project convert it to an .obj file, following the instructions on: https://osm2world.org/
2) You need to manually set the GPS coodinates of your chosen Point of Reference in GPSInputLocal.cs on the GPSMOveTargetForAndroid Game-Object.
3) It's crucial that you align the map in the editor, so the Point of Reference GameObject points to the above chosen Point of Reference in the real world AND in the unity
map. NOTE: The PointofReference Object needs to be at 0,0,0. To align move the map instead.

For the basic ARSetup and Project Settings, visit the ARFoundation documentation: https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.2/manual/index.html#samples

# Use case
New in this package is the integration of the networking library mirror. With this Code and the DemoScene you have everything at hand to realise sight specific, locationbased, multiplayer theater experiences. The players can connect to the networked virtual world via Desktop (windows build) or via Browser (WebGL build) and via locationbased AR (Android build). In the desktop and broswer application players can navigate in the space via keyboard and mouse, on android players walk around in the physical space in order to move around in the shared virtual space. You can navigate people using the GPS Camera in larger surroundings, like streets or a forest. Once they have arrived a Point Of Interest (POI) you can let them switch to the ARCore Camera, in order to have detailed and anchored view on the content you want them to see or hear or interact with. Once they leave the surroundings of the POI again, you switch back to the GPS navigation.

This package is a further development of "Locationbased AR" and "GPS-AR-Switch", before installing the "networked-locationbase-AR" package, make sure you remove the other packages, as they don't work together.

This is still in development, feel free to share your thoughts and/or troubles.
I will add more detailed documentation and tutorials continously. Feel free to contact me if you want to use the package and need more specific information.

# note
This package is the third of three packages. The previous packages (GPS-AR-Switch and Locationbased AR) you can find as different repositories on my Github page. Make sure you only have one of these packages installed in one project. They don't build up on each other.

This publication is made possible by the Recherchefoerderung of Fonds Darstellende Kuenste under the name of "Vernetzte Fiktionen" 
Gefoerdert vom Fonds Darstellende Kuenste aus Mitteln der Beauftragten der Bundesregierung fuer Kultur und Medien.

I want to thank Friedrich Kirschner, with whom I developed all of the code during the course of the last years. <3

