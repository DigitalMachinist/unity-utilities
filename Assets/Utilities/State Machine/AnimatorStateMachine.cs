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
    public FoldablePromise Ready;
    public List<AnimatorLayer> Layers;

    Animator animator;

    public List<AnimatorStateBehaviour> States { get; private set; }

    public Animator Animator
    {
        get { return animator ?? ( animator = GetComponent<Animator>() ); }
    }


    public AnimatorLayer GetLayer( int layerIndex )
    {
        return Layers[ layerIndex ];
    }
    public AnimatorLayer GetLayerByName( string layerName )
    {
        var layerIndex = Animator.GetLayerIndex( layerName );
        return GetLayer( layerIndex );
    }


    void OnDisable()
    {
        DeregisterEvents();
    }
    void OnEnable()
    {
        RebuildLayersIfEmpty();
        RegisterEvents();
    }
    void OnValidate()
    {
        RebuildLayersIfEmpty();
    }


    void DeregisterEvents()
    {
        foreach ( var state in States )
        {
            state.ControlEnter.RemoveListener( OnStateMachineControlEnter );
            state.StateEnter.RemoveListener( OnStateMachineStateEnter );
            state.ControlExit.RemoveListener( OnStateMachineControlExit );
            state.StateExit.RemoveListener( OnStateMachineStateExit );
            state.ControlUpdate.RemoveListener( OnStateMachineStateUpdate );
            state.StateUpdate.RemoveListener( OnStateMachineControlUpdate );
        }
        States.Clear();
    }
    void RegisterEvents()
    {
        States = Animator.GetBehaviours<AnimatorStateBehaviour>().ToList();
        foreach ( var state in States )
        {
            state.ControlEnter.AddListener( OnStateMachineControlEnter );
            state.StateEnter.AddListener( OnStateMachineStateEnter );
            state.ControlExit.AddListener( OnStateMachineControlExit );
            state.StateExit.AddListener( OnStateMachineStateExit );
            state.ControlUpdate.AddListener( OnStateMachineStateUpdate );
            state.StateUpdate.AddListener( OnStateMachineControlUpdate );
        }

        // Signal to listeners that we're ready to go.
        Ready.Invoke();
    }
    void RebuildLayersIfEmpty()
    {
        if ( Animator == null )
        {
            return;
        }

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


    void OnStateMachineControlEnter( AnimatorStateBehaviour state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.ControlEnter.Invoke( layer, state );
    }
    void OnStateMachineStateEnter( AnimatorStateBehaviour state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.StateEnter.Invoke( layer, state );
    }
    void OnStateMachineControlExit( AnimatorStateBehaviour state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.ControlExit.Invoke( layer, state );
    }
    void OnStateMachineStateExit( AnimatorStateBehaviour state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.StateExit.Invoke( layer, state );
    }
    void OnStateMachineControlUpdate( AnimatorStateBehaviour state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.ControlUpdate.Invoke( layer, state );
    }
    void OnStateMachineStateUpdate( AnimatorStateBehaviour state, Animator anim, AnimatorStateInfo animStateInfo, int layerIndex )
    {
        var layer = GetLayer( layerIndex );
        layer.StateUpdate.Invoke( layer, state );
    }
}
