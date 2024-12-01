using ProjectHH;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class MoveInterruptPlayableAsset : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    [LabelText("是否启动移动打断")]public bool MoveInterrupt = true;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<MoveInterruptPlayableBehavior>.Create(graph);
        var behavior = playable.GetBehaviour();
        behavior.Character = go.GetComponent<TestCharacter>();
        behavior.MoveInterrupt = MoveInterrupt;
        return playable;
    }
}
