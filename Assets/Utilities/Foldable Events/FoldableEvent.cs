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

    public virtual void Invoke()
    {
        Event.Invoke();
    }

    public virtual void AddListener( UnityAction action )
    {
        Event.AddListener( action );
    }

    public virtual void RemoveListener( UnityAction action )
    {
        Event.RemoveListener( action );
    }
}



/// <summary>
/// The idea behind this is to make FoldableEvent contain a UnityEvent that is public, so 
/// FoldableEvent will show up in the Inspector as serializable, but it will appear in the 
/// Inspector as the default toggle dropdown -- and this takes up a lot less space!
/// </summary>
[Serializable]
public class FoldableEvent<TEvent, TArg0> 
    where TEvent : UnityEvent<TArg0>
{
    public TEvent Event;

    public virtual void Invoke( TArg0 arg0 )
    {
        Event.Invoke( arg0 );
    }

    public virtual void AddListener( UnityAction<TArg0> action )
    {
        Event.AddListener( action );
    }

    public virtual void RemoveListener( UnityAction<TArg0> action )
    {
        Event.RemoveListener( action );
    }
}


/// <summary>
/// The idea behind this is to make FoldableEvent contain a UnityEvent that is public, so 
/// FoldableEvent will show up in the Inspector as serializable, but it will appear in the 
/// Inspector as the default toggle dropdown -- and this takes up a lot less space!
/// </summary>
[Serializable]
public class FoldableEvent<TEvent, TArg0, TArg1> 
    where TEvent : UnityEvent<TArg0, TArg1>
{
    public TEvent Event;

    public virtual void Invoke( TArg0 arg0, TArg1 arg1 )
    {
        Event.Invoke( arg0, arg1 );
    }

    public virtual void AddListener( UnityAction<TArg0, TArg1> action )
    {
        Event.AddListener( action );
    }

    public virtual void RemoveListener( UnityAction<TArg0, TArg1> action )
    {
        Event.RemoveListener( action );
    }
}


/// <summary>
/// The idea behind this is to make FoldableEvent contain a UnityEvent that is public, so 
/// FoldableEvent will show up in the Inspector as serializable, but it will appear in the 
/// Inspector as the default toggle dropdown -- and this takes up a lot less space!
/// </summary>
[Serializable]
public class FoldableEvent<TEvent, TArg0, TArg1, TArg2> 
    where TEvent : UnityEvent<TArg0, TArg1, TArg2>
{
    public TEvent Event;

    public virtual void Invoke( TArg0 arg0, TArg1 arg1, TArg2 arg2 )
    {
        Event.Invoke( arg0, arg1, arg2 );
    }

    public virtual void AddListener( UnityAction<TArg0, TArg1, TArg2> action )
    {
        Event.AddListener( action );
    }

    public virtual void RemoveListener( UnityAction<TArg0, TArg1, TArg2> action )
    {
        Event.RemoveListener( action );
    }
}


/// <summary>
/// The idea behind this is to make FoldableEvent contain a UnityEvent that is public, so 
/// FoldableEvent will show up in the Inspector as serializable, but it will appear in the 
/// Inspector as the default toggle dropdown -- and this takes up a lot less space!
/// </summary>
[Serializable]
public class FoldableEvent<TEvent, TArg0, TArg1, TArg2, TArg3> 
    where TEvent : UnityEvent<TArg0, TArg1, TArg2, TArg3>
{
    public TEvent Event;

    public virtual void Invoke( TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3 )
    {
        Event.Invoke( arg0, arg1, arg2, arg3 );
    }

    public virtual void AddListener( UnityAction<TArg0, TArg1, TArg2, TArg3> action )
    {
        Event.AddListener( action );
    }

    public virtual void RemoveListener( UnityAction<TArg0, TArg1, TArg2, TArg3> action )
    {
        Event.RemoveListener( action );
    }
}
