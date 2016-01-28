using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


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

    Dictionary<Xbox360GamepadAxis, float> axesCurrent { get; set; }
    Dictionary<Xbox360GamepadAxis, float> axesPrevious { get; set; }
    Dictionary<Xbox360GamepadAxis, string> axesToUnityInputMap { get; set; }

    Dictionary<Xbox360GamepadButton, bool> buttonsCurrent { get; set; }
    Dictionary<Xbox360GamepadButton, bool> buttonsPrevious { get; set; }
    Dictionary<Xbox360GamepadButton, string> buttonsToUnityInputMap { get; set; }
    Dictionary<Xbox360GamepadButton, FoldableEvent> buttonsToDownEventsMap { get; set; }
    Dictionary<Xbox360GamepadButton, FoldableEvent> buttonsToUpEventsMap { get; set; }

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
        // Current and previous states of each control axis.
        {
            axesCurrent = new Dictionary<Xbox360GamepadAxis, float>()
            {
                { Xbox360GamepadAxis.DPadX, 0f },
                { Xbox360GamepadAxis.DPadY, 0f },
                { Xbox360GamepadAxis.LAnalogX, 0f },
                { Xbox360GamepadAxis.LAnalogY, 0f },
                { Xbox360GamepadAxis.RAnalogX, 0f },
                { Xbox360GamepadAxis.RAnalogY, 0f },
                { Xbox360GamepadAxis.LTrigger, 0f },
                { Xbox360GamepadAxis.RTrigger, 0f }
            };
            axesPrevious = new Dictionary<Xbox360GamepadAxis, float>()
            {
                { Xbox360GamepadAxis.DPadX, 0f },
                { Xbox360GamepadAxis.DPadY, 0f },
                { Xbox360GamepadAxis.LAnalogX, 0f },
                { Xbox360GamepadAxis.LAnalogY, 0f },
                { Xbox360GamepadAxis.RAnalogX, 0f },
                { Xbox360GamepadAxis.RAnalogY, 0f },
                { Xbox360GamepadAxis.LTrigger, 0f },
                { Xbox360GamepadAxis.RTrigger, 0f }
            };
        }

        // Current and previous states of each button.
        {
            buttonsCurrent = new Dictionary<Xbox360GamepadButton, bool>()
            {
                { Xbox360GamepadButton.A, false },
                { Xbox360GamepadButton.B, false },
                { Xbox360GamepadButton.X, false },
                { Xbox360GamepadButton.Y, false },
                { Xbox360GamepadButton.LTrigger, false },
                { Xbox360GamepadButton.RTrigger, false },
                { Xbox360GamepadButton.LBumper, false },
                { Xbox360GamepadButton.RBumper, false },
                { Xbox360GamepadButton.LAnalog, false },
                { Xbox360GamepadButton.RAnalog, false },
                { Xbox360GamepadButton.Back, false },
                { Xbox360GamepadButton.Start, false }
            };
            buttonsPrevious = new Dictionary<Xbox360GamepadButton, bool>()
            {
                { Xbox360GamepadButton.A, false },
                { Xbox360GamepadButton.B, false },
                { Xbox360GamepadButton.X, false },
                { Xbox360GamepadButton.Y, false },
                { Xbox360GamepadButton.LTrigger, false },
                { Xbox360GamepadButton.RTrigger, false },
                { Xbox360GamepadButton.LBumper, false },
                { Xbox360GamepadButton.RBumper, false },
                { Xbox360GamepadButton.LAnalog, false },
                { Xbox360GamepadButton.RAnalog, false },
                { Xbox360GamepadButton.Back, false },
                { Xbox360GamepadButton.Start, false },
            };
        }


        // Map from internal axis and buttons enums to Unity Input Manager keys.
        {
            // Convert the player num to a string once here because it gets read a lot.
            var player = PlayerNum.ToString();

            // Map from internal axis enum to Unity Input Manager joystick axes.
            axesToUnityInputMap = new Dictionary<Xbox360GamepadAxis, string>()
            {
                { Xbox360GamepadAxis.LAnalogX, "L_XAxis_" + player },
                { Xbox360GamepadAxis.LAnalogY, "L_YAxis_" + player },
                { Xbox360GamepadAxis.RAnalogX, "R_XAxis_" + player },
                { Xbox360GamepadAxis.RAnalogY, "R_YAxis_" + player },
                { Xbox360GamepadAxis.LTrigger, "TriggersL_" + player },
                { Xbox360GamepadAxis.RTrigger, "TriggersR_" + player },
                { Xbox360GamepadAxis.DPadX, "DPad_XAxis_" + player },
                { Xbox360GamepadAxis.DPadY, "DPad_YAxis_" + player }
            };

            // Map from internal button enum to Unity Input Manager joystick buttons.
            buttonsToUnityInputMap = new Dictionary<Xbox360GamepadButton, string>()
            {
                { Xbox360GamepadButton.A, "A_" + player },
                { Xbox360GamepadButton.B, "B_" + player },
                { Xbox360GamepadButton.X, "X_" + player },
                { Xbox360GamepadButton.Y, "Y_" + player },
                { Xbox360GamepadButton.LTrigger, "TriggersL_" + player },
                { Xbox360GamepadButton.RTrigger, "TriggersR_" + player },
                { Xbox360GamepadButton.LBumper, "LB_" + player },
                { Xbox360GamepadButton.RBumper, "RB_" + player },
                { Xbox360GamepadButton.LAnalog, "LS_" + player },
                { Xbox360GamepadButton.RAnalog, "RS_" + player },
                { Xbox360GamepadButton.Back, "Back_" + player },
                { Xbox360GamepadButton.Start, "Start_" + player }
            };
        }

        // Map controller buttons to button down and button up events to be invoked.
        {
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
    }

    void Update()
    {
        if ( IsUpdating )
        {
            // Persist the current state as the previous state and sample the new current state out of
            // the Unity Input Manager.
            {
                // Note!
                // These are here to store copies of the maps' keys so that the maps can be modified
                // inside the following foreach loops -- otherwise they would throw exceptions.
                var axesKeys = axesCurrent.Keys.ToArray();
                var buttonsKeys = buttonsCurrent.Keys.ToArray();

                // Read in the new axis states.
                foreach ( var key in axesKeys )
                {
                    axesPrevious[ key ] = axesCurrent[ key ];
                    axesCurrent[ key ] = Input.GetAxis( axesToUnityInputMap[ key ] );
                }

                // Read in the new button states (and translate the triggers into button states).
                foreach ( var key in buttonsKeys )
                {
                    buttonsPrevious[ key ] = buttonsCurrent[ key ];
                    if ( key == Xbox360GamepadButton.LTrigger || key == Xbox360GamepadButton.RTrigger )
                    {
                        // Trigger pathway: Read the axis state and translate into a button value.
                        var axisValue = Input.GetAxis( buttonsToUnityInputMap[ key ] );
                        buttonsCurrent[ key ] = ( axisValue >= TriggerThreshold );
                    }
                    else
                    {
                        // Default pathway: Read the button state directly.
                        var buttonValue = Input.GetButton( buttonsToUnityInputMap[ key ] );
                        buttonsCurrent[ key ] = buttonValue;
                    }

                    // Test for button state change events and emit them as they come up.
                    if ( DidButtonPressBegin( key ) )
                    {
                        var downEvent = buttonsToDownEventsMap[ key ];
                        downEvent.Event.Invoke();
                    }
                    else if ( DidButtonPressEnd( key ) )
                    {
                        var upEvent = buttonsToUpEventsMap[ key ];
                        upEvent.Event.Invoke();
                    }
                }
            }
        }

        if ( IsDebugLogging )
        {
            Debug.Log( this );
        }
    }

    #endregion


    #region Detection of start/end of button presses

    public float Axis( Xbox360GamepadAxis key )
    {
        return axesCurrent[ key ];
    }

    public bool Button( Xbox360GamepadButton key )
    {
        return buttonsCurrent[ key ];
    }

    public bool DidButtonPressBegin( Xbox360GamepadButton b )
    {
        return ( buttonsCurrent[ b ] && !buttonsPrevious[ b ] );
    }

    public bool DidButtonPressEnd( Xbox360GamepadButton b )
    {
        return ( !buttonsCurrent[ b ] && buttonsPrevious[ b ] ) ;
    }

    #endregion

    #region Axis Threshold Functions

    /// <summary>  Check if an axis is past a threshold and was not the check before. </summary>
    /// <remarks>  James, 2014-05-02. </remarks>
    /// <param name="Axis">       The axis to check. </param>
    /// <param name="Threshold">  The threashold between 0 and 1. </param>
    /// <returns>
    ///   true if the axis is past the threshold and previously was not, otherwise false.
    /// </returns>
    public bool AxisJustPastThreshold( Xbox360GamepadAxis Axis, float Threshold )
    {
        float threshold = Mathf.Clamp( Threshold, -1f, 1f );

        if ( threshold > 0 )
        {
            if ( axesCurrent[ Axis ] >= threshold && axesPrevious[ Axis ] < threshold )
                return true;
        }
        else
        {
            if ( axesCurrent[ Axis ] < threshold && axesPrevious[ Axis ] > threshold )
                return true;
        }

        return false;
    }

    #endregion

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append( "Xbox 360 Gamepad " );
        sb.Append( PlayerNum );
        sb.Append( "\n\n" );

        sb.AppendLine( "Axes:\n" );
        foreach ( Xbox360GamepadAxis key in axesCurrent.Keys )
        {
            sb.Append( key.ToString() );
            sb.Append( ": " );
            if ( key != Xbox360GamepadAxis.RAnalogX )
            {
                sb.Append( "\t" );
            }
            sb.Append( axesCurrent[ key ].ToString() );
            sb.Append( "\n" );
        }
        sb.Append( "\n" );

        sb.Append( "Buttons:\n" );
        foreach ( Xbox360GamepadButton key in buttonsCurrent.Keys )
        {
            sb.Append( key.ToString() );
            sb.Append( ": \t" );
            //if ( key != GamepadButton. && key != GamepadButton. )
            //{
            //    sb.Append( "\t" );
            //}
            sb.Append( buttonsCurrent[ key ].ToString() );
            sb.Append( "\n" );
        }
        sb.AppendLine( "" );

        return sb.ToString();
    }
}
