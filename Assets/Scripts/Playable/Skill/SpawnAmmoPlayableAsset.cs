using ProjectHH;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SpawnAmmoPlayableAsset : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    [AssetList]
    public Ammo AmmoPrefab;

    public Transform SpawnPosition;
    
    public float SpawnTime;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<SpawnAmmoPlayableBehavior>.Create(graph);
        var behavior = playable.GetBehaviour();
        behavior.AmmoPrefab = AmmoPrefab;
        behavior.Director = go.GetComponent<PlayableDirector>();
        behavior.SpawnPosition = SpawnPosition;
        behavior.SpawnTime = SpawnTime;
        return playable;
    }
}
