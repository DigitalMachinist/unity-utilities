using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
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
    #region Fields / Properties

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

    [ Header( "Configuration" )]
    public int PlayerNum = 1;
    public bool IsUpdating = true;
    public bool IsDebugLogging = false;

    GamePadState stateCurrent;
    GamePadState statePrevious;

    //Dictionary<Xbox360GamepadAxis, float> axesCurrent { get; set; }
    //Dictionary<Xbox360GamepadAxis, float> axesPrevious { get; set; }
    //Dictionary<Xbox360GamepadAxis, string> axesToUnityInputMap { get; set; }

    //Dictionary<Xbox360GamepadButton, bool> buttonsCurrent { get; set; }
    //Dictionary<Xbox360GamepadButton, bool> buttonsPrevious { get; set; }
    //Dictionary<Xbox360GamepadButton, string> buttonsToUnityInputMap { get; set; }

    Dictionary<Xbox360GamepadAxis, Func<GamePadState, float>> axesToFuncsMap { get; set; }
    Dictionary<Xbox360GamepadButton, Func<GamePadState, bool>> buttonsToFuncsMap { get; set; }
    Dictionary<Xbox360GamepadButton, FoldableEvent> buttonsToDownEventsMap { get; set; }
    Dictionary<Xbox360GamepadButton, FoldableEvent> buttonsToUpEventsMap { get; set; }

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

    [Header( "Events" )]
    public FoldableEvent Connected;
    public FoldableEvent Disconnected;

    [Header( "A Button" )]
    public FoldableEvent ButtonDownA;
    public FoldableEvent ButtonUpA;

    [Header( "B Button" )]
    public FoldableEvent ButtonDownB;
    public FoldableEvent ButtonUpB;

    [Header( "X Button" )]
    public FoldableEvent ButtonDownX;
    public FoldableEvent ButtonUpX;

    [Header( "Y Button" )]
    public FoldableEvent ButtonDownY;
    public FoldableEvent ButtonUpY;

    [Header( "Left Trigger" )]
    public FoldableEvent ButtonDownLTrigger;
    public FoldableEvent ButtonUpLTrigger;

    [Header( "Right Trigger" )]
    public FoldableEvent ButtonDownRTrigger;
    public FoldableEvent ButtonUpRTrigger;

    [Header( "Left Bumper" )]
    public FoldableEvent ButtonDownLBumper;
    public FoldableEvent ButtonUpLBumper;

    [Header( "Right Bumper" )]
    public FoldableEvent ButtonDownRBumper;
    public FoldableEvent ButtonUpRBumper;

    [Header( "Left Analog Button" )]
    public FoldableEvent ButtonDownLAnalog;
    public FoldableEvent ButtonUpLAnalog;

    [Header( "Right Analog Button" )]
    public FoldableEvent ButtonDownRAnalog;
    public FoldableEvent ButtonUpRAnalog;

    [Header( "Back Button" )]
    public FoldableEvent ButtonDownBack;
    public FoldableEvent ButtonUpBack;

    [Header( "Start Button" )]
    public FoldableEvent ButtonDownStart;
    public FoldableEvent ButtonUpStart;

    #endregion


    #region Unity Message Handlers

    void Start()
    {
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

        // Map from internal button enum to functions that return values from the given state.
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

        // Map controller buttons to button down and button up events to be invoked.
        buttonsToDownEventsMap = new Dictionary<Xbox360GamepadButton, FoldableEvent>()
        {
            { Xbox360GamepadButton.A, ButtonDownA },
            { Xbox360GamepadButton.B, ButtonDownB },
            { Xbox360GamepadButton.X, ButtonDownX },
            { Xbox360GamepadButton.Y, ButtonDownY },
            { Xbox360GamepadButton.LTrigger, ButtonDownLTrigger },
            { Xbox360GamepadButton.RTrigger, ButtonDownRTrigger },
            { Xbox360GamepadButton.LBumper, ButtonDownLBumper },
            { Xbox360GamepadButton.RBumper, ButtonDownRBumper },
            { Xbox360GamepadButton.LAnalog, ButtonDownLAnalog },
            { Xbox360GamepadButton.RAnalog, ButtonDownRAnalog },
            { Xbox360GamepadButton.Back, ButtonDownBack },
            { Xbox360GamepadButton.Start, ButtonDownStart }
        };
        buttonsToUpEventsMap = new Dictionary<Xbox360GamepadButton, FoldableEvent>()
        {
            { Xbox360GamepadButton.A, ButtonUpA },
            { Xbox360GamepadButton.B, ButtonUpB },
            { Xbox360GamepadButton.X, ButtonUpX },
            { Xbox360GamepadButton.Y, ButtonUpY },
            { Xbox360GamepadButton.LTrigger, ButtonUpLTrigger },
            { Xbox360GamepadButton.RTrigger, ButtonUpRTrigger },
            { Xbox360GamepadButton.LBumper, ButtonUpLBumper },
            { Xbox360GamepadButton.RBumper, ButtonUpRBumper },
            { Xbox360GamepadButton.LAnalog, ButtonUpLAnalog },
            { Xbox360GamepadButton.RAnalog, ButtonUpRAnalog },
            { Xbox360GamepadButton.Back, ButtonUpBack },
            { Xbox360GamepadButton.Start, ButtonUpStart },
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
                    buttonsToDownEventsMap[ key ].Event.Invoke();
                }
                else if ( DidButtonPressEnd( key ) )
                {
                    buttonsToUpEventsMap[ key ].Event.Invoke();
                }
            }

            // Detect donnection and disconnection events.
            if ( DidConnectionBegin() )
            {
                Connected.Event.Invoke();
            }
            else if ( DidConnectionEnd() )
            {
                Disconnected.Event.Invoke();
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

    public void SetVibration( float leftMotor, float rightMotor )
    {
        GamePad.SetVibration( PlayerIndex, leftMotor, rightMotor );
    }

    #endregion


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
            //if ( key != GamepadButton. && key != GamepadButton. )
            //{
            //    sb.Append( "\t" );
            //}
            sb.Append( GetButton( key ).ToString() );
            sb.Append( "\n" );
        }
        sb.AppendLine( "" );

        return sb.ToString();
    }
}
