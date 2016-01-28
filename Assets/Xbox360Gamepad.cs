using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// An enumeration of button signals that are emitted from the Xbox 360 Gamepad.
/// </summary>
public enum GamepadButton : byte
{
    NONE = 0,
    A,
    B,
    X,
    Y,
    Back,
    Start,
    LAnalog,
    RAnalog,
    LBumper,
    RBumper
};


/// <summary>
/// An enumeration of control axes that are measured by the Xbox 360 Gamepad.
/// </summary>
public enum GamepadAxis : byte
{
    NONE = 0,
    DPadX,
    DPadY,
    LAnalogX,
    LAnalogY,
    RAnalogX,
    RAnalogY,
    RTrigger,
    LTrigger
}


/// <summary>
///
/// </summary>
//[Serializable]
//public class AxisEvent : FoldableEvent<UnityEvent<float>, float> { }


/// <summary>
///
/// </summary>
[Serializable]
public class ButtonEvent : FoldableEvent<UnityEvent<bool>, bool> { }


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

    [Header( "Configuration" )]
    public int PlayerNum = 1;
    public bool IsUpdating = true;
    public bool IsDebugLogging = false;

    private Dictionary<GamepadAxis, float> axesCurrent { get; set; }
    private Dictionary<GamepadAxis, float> axesPrevious { get; set; }
    private Dictionary<GamepadAxis, string> axesToUnityInputMap { get; set; }

    private Dictionary<GamepadButton, bool> buttonsCurrent { get; set; }
    private Dictionary<GamepadButton, bool> buttonsPrevious { get; set; }
    private Dictionary<GamepadButton, string> buttonsToUnityInputMap { get; set; }
    private Dictionary<GamepadButton, ButtonEvent> buttonsToDownEventsMap { get; set; }
    private Dictionary<GamepadButton, ButtonEvent> buttonsToUpEventsMap { get; set; }

    [Header( "A Button" )]
    public UnityEvent ButtonDownA;
    public UnityEvent ButtonUpA;

    [Header( "B Button" )]
    public UnityEvent ButtonDownB;
    public UnityEvent ButtonUpB;

    [Header( "X Button" )]
    public UnityEvent ButtonDownX;
    public UnityEvent ButtonUpX;

    [Header( "Y Button" )]
    public UnityEvent ButtonDownY;
    public UnityEvent ButtonUpY;

    [Header( "Back Button" )]
    public UnityEvent ButtonDownBack;
    public UnityEvent ButtonUpBack;

    [Header( "Start Button" )]
    public UnityEvent ButtonDownStart;
    public UnityEvent ButtonUpStart;

    [Header( "Left Analog Button" )]
    public UnityEvent ButtonDownLAnalog;
    public UnityEvent ButtonUpLAnalog;

    [Header( "Left Bumper" )]
    public UnityEvent ButtonDownLBumper;
    public UnityEvent ButtonUpLBumper;

    [Header( "Right Analog Button" )]
    public UnityEvent ButtonDownRAnalog;
    public UnityEvent ButtonUpRAnalog;

    [Header( "Right Bumper" )]
    public ButtonEvent ButtonDownRBumper;
    public ButtonEvent ButtonUpRBumper;

    #endregion


    #region Unity Message Handlers

    void Start()
    {
        // Current and previous states of each control axis.
        {
            axesCurrent = new Dictionary<GamepadAxis, float>()
            {
                { GamepadAxis.DPadX, 0f },
                { GamepadAxis.DPadY, 0f },
                { GamepadAxis.LAnalogX, 0f },
                { GamepadAxis.LAnalogY, 0f },
                { GamepadAxis.RAnalogX, 0f },
                { GamepadAxis.RAnalogY, 0f },
                { GamepadAxis.LTrigger, 0f },
                { GamepadAxis.RTrigger, 0f }
            };
            axesPrevious = new Dictionary<GamepadAxis, float>()
            {
                { GamepadAxis.DPadX, 0f },
                { GamepadAxis.DPadY, 0f },
                { GamepadAxis.LAnalogX, 0f },
                { GamepadAxis.LAnalogY, 0f },
                { GamepadAxis.RAnalogX, 0f },
                { GamepadAxis.RAnalogY, 0f },
                { GamepadAxis.LTrigger, 0f },
                { GamepadAxis.RTrigger, 0f }
            };
        }

        // Current and previous states of each button.
        {
            buttonsCurrent = new Dictionary<GamepadButton, bool>()
            {
                { GamepadButton.A, false },
                { GamepadButton.B, false },
                { GamepadButton.Back, false },
                { GamepadButton.LAnalog, false },
                { GamepadButton.LBumper, false },
                { GamepadButton.RAnalog, false },
                { GamepadButton.RBumper, false },
                { GamepadButton.Start, false },
                { GamepadButton.X, false },
                { GamepadButton.Y, false }
            };
            buttonsPrevious = new Dictionary<GamepadButton, bool>()
            {
                { GamepadButton.A, false },
                { GamepadButton.B, false },
                { GamepadButton.Back, false },
                { GamepadButton.LAnalog, false },
                { GamepadButton.LBumper, false },
                { GamepadButton.RAnalog, false },
                { GamepadButton.RBumper, false },
                { GamepadButton.Start, false },
                { GamepadButton.X, false },
                { GamepadButton.Y, false }
            };
        }


        // Map from internal axis and buttons enums to Unity Input Manager keys.
        {
            // Convert the player num to a string once here because it gets read a lot.
            var player = PlayerNum.ToString();

            // Map from internal axis enum to Unity Input Manager joystick axes.
            axesToUnityInputMap = new Dictionary<GamepadAxis, string>()
            {
                { GamepadAxis.DPadX, "DPad_XAxis_" + player },
                { GamepadAxis.DPadY, "DPad_YAxis_" + player },
                { GamepadAxis.LAnalogX, "L_XAxis_" + player },
                { GamepadAxis.LAnalogY, "L_YAxis_" + player },
                { GamepadAxis.RAnalogX, "R_XAxis_" + player },
                { GamepadAxis.RAnalogY, "R_YAxis_" + player },
                { GamepadAxis.LTrigger, "TriggersR_" + player },
                { GamepadAxis.RTrigger, "TriggersL_" + player }
            };

            // Map from internal button enum to Unity Input Manager joystick buttons.
            buttonsToUnityInputMap = new Dictionary<GamepadButton, string>()
            {
                { GamepadButton.A, "A_" + player },
                { GamepadButton.B, "B_" + player },
                { GamepadButton.Back, "X_" + player },
                { GamepadButton.LAnalog, "Y_" + player },
                { GamepadButton.LBumper, "Back_" + player },
                { GamepadButton.RAnalog, "Start_" + player },
                { GamepadButton.RBumper, "LS_" + player },
                { GamepadButton.Start, "RS_" + player },
                { GamepadButton.X, "LB_" + player },
                { GamepadButton.Y, "RB_" + player }
            };
        }

        //
        buttonsToDownEventsMap = new Dictionary<GamepadButton, string>()
        {
            { GamepadButton.A, ButtonDownA },
            { GamepadButton.B, ButtonDownB },
            { GamepadButton.Back, ButtonDownBack },
            { GamepadButton.LAnalog, ButtonDownLAnalog },
            { GamepadButton.LBumper, ButtonDownLBumper },
            { GamepadButton.RAnalog, ButtonDownRAnalog },
            { GamepadButton.RBumper, ButtonDownRBumper },
            { GamepadButton.Start, ButtonDownStart },
            { GamepadButton.X, ButtonDownX },
            { GamepadButton.Y, ButtonDownY }
        }

        //
        buttonsToUpEventsMap = new Dictionary<GamepadButton, string>()
        {
            { GamepadButton.A, ButtonUpA },
            { GamepadButton.B, ButtonUpB },
            { GamepadButton.Back, ButtonUpBack },
            { GamepadButton.LAnalog, ButtonUpLAnalog },
            { GamepadButton.LBumper, ButtonUpLBumper },
            { GamepadButton.RAnalog, ButtonUpRAnalog },
            { GamepadButton.RBumper, ButtonUpRBumper },
            { GamepadButton.Start, ButtonUpStart },
            { GamepadButton.X, ButtonUpX },
            { GamepadButton.Y, ButtonUpY }
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
                var axesKeys = axesCurrent.Keys;
                foreach ( var key in axesKeys )
                {
                    axesPrevious[ key ] = axesCurrent[ key ];
                    axesCurrent[ key ] = Input.GetAxis( axesToUnityInputMap[ key ] );
                }
                var buttonsKeys = buttonsCurrent.Keys;
                foreach ( var key in buttonsKeys )
                {
                    buttonsPrevious[ key ] = buttonsCurrent[ key ];
                    buttonsCurrent[ key ] = Input.GetButton( buttonsToUnityInputMap[ key ] );

                    // Test for button state change events and emit them as they come up.
                    if ( DidButtonPressBegin( key ) )
                    {
                        var downEvent = buttonsToDownEventsMap[ key ];
                        downEvent.Invoke();
                    }
                    else if ( DidButtonPressEnd( key ) )
                    {
                        var upEvent = buttonsToUpEventsMap[ key ];
                        upEvent.Invoke();
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

    public bool DidButtonPressBegin( GamepadButton b )
    {
        return ( buttonsCurrent[ b ] && !buttonsPrevious[ b ] );
    }

    public bool DidButtonPressEnd( GamepadButton b )
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
    public bool AxisJustPastThreshold( GamepadAxis Axis, float Threshold )
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
        foreach ( GamepadAxis key in axesCurrent.Keys )
        {
            sb.Append( key.ToString() );
            sb.Append( ": \t" );
            // if ( key != GamepadAxis. )
            // {
            //   sb.Append( "\t" );
            // }
            sb.Append( axesCurrent[ key ].ToString() );
            sb.Append( "\n" );
        }
        sb.Append( "\n" );

        sb.Append( "Buttons:\n" );
        foreach ( GamepadButton key in buttonsCurrent.Keys )
        {
            sb.Append( key.ToString() );
            sb.Append( ": \t" );
            // if ( key != GamepadButton. && key != GamepadButton. )
            // {
            //   sb.Append( "\t" );
            // }
            sb.Append( buttonsCurrent[ key ].ToString() );
            sb.Append( "\n" );

            sb.AppendLine( key.ToString() + ": \t\t" + buttonsCurrent[ key ].ToString() );
        }
        sb.AppendLine( "" );

        return sb.ToString();
    }
}
