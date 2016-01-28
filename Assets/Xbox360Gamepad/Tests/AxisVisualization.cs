using UnityEngine;

public class AxisVisualization : MonoBehaviour
{
    public Xbox360Gamepad Gamepad;
    public Xbox360GamepadAxis Axis;
    public float ScaleLength = 5f;
    public float MinValue = -1f;
    public float MaxValue = 1f;

    Transform dial;

    void Start()
    {
        dial = transform.FindChild( "Dial" ).transform;
    }

	void Update()
    {
        var fraction = ( Gamepad.GetAxis( Axis ) - MinValue ) / ( MaxValue - MinValue );
        var halfScale = ( 0.5f * ScaleLength );
        var position = dial.transform.localPosition;
        position.x = Mathf.Lerp( -halfScale, halfScale, fraction );
        dial.transform.localPosition = position;
    }
}
