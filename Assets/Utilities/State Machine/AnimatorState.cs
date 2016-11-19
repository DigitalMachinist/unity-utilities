using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An event used to emit information about the current layer/state being handled to the state machine.
/// </summary>
[Serializable]
public class AnimatorStateEvent : UnityEvent<AnimatorState, Animator, AnimatorStateInfo, int> { }

/// <summary>
/// The AnimatorState class is a specialized StateMachineBehaviour that provides an event interface 
/// for the messages that Unity calls. Perhaps more importantly, AnimatorState communicates with the 
/// next state (when transitioning into something else) or the previous state (when transitioning from 
/// something else). This allows the end-programmer to utilize transition times without losing the 
/// reliability of events that can signal changes in control contexts (otherwise there are overlaps
/// when 2 control contexts are simultaneously valid).
/// </summary>
public class AnimatorState : StateMachineBehaviour
{
    public string Name = "";

    [NonSerialized] public AnimatorStateEvent StateEnter = new AnimatorStateEvent();
    [NonSerialized] public AnimatorStateEvent StateUpdate = new AnimatorStateEvent();
    [NonSerialized] public AnimatorStateEvent StateExit = new AnimatorStateEvent();
    [NonSerialized] public AnimatorStateEvent ControlEnter = new AnimatorStateEvent();
    [NonSerialized] public AnimatorStateEvent ControlUpdate = new AnimatorStateEvent();
    [NonSerialized] public AnimatorStateEvent ControlExit = new AnimatorStateEvent();

    public AnimatorStateMachine StateMachine { get; private set; }

    /// <summary>
    /// Called when the state begins.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        // Get a ref to the StateMachine if one isn't set already.
        if ( StateMachine == null )
        {
            // TODO Consider providing a way to automatically create a StateMachine on the parent object.
            StateMachine = animator.GetComponent<AnimatorStateMachine>();
        }

        // If StateMachine is still null then we have a problem.
        if ( StateMachine == null )
        {
            Debug.LogError( "State requires a StateMachine to support control context callbacks! Control contest events will not be emitted and OnControlEnter(), OnControlUpdate() and OnControlExit() will not be called until a StateMachine is available." );
            return;
        }

        // This state is now entering.
        var layer = StateMachine.GetLayer( layerIndex );
        layer.EnteringState = this;

        // Emit an event to signal this state enter to other objects.
        StateEnter.Invoke( this, animator, stateInfo, layerIndex );

        if ( layer.LogStateEnters )
        {
            Debug.Log( "<" + layer.Name + "> OnStateEnter: " + Name );
        }

        // Notify the previous state that the next state (this one) has entered.
        if ( layer.MostRecentState != null )
        {
            // If there is a most recent state, then we're resuming control state from
            // where another state left off after passing through one or more mechanim
            // states that didn't have States attached. We need to clear MostRecentState
            // now so this doesn't get detected again incorrectly.
            layer.MostRecentState.OnControlExit( animator, stateInfo, layerIndex );
            OnControlEnter( animator, stateInfo, layerIndex );
            layer.MostRecentState = null;
            layer.ExitingState = null;
        }
        else if ( layer.CurrentState != null )
        {
            layer.CurrentState.OnControlExit( animator, stateInfo, layerIndex );
        }
        else if ( !layer.IsStarted )
        {
            // If CurrentState is null, then the StateMachine just started up and we need to 
            // set it to the current state and start the control context handlers firing.
            layer.IsStarted = true;
            layer.CurrentState = this;
            OnControlEnter( animator, stateInfo, layerIndex );
        }
    }

    /// <summary>
    /// Called when the transition from the previous state has ended. Use this to set up control 
    /// event listeners or otherwise configure your control context for this state.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public virtual void OnControlEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        // This method shouldn't run if no StateMachine is available.
        if ( StateMachine == null )
        {
            return;
        }

        // This state is no longer entering -- it is now the current state.
        var layer = StateMachine.GetLayer( layerIndex );
        layer.EnteringState = null;
        layer.CurrentState = this;

        // Emit an event to signal this state enter to other objects.
        ControlEnter.Invoke( this, animator, stateInfo, layerIndex );

        if ( layer.LogControlEnters )
        {
            Debug.Log( "<" + layer.Name + "> OnControlEnter: " + Name );
        }
    }

    /// <summary>
    /// Called each frame while the state is active.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        // Emit an event to signal this state update to other objects.
        StateUpdate.Invoke( this, animator, stateInfo, layerIndex );

        var layer = StateMachine.GetLayer( layerIndex );
        if ( layer.LogStateUpdates )
        {
            Debug.Log( "<" + layer.Name + "> OnStateUpdate: " + Name );
        }

        // The rest of this method shouldn't run if no StateMachine is available.
        if ( StateMachine == null )
        {
            return;
        }

        // Drive the special update function below. It only executes when this state is the only 
        // active state. As soon as any transition begins it no longer runs.
        if ( layer.CurrentState == this )
        {
            OnControlUpdate( animator, stateInfo, layerIndex );
        }
    }

    /// <summary>
    /// Called each frame while the state is the only active state. Use this to execute control 
    /// polling logic.
    /// </remarks>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public virtual void OnControlUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        // This method shouldn't run if no StateMachine is available.
        if ( StateMachine == null )
        {
            return;
        }

        // Emit an event to signal this state update to other objects.
        ControlUpdate.Invoke( this, animator, stateInfo, layerIndex );

        var layer = StateMachine.GetLayer( layerIndex );
        if ( layer.LogControlUpdates )
        {
            Debug.Log( "<" + layer.Name + "> OnControlUpdate: " + Name );
        }
    }

    /// <summary>
    /// Called when the state ends.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateExit( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        // Emit an event to signal this state exit to other objects.
        StateExit.Invoke( this, animator, stateInfo, layerIndex );

        var layer = StateMachine.GetLayer( layerIndex );
        if ( layer.LogStateExits )
        {
            Debug.Log( "<" + layer.Name + "> OnStateExit: " + Name );
        }

        // The rest of this method shouldn't run if no StateMachine is available.
        if ( StateMachine == null )
        {
            return;
        }

        // This state has now finished exiting.
        layer.ExitingState = null;

        if ( layer.EnteringState != null )
        {
            // Notify the next state that the previous state (this one) has exited.
            layer.EnteringState.OnControlEnter( animator, stateInfo, layerIndex );
        }
        else
        {
            // Store the current state as the most recent state so that the next active state 
            // can pick up from where we left off.
            layer.MostRecentState = this;
        }
    }

    /// <summary>
    /// Called when the transition from the next state has begins. Use this to tear down control 
    /// event listeners or otherwise clean up your control context for this state.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public virtual void OnControlExit( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        // This method shouldn't run if no StateMachine is available.
        if ( StateMachine == null )
        {
            return;
        }

        // This state is no longer the current state -- it has begun exiting.
        var layer = StateMachine.GetLayer( layerIndex );
        layer.CurrentState = null;
        layer.ExitingState = this;

        // Emit an event to signal this state exit to other objects.
        ControlExit.Invoke( this, animator, stateInfo, layerIndex );

        if ( layer.LogControlExits )
        {
            Debug.Log( "<" + layer.Name + "> OnControlExit: " + Name );
        }
    }
}

