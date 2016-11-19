using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This event is used only as an intermediary for FoldableLayerEvent below. Ignore.
/// </summary>
[Serializable]
public class AnimatorLayerEvent : UnityEvent<AnimatorLayer, AnimatorState> { }

/// <summary>
/// An event used to emit information about the current layer/state being handled to end-listeners.
/// </summary>
[Serializable]
public class FoldableLayerEvent : FoldableEvent<AnimatorLayerEvent, AnimatorLayer, AnimatorState> { }

[Serializable]
public class AnimatorLayer
{
    public string Name;

    [Header( "Layer Events" )]
    public FoldableLayerEvent ControlEnter;
    public FoldableLayerEvent StateEnter;
    public FoldableLayerEvent ControlExit;
    public FoldableLayerEvent StateExit;
    public FoldableLayerEvent ControlUpdate;
    public FoldableLayerEvent StateUpdate;

    [Header( "Layer Context" )]
    [ReadOnly] public AnimatorState CurrentState;
    [ReadOnly] public AnimatorState EnteringState;
    [ReadOnly] public AnimatorState ExitingState;
    [ReadOnly] public AnimatorState MostRecentState;
    [ReadOnly] public bool IsStarted;

    [Header( "Debug Logging" )]
    public bool LogControlEnters;
    public bool LogStateEnters;
    public bool LogControlExits;
    public bool LogStateExits;
    public bool LogControlUpdates;
    public bool LogStateUpdates;

    public AnimatorLayer( string name = "" )
    {
        Name = name;
    }

    public bool IsCurrentState( AnimatorState state )
    {
        return CurrentState == state;
    }
}
