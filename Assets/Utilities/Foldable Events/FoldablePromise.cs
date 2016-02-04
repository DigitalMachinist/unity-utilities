using System;
using UnityEngine.Events;


[Serializable]
public class FoldablePromise : FoldableEvent
{
    bool isResolved = false;

    public void Resolve()
    {
        Invoke();
    }

    public override void Invoke()
    {
        if ( !isResolved )
        {
            isResolved = true;
            unityEvent.Invoke();
        }
    }

    public override void AddListener( UnityAction action )
    {
        unityEvent.AddListener( action );
        if ( isResolved )
        {
            action();
        }
    }
}


[Serializable]
public class FoldablePromise<TEvent, TArg0> 
    : FoldableEvent<TEvent, TArg0>
    where TEvent : UnityEvent<TArg0>
{
    bool isResolved = false;
    TArg0 arg0;

    public void Resolve( TArg0 arg0 )
    {
        Invoke( arg0 );
    }

    public override void Invoke( TArg0 arg0 )
    {
        if ( !isResolved )
        {
            isResolved = true;
            this.arg0 = arg0;
            unityEvent.Invoke( arg0 );
        }
    }

    public override void AddListener( UnityAction<TArg0> action )
    {
        unityEvent.AddListener( action );
        if ( isResolved )
        {
            action( arg0 );
        }
    }
}


[Serializable]
public class FoldablePromise<TEvent, TArg0, TArg1> 
    : FoldableEvent<TEvent, TArg0, TArg1>
    where TEvent : UnityEvent<TArg0, TArg1>
{
    bool isResolved = false;
    TArg0 arg0;
    TArg1 arg1;

    public void Resolve( TArg0 arg0, TArg1 arg1 )
    {
        Invoke( arg0, arg1 );
    }

    public override void Invoke( TArg0 arg0, TArg1 arg1 )
    {
        if ( !isResolved )
        {
            isResolved = true;
            this.arg0 = arg0;
            this.arg1 = arg1;
            unityEvent.Invoke( arg0, arg1 );
        }
    }

    public override void AddListener( UnityAction<TArg0, TArg1> action )
    {
        unityEvent.AddListener( action );
        if ( isResolved )
        {
            action( arg0, arg1 );
        }
    }
}


[Serializable]
public class FoldablePromise<TEvent, TArg0, TArg1, TArg2> 
    : FoldableEvent<TEvent, TArg0, TArg1, TArg2>
    where TEvent : UnityEvent<TArg0, TArg1, TArg2>
{
    bool isResolved = false;
    TArg0 arg0;
    TArg1 arg1;
    TArg2 arg2;

    public void Resolve( TArg0 arg0, TArg1 arg1, TArg2 arg2 )
    {
        Invoke( arg0, arg1, arg2 );
    }

    public override void Invoke( TArg0 arg0, TArg1 arg1, TArg2 arg2 )
    {
        if ( !isResolved )
        {
            isResolved = true;
            this.arg0 = arg0;
            this.arg1 = arg1;
            this.arg2 = arg2;
            unityEvent.Invoke( arg0, arg1, arg2 );
        }
    }

    public override void AddListener( UnityAction<TArg0, TArg1, TArg2> action )
    {
        unityEvent.AddListener( action );
        if ( isResolved )
        {
            action( arg0, arg1, arg2 );
        }
    }
}


[Serializable]
public class FoldablePromise<TEvent, TArg0, TArg1, TArg2, TArg3> 
    : FoldableEvent<TEvent, TArg0, TArg1, TArg2, TArg3>
    where TEvent : UnityEvent<TArg0, TArg1, TArg2, TArg3>
{
    bool isResolved = false;
    TArg0 arg0;
    TArg1 arg1;
    TArg2 arg2;
    TArg3 arg3;

    public void Resolve( TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3 )
    {
        Invoke( arg0, arg1, arg2, arg3 );
    }

    public override void Invoke( TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3 )
    {
        if ( !isResolved )
        {
            isResolved = true;
            this.arg0 = arg0;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            unityEvent.Invoke( arg0, arg1, arg2, arg3 );
        }
    }

    public override void AddListener( UnityAction<TArg0, TArg1, TArg2, TArg3> action )
    {
        unityEvent.AddListener( action );
        if ( isResolved )
        {
            action( arg0, arg1, arg2, arg3 );
        }
    }
}
