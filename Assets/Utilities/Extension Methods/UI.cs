using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class UnityExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    public static void SetDefaultScale( this RectTransform trans )
    {
        trans.localScale = new Vector3( 1, 1, 1 );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="aVec"></param>
    public static void SetPivotAndAnchors( this RectTransform trans, Vector2 aVec )
    {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static Vector2 GetSize( this RectTransform trans )
    {
        return trans.rect.size;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static float GetWidth( this RectTransform trans )
    {
        return trans.rect.width;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static float GetHeight( this RectTransform trans )
    {
        return trans.rect.height;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="newPos"></param>
    public static void SetPositionOfPivot( this RectTransform trans, Vector2 newPos )
    {
        trans.localPosition = new Vector3( newPos.x, newPos.y, trans.localPosition.z );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="newPos"></param>
    public static void SetLeftBottomPosition( this RectTransform trans, Vector2 newPos )
    {
        trans.localPosition = new Vector3( newPos.x + ( trans.pivot.x * trans.rect.width ), newPos.y + ( trans.pivot.y * trans.rect.height ), trans.localPosition.z );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="newPos"></param>
    public static void SetLeftTopPosition( this RectTransform trans, Vector2 newPos )
    {
        trans.localPosition = new Vector3( newPos.x + ( trans.pivot.x * trans.rect.width ), newPos.y - ( ( 1f - trans.pivot.y ) * trans.rect.height ), trans.localPosition.z );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="newPos"></param>
    public static void SetRightBottomPosition( this RectTransform trans, Vector2 newPos )
    {
        trans.localPosition = new Vector3( newPos.x - ( ( 1f - trans.pivot.x ) * trans.rect.width ), newPos.y + ( trans.pivot.y * trans.rect.height ), trans.localPosition.z );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="newPos"></param>
    public static void SetRightTopPosition( this RectTransform trans, Vector2 newPos )
    {
        trans.localPosition = new Vector3( newPos.x - ( ( 1f - trans.pivot.x ) * trans.rect.width ), newPos.y - ( ( 1f - trans.pivot.y ) * trans.rect.height ), trans.localPosition.z );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="newSize"></param>
    public static void SetSize( this RectTransform trans, Vector2 newSize )
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2( deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y );
        trans.offsetMax = trans.offsetMax + new Vector2( deltaSize.x * ( 1f - trans.pivot.x ), deltaSize.y * ( 1f - trans.pivot.y ) );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="newSize"></param>
    public static void SetWidth( this RectTransform trans, float newSize )
    {
        SetSize( trans, new Vector2( newSize, trans.rect.size.y ) );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="newSize"></param>
    public static void SetHeight( this RectTransform trans, float newSize )
    {
        SetSize( trans, new Vector2( trans.rect.size.x, newSize ) );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    public static string ToResolutionString( this Resolution res )
    {
        return string.Format( "{0}x{1}@{2}hz", res.height, res.width, res.refreshRate );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="worldPosition"></param>
    /// <param name="camera"></param>
    /// <returns></returns>
    public static Vector2 WorldToCanvas( this Canvas canvas, Vector3 worldPosition, Camera camera = null )
    {
        camera = camera ?? Camera.main;

        var viewportPosition = camera.WorldToViewportPoint( worldPosition );
        var canvasRect = canvas.GetComponent<RectTransform>();

        return new Vector2( ( viewportPosition.x * canvasRect.sizeDelta.x ) - ( canvasRect.sizeDelta.x * 0.5f ),
                            ( viewportPosition.y * canvasRect.sizeDelta.y ) - ( canvasRect.sizeDelta.y * 0.5f ) );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="graphic"></param>
    /// <param name="seconds"></param>
    /// <param name="targetAlpha"></param>
    /// <param name="UseUnscaledTime"></param>
    /// <returns></returns>
    public static Coroutine FadeGraphic( this Graphic graphic, float seconds, float targetAlpha, bool UseUnscaledTime )
    {
        return graphic.StartCoroutine( FadeGraphicOverSeconds( graphic, seconds, targetAlpha, UseUnscaledTime ) );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cg"></param>
    /// <param name="seconds"></param>
    /// <param name="targetAlpha"></param>
    /// <param name="UseUnscaledTime"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static Coroutine FadeCanvasGroup( this CanvasGroup cg, float seconds, float targetAlpha, bool UseUnscaledTime, MonoBehaviour context )
    {
        return context.StartCoroutine( FadeCanvasGroupOverSeconds( cg, seconds, targetAlpha, UseUnscaledTime ) );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="graphic"></param>
    /// <param name="seconds"></param>
    /// <param name="targetAlpha"></param>
    /// <param name="useUnscaledTime"></param>
    /// <returns></returns>
    static IEnumerator FadeGraphicOverSeconds( Graphic graphic, float seconds, float targetAlpha, bool useUnscaledTime )
    {
        var enumerator = FadeAlphaOverSeconds( graphic.color.a, seconds, targetAlpha, useUnscaledTime );
        yield return null;
        while ( true )
        {
            if ( enumerator.MoveNext() )
            {
                var color = graphic.color;
                color.a = enumerator.Current;
                graphic.color = color;
            }
            else
                break;

            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cg"></param>
    /// <param name="seconds"></param>
    /// <param name="targetAlpha"></param>
    /// <param name="useUnscaledTime"></param>
    /// <returns></returns>
    static IEnumerator FadeCanvasGroupOverSeconds( CanvasGroup cg, float seconds, float targetAlpha, bool useUnscaledTime )
    {
        var enumerator = FadeAlphaOverSeconds( cg.alpha, seconds, targetAlpha, useUnscaledTime );
        yield return null;
        while ( true )
        {
            if ( enumerator.MoveNext() )
                cg.alpha = enumerator.Current;
            else
                break;

            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="seconds"></param>
    /// <param name="targetAlpha"></param>
    /// <param name="useUnscaledTime"></param>
    /// <returns></returns>
    static IEnumerator<float> FadeAlphaOverSeconds( float alpha, float seconds, float targetAlpha, bool useUnscaledTime )
    {
        var dtAcc = 0f;
        var initalAlpha = alpha;
        do
        {
            var dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            dtAcc += dt;
            alpha = Mathf.Lerp( initalAlpha, targetAlpha, dtAcc / seconds );
            yield return alpha;
        } while ( dtAcc < seconds );
    }
}
