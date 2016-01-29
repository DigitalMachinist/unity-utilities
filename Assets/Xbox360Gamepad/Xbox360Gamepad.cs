using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XInputDotNetPure;


/// <summary>
/// An enumeration of button signals that are emitted from the Xbox 360 Gamepad.
/// </summary>
public enum Xbox360GamepadButton : byte
{
    A,
    B,
    X,
    Y,
    LTrigger,
    RTrigger,
    LBumper,
    RBumper,
    LAnalog,
    RAnalog,
    Back,
    Start
};


/// <summary>
/// An enumeration of control axes that are measured by the Xbox 360 Gamepad.
/// </summary>
public enum Xbox360GamepadAxis : byte
{
    LAnalogX,
    LAnalogY,
    RAnalogX,
    RAnalogY,
    LTrigger,
    RTrigger,
    DPadX,
    DPadY
}


/// <summary>
/// A data structure to wrap up the events that a given button might invoke.
/// </summary>
[Serializable]
public struct ButtonEvents
{
    public UnityEvent Pressed;
    public UnityEvent Released;
}


/// <summary>
/// A stateful representation of an Xbox 360 Gamepad that can automatically remain syncronized
/// with Unity's Input Manager. By keeping the previous button state as well as the most current
/// one, this class is able to detect when a button's state has changed and emit an event to
/// notify other listeners.
/// </summary>
/// <remarks>
/// This all is based on the assumption that positive x faces right, and positive y faces up.
///          pos +y
///             ^
///             ^
///  neg -x <<<<<>>>>> pos +x
///             v
///             v
///          neg -y
///</remarks>
public class Xbox360Gamepad : MonoBehaviour
{
    #region Static Members
    
    static float triggerThreshold = 0.2f;
    public static float TriggerThreshold
    {
        get { return triggerThreshold; }
        set
        {
            if ( value < 0f || value > 1f )
            {
                throw new ArgumentOutOfRangeException( "value", "Value must belong to the range [0f, 1f]!" );
            }
            triggerThreshold = value;
        }
    }

    #endregion


    #region Fields / Properties

    [ Header( "Configuration" )]
    public int PlayerNum = 1;
    public bool IsUpdating = true;
    public bool IsDebugLogging = false;
    
    [Header( "Events" )]
    public FoldableEvent Connected;
    public FoldableEvent Disconnected;
    public ButtonEvents AButton;
    public ButtonEvents BButton;
    public ButtonEvents XButton;
    public ButtonEvents YButton;
    public ButtonEvents LeftTrigger;
    public ButtonEvents RightTrigger;
    public ButtonEvents LeftBumper;
    public ButtonEvents RightBumper;
    public ButtonEvents LeftAnalogButton;
    public ButtonEvents RightAnalogButton;
    public ButtonEvents BackButton;
    public ButtonEvents StartButton;

    Dictionary<Xbox360GamepadAxis, Func<GamePadState, float>> axesToFuncsMap;
    Dictionary<Xbox360GamepadButton, Func<GamePadState, bool>> buttonsToFuncsMap;
    Dictionary<Xbox360GamepadButton, ButtonEvents> buttonsToEventsMap;
    GamePadState stateCurrent;
    GamePadState statePrevious;

    public bool IsConnected
    {
        get { return stateCurrent.IsConnected; }
    }

    public PlayerIndex PlayerIndex
    {
        get { return (PlayerIndex)( PlayerNum - 1 ); }
    }

    public bool WasConnected
    {
        get { return statePrevious.IsConnected; }
    }

    #endregion


    #region Event & Message Handlers

    void Start()
    {
        // When the controller starts up, fire a connected event right away (if it's actually connected).
        if ( IsConnected )
        {
            Connected.Invoke();
        }

        // Make sure button presses end if the controller disconnects.
        Disconnected.AddListener( EndAllButtonPresses );
    }

