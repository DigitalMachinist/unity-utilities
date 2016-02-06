using System;
using System.Globalization;
using System.Linq;
using UnityEngine;

public static partial class UnityExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static bool HasParentComponent<T>( this Transform transform ) where T : Component
    {
        return transform.GetComponentInParent<T>() != null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool HasParentWithName( this Transform transform, string name )
    {
        if ( transform.parent == null )
        {
            return false;
        }
        if ( transform.parent.name == name )
        {
            return true;
        }
        return transform.parent.HasParentWithName( name );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform GetParentWithName( this Transform transform, string name )
    {
        if ( transform.parent == null )
        {
            return null;
        }
        if ( transform.parent.name == name )
        {
            return transform.parent;
        }
        return transform.parent.GetParentWithName( name );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    public static void SortChildrenByName( this Transform transform )
    {
        var children = transform.GetChildren().ToList();
        children.Sort( ( lhs, rhs ) => string.Compare( lhs.name, rhs.name, StringComparison.Ordinal ) );
        for ( var i = 0; i < children.Count; i++ )
        {
            var child = children[ i ];
            child.SetSiblingIndex( i );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static bool AreChildrenSorted( this Transform transform )
    {
        var needsToSort = false;
        transform.GetChildren()
        .Aggregate( "", ( carry, obj ) => {
            var compare = carry.CompareTo( obj.name );
            if ( compare > 0 )
            {
                needsToSort = true;
            }
            return obj.name;
        } );
        return needsToSort;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static Transform[] GetChildren( this Transform transform )
    {
        var children = new Transform[ transform.childCount ];
        for ( var i = 0; i < transform.childCount; i++ )
        {
            children[ i ] = transform.GetChild( i );
        }
        return children;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static bool HasComponent<T>( this GameObject gameObject )
        where T : Component
    {
        return gameObject.GetComponent<T>() != null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static bool HasComponent<T>( this Component gameObject )
        where T : Component
    {
        return gameObject.GetComponent<T>() != null;
    }
}
