using System;
using UnityEditor;
using UnityEngine;

// [Flags] is already defined as System.FlagsAttribute, so we only need this to make a nice Unity
// Inspector property drawer for flags-style enums.
[CustomPropertyDrawer( typeof( FlagsAttribute ) )]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        property.intValue = EditorGUI.MaskField( position, label, property.intValue, property.enumNames );
    }
}