    void Awake()
    {
        // Initialize the state variables.
        statePrevious = GamePad.GetState( PlayerIndex );
        stateCurrent = GamePad.GetState( PlayerIndex );

        // Map from internal axes enum to functions that return values from the given state.
        axesToFuncsMap = new Dictionary<Xbox360GamepadAxis, Func<GamePadState, float>>()
        {
            { Xbox360GamepadAxis.LAnalogX, state => state.ThumbSticks.Left.X },
            { Xbox360GamepadAxis.LAnalogY, state => state.ThumbSticks.Left.Y },
            { Xbox360GamepadAxis.RAnalogX, state => state.ThumbSticks.Right.X },
            { Xbox360GamepadAxis.RAnalogY, state => state.ThumbSticks.Right.Y },
            { Xbox360GamepadAxis.LTrigger, state => state.Triggers.Left },
            { Xbox360GamepadAxis.RTrigger, state => state.Triggers.Right },
            { Xbox360GamepadAxis.DPadX, state => {
                if ( state.DPad.Left == ButtonState.Pressed )               return -1f;
                else if ( stateCurrent.DPad.Right == ButtonState.Pressed )  return  1f;
                else                                                        return  0f;
            } },
            { Xbox360GamepadAxis.DPadY, state => {
                if ( state.DPad.Down == ButtonState.Pressed )               return -1f;
                else if ( stateCurrent.DPad.Up == ButtonState.Pressed )     return  1f;
                else                                                        return  0f;
            } }
        };

        // Map from internal buttons enum to functions that return values from the given state.
        buttonsToFuncsMap = new Dictionary<Xbox360GamepadButton, Func<GamePadState, bool>>()
        {
            { Xbox360GamepadButton.A, state => state.Buttons.A == ButtonState.Pressed },
            { Xbox360GamepadButton.B, state => state.Buttons.B == ButtonState.Pressed },
            { Xbox360GamepadButton.X, state => state.Buttons.X == ButtonState.Pressed },
            { Xbox360GamepadButton.Y, state => state.Buttons.Y == ButtonState.Pressed },
            { Xbox360GamepadButton.LTrigger, state => state.Triggers.Left >= TriggerThreshold },
            { Xbox360GamepadButton.RTrigger, state => state.Triggers.Right >= TriggerThreshold },
            { Xbox360GamepadButton.LBumper, state => state.Buttons.LeftShoulder == ButtonState.Pressed },
            { Xbox360GamepadButton.RBumper, state => state.Buttons.RightShoulder == ButtonState.Pressed },
            { Xbox360GamepadButton.LAnalog, state => state.Buttons.LeftStick == ButtonState.Pressed },
            { Xbox360GamepadButton.RAnalog, state => state.Buttons.RightStick == ButtonState.Pressed },
            { Xbox360GamepadButton.Back, state => state.Buttons.Back == ButtonState.Pressed },
            { Xbox360GamepadButton.Start, state => state.Buttons.Start == ButtonState.Pressed }
        };

        // Map from internal buttons enum to button events to be invoked.
        buttonsToEventsMap = new Dictionary<Xbox360GamepadButton, ButtonEvents>()
        {
            { Xbox360GamepadButton.A, AButton },
            { Xbox360GamepadButton.B, BButton },
            { Xbox360GamepadButton.X, XButton },
            { Xbox360GamepadButton.Y, YButton },
            { Xbox360GamepadButton.LTrigger, LeftTrigger },
            { Xbox360GamepadButton.RTrigger, RightTrigger },
            { Xbox360GamepadButton.LBumper, LeftBumper },
            { Xbox360GamepadButton.RBumper, RightBumper },
            { Xbox360GamepadButton.LAnalog, LeftAnalogButton },
            { Xbox360GamepadButton.RAnalog, RightAnalogButton },
            { Xbox360GamepadButton.Back, BackButton },
            { Xbox360GamepadButton.Start, StartButton }
        };
    }

    void Update()
    {
        if ( IsUpdating )
        {
            // Persist the current state as the previous state and sample the new current state out of
            // the Unity Input Manager.
            statePrevious = stateCurrent;
            stateCurrent = GamePad.GetState( PlayerIndex );

            // Detect button down and button up events.
            foreach ( var key in buttonsToFuncsMap.Keys )
            {
                if ( DidButtonPressBegin( key ) )
                {
                    buttonsToEventsMap[ key ].Pressed.Invoke();
                }
                else if ( DidButtonPressEnd( key ) )
                {
                    buttonsToEventsMap[ key ].Released.Invoke();
                }
            }

            // Detect donnection and disconnection events.
            if ( DidConnectionBegin() )
            {
                Connected.Invoke();
            }
            else if ( DidConnectionEnd() )
            {
                Disconnected.Invoke();
            }
        }

        if ( IsDebugLogging )
        {
            Debug.Log( this );
        }
    }

    #endregion


    #region Public API

    public bool DidButtonPressBegin( Xbox360GamepadButton key )
    {
        return ( GetButton( key ) && !GetButtonPrevious( key ) );
    }

    public bool DidButtonPressEnd( Xbox360GamepadButton key )
    {
        return ( !GetButton( key ) && GetButtonPrevious( key ) );
    }

    public bool DidConnectionBegin()
    {
        return ( IsConnected && !WasConnected );
    }

    public bool DidConnectionEnd()
    {
        return ( !IsConnected && WasConnected );
    }

    public float GetAxis( Xbox360GamepadAxis key )
    {
        return axesToFuncsMap[ key ]( stateCurrent );
    }

    public float GetAxisPrevious( Xbox360GamepadAxis key )
    {
        return axesToFuncsMap[ key ]( statePrevious );
    }

    public bool GetButton( Xbox360GamepadButton key )
    {
        return buttonsToFuncsMap[ key ]( stateCurrent );
    }

    public bool GetButtonPrevious( Xbox360GamepadButton key )
    {
        return buttonsToFuncsMap[ key ]( statePrevious );
    }

    public GamePadState GetState()
    {
        return stateCurrent;
    }

    public GamePadState GetStatePrevious()
    {
        return statePrevious;
    }

    public void SetVibration( float leftMotor, float rightMotor )
    {
        GamePad.SetVibration( PlayerIndex, leftMotor, rightMotor );
    }

    #endregion


    void EndAllButtonPresses()
    {
        foreach ( var key in buttonsToEventsMap.Keys )
        {
            if ( GetButton( key ) )
            {
                buttonsToEventsMap[ key ].Released.Invoke();
            }
        }
    }


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append( "Xbox 360 Gamepad " );
        sb.Append( PlayerNum );
        sb.Append( "\n\n" );

        sb.AppendLine( "Axes:\n" );
        foreach ( Xbox360GamepadAxis key in axesToFuncsMap.Keys )
        {
            sb.Append( key.ToString() );
            sb.Append( ": " );
            if ( key != Xbox360GamepadAxis.RAnalogX )
            {
                sb.Append( "\t" );
            }
            sb.Append( GetAxis( key ).ToString() );
            sb.Append( "\n" );
        }
        sb.Append( "\n" );

        sb.Append( "Buttons:\n" );
        foreach ( Xbox360GamepadButton key in buttonsToFuncsMap.Keys )
        {
            sb.Append( key.ToString() );
            sb.Append( ": \t" );
            sb.Append( GetButton( key ).ToString() );
            sb.Append( "\n" );
        }
        sb.AppendLine( "" );

        return sb.ToString();
    }
}
