Color Picker
============

Part of the [unity-utilities](https://github.com/DigitalMachinist/unity-utilities) GitHub repo by [@DigitalMachinist](https://github.com/DigitalMachinist).

The ```ColorPicker``` class is a color picker that can sample colors from materials in Unity 3D scenes both in the editor and in real-time during play mode. It accomplishes this using ```Physics.Raycast()``` and cane sample color from materials with uniform color or textured color , as long as the mesh has a ```MshCollider``` component to collide with. It can be purposed for use in editor scripts and tools or used in games both, without need for modification.

This library is compatible with both Unity Free and Unity Pro and should run on Unity versions back to 3.x (and possibly earlier).

**Note:** This library depends on the ```ReadOnlyAttribute``` class (also contained in this repo).

## Usage

Place a ```GameObject``` in your scene and add a ```ColorPicker``` component to it. You now have a working color picker. Congrats.

Now add a quad to the scene, create a new material and give it a color (or a texture). Drag the material onto your quad. Also, make sure your quad has a ```MeshCollider``` component, because the ```ColorPicker``` uses ```Physics.Raycast()``` work its magic. Chill out. A ```MeshCollider``` composed of 2 tris is cheaper to collide than a ```BoxCollider```!

**If you are using a texture, make sure that your texture is imported using advanced mode and switch on the Read/Write Enabled flag or sampling of colors will fail!**

Once you have a ```ColorPicker``` and some geometry with a material to sample, you're good to go! Aim your ```ColorPicker``` at that quad and enjoy the pretty colors (assuming you chose pretty colors).

You can call ```ColorPicker.Sample()``` directly to sample a color on-demand.

The ```ColorPicker.Color``` (read-only) property gives you access to the most recently sampled color.

The ```ColorPicker``` component uses the ```[ExecuteInEditMode]```, so it can sample colors from textures in your scene even while your scene isn't playing, although it will only sample when the scene is changed.

And that's about it! Try playing around with the settings available in the Inspector to learn more.
