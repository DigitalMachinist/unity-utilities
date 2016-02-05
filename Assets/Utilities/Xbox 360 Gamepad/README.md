Xbox 360 Gamepad
================

Part of the [unity-utilities](https://github.com/DigitalMachinist/unity-utilities) GitHub repo by [@DigitalMachinist](https://github.com/DigitalMachinist).

This library contains a class that serves as a software interface for 1 to 4 Xbox 360 Gamepads through the [XInputDotNet](https://github.com/speps/XInputDotNet) library. While this implementation provides excellent features, **it will only run on a Windows PC.** Although this limits the platform targets this can be deployed to, the decision to use XInput enables a variety of gamepad features not available through the standard Unity Input Manager.

## Gamepad tester

A gamepad tester scene is included with this library to both assist with debugging gamepad issues and to provide simple examples of usage so getting started with the interface isn't too difficult. It provides visualizations of 1 to 4 gamepads and expresses all of the outputs that this interface provides, so that you can confirm that your gamepads are producing the output you expect.

![It looks just like this!](https://raw.githubusercontent.com/DigitalMachinist/unity-utilities/master/Assets/Utilities/Xbox%20360%20Gamepad/Xbox360Gamepad.png)

## Features

 - Access to raw buttons and control axes via enumerations and properties for both the current and previous frames
 - Access to Vector2 outputs for left and right analog as well as the directional pad
 - Transformation of control axes into button press/release events for cases where it's easier to treat the player's action as a Boolean value than as a floating point (this includes triggers, analog sticks and the directional pad)
 - UnityEvents that signal for any detected button press and release to make mapping controls to button presses simple and flexible in the Unity Inspector
 - Ability to set and get vibration motor speeds
 - A visual gamepad tester that expresses every control output for all 4 controllers simultaneously to help with diagnosing issues and getting started
