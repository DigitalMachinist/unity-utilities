Extension Methods
=================

Part of the [unity-utilities](https://github.com/DigitalMachinist/unity-utilities) GitHub repo by [@DigitalMachinist](https://github.com/DigitalMachinist).

*Credit for this all methods in this library goes to [@JamesZinger](https://github.com/JamesZinger).*

The extension methods in this library provide additional functionality to built-in Unity types in to make common development tasks easier and more intuitive. This library is broken into several files that each relate to a specific type or use case so you can easily import only the extensions you need into your project.

This library is compatible with both Unity Free and Unity Pro and should run on Unity versions back to 3.x (and possibly earlier).

## Included files

 - ```Color.cs```: Contains helpers for dealing with Colors.
 - ```File.cs```: Contains helpers for dealing with files and Unity asset paths.
 - ```Transform.cs```: Contains helpers for looking up objects in the scene hierarchy.
 - ```UI.cs```: Contains helpers for dealing with layout and displaying Unity UI components.

### Color

TODO

#### Color string.ToColor()



### File

TODO

#### bool string.IsValidPersistentDataPath()



#### bool string.IsValidResourceBundlePath()



#### bool string.PathExists()



#### string string.ToPersistentDataPath()



#### string string.ToResourceBundlePath()



### Transform

TODO

#### bool Transform.AreChildrenSorted()



#### Transform[] Transform.GetChildren()



#### Transform Transform.GetParentWithName( string name )



#### bool GameObject.HasComponent<T>()



#### bool Component.HasComponent<T>()



#### bool Transform.HasParentComponent<T>()



#### bool Transform.HasParentWithName( string name )



#### void Transform.SortChildrenByName()



### UI

TODO

#### Coroutine CanvasGroup.FadeCanvasGroup( float seconds, float targetAlpha, bool useUnscaledTime, MonoBehaviour context )



#### Coroutine Graphic.FadeGraphic( float seconds, float targetAlpha, bool useUnscaledTime )



#### float RectTransform.GetHeight()



#### Vector2 RectTransform.GetSize()



#### float RectTransform.GetWidth()



#### void RectTransform.SetDefaultScale()



#### void RectTransform.SetHeight( float newSize )



#### void RectTransform.SetLeftBottomPosition( Vector2 newPos )



#### void RectTransform.SetLeftTopPosition( Vector2 newPos )



#### void RectTransform.SetPivotAndAnchors( Vector2 aVec )



#### void RectTransform.SetPositionOfPivot( Vector2 newPos )



#### void RectTransform.SetRightBottomPosition( Vector2 newPos )



#### void RectTransform.SetRightTopPosition( Vector2 newPos )



#### void RectTransform.SetSize( Vector2 newSize )



#### void RectTransform.SetWidth( float newSize )



#### string Resolution.ToResolutionString()



#### Vector2 Canvas.WorldToCanvas( Vector3 worldPosition, Camera camera = null )
