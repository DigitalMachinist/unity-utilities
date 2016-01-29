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
        Gamepad.AButton.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": A button pressed!" ) );
        Gamepad.AButton.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": A button released!" ) );
        Gamepad.BButton.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": B button pressed!" ) );
        Gamepad.BButton.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": B button released!" ) );
        Gamepad.XButton.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": X button pressed!" ) );
        Gamepad.XButton.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": X button released!" ) );
        Gamepad.YButton.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": Y button pressed!" ) );
        Gamepad.YButton.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": Y button released!" ) );
        Gamepad.LeftTrigger.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left trigger pressed!" ) );
        Gamepad.LeftTrigger.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left trigger released!" ) );
        Gamepad.RightTrigger.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right trigger pressed!" ) );
        Gamepad.RightTrigger.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right trigger released!" ) );
        Gamepad.LeftBumper.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left bumper pressed!" ) );
        Gamepad.LeftBumper.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left bumper released!" ) );
        Gamepad.RightBumper.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right bumper pressed!" ) );
        Gamepad.RightBumper.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right bumper released!" ) );
        Gamepad.LeftAnalogButton.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left stick button pressed!" ) );
        Gamepad.LeftAnalogButton.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left stick button released!" ) );
        Gamepad.RightAnalogButton.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right stick button pressed!" ) );
        Gamepad.RightAnalogButton.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right stick button released!" ) );
        Gamepad.BackButton.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": back button pressed!" ) );
        Gamepad.BackButton.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": back button released!" ) );
        Gamepad.StartButton.Pressed.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": start button pressed!" ) );
        Gamepad.StartButton.Released.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": start button released!" ) );
    }

    void Log( string message )
    {
        if ( IsDebugLogging )
        {
            Debug.Log( message );
        }
    }
}
