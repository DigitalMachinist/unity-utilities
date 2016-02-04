using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class FoldableEvent
{
    [SerializeField] protected UnityEvent unityEvent;

    public virtual void Invoke()
    {
        unityEvent.Invoke();
    }

    public virtual void AddListener( UnityAction action )
    {
        unityEvent.AddListener( action );
    }

    public virtual void RemoveListener( UnityAction action )
    {
        unityEvent.RemoveListener( action );
    }

    public int GetPersistentEventCount()
    {
        return unityEvent.GetPersistentEventCount();
    }

    public string GetPersistentMethodName( int index )
    {
        return unityEvent.GetPersistentMethodName( index );
    }

    public UnityEngine.Object GetPersistentTarget( int index )
    {
        return unityEvent.GetPersistentTarget( index );
    }
}



[Serializable]
public class FoldableEvent<TEvent, TArg0> 
    where TEvent : UnityEvent<TArg0>
{
    [SerializeField] protected TEvent unityEvent;

    public virtual void Invoke( TArg0 arg0 )
    {
        unityEvent.Invoke( arg0 );
    }

    public virtual void AddListener( UnityAction<TArg0> action )
    {
        unityEvent.AddListener( action );
    }

    public virtual void RemoveListener( UnityAction<TArg0> action )
    {
        unityEvent.RemoveListener( action );
    }

    public int GetPersistentEventCount()
    {
        return unityEvent.GetPersistentEventCount();
    }

    public string GetPersistentMethodName( int index )
    {
        return unityEvent.GetPersistentMethodName( index );
    }

    public UnityEngine.Object GetPersistentTarget( int index )
    {
        return unityEvent.GetPersistentTarget( index );
    }
}


[Serializable]
public class FoldableEvent<TEvent, TArg0, TArg1> 
    where TEvent : UnityEvent<TArg0, TArg1>
{
    [SerializeField] protected TEvent unityEvent;

    public virtual void Invoke( TArg0 arg0, TArg1 arg1 )
    {
        unityEvent.Invoke( arg0, arg1 );
    }

    public virtual void AddListener( UnityAction<TArg0, TArg1> action )
    {
        unityEvent.AddListener( action );
    }

    public virtual void RemoveListener( UnityAction<TArg0, TArg1> action )
    {
        unityEvent.RemoveListener( action );
    }

    public int GetPersistentEventCount()
    {
        return unityEvent.GetPersistentEventCount();
    }

    public string GetPersistentMethodName( int index )
    {
        return unityEvent.GetPersistentMethodName( index );
    }

    public UnityEngine.Object GetPersistentTarget( int index )
    {
        return unityEvent.GetPersistentTarget( index );
    }
}


[Serializable]
public class FoldableEvent<TEvent, TArg0, TArg1, TArg2> 
    where TEvent : UnityEvent<TArg0, TArg1, TArg2>
{
    [SerializeField] protected TEvent unityEvent;

    public virtual void Invoke( TArg0 arg0, TArg1 arg1, TArg2 arg2 )
    {
        unityEvent.Invoke( arg0, arg1, arg2 );
    }

    public virtual void AddListener( UnityAction<TArg0, TArg1, TArg2> action )
    {
        unityEvent.AddListener( action );
    }

    public virtual void RemoveListener( UnityAction<TArg0, TArg1, TArg2> action )
    {
        unityEvent.RemoveListener( action );
    }
    
    public int GetPersistentEventCount()
    {
        return unityEvent.GetPersistentEventCount();
    }
    
    public string GetPersistentMethodName( int index )
    {
        return unityEvent.GetPersistentMethodName( index );
    }
    
    public UnityEngine.Object GetPersistentTarget( int index )
    {
        return unityEvent.GetPersistentTarget( index );
    }
}


[Serializable]
public class FoldableEvent<TEvent, TArg0, TArg1, TArg2, TArg3> 
    where TEvent : UnityEvent<TArg0, TArg1, TArg2, TArg3>
{
    [SerializeField] protected TEvent unityEvent;

    public virtual void Invoke( TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3 )
    {
        unityEvent.Invoke( arg0, arg1, arg2, arg3 );
    }

    public virtual void AddListener( UnityAction<TArg0, TArg1, TArg2, TArg3> action )
    {
        unityEvent.AddListener( action );
    }

    public virtual void RemoveListener( UnityAction<TArg0, TArg1, TArg2, TArg3> action )
    {
        unityEvent.RemoveListener( action );
    }

    public int GetPersistentEventCount()
    {
        return unityEvent.GetPersistentEventCount();
    }

    public string GetPersistentMethodName( int index )
    {
        return unityEvent.GetPersistentMethodName( index );
    }

    public UnityEngine.Object GetPersistentTarget( int index )
    {
        return unityEvent.GetPersistentTarget( index );
    }
}
