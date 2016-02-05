using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XInputDotNetPure;


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
    LAnalogButton,
    LAnalogLeft,
    LAnalogRight,
    LAnalogUp,
    LAnalogDown,
    RAnalogButton,
    RAnalogLeft,
    RAnalogRight,
    RAnalogUp,
    RAnalogDown,
    DPadLeft,
    DPadRight,
    DPadUp,
    DPadDown,
    Back,
    Start
};


/// <summary>
/// An enumeration of control axes that are measured by the Xbox 360 Gamepad.
/// </summary>
public enum Xbox360GamepadVector : byte
{
    LAnalog,
    RAnalog,
    DPad
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
    #region Fields / Properties

    [ Header( "Configuration" )]
    [Range( 1, 4 )]
    public int PlayerNum = 1;
    [Range( 0f, 1f )]
    public float TriggerPressedThreshold = 0.2f;
    [Range( 0f, 1f )]
    public float LeftAnalogPressedThreshold = 0.2f;
    [Range( 0f, 1f )]
    public float RightAnalogPressedThreshold = 0.2f;
    public bool IsUpdating = true;
    public bool IsDebugLogging = false;

    [Header( "Events" )]
    public FoldableEvent Connected;
    public FoldableEvent Disconnected;
    public ButtonEvents AButton;
    public ButtonEvents BButton;
    public ButtonEvents XButton;
    public ButtonEvents YButton;
    public ButtonEvents LeftTriggerButton;
    public ButtonEvents RightTriggerButton;
    public ButtonEvents LeftBumperButton;
    public ButtonEvents RightBumperButton;
    public ButtonEvents LeftAnalogButton;
    public ButtonEvents LeftAnalogLeft;
    public ButtonEvents LeftAnalogRight;
    public ButtonEvents LeftAnalogUp;
    public ButtonEvents LeftAnalogDown;
    public ButtonEvents RightAnalogButton;
    public ButtonEvents RightAnalogLeft;
    public ButtonEvents RightAnalogRight;
    public ButtonEvents RightAnalogUp;
    public ButtonEvents RightAnalogDown;
    public ButtonEvents DPadLeft;
    public ButtonEvents DPadRight;
    public ButtonEvents DPadUp;
    public ButtonEvents DPadDown;
    public ButtonEvents BackButton;
    public ButtonEvents StartButton;

    Dictionary<Xbox360GamepadAxis, Func<GamePadState, float>> axesToFuncsMap;
    Dictionary<Xbox360GamepadButton, ButtonEvents> buttonsToEventsMap;
    Dictionary<Xbox360GamepadButton, Func<GamePadState, bool>> buttonsToFuncsMap;
    Dictionary<Xbox360GamepadVector, Func<GamePadState, Vector2>> vectorsToFuncsMap;
    GamePadState stateCurrent;
    GamePadState statePrevious;
    float slowVibration;
    float fastVibration;
    bool didVibrationChange;

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

    #region Control Getters

    public bool A
    {
        get { return GetButton( Xbox360GamepadButton.A ); }
    }

    public bool B
    {
        get { return GetButton( Xbox360GamepadButton.B ); }
    }

    public bool X
    {
        get { return GetButton( Xbox360GamepadButton.X ); }
    }

    public bool Y
    {
        get { return GetButton( Xbox360GamepadButton.Y ); }
    }

    public bool LeftTrigger
    {
        get { return GetButton( Xbox360GamepadButton.LTrigger ); }
    }

    public bool RightTrigger
    {
        get { return GetButton( Xbox360GamepadButton.RTrigger ); }
    }

    public bool LeftBumper
    {
        get { return GetButton( Xbox360GamepadButton.LBumper ); }
    }

    public bool RightBumper
    {
        get { return GetButton( Xbox360GamepadButton.RBumper ); }
    }

    public Vector2 LeftAnalog
    {
        get { return GetVector( Xbox360GamepadVector.LAnalog ); }
    }

    public Vector2 RightAnalog
    {
        get { return GetVector( Xbox360GamepadVector.RAnalog ); }
    }

