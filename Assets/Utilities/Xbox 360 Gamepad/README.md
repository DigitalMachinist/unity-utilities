Xbox 360 Gamepad
================

Part of the [unity-utilities](https://github.com/DigitalMachinist/unity-utilities) GitHub repo by [@DigitalMachinist](https://github.com/DigitalMachinist).

This library contains a class that serves as a software interface for 1 to 4 Xbox 360 Gamepads through the [XInputDotNet](https://github.com/speps/XInputDotNet) library. While this implementation provides excellent features, **it will only run on a Windows PC.** Although this limits the platform targets this can be deployed to, the decision to use XInput enables a variety of gamepad features not available through the standard Unity Input Manager.

This library is compatible with both Unity Free and Unity Pro and should run on Unity versions back to 3.x (and possibly earlier).

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

## Usage

There are lots of ways to get control data from this gamepad interface:

Let's say you want to check when the A button is pressed and you have a reference to an Xbox 360 Gamepad named ```gamepad``` in scope. You could:
 - Call the Gamepad's ```GetButton()``` method raw like you would with the Unity Input Manager (handy if you need to iterate over buttons/axes).
 - Access the Gamepad's ```A``` property (cleaner syntax).

```csharp
// Using the raw GetButton() call.
if ( gamepad.GetButton( Xbox360GamepadButton.A ) )
{
  Debug.Log( "A is down!" );
}

// Using the A button property.
if ( gamepad.A )
{
  Debug.Log( "A is down!" );
}
```

Maybe you want to react to only exactly when the A button begins being pressed or was just released. If you're a fan of delegates (and I sure am) it's easy!

 - Subscribe to the ```AButton.Pressed``` ```UnityEvent```.

```csharp
// Using UnityEvent subscriber delegates.
gamepad.AButton.Pressed.AddListener( () => Debug.Log( "A was pressed!" ) );
gamepad.AButton.Released.AddListener( () => Debug.Log( "A was released!" ) );
```

So what about getting something more complicated, like the the direction of the left analog stick? Well, you've got a few options. You could:
- Make 2 separate calls to ```GetAxis()``` like you would with the Unity Input Manager, then assemble them into a new ```Vector2```.
- Call the Gamepad's ```GetVector()``` to get a ```Vector2``` back directly.
- Access the Gamepad's ```LeftAnalog``` property to get a ```Vector2``` directly, but with less work (less work is good).

```csharp
// Using raw GetAxis() calls.
Vector2 leftAnalog1 = new Vector2(
  gamepad.GetAxis( Xbox360GamepadAxis.LAnalogX ),
  gamepad.GetAxis( Xbox360GamepadAxis.LAnalogY )
);

// Using a call to GetVector().
Vector2 leftAnalog2 = gamepad.GetVector( Xbox360GamepadVector.LAnalog );

// Using the LeftAnalog property.
Vector2 leftAnalog3 = gamepad.LeftAnalog;
```

*"Using my analog stick for menus sucks. Sure, I could use the D-Pad, but it's annoying to switch."*

Often, players have a crappy experience trying to use their analog sticks to navigate menus. Just listen for the left analog stick button events that this interface emits by transforming axes into pressed and released events, like so:

```csharp
// Using UnityEvent subscriber delegates.
gamepad.LeftAnalogLeft.Pressed.AddListener( () => Debug.Log( "Left analog pressed left!" ) );
gamepad.LeftAnalogRight.Pressed.AddListener( () => Debug.Log( "Left analog pressed right!" ) );
gamepad.LeftAnalogUp.Pressed.AddListener( () => Debug.Log( "Left analog pressed up!" ) );
gamepad.LeftAnalogDown.Pressed.AddListener( () => Debug.Log( "Left analog pressed down!" ) );
```

*"Wow! That was so much like the A button!"*

Yup. It's the same as far as this interface is concerned. Every button, trigger, analog stick or directional pad on the controller can be handled as a button with the appropriate pressed and released events that any button should have.

*"I need to my controller to vibrate for... reasons."*

Yeah, no problem. You can make it vibrate either slowly or quickly (depending which motor you use) or both at the same time if that's your thing. Try this:

```csharp
gamepad.SetSlowVibration( 0.5f );
gamepad.SetFastVibration( 0.9f );
```

That pretty much convers the basics. Read the source yourself like a grown-up if you need more!
