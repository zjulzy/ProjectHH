using System.Collections.Generic;
using ProjectHH;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class CheckContinuePlayableBehavior : PlayableBehaviour
{
    public TestCharacter Character;

    public InputType CheckInputType;
    public PlayableDirector Director;
    public float JumpTime;
    private bool _shouldContinue;

    public bool MoveInterrupt;
    public float MoveInterruptTime;
    
    public Dictionary<InputType,ComboSkillType> AllowedInputCombos;
    public List<InputType> AllowedInputTransforms;

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
        _shouldContinue = false;
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }
    

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (playable.GetDuration() - playable.GetTime() < 0.01)
        {
            if(!_shouldContinue)
                Director.Stop();
        }

        switch (CheckInputType)
        {
            case InputType.Attack01:
                bool isAttack01 = Input.GetMouseButtonDown(0);
                if (isAttack01)
                {
                    _shouldContinue = true;
                    PlayableDirector director = Character.GetComponent<PlayableDirector>();   
                    director.time = JumpTime;
                }
                break;
            case InputType.Attack02:
            case InputType.SoulAbility:
                break;
        }

        if (MoveInterrupt && Director.time>=MoveInterruptTime)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01)
            {
                _shouldContinue = false;
                Director.Stop();
            }
        }
    }
}