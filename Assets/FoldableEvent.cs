using UnityEngine.Events;


/// <summary>
/// The idea behind this is to make FoldableEvent contain a UnityEvent that is public, so 
/// FoldableEvent will show up in the Inspector as serializable, but it will appear in the 
/// Inspector as the default toggle dropdown -- and this takes up a lot less space!
/// </summary>
public class FoldableEvent<T> where T : UnityEvent
{
    public T Event;
}
