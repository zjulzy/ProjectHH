using ProjectHH;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class MoveInterruptPlayableBehavior : PlayableBehaviour
{
    public TestCharacter Character;

    public bool MoveInterrupt;
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        PlayableDirector director = Character.GetComponent<PlayableDirector>();
        if (MoveInterrupt)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01)
            {
                director.Stop();
            }
        }
    }
}