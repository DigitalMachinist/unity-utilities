using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The StateMachine class is used by States to communicate to one another and supports the action 
/// of other game objects in response to events signaling state changes.
/// </summary>
[RequireComponent( typeof( Animator ) )]
public class AnimatorStateMachine : MonoBehaviour
{
    public List<AnimatorLayer> Layers;

    Animator animator;
    public Animator Animator
    {
        get { return animator ?? ( animator = GetComponent<Animator>() ); }
    }

    public AnimatorLayer GetLayer( int layerIndex )
    {
        return Layers[ layerIndex ];
    }

    void Awake()
    {
        RebuildLayersIfEmpty();
    }

    void OnEnable()
    {
        RebuildLayersIfEmpty();
    }

    void OnValidate()
    {
        RebuildLayersIfEmpty();
    }

    void Start()
    {
        // Register event listeners for all of the states.
        Animator
            .GetBehaviours<AnimatorState>()
            .ToList()
            .ForEach( state => {
                state.ControlEnter.AddListener( OnStateMachineControlEnter );
                state.StateEnter.AddListener( OnStateMachineStateEnter );
                state.ControlExit.AddListener( OnStateMachineControlExit );
                state.StateExit.AddListener( OnStateMachineStateExit );
                state.ControlUpdate.AddListener( OnStateMachineStateUpdate );
                state.StateUpdate.AddListener( OnStateMachineControlUpdate );
            } );
    }

    void OnDestroy()
    {
        // Deregister event listeners for all of the states.
        Animator
            .GetBehaviours<AnimatorState>()
            .ToList()
            .ForEach( state => {
                state.ControlEnter.RemoveListener( OnStateMachineControlEnter );
                state.StateEnter.RemoveListener( OnStateMachineStateEnter );
                state.ControlExit.RemoveListener( OnStateMachineControlExit );
                state.StateExit.RemoveListener( OnStateMachineStateExit );
                state.ControlUpdate.RemoveListener( OnStateMachineStateUpdate );
                state.StateUpdate.RemoveListener( OnStateMachineControlUpdate );
            } );
    }

    void RebuildLayersIfEmpty()
    {
        // Create a list of Layer objects mapping to each of the Mechanim layers.
        if ( Animator != null )
        {
            RebuildLayers();
        }
    }

    void RebuildLayers()
    {
        if ( Layers == null )
        {
            // Create a new layer collection, if it's missing.
            Layers = new List<AnimatorLayer>();
        }
        if ( Layers.Count == 0 )
        {
            // Create new layers, if they're missing.
            for ( var i = 0; i < Animator.layerCount; i++ )
            {
                var name = Animator.GetLayerName( i );
                Layers.Add( new AnimatorLayer( name ) );
            }
        }
        else
        {
            // If the layers exist, update their names just in case.
            for ( var i = 0; i < Animator.layerCount; i++ )
            {
                GetLayer( i ).Name = Animator.GetLayerName( i );
            }
        }
    }

    void OnStateMachineControlEnter( AnimatorState state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.ControlEnter.Invoke( layer, state );
    }

    void OnStateMachineStateEnter( AnimatorState state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.StateEnter.Invoke( layer, state );
    }

    void OnStateMachineControlExit( AnimatorState state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.ControlExit.Invoke( layer, state );
    }

    void OnStateMachineStateExit( AnimatorState state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.StateExit.Invoke( layer, state );
    }

    void OnStateMachineControlUpdate( AnimatorState state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.ControlUpdate.Invoke( layer, state );
    }

    void OnStateMachineStateUpdate( AnimatorState state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.StateUpdate.Invoke( layer, state );
    }
}
