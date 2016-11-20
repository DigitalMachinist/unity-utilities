using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An event used to emit information about the current layer/state being handled to the state machine.
/// </summary>
[Serializable]
public class AnimatorStateBehaviourEvent : UnityEvent<AnimatorStateBehaviour, Animator, AnimatorStateInfo, int> { }

/// <summary>
/// The AnimatorState class is a specialized StateMachineBehaviour that provides an event interface 
/// for the messages that Unity calls. Perhaps more importantly, AnimatorState communicates with the 
/// next state (when transitioning into something else) or the previous state (when transitioning from 
/// something else). This allows the end-programmer to utilize transition times without losing the 
/// reliability of events that can signal changes in control contexts (otherwise there are overlaps
/// when 2 control contexts are simultaneously valid).
/// </summary>
/// <remarks>
/// The events fired by this class provide 2 major concepts to use:
/// 
/// 1) State enter/exit events that represent the beginning and end of Mechanic states.
/// 2) Control enter/exit events that represent periods of time where Mechanim states are guaranteed 
///    not to be overlapping with each other i.e. there is one active state in charge.
///    
/// Expect event execution for a typical cycle of Mechanim states to proceed as follows:
/// 
/// : Time advancing this way -->
/// : StateEnter (1)
/// : ControlEnter (1)
/// :     StateEnter (2)
/// :     ControlExit (1)
/// :         StateExit (1)
/// :         ControlEnter (2)
/// :             StateEnter (3)
/// :             ControlExit (2)
/// :                 StateExit (2)
/// :                 ControlEnter (3)
/// </remarks>
public class AnimatorStateBehaviour : StateMachineBehaviour
{
    public string Name = "";

    [NonSerialized] public AnimatorStateBehaviourEvent StateEnter = new AnimatorStateBehaviourEvent();
    [NonSerialized] public AnimatorStateBehaviourEvent StateUpdate = new AnimatorStateBehaviourEvent();
    [NonSerialized] public AnimatorStateBehaviourEvent StateExit = new AnimatorStateBehaviourEvent();
    [NonSerialized] public AnimatorStateBehaviourEvent ControlEnter = new AnimatorStateBehaviourEvent();
    [NonSerialized] public AnimatorStateBehaviourEvent ControlUpdate = new AnimatorStateBehaviourEvent();
    [NonSerialized] public AnimatorStateBehaviourEvent ControlExit = new AnimatorStateBehaviourEvent();

    public AnimatorStateInfo Info { get; private set; }
    public AnimatorStateMachine StateMachine { get; private set; }

