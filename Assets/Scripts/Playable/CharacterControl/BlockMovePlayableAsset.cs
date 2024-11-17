using ProjectHH;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class BlockMovePlayableAsset : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    private BlockMovePlayableBehavior _behavior = new BlockMovePlayableBehavior();
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var Character = go.GetComponent<TestCharacter>();
        var playable = ScriptPlayable<BlockMovePlayableBehavior>.Create(graph, _behavior);
        var blockMovePlayable = playable.GetBehaviour();
        blockMovePlayable.Character = Character;
        return playable;
    }
}