    public Vector2 DPad
    {
        get { return GetVector( Xbox360GamepadVector.DPad ); }
    }

    public bool BACK
    {
        get { return GetButton( Xbox360GamepadButton.Back ); }
    }
    
    public bool START
    {
        get { return GetButton( Xbox360GamepadButton.Start ); }
    }

    #endregion

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

        // Starting vibration values.
        slowVibration = 0f;
        fastVibration = 0f;
        didVibrationChange = false;

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
                // Transform DPad left and right button bools into a floating point axis.
                if ( state.DPad.Left == ButtonState.Pressed )               return -1f;
                else if ( stateCurrent.DPad.Right == ButtonState.Pressed )  return  1f;
                else                                                        return  0f;
            } },
            { Xbox360GamepadAxis.DPadY, state => {
                // Transform DPad down and up button bools into a floating point axis.
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
            { Xbox360GamepadButton.LTrigger, state => {
                // Transform left trigger floating point axis into a bool.
                return state.Triggers.Left >= TriggerPressedThreshold; 
            } },
            { Xbox360GamepadButton.RTrigger, state => {
                // Transform right trigger floating point axis into a bool.
                return state.Triggers.Right >= TriggerPressedThreshold; 
            } },
            { Xbox360GamepadButton.LAnalogLeft, state => {
                // Transform left analog stick x-axis floating point axis into a bool.
                return state.ThumbSticks.Left.X <= -LeftAnalogPressedThreshold;
            } },
            { Xbox360GamepadButton.LAnalogRight, state => {
                // Transform left analog stick x-axis floating point axis into a bool.
                return state.ThumbSticks.Left.X >= LeftAnalogPressedThreshold;
            } },
            { Xbox360GamepadButton.LAnalogDown, state => {
                // Transform left analog stick y-axis floating point axis into a bool.
                return state.ThumbSticks.Left.Y <= -LeftAnalogPressedThreshold;
            } },
            { Xbox360GamepadButton.LAnalogUp, state => {
                // Transform left analog stick y-axis floating point axis into a bool.
                return state.ThumbSticks.Left.Y >= LeftAnalogPressedThreshold;
            } },
            { Xbox360GamepadButton.RAnalogLeft, state => {
                // Transform right analog stick x-axis floating point axis into a bool.
                return state.ThumbSticks.Right.X <= -RightAnalogPressedThreshold;
            } },
            { Xbox360GamepadButton.RAnalogRight, state => {
                // Transform right analog stick x-axis floating point axis into a bool.
                return state.ThumbSticks.Right.X >= RightAnalogPressedThreshold;
            } },
            { Xbox360GamepadButton.RAnalogDown, state => {
                // Transform right analog stick y-axis floating point axis into a bool.
                return state.ThumbSticks.Right.Y <= -RightAnalogPressedThreshold;
            } },
            { Xbox360GamepadButton.RAnalogUp, state => {
                // Transform right analog stick y-axis floating point axis into a bool.
                return state.ThumbSticks.Right.Y >= RightAnalogPressedThreshold;
            } },
            { Xbox360GamepadButton.DPadLeft, state => state.DPad.Left == ButtonState.Pressed },
            { Xbox360GamepadButton.DPadRight, state => state.DPad.Right == ButtonState.Pressed },
            { Xbox360GamepadButton.DPadDown, state => state.DPad.Down == ButtonState.Pressed },
            { Xbox360GamepadButton.DPadUp, state => state.DPad.Up == ButtonState.Pressed },
            { Xbox360GamepadButton.LBumper, state => state.Buttons.LeftShoulder == ButtonState.Pressed },
            { Xbox360GamepadButton.RBumper, state => state.Buttons.RightShoulder == ButtonState.Pressed },
            { Xbox360GamepadButton.LAnalogButton, state => state.Buttons.LeftStick == ButtonState.Pressed },
            { Xbox360GamepadButton.RAnalogButton, state => state.Buttons.RightStick == ButtonState.Pressed },
            { Xbox360GamepadButton.Back, state => state.Buttons.Back == ButtonState.Pressed },
            { Xbox360GamepadButton.Start, state => state.Buttons.Start == ButtonState.Pressed }
        };

        // Map from internal vectors enum to functions that return values from the given state.
        vectorsToFuncsMap = new Dictionary<Xbox360GamepadVector, Func<GamePadState, Vector2>>()
        {
            { Xbox360GamepadVector.LAnalog, state => {
                // Get both the x and y axes of the left analog stick and form them into a 2D vector.
                return new Vector2(
                    axesToFuncsMap[ Xbox360GamepadAxis.LAnalogX ]( state ),
                    axesToFuncsMap[ Xbox360GamepadAxis.LAnalogY ]( state )
                );
            } },
            { Xbox360GamepadVector.RAnalog, state => {
                // Get both the x and y axes of the right analog stick and form them into a 2D vector.
                return new Vector2(
                    axesToFuncsMap[ Xbox360GamepadAxis.RAnalogX ]( state ),
                    axesToFuncsMap[ Xbox360GamepadAxis.RAnalogY ]( state )
                );
            } },
            { Xbox360GamepadVector.DPad, state => {
                // Get both the x and y axes of the directional pad and form them into a 2D vector.
                return new Vector2( 
                    axesToFuncsMap[ Xbox360GamepadAxis.DPadX ]( state ), 
                    axesToFuncsMap[ Xbox360GamepadAxis.DPadY ]( state ) 
                );
            } }
        };

        // Map from internal buttons enum to button events to be invoked.
        buttonsToEventsMap = new Dictionary<Xbox360GamepadButton, ButtonEvents>()
        {
            { Xbox360GamepadButton.A, AButton },
            { Xbox360GamepadButton.B, BButton },
            { Xbox360GamepadButton.X, XButton },
            { Xbox360GamepadButton.Y, YButton },
            { Xbox360GamepadButton.LTrigger, LeftTriggerButton },
            { Xbox360GamepadButton.RTrigger, RightTriggerButton },
            { Xbox360GamepadButton.LBumper, LeftBumperButton },
            { Xbox360GamepadButton.RBumper, RightBumperButton },
            { Xbox360GamepadButton.LAnalogButton, LeftAnalogButton },
            { Xbox360GamepadButton.LAnalogLeft, LeftAnalogLeft },
            { Xbox360GamepadButton.LAnalogRight, LeftAnalogRight },
            { Xbox360GamepadButton.LAnalogUp, LeftAnalogUp },
            { Xbox360GamepadButton.LAnalogDown, LeftAnalogDown },
            { Xbox360GamepadButton.RAnalogButton, RightAnalogButton },
            { Xbox360GamepadButton.RAnalogLeft, RightAnalogLeft },
            { Xbox360GamepadButton.RAnalogRight, RightAnalogRight },
            { Xbox360GamepadButton.RAnalogUp, RightAnalogUp },
            { Xbox360GamepadButton.RAnalogDown, RightAnalogDown },
            { Xbox360GamepadButton.DPadLeft, DPadLeft },
            { Xbox360GamepadButton.DPadRight, DPadRight },
            { Xbox360GamepadButton.DPadUp, DPadUp },
            { Xbox360GamepadButton.DPadDown, DPadDown },
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

            // Set vibration state if it was updated within the last frame.
            if ( didVibrationChange )
            {
                GamePad.SetVibration( PlayerIndex, slowVibration, fastVibration );
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

    public float SetFastVibration()
    {
        return fastVibration;
    }

    public GamePadState GetState()
    {
        return stateCurrent;
    }

    public GamePadState GetStatePrevious()
    {
        return statePrevious;
    }

    public float GetSlowVibration()
    {
        return slowVibration;
    }

    public Vector2 GetVector( Xbox360GamepadVector key )
    {
        return vectorsToFuncsMap[ key ]( stateCurrent );
    }

    public Vector2 GetVectorPrevious( Xbox360GamepadVector key )
    {
        return vectorsToFuncsMap[ key ]( statePrevious );
    }

    public void SetFastVibration( float value )
    {
        fastVibration = value;
        didVibrationChange = true;
    }

    public void SetSlowVibration( float value )
    {
        slowVibration = value;
        didVibrationChange = true;
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
