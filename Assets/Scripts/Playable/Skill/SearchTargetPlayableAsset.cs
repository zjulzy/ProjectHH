using ProjectHH;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SearchTargetPlayableAsset : PlayableAsset
{
    private SearchTargetPlayableBehavior _behavior = new SearchTargetPlayableBehavior();
    
    // 技能起始点相较于角色逻辑位置的便宜
    public Vector3 SkillStartOffet;
    
    // 技能检测盒的half size
    public Vector3 HitBoxExtent;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var character = go.GetComponent<TestCharacter>();
        var playable = ScriptPlayable<SearchTargetPlayableBehavior>.Create(graph, _behavior);
        var searchTargetPlayable = playable.GetBehaviour();
        
        searchTargetPlayable.HitBoxCenterOffset = SkillStartOffet + Vector3.forward * HitBoxExtent.z;
        searchTargetPlayable.HitBoxExtent = HitBoxExtent;
        searchTargetPlayable.Character = character;
        return playable;
    }
}