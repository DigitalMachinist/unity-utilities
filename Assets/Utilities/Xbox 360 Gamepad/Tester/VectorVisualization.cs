using UnityEngine;

public class VectorVisualization : MonoBehaviour
{
    public Xbox360Gamepad Gamepad;
    public Xbox360GamepadVector Vector;
    public float ScaleLength = 2f;
    public bool InvertXAxis = false;
    public bool InvertYAxis = false;

    Transform cylinder;
    Transform dial;
    Transform line;

    void Start()
    {
        dial = transform.FindChild( "Dial" ).transform;
        line = dial.FindChild( "Line" );
        cylinder = line.FindChild( "Cylinder" );
    }

    void Update()
    {
        var value = Gamepad.GetVector( Vector );
        var xFraction = ( ( value.x + 1f ) / 2f );
        var yFraction = ( ( value.y + 1f ) / 2f );
        var halfScale = ( 0.5f * ScaleLength );

        // Apply axis inversion.
        if ( InvertXAxis )
        {
            xFraction = 1f - xFraction;
        }
        if ( InvertYAxis )
        {
            yFraction = 1f - yFraction;
        }

        // Determine the vector pointed to by the control axes.
        var position = dial.localPosition;
        position.x = Mathf.Lerp( -halfScale, halfScale, xFraction );
        position.y = Mathf.Lerp( -halfScale, halfScale, yFraction );
        position.z = -0.1f;

        // Align the dial line with the control vector.
        line.LookAt( transform.TransformPoint( position ) );

        // Scale the dial line to the appropriate length.
        var cylinderScale = cylinder.localScale;
        cylinderScale.y = position.magnitude / 2f;
        cylinder.localScale = cylinderScale;

        // Position the dial line half way along the vector.
        dial.localPosition = position / 2f;
    }
}
