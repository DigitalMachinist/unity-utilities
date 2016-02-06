using System;
using System.Globalization;
using UnityEngine;

public static partial class UnityExtensionMethods
{
    /// <summary>
    /// Convert a 7-character colour string (RGB) or a 9-character colour string (RGBA) into a 
    /// Unity Color object.
    /// </summary>
    /// <param name="colourString">An RGB (#RRGGBB) or RGBA (#RRGGBBAA) colour string.</param>
    /// <returns>A Unity Color object corresponding to the hexidecimal colour string.</returns>
    /// <remarks>RGB colours are assumed to have 255 (#FF) alpha.</remarks>
    public static Color ToColor( this string colourString )
    {
        if ( colourString.Substring( 0, 1 ) != "#" )
        {
            throw new ArgumentOutOfRangeException( "colourString", "colourString must begin with # to be treated as a colour." );
        }

        float r = byte.Parse( colourString.Substring( 1, 2 ), NumberStyles.HexNumber ) / 255f;
        float g = byte.Parse( colourString.Substring( 3, 2 ), NumberStyles.HexNumber ) / 255f;
        float b = byte.Parse( colourString.Substring( 5, 2 ), NumberStyles.HexNumber ) / 255f;
        float a;

        if ( colourString.Length == 7 )
        {
            a = 1f;
        }
        else if ( colourString.Length == 9 )
        {
            a = byte.Parse( colourString.Substring( 7, 2 ), NumberStyles.HexNumber ) / 255f;
        }
        else
        {
            throw new ArgumentOutOfRangeException( "colourString", "colourString must be either 7 or 9 characters in length e.g. #112233 or #11223344." );
        }

        return new Color( r, g, b, a );
    }
}
