using UnityEngine;

[RequireComponent( typeof( AnimatorState ) )]
public abstract class AnimatorStateAddon : MonoBehaviour
{
    [Header( "Events" )]
    public FoldableEvent Ready;

    AnimatorState animatorState;

    public Animator Animator
    {
        get { return AnimatorStateMachine.Animator; }
    }
    public AnimatorState AnimatorState
    {
        get { return animatorState ?? ( animatorState = GetComponent<AnimatorState>() ); }
    }
    public AnimatorStateMachine AnimatorStateMachine
    {
        get { return AnimatorState.AnimatorStateMachine; }
    }

    void Awake()
    {
        // When the AnimatorState is ready, register to its events.
        AnimatorState
            .Ready
            .AddListener( () => {
                AnimatorState.ControlEnter.AddListener( OnControlEnter );
                AnimatorState.StateEnter.AddListener( OnStateEnter );
                AnimatorState.ControlExit.AddListener( OnControlExit );
                AnimatorState.StateExit.AddListener( OnStateEnter );
                AnimatorState.ControlUpdate.AddListener( OnControlUpdate );
                AnimatorState.StateUpdate.AddListener( OnStateUpdate );

                // Notify any listeners that we're ready for action.
                Ready.Invoke();
            } );
    }

    protected virtual void OnControlEnter( AnimatorLayer layer, AnimatorStateBehaviour state )
    {

    }
    protected virtual void OnStateEnter( AnimatorLayer layer, AnimatorStateBehaviour state )
    {

    }
    protected virtual void OnControlExit( AnimatorLayer layer, AnimatorStateBehaviour state )
    {

    }
    protected virtual void OnStateExit( AnimatorLayer layer, AnimatorStateBehaviour state )
    {

    }
    protected virtual void OnControlUpdate( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        
    }
    protected virtual void OnStateUpdate( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        
    }
}
