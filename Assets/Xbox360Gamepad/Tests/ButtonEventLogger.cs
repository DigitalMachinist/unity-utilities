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
        Gamepad.ButtonDownA.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": A button down!" ) );
        Gamepad.ButtonUpA.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": A button up!" ) );
        Gamepad.ButtonDownB.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": B button down!" ) );
        Gamepad.ButtonUpB.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": B button up!" ) );
        Gamepad.ButtonDownX.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": X button down!" ) );
        Gamepad.ButtonUpX.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": X button up!" ) );
        Gamepad.ButtonDownY.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": Y button down!" ) );
        Gamepad.ButtonUpY.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": Y button up!" ) );
        Gamepad.ButtonDownLTrigger.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left trigger down!" ) );
        Gamepad.ButtonUpLTrigger.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left trigger up!" ) );
        Gamepad.ButtonDownRTrigger.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right trigger down!" ) );
        Gamepad.ButtonUpRTrigger.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right trigger up!" ) );
        Gamepad.ButtonDownLBumper.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left bumper down!" ) );
        Gamepad.ButtonUpLBumper.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left bumper up!" ) );
        Gamepad.ButtonDownRBumper.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right bumper down!" ) );
        Gamepad.ButtonUpRBumper.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right bumper up!" ) );
        Gamepad.ButtonDownLAnalog.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left stick button down!" ) );
        Gamepad.ButtonUpLAnalog.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": left stick button up!" ) );
        Gamepad.ButtonDownRAnalog.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right stick button down!" ) );
        Gamepad.ButtonUpRAnalog.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": right stick button up!" ) );
        Gamepad.ButtonDownBack.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": back button down!" ) );
        Gamepad.ButtonUpBack.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": back button up!" ) );
        Gamepad.ButtonDownStart.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": start button down!" ) );
        Gamepad.ButtonUpStart.AddListener( () => Log( "Gamepad " + Gamepad.PlayerNum + ": start button up!" ) );
    }

    void Log( string message )
    {
        if ( IsDebugLogging )
        {
            Debug.Log( message );
        }
    }
}
