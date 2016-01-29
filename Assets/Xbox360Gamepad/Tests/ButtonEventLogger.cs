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
        Gamepad.LeftTrigger.Pressed.AddListener( () => LogPressed( "left" ) );
        Gamepad.LeftTrigger.Released.AddListener( () => LogReleased( "left" ) );
        Gamepad.RightTrigger.Pressed.AddListener( () => LogPressed( "right" ) );
        Gamepad.RightTrigger.Released.AddListener( () => LogReleased( "right" ) );
        Gamepad.LeftBumper.Pressed.AddListener( () => LogPressed( "left" ) );
        Gamepad.LeftBumper.Released.AddListener( () => LogReleased( "left" ) );
        Gamepad.RightBumper.Pressed.AddListener( () => LogPressed( "right" ) );
        Gamepad.RightBumper.Released.AddListener( () => LogReleased( "right" ) );
        Gamepad.LeftAnalogButton.Pressed.AddListener( () => LogPressed( "left stick" ) );
        Gamepad.LeftAnalogButton.Released.AddListener( () => LogReleased( "left stick" ) );
        Gamepad.RightAnalogButton.Pressed.AddListener( () => LogPressed( "right stick" ) );
        Gamepad.RightAnalogButton.Released.AddListener( () => LogReleased( "right stick" ) );
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
