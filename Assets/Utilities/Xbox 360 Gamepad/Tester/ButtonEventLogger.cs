using UnityEngine;

public class ButtonEventLogger : MonoBehaviour
{
    public Xbox360Gamepad Gamepad;
    public bool IsDebugLogging = true;

    // Use this for initialization
    void Start()
    {
        Gamepad.Connected.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + " connected!" ) );
        Gamepad.Disconnected.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + " disconnected!" ) );
        Gamepad.AButton.Pressed.AddListener( () => LogPressed( "A" ) );
        Gamepad.AButton.Released.AddListener( () => LogReleased( "A" ) );
        Gamepad.BButton.Pressed.AddListener( () => LogPressed( "B" ) );
        Gamepad.BButton.Released.AddListener( () => LogReleased( "B" ) );
        Gamepad.XButton.Pressed.AddListener( () => LogPressed( "X" ) );
        Gamepad.XButton.Released.AddListener( () => LogReleased( "X" ) );
        Gamepad.YButton.Pressed.AddListener( () => LogPressed( "Y" ) );
        Gamepad.YButton.Released.AddListener( () => LogReleased( "Y" ) );
        Gamepad.LeftTriggerButton.Pressed.AddListener( () => LogPressed( "left" ) );
        Gamepad.LeftTriggerButton.Released.AddListener( () => LogReleased( "left" ) );
        Gamepad.RightTriggerButton.Pressed.AddListener( () => LogPressed( "right" ) );
        Gamepad.RightTriggerButton.Released.AddListener( () => LogReleased( "right" ) );
        Gamepad.LeftBumperButton.Pressed.AddListener( () => LogPressed( "left" ) );
        Gamepad.LeftBumperButton.Released.AddListener( () => LogReleased( "left" ) );
        Gamepad.RightBumperButton.Pressed.AddListener( () => LogPressed( "right" ) );
        Gamepad.RightBumperButton.Released.AddListener( () => LogReleased( "right" ) );
        Gamepad.LeftAnalogButton.Pressed.AddListener( () => LogPressed( "left stick" ) );
        Gamepad.LeftAnalogButton.Released.AddListener( () => LogReleased( "left stick" ) );
        Gamepad.LeftAnalogLeft.Pressed.AddListener( () => LogPressed( "left analog left" ) );
        Gamepad.LeftAnalogLeft.Released.AddListener( () => LogReleased( "left analog left" ) );
        Gamepad.LeftAnalogRight.Pressed.AddListener( () => LogPressed( "left analog right" ) );
        Gamepad.LeftAnalogRight.Released.AddListener( () => LogReleased( "left analog right" ) );
        Gamepad.LeftAnalogUp.Pressed.AddListener( () => LogPressed( "left analog up" ) );
        Gamepad.LeftAnalogUp.Released.AddListener( () => LogReleased( "left analog up" ) );
        Gamepad.LeftAnalogDown.Pressed.AddListener( () => LogPressed( "left analog down" ) );
        Gamepad.LeftAnalogDown.Released.AddListener( () => LogReleased( "left analog down" ) );
        Gamepad.RightAnalogButton.Pressed.AddListener( () => LogPressed( "right stick" ) );
        Gamepad.RightAnalogButton.Released.AddListener( () => LogReleased( "right stick" ) );
        Gamepad.RightAnalogLeft.Pressed.AddListener( () => LogPressed( "right analog left" ) );
        Gamepad.RightAnalogLeft.Released.AddListener( () => LogReleased( "right analog left" ) );
        Gamepad.RightAnalogRight.Pressed.AddListener( () => LogPressed( "right analog right" ) );
        Gamepad.RightAnalogRight.Released.AddListener( () => LogReleased( "right analog right" ) );
        Gamepad.RightAnalogUp.Pressed.AddListener( () => LogPressed( "right analog up" ) );
        Gamepad.RightAnalogUp.Released.AddListener( () => LogReleased( "right analog up" ) );
        Gamepad.RightAnalogDown.Pressed.AddListener( () => LogPressed( "right analog down" ) );
        Gamepad.RightAnalogDown.Released.AddListener( () => LogReleased( "right analog down" ) );
        Gamepad.DPadLeft.Pressed.AddListener( () => LogPressed( "dpad left" ) );
        Gamepad.DPadLeft.Released.AddListener( () => LogReleased( "dpad left" ) );
        Gamepad.DPadRight.Pressed.AddListener( () => LogPressed( "dpad right" ) );
        Gamepad.DPadRight.Released.AddListener( () => LogReleased( "dpad right" ) );
        Gamepad.DPadUp.Pressed.AddListener( () => LogPressed( "dpad up" ) );
        Gamepad.DPadUp.Released.AddListener( () => LogReleased( "dpad up" ) );
        Gamepad.DPadDown.Pressed.AddListener( () => LogPressed( "dpad down" ) );
        Gamepad.DPadDown.Released.AddListener( () => LogReleased( "dpad down" ) );
        Gamepad.BackButton.Pressed.AddListener( () => LogPressed( "back" ) );
        Gamepad.BackButton.Released.AddListener( () => LogReleased( "back" ) );
        Gamepad.StartButton.Pressed.AddListener( () => LogPressed( "start" ) );
        Gamepad.StartButton.Released.AddListener( () => LogReleased( "start" ) );
    }

    void LogPressed( string buttonName )
    {
        Log( "Gamepad " + Gamepad.PlayerNum + ": " + buttonName + " button pessed!" );
    }

    void LogReleased( string buttonName )
    {
        Log( "Gamepad " + Gamepad.PlayerNum + ": " + buttonName + " button released!" );
    }

    void Log( string message )
    {
        if ( IsDebugLogging )
        {
            Debug.Log( message );
        }
    }
}
