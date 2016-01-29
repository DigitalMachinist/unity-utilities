using System;
using UnityEngine.Events;


/// <summary>
/// Not only does this fold up nicely in the Inspector, but it retains the arguments with which 
/// it was invoked so that any subscriber who joins after invocation will be immediately executed 
/// with the same arguments/context as the invocation when it happened. This is great for ready 
/// signals and declarations of dependency -- notifications that need to only happen once.
/// </summary>
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
            Event.Invoke();
        }
    }

    public override void AddListener( UnityAction action )
    {
        Event.AddListener( action );
        if ( isResolved )
        {
            action();
        }
    }
}


/// <summary>
/// Not only does this fold up nicely in the Inspector, but it retains the arguments with which 
/// it was invoked so that any subscriber who joins after invocation will be immediately executed 
/// with the same arguments/context as the invocation when it happened. This is great for ready 
/// signals and declarations of dependency -- notifications that need to only happen once.
/// </summary>
[Serializable]
public class FoldablePromise<TArg0> : FoldableEvent<TArg0>
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
            Event.Invoke( arg0 );
        }
    }

    public override void AddListener( UnityAction<TArg0> action )
    {
        Event.AddListener( action );
        if ( isResolved )
        {
            action( arg0 );
        }
    }
}


/// <summary>
/// Not only does this fold up nicely in the Inspector, but it retains the arguments with which 
/// it was invoked so that any subscriber who joins after invocation will be immediately executed 
/// with the same arguments/context as the invocation when it happened. This is great for ready 
/// signals and declarations of dependency -- notifications that need to only happen once.
/// </summary>
[Serializable]
public class FoldablePromise<TArg0, TArg1> : FoldableEvent<TArg0, TArg1>
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
            Event.Invoke( arg0, arg1 );
        }
    }

    public override void AddListener( UnityAction<TArg0, TArg1> action )
    {
        Event.AddListener( action );
        if ( isResolved )
        {
            action( arg0, arg1 );
        }
    }
}


/// <summary>
/// Not only does this fold up nicely in the Inspector, but it retains the arguments with which 
/// it was invoked so that any subscriber who joins after invocation will be immediately executed 
/// with the same arguments/context as the invocation when it happened. This is great for ready 
/// signals and declarations of dependency -- notifications that need to only happen once.
/// </summary>
[Serializable]
public class FoldablePromise<TArg0, TArg1, TArg2> : FoldableEvent<TArg0, TArg1, TArg2>
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
            Event.Invoke( arg0, arg1, arg2 );
        }
    }

    public override void AddListener( UnityAction<TArg0, TArg1, TArg2> action )
    {
        Event.AddListener( action );
        if ( isResolved )
        {
            action( arg0, arg1, arg2 );
        }
    }
}


/// <summary>
/// Not only does this fold up nicely in the Inspector, but it retains the arguments with which 
/// it was invoked so that any subscriber who joins after invocation will be immediately executed 
/// with the same arguments/context as the invocation when it happened. This is great for ready 
/// signals and declarations of dependency -- notifications that need to only happen once.
/// </summary>
[Serializable]
public class FoldablePromise<TArg0, TArg1, TArg2, TArg3> : FoldableEvent<TArg0, TArg1, TArg2, TArg3>
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
            Event.Invoke( arg0, arg1, arg2, arg3 );
        }
    }

    public override void AddListener( UnityAction<TArg0, TArg1, TArg2, TArg3> action )
    {
        Event.AddListener( action );
        if ( isResolved )
        {
            action( arg0, arg1, arg2, arg3 );
        }
    }
}
