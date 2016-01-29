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

    public void Invoke()
    {
        Event.Invoke();
    }

    public void AddListener( UnityAction action )
    {
        Event.AddListener( action );
    }

    public void RemoveListener( UnityAction action )
    {
        Event.AddListener( action );
    }
}
[Serializable]
public class FoldableEvent<TArg0>
{
    public UnityEvent<TArg0> Event;

    public void Invoke( TArg0 arg0 )
    {
        Event.Invoke( arg0 );
    }

    public void AddListener( UnityAction<TArg0> action )
    {
        Event.AddListener( action );
    }

    public void RemoveListener( UnityAction<TArg0> action )
    {
        Event.AddListener( action );
    }
}
[Serializable]
public class FoldableEvent<TArg0, TArg1>
{
    public UnityEvent<TArg0, TArg1> Event;

    public void Invoke( TArg0 arg0, TArg1 arg1 )
    {
        Event.Invoke( arg0, arg1 );
    }

    public void AddListener( UnityAction<TArg0, TArg1> action )
    {
        Event.AddListener( action );
    }

    public void RemoveListener( UnityAction<TArg0, TArg1> action )
    {
        Event.AddListener( action );
    }
}
[Serializable]
public class FoldableEvent<TArg0, TArg1, TArg2>
{
    public UnityEvent<TArg0, TArg1, TArg2> Event;

    public void Invoke( TArg0 arg0, TArg1 arg1, TArg2 arg2 )
    {
        Event.Invoke( arg0, arg1, arg2 );
    }

    public void AddListener( UnityAction<TArg0, TArg1, TArg2> action )
    {
        Event.AddListener( action );
    }

    public void RemoveListener( UnityAction<TArg0, TArg1, TArg2> action )
    {
        Event.AddListener( action );
    }
}
[Serializable]
public class FoldableEvent<TArg0, TArg1, TArg2, TArg3>
{
    public UnityEvent<TArg0, TArg1, TArg2, TArg3> Event;

    public void Invoke( TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3 )
    {
        Event.Invoke( arg0, arg1, arg2, arg3 );
    }

    public void AddListener( UnityAction<TArg0, TArg1, TArg2, TArg3> action )
    {
        Event.AddListener( action );
    }

    public void RemoveListener( UnityAction<TArg0, TArg1, TArg2, TArg3> action )
    {
        Event.AddListener( action );
    }
}
