using System;
using UnityEngine.Events;


/// <summary>
/// The idea behind this is to make FoldableEvent contain a UnityEvent that is public, so 
/// FoldableEvent will show up in the Inspector as serializable, but it will appear in the 
/// Inspector as the default toggle dropdown -- and this takes up a lot less space!
/// </summary>
[Serializable]
public class FoldableEvent
{
    public UnityEvent Event;
}
[Serializable]
public class FoldableEvent<TArg0>
{
    public UnityEvent<TArg0> Event;
}
[Serializable]
public class FoldableEvent<TArg0, TArg1>
{
    public UnityEvent<TArg0, TArg1> Event;
}
[Serializable]
public class FoldableEvent<TArg0, TArg1, TArg2>
{
    public UnityEvent<TArg0, TArg1, TArg2> Event;
}
[Serializable]
public class FoldableEvent<TArg0, TArg1, TArg2, TArg4>
{
    public UnityEvent<TArg0, TArg1, TArg2, TArg4> Event;
}
