using UnityEngine;

public class ButtonVisualization : MonoBehaviour
{
    public Xbox360Gamepad Gamepad;
    public Xbox360GamepadButton Button;

    public Color IdleColor = new Color( 0.5f, 0.5f, 0.5f );
    public Color PressedColor = new Color( 1f, 0f, 1f );

    Color initialColor;
    Material material;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

	void Update()
    {
        material.color = 
            Gamepad.GetButton( Button )
                ? PressedColor
                : IdleColor;
    }
}
