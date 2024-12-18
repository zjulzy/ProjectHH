using ProjectHH;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class BlockMovePlayableBehavior : PlayableBehaviour
{
    public TestCharacter Character;
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
        Character.SkillBlockMoving = true;
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        Character.SkillBlockMoving = false;
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
    }
}
