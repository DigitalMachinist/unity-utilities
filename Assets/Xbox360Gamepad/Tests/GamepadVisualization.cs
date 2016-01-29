using UnityEngine;
using UnityEngine.UI;

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

    [Header( "UI" )]
    public Text TextPlayerNum;
    public Text LTriggerValueText;
    public Text RTriggerValueText;
    public Text LAnalogValueText;
    public Text RAnalogValueText;
    public Text DPadValueText;

    void Start ()
    {
        // Set the player number of the visualization based on the Gamepad.
        TextPlayerNum.text = "Player " + Gamepad.PlayerNum;

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

    void Update()
    {
        // Continually update the UI with the values of the axes.
        LTriggerValueText.text = Gamepad.GetAxis( Xbox360GamepadAxis.LTrigger ).ToString( "F1" );
        RTriggerValueText.text = Gamepad.GetAxis( Xbox360GamepadAxis.RTrigger ).ToString( "F1" );
        LAnalogValueText.text = "( " + Gamepad.GetAxis( Xbox360GamepadAxis.LAnalogX ).ToString( "F1" ) + ", " + Gamepad.GetAxis( Xbox360GamepadAxis.LAnalogY ).ToString( "F1" ) + " )";
        RAnalogValueText.text = "( " + Gamepad.GetAxis( Xbox360GamepadAxis.RAnalogX ).ToString( "F1" ) + ", " + Gamepad.GetAxis( Xbox360GamepadAxis.RAnalogY ).ToString( "F1" ) + " )";
        DPadValueText.text = "( " + Gamepad.GetAxis( Xbox360GamepadAxis.DPadX ).ToString( "F1" ) + ", " + Gamepad.GetAxis( Xbox360GamepadAxis.DPadY ).ToString( "F1" ) + " )";
    }
}
