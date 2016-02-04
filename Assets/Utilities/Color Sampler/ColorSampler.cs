using UnityEngine;

public class ColorSampler : MonoBehaviour
{
    public Vector3 RaycastVector = 10f * Vector3.down;
    public LayerMask LayerMask = -1;
    public QueryTriggerInteraction InteractsWithTriggers = QueryTriggerInteraction.UseGlobal;
    public Color SampledColor = Color.black;
    public bool IsDebugDrawing = false;
    public bool IsDebugLogging = false;

    void Update()
    {
        SampledColor = Color.black;

        RaycastHit raycastHit;
        var ray = new Ray( transform.position, RaycastVector.normalized );
        var didRaycastHit = Physics.Raycast( ray, out raycastHit, RaycastVector.magnitude, LayerMask, InteractsWithTriggers );
        if ( !didRaycastHit )
        {
            return;
        }

        var renderer = raycastHit.collider.GetComponent<Renderer>();
        if ( renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null )
        {
            return;
        }

        var texture = renderer.material.mainTexture as Texture2D;
        var uv = raycastHit.textureCoord;
        SampledColor = texture.GetPixelBilinear( uv.x, uv.y );

        if ( IsDebugDrawing )
        {
            Debug.DrawLine( transform.position, transform.position + ( 10f * Vector3.down ), SampledColor, 60f, true );
        }
        if ( IsDebugLogging )
        {
            Debug.Log( "Hit " + SampledColor + "!" );
        }
    }
}
