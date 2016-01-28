using UnityEngine;
using System.Collections;

public class GamepadVisualization : MonoBehaviour
{
    [Space( 5 )]
    public Xbox360Gamepad Gamepad;

    [Header( "Axes" )]
    public AxisVisualization LAnalogXAxis;
    public AxisVisualization LAnalogYAxis;
    public AxisVisualization RAnalogXAxis;
    public AxisVisualization RAnalogYAxis;
    public AxisVisualization LTriggerAxis;
    public AxisVisualization RTriggerAxis;
    public AxisVisualization DPadXAxis;
    public AxisVisualization DPadYAxis;

    [Header( "Buttons" )]
    public ButtonVisualization AButton;
    public ButtonVisualization BButton;
    public ButtonVisualization XButton;
    public ButtonVisualization YButton;
    public ButtonVisualization LTriggerButton;
    public ButtonVisualization RTriggerButton;
    public ButtonVisualization LBumperButton;
    public ButtonVisualization RBumperButton;
    public ButtonVisualization LAnalogButton;
    public ButtonVisualization RAnalogButton;
    public ButtonVisualization BackButton;
    public ButtonVisualization StartButton;

    [Header( "Joysticks" )]
    public JoystickVisualization LAnalogJoystick;
    public JoystickVisualization RAnalogJoystick;
    public JoystickVisualization DPadJoystick;

    void Start ()
    {
        // Initialize axes' gamepad source.
        LAnalogXAxis.Gamepad = Gamepad;
        LAnalogYAxis.Gamepad = Gamepad;
        RAnalogXAxis.Gamepad = Gamepad;
        RAnalogYAxis.Gamepad = Gamepad;
        LTriggerAxis.Gamepad = Gamepad;
        RTriggerAxis.Gamepad = Gamepad;
        DPadXAxis.Gamepad = Gamepad;
        DPadYAxis.Gamepad = Gamepad;

        // Initialize buttons' gamepad source.
        AButton.Gamepad = Gamepad;
        BButton.Gamepad = Gamepad;
        XButton.Gamepad = Gamepad;
        YButton.Gamepad = Gamepad;
        LTriggerButton.Gamepad = Gamepad;
        RTriggerButton.Gamepad = Gamepad;
        LBumperButton.Gamepad = Gamepad;
        RBumperButton.Gamepad = Gamepad;
        LAnalogButton.Gamepad = Gamepad;
        RAnalogButton.Gamepad = Gamepad;
        BackButton.Gamepad = Gamepad;
        StartButton.Gamepad = Gamepad;

        // Initialize joysticks' gamepad source.
        LAnalogJoystick.Gamepad = Gamepad;
        RAnalogJoystick.Gamepad = Gamepad;
        DPadJoystick.Gamepad = Gamepad;
    }
}
