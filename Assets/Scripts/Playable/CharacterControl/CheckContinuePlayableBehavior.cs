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
        // 检查当前技能的按键，是否可以延续当前技能
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
        
        // 如果期间没有按键或者在移动打断点之前检测到了移动输入，需要打断技能
        if (playable.GetDuration() - playable.GetTime() < 0.01)
        {
            if(!_shouldContinue)
                Director.Stop();
        }
        
        
        // TODO：检测按键进入其他技能或者连招
    }
}