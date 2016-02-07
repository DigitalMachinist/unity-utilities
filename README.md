unity-utilities
===============

A collection of utilities for Unity (with accompanying documentation and test scenes) that I use in personal projects and at Axon Interactive. It includes a variety of tools that can be used together or separately to make game development a little bit easier.

The root folder of this project contains a series of .unitypackage files that make importing specific features into projects, but of course you are free to make use of these files however suits you best.

## Attributes

[Assets/Utilities/Attributes](https://github.com/DigitalMachinist/unity-utilities/tree/master/Assets/Utilities/Attributes)

This library contains a collection of attributes that modify how different types are rendered in the Inspector.

## Color Picker

[Assets/Utilities/Color Picker](https://github.com/DigitalMachinist/unity-utilities/tree/master/Assets/Utilities/Color%20Picker)

This library contains the ```ColorPicker``` component; a utility to sample colors from geometry in Unity scenes both in the editor and while playing.

## Extension Methods

[Assets/Utilities/Extension Methods](https://github.com/DigitalMachinist/unity-utilities/tree/master/Assets/Utilities/Extension%20Methods)

This library contains a collection of extension methods for built-in Unity types that make common development tasks easier.

## Foldable Events and Promises

[Assets/Utilities/Foldable Events](https://github.com/DigitalMachinist/unity-utilities/tree/master/Assets/Utilities/Foldable%20Events)

This library contains the ```FoldableEvent``` class, which folds up in the inspector to hide the base ```UnityEvent``` they rely upon, and the ```FoldablePromise``` class which provides powerful features for unraveling dependency chains, such as complex scene loading processes.

## Health System

[Assets/Utilities/Health](https://github.com/DigitalMachinist/unity-utilities/tree/master/Assets/Utilities/Health)

This library contains the ```Health``` component, which provides the usual HP health systems one might expect and exposes a wide variety of events to signal changes in health. Also, this component provides special handling for regenerative, degenerative, and effects that heal above the standard maximum HP.

## State Machine

[Assets/Utilities/State Machine](https://github.com/DigitalMachinist/unity-utilities/tree/master/Assets/Utilities/State%20Machine)

This library contains the ```StateMachine``` component, which augments the built-in ```Animator``` to provide a full-featured state machine for handling game state, control context, and UI screens. It also exposes the ```State``` class, a specialized ```StateMachineBehaviour``` which complements ```StateMachine``` to provide these features.

## Xbox 360 Gamepad

[Assets/Utilities/Xbox 360 Gamepad](https://github.com/DigitalMachinist/unity-utilities/tree/master/Assets/Utilities/Xbox%20360%20Gamepad)

This library contains the ```Xbox360Gamepad``` component, a robust and full-featured control abstraction for 1 to 4 Xbox 360 Gamepads. It exposes all buttons as events, provides a variety of options for accessing control axes and buttons, and provides an API for controlling vibration. It is based on XInput, and thus is only compatible with Windows PCs.
