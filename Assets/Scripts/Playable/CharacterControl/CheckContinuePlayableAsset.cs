using NUnit.Framework.Constraints;
using ProjectHH;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

public enum InputType: uint
{
    Attack01 = 0,
    Attack02 = 1,
    SoulAbility = 2
    
}

[System.Serializable]
public class CheckContinuePlayableAsset : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    public bool MoveInterrupt = true;
    public InputType InputType;
    
    public float MoveInterruptStartTime = 0;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<CheckContinuePlayableBehavior>.Create(graph);
        var behavior = playable.GetBehaviour();
        behavior.Character = go.GetComponent<TestCharacter>();
        behavior.CheckInputType = InputType;
        behavior.MoveInterrupt = MoveInterrupt;
        behavior.MoveInterruptTime = MoveInterruptStartTime;
        behavior.Director = go.GetComponent<PlayableDirector>();
        return playable;
    }
}
