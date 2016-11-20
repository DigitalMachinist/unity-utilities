using UnityEngine;
using UnityEngine.Audio;

[RequireComponent( typeof( AnimatorState ) )]
public class AnimatorStateAudioController : AnimatorStateAddon
{
    [Header( "Mixer Controls" )]
    public AudioMixerSnapshot SnapshotDuringControl;
    public AudioMixerSnapshot SnapshotAfterControl;
    public float SnapshotEnterSeconds = 1f;
    public float SnapshotExitSeconds = 1f;

    //[Header( "Music & Ambiance" )]


    protected override void OnControlEnter( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( SnapshotDuringControl != null )
        {
            SnapshotDuringControl.TransitionTo( SnapshotEnterSeconds );
        }
    }

    protected override void OnControlExit( AnimatorLayer layer, AnimatorStateBehaviour state )
    {
        if ( SnapshotAfterControl != null )
        {
            SnapshotAfterControl.TransitionTo( SnapshotExitSeconds );
        }
    }
}
