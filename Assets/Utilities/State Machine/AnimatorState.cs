using UnityEngine;

public class AnimatorState : MonoBehaviour
{
    [Header( "State Machine" )]
    [Tooltip( "The AnimatorStateMachine that drives this AnimatorState. If not set, this will automatically look up the hierarchy parentage for an appropriate component to use." )]
    public AnimatorStateMachine stateMachine;

    [Header( "State ID" )]
    [Tooltip( "The name of the Mechanim state machine layer to which the state below belongs. Must be exactly correct!" )]
    public string LayerName = "Base Layer";
    [Tooltip( "The name of the Mechanim state to watch for events from. Must be exactly correct!" )]
    public string StateName;

    [Header( "Events" )]
    public FoldableEvent Ready;
    public FoldableLayerEvent ControlEnter;
    public FoldableLayerEvent StateEnter;
    public FoldableLayerEvent ControlExit;
    public FoldableLayerEvent StateExit;
    public FoldableLayerEvent ControlUpdate;
    public FoldableLayerEvent StateUpdate;

    [Header( "Debug" )]

    int stateHash;

    public Animator Animator
    {
        get { return AnimatorStateMachine.Animator; }
    }
    public int StateHash
    {
        get
        {
            if ( stateHash == 0 )
            {
                stateHash = Animator.StringToHash( LayerName + "." + StateName );
            }
            return stateHash;
        }
    }
    public AnimatorStateMachine AnimatorStateMachine
    {
        get { return stateMachine ?? ( stateMachine = GetComponentInParent<AnimatorStateMachine>() ); }
    }


    void Awake()
    {
        // When the AnimatorStateMachine is ready, register to the appropriate layer's events.
        var layer = AnimatorStateMachine.GetLayerByName( LayerName );
        AnimatorStateMachine
            .Ready
            .AddListener( () => {
                layer.ControlEnter.AddListener( OnControlEnter );
                layer.StateEnter.AddListener( OnStateEnter );
                layer.ControlExit.AddListener( OnControlExit );
                layer.StateExit.AddListener( OnStateEnter );
                layer.ControlUpdate.AddListener( OnControlUpdate );
                layer.StateUpdate.AddListener( OnStateUpdate );

                // Notify any listeners that we're ready for action.
                Ready.Invoke();
            } );
    }


    void OnControlEnter( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( state.Info.fullPathHash == StateHash )
        {
            ControlEnter.Invoke( layer, state );
        }
    }
    void OnStateEnter( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( state.Info.fullPathHash == StateHash )
        {
            StateEnter.Invoke( layer, state );
        }
    }
    void OnControlExit( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( state.Info.fullPathHash == StateHash )
        {
            ControlExit.Invoke( layer, state );
        }
    }
    void OnStateExit( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( state.Info.fullPathHash == StateHash )
        {
            StateExit.Invoke( layer, state );
        }
    }
    void OnControlUpdate( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( state.Info.fullPathHash == StateHash )
        {
            ControlUpdate.Invoke( layer, state );
        }
    }
    void OnStateUpdate( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( state.Info.fullPathHash == StateHash )
        {
            StateUpdate.Invoke( layer, state );
        }
    }
}
