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
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        return Playable.Create(graph);
    }
}
