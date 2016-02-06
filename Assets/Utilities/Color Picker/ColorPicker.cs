using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class ColorPicker : MonoBehaviour
{
    public static readonly Color NO_COLOR = new Color( 0f, 0f, 0f, 0f );

    [Header( "Sampled Color" )]
    [SerializeField, ReadOnly]
    Color color = NO_COLOR;

    [Header( "Configuration" )]
    public bool IsSampling = true;
    public float SamplingPeriod = 0.1f;
    public Vector3 RaycastVector = 10f * Vector3.down;
    public LayerMask LayerMask = -1;
    public QueryTriggerInteraction InteractsWithTriggers = QueryTriggerInteraction.UseGlobal;

    [Header( "Debug" )]
    public bool IsDebugDrawing = false;
    public bool IsDebugLogging = false;

    public Color Color
    {
        get { return color; }
        private set { color = value; }
    }

    void OnEnable()
    {
        StartCoroutine( DelaySample() );
    }

    public Color Sample()
    {
        Color = NO_COLOR;

        RaycastHit raycastHit;
        var ray = new Ray( transform.position, transform.TransformDirection( RaycastVector.normalized ) );
        var didRaycastHit = Physics.Raycast( ray, out raycastHit, RaycastVector.magnitude, LayerMask, InteractsWithTriggers );
        if ( !didRaycastHit )
        {
            return Color;
        }

        var renderer = raycastHit.collider.GetComponent<Renderer>();
        if ( renderer == null || renderer.sharedMaterial == null )
        {
            return Color;
        }

        if ( renderer.sharedMaterial.mainTexture == null )
        {
            Color = renderer.sharedMaterial.color;
            return Color;
        }

        var texture = renderer.sharedMaterial.mainTexture as Texture2D;
        var uv = raycastHit.textureCoord;
        Color = texture.GetPixelBilinear( uv.x, uv.y );

        if ( IsDebugDrawing )
        {
            var direction = transform.TransformDirection( RaycastVector.normalized );
            Debug.DrawLine( transform.position, transform.position + direction, Color, 60f, true );
        }
        if ( IsDebugLogging )
        {
            Debug.Log( "Hit " + Color + "!" );
        }

        return Color;
    }

    IEnumerator DelaySample()
    {
        while ( true )
        {
            if ( IsSampling )
            {
                Sample();
            }

            yield return
                ( Application.isPlaying && SamplingPeriod > 0f )
                    ? new WaitForSeconds( SamplingPeriod )
                    : null;
        }
    }
}
