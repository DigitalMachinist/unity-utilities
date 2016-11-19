using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An event used to pass information about the state machine and the state in question.
/// </summary>
[Serializable] public class StateMachineEvent : UnityEvent<State, Animator, AnimatorStateInfo, int> { }

/// <summary>
/// The StateMachine class is used by States to communicate to one another and supports the action 
/// of other game objects in response to events signaling state changes.
/// </summary>
[RequireComponent( typeof( Animator ) )]
public class StateMachine : MonoBehaviour
{
    [Header( "Debug Logging" )]
    public bool LogControlEnters = false;
    public bool LogStateEnters = false;
    public bool LogControlExits = false;
    public bool LogStateExits = false;
    public bool LogControlUpdates = false;
    public bool LogStateUpdates = false;

    [Header( "Events" )]
    public StateMachineEvent ControlEnter;
    public StateMachineEvent ControlUpdate;
    public StateMachineEvent ControlExit;
    public StateMachineEvent StateEnter;
    public StateMachineEvent StateUpdate;
    public StateMachineEvent StateExit;

    public Animator Animator { get; private set; }
    public State CurrentState { get; private set; }
    public State EnteringState { get; private set; }
    public State ExitingState { get; private set; }
    public bool IsStarted { get; private set; }
    public bool IsTransitioning { get; private set; }
    public State MostRecentState { get; private set; }

    public void SetCurrentState( State state )
    {
        MostRecentState = CurrentState;
        CurrentState = state;
        IsTransitioning = true;
    }

    public void SetEnteringState( State state )
    {
        EnteringState = state;
    }

    public void SetExitingState( State state )
    {
        ExitingState = state;
        IsTransitioning = false;
    }

    public void SetMostRecentState( State state )
    {
        MostRecentState = state;
    }

    public void SetIsStarted()
    {
        IsStarted = true;
    }

    void Awake()
    {
        Animator = GetComponent<Animator>();
        if ( Animator == null )
        {
            Debug.LogError( "StateMachine requires an animator component!" );
            return;
        }

        Animator
            .GetBehaviours<State>()
            .ToList()
            .ForEach( state => {
                state.ControlEnter.AddListener( ControlEnter.Invoke );
                state.ControlUpdate.AddListener( ControlUpdate.Invoke );
                state.ControlExit.AddListener( ControlExit.Invoke );
                state.StateEnter.AddListener( StateEnter.Invoke );
                state.StateUpdate.AddListener( StateUpdate.Invoke );
                state.StateExit.AddListener( StateExit.Invoke );
            } );
    }

    void OnDestroy()
    {
        if ( Animator == null )
        {
            return;
        }

        Animator
            .GetBehaviours<State>()
            .ToList()
            .ForEach( state => {
                state.ControlEnter.RemoveListener( ControlEnter.Invoke );
                state.ControlUpdate.RemoveListener( ControlUpdate.Invoke );
                state.ControlExit.RemoveListener( ControlExit.Invoke );
                state.StateEnter.RemoveListener( StateEnter.Invoke );
                state.StateUpdate.RemoveListener( StateUpdate.Invoke );
                state.StateExit.RemoveListener( StateExit.Invoke );
            } );
    }
}
