using UnityEngine;
using UnityEngine.Audio;

[RequireComponent( typeof( AnimatorState ) )]
public class AnimatorStateDebugLogger : AnimatorStateAddon
{
    [Header( "Debug Logging" )]
    public bool LogControlEnters;
    public bool LogStateEnters;
    public bool LogControlExits;
    public bool LogStateExits;
    public bool LogControlUpdates;
    public bool LogStateUpdates;

    protected override void OnControlEnter( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( LogControlEnters )
        {
            Debug.Log( "<" + layer.Name + "> OnControlEnter: " + state.Name );
        }
    }
    protected override void OnStateEnter( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( LogStateEnters )
        {
            Debug.Log( "<" + layer.Name + "> OnStateEnter: " + state.Name );
        }
    }
    protected override void OnControlExit( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( LogControlExits )
        {
            Debug.Log( "<" + layer.Name + "> OnControlExit: " + state.Name );
        }
    }
    protected override void OnStateExit( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( LogStateExits )
        {
            Debug.Log( "<" + layer.Name + "> OnControlEnter: " + state.Name );
        }
    }
    protected override void OnControlUpdate( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( LogControlUpdates )
        {
            Debug.Log( "<" + layer.Name + "> OnStateExit: " + state.Name );
        }
    }
    protected override void OnStateUpdate( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( LogStateUpdates )
        {
            Debug.Log( "<" + layer.Name + "> OnStateUpdate: " + state.Name );
        }
    }
}