    /// <summary>
    /// Called when the transition from the previous state has ended. Use this to set up control 
    /// event listeners or otherwise configure your control context for this state.
    /// </summary>
    /// <param name="animator">The Animator instance driving this Mechanim state machine.</param>
    /// <param name="stateInfo">Mechanim state information about the current state.</param>
    /// <param name="layerIndex">The layer index that this Mechanim state belongs to. Use this to 
    /// index the correct AnimatorLayer instance by calling AnimatorStateMachine.GetLayer().</param>
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
    }

    /// <summary>
    /// Called when the state begins.
    /// </summary>
    /// <param name="animator">The Animator instance driving this Mechanim state machine.</param>
    /// <param name="stateInfo">Mechanim state information about the current state.</param>
    /// <param name="layerIndex">The layer index that this Mechanim state belongs to. Use this to 
    /// index the correct AnimatorLayer instance by calling AnimatorStateMachine.GetLayer().</param>
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

        // Store the state info so we can quickly test for equality with this state later.
        Info = stateInfo;

        // This state is now entering.
        var layer = StateMachine.GetLayer( layerIndex );
        layer.EnteringState = this;

        // Emit an event to signal this state enter to other objects.
        StateEnter.Invoke( this, animator, stateInfo, layerIndex );

        // Notify the previous state that the next state (this one) has entered.
        if ( layer.MostRecentState != null )
        {
            // If there is a most recent state, then we're resuming control state from
            // where another state left off after passing through one or more mechanim
            // states that didn't have States attached. We need to clear MostRecentState
            // now so this doesn't get detected again incorrectly.
            OnControlEnter( animator, stateInfo, layerIndex );
            layer.MostRecentState = null;
            layer.ExitingState = null;
        }
        else if ( layer.CurrentState != null )
        {
            // The most common situation is that we are continuing on from another 
            // AnimationState that will begin exiting.
            layer.CurrentState.OnControlExit( animator, layer.CurrentState.Info, layerIndex );
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
    /// Called when the transition from the next state has begins. Use this to tear down control 
    /// event listeners or otherwise clean up your control context for this state.
    /// </summary>
    /// <param name="animator">The Animator instance driving this Mechanim state machine.</param>
    /// <param name="stateInfo">Mechanim state information about the current state.</param>
    /// <param name="layerIndex">The layer index that this Mechanim state belongs to. Use this to 
    /// index the correct AnimatorLayer instance by calling AnimatorStateMachine.GetLayer().</param>
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
    }

    /// <summary>
    /// Called when the state ends.
    /// </summary>
    /// <param name="animator">The Animator instance driving this Mechanim state machine.</param>
    /// <param name="stateInfo">Mechanim state information about the current state.</param>
    /// <param name="layerIndex">The layer index that this Mechanim state belongs to. Use this to 
    /// index the correct AnimatorLayer instance by calling AnimatorStateMachine.GetLayer().</param>
    public override void OnStateExit( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        // Emit an event to signal this state exit to other objects.
        StateExit.Invoke( this, animator, stateInfo, layerIndex );

        // The rest of this method shouldn't run if no StateMachine is available.
        if ( StateMachine == null )
        {
            return;
        }

        // This state has now finished exiting.
        var layer = StateMachine.GetLayer( layerIndex );
        layer.ExitingState = null;

        if ( layer.EnteringState != null )
        {
            // Notify the next state that the previous state (this one) has exited.
            layer.EnteringState.OnControlEnter( animator, layer.EnteringState.Info, layerIndex );
        }
        else
        {
            // Store the current state as the most recent state so that the next active state 
            // can pick up from where we left off. We also need to force exit the control state
            // because there is no next state that would have ended it for us by now.
            OnControlExit( animator, stateInfo, layerIndex );
            layer.MostRecentState = this;
            layer.ExitingState = null;
        }
    }

    /// <summary>
    /// Called each frame while the state is the only active state. Use this to execute control 
    /// polling logic.
    /// </remarks>
    /// <param name="animator">The Animator instance driving this Mechanim state machine.</param>
    /// <param name="stateInfo">Mechanim state information about the current state.</param>
    /// <param name="layerIndex">The layer index that this Mechanim state belongs to. Use this to 
    /// index the correct AnimatorLayer instance by calling AnimatorStateMachine.GetLayer().</param>
    public virtual void OnControlUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        // This method shouldn't run if no StateMachine is available.
        if ( StateMachine == null )
        {
            return;
        }

        // Emit an event to signal this state update to other objects.
        ControlUpdate.Invoke( this, animator, stateInfo, layerIndex );
    }

    /// <summary>
    /// Called each frame while the state is active.
    /// </summary>
    /// <param name="animator">The Animator instance driving this Mechanim state machine.</param>
    /// <param name="stateInfo">Mechanim state information about the current state.</param>
    /// <param name="layerIndex">The layer index that this Mechanim state belongs to. Use this to 
    /// index the correct AnimatorLayer instance by calling AnimatorStateMachine.GetLayer().</param>
    public override void OnStateUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        // Emit an event to signal this state update to other objects.
        StateUpdate.Invoke( this, animator, stateInfo, layerIndex );

        // The rest of this method shouldn't run if no StateMachine is available.
        if ( StateMachine == null )
        {
            return;
        }

        // Drive the special update function below. It only executes when this state is the only 
        // active state. As soon as any transition begins it no longer runs.
        var layer = StateMachine.GetLayer( layerIndex );
        if ( layer.CurrentState == this )
        {
            OnControlUpdate( animator, stateInfo, layerIndex );
        }
    }
}
