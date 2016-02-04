using UnityEngine;

/// <summary>
/// The State class is a specialized StateMachineBehaviour that provides an event interface for 
/// the messages that Unity calls. Perhaps more importantly, States communicate with the next state 
/// (when transitioning into something else) or the previous state (when transitioning from 
/// something else). This allows the end-programmer to utilize transition times without losing the 
/// reliability of events that can signal changes in control contexts (otherwise there are overlaps
/// when 2 control contexts are simultaneously valid).
/// </summary>
public class State : StateMachineBehaviour
{
    public string Name = "";

    public StateMachineEvent StateEnter;
    public StateMachineEvent StateUpdate;
    public StateMachineEvent StateExit;
    public StateMachineEvent ControlEnter;
    public StateMachineEvent ControlUpdate;
    public StateMachineEvent ControlExit;

    public StateMachine StateMachine { get; private set; }

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
            StateMachine = animator.GetComponent<StateMachine>();
        }

        // Emit an event to signal this state enter to other objects.
        StateEnter.Invoke( this, animator, stateInfo, layerIndex );

        if ( StateMachine != null )
        {
            if ( StateMachine.IsDebugLogging )
            {
                Debug.Log( "OnStateEnter: " + Name );
            }
        }
        else
        {
            // If StateMachine is still null then we have a problem.
            Debug.LogError( "State requires a StateMachine to support control context callbacks! Control contest events will not be emitted and OnControlEnter(), OnControlUpdate() and OnControlExit() will not be called until a StateMachine is available." );
            return;
        }
        
        // This state is now entering.
        StateMachine.SetEnteringState( this );
        
        // Notify the previous state that the next state (this one) has entered.
        if ( StateMachine.CurrentState != null )
        {
            StateMachine.CurrentState.OnControlExit( animator, stateInfo, layerIndex );
        }
        else if ( !StateMachine.IsStarted )
        {
            // If CurrentState is null, then the StateMachine just started up and we need to 
            // set it to the current state and start the control context handlers firing.
            StateMachine.SetIsStarted();
            StateMachine.SetCurrentState( this );
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
        StateMachine.SetEnteringState( null );
        StateMachine.SetCurrentState( this );

        // Emit an event to signal this state enter to other objects.
        ControlEnter.Invoke( this, animator, stateInfo, layerIndex );

        if ( StateMachine.IsDebugLogging )
        {
            Debug.Log( "OnControlEnter: " + Name );
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

        // Drive the special update function below. It only executes when this state is the only 
        // active state. As soon as any transition begins it no longer runs.
        if ( StateMachine != null )
        {
            if ( StateMachine.IsDebugLogging )
            {
                Debug.Log( "OnStateUpdate: " + Name );
            }

            if ( StateMachine.CurrentState == this )
            {
                OnControlUpdate( animator, stateInfo, layerIndex );
            }
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

        if ( StateMachine.IsDebugLogging )
        {
            Debug.Log( "OnControlUpdate: " + Name );
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

        if ( StateMachine != null )
        {
            if ( StateMachine.IsDebugLogging )
            {
                Debug.Log( "OnStateExit: " + Name );
            }

            // This state has now finished exiting.
            StateMachine.SetExitingState( null );

            // Notify the next state that the previous state (this one) has exited.
            if ( StateMachine.EnteringState != null )
            {
                StateMachine.EnteringState.OnControlEnter( animator, stateInfo, layerIndex );
            }
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
        StateMachine.SetCurrentState( null );
        StateMachine.SetExitingState( this );

        // Emit an event to signal this state exit to other objects.
        ControlExit.Invoke( this, animator, stateInfo, layerIndex );

        if ( StateMachine.IsDebugLogging )
        {
            Debug.Log( "OnControlExit: " + Name );
        }
    }
}

