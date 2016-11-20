using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This event is used only as an intermediary for FoldableLayerEvent below. Ignore.
/// </summary>
[Serializable]
public class AnimatorLayerEvent : UnityEvent<AnimatorLayer, AnimatorStateBehaviour> { }

/// <summary>
/// An event used to emit information about the current layer/state being handled to end-listeners.
/// </summary>
[Serializable]
public class FoldableLayerEvent : FoldableEvent<AnimatorLayerEvent, AnimatorLayer, AnimatorStateBehaviour> { }

[Serializable]
public class AnimatorLayer
{
    public string Name;

    [Header( "Events" )]
    public FoldableLayerEvent ControlEnter;
    public FoldableLayerEvent StateEnter;
    public FoldableLayerEvent ControlExit;
    public FoldableLayerEvent StateExit;
    public FoldableLayerEvent ControlUpdate;
    public FoldableLayerEvent StateUpdate;
    
    [NonSerialized] public bool IsStarted;
    [NonSerialized] public AnimatorStateBehaviour CurrentState;
    [NonSerialized] public AnimatorStateBehaviour EnteringState;
    [NonSerialized] public AnimatorStateBehaviour ExitingState;
    [NonSerialized] public AnimatorStateBehaviour MostRecentState;

    public AnimatorLayer( string name = "" )
    {
        Name = name;
    }

    public bool IsCurrentState( AnimatorStateBehaviour state )
    {
        return CurrentState == state;
    }
}
