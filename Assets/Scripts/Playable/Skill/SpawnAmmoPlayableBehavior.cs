using ProjectHH;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SpawnAmmoPlayableBehavior : PlayableBehaviour
{
    // Called when the owning graph starts playing
    public Ammo AmmoPrefab;
    public string SpawnPositionAttachPoint;

    public float SpawnTime;
    public PlayableDirector Director;

    private bool _hasSpawned = false;

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
        if (!_hasSpawned && Director.time >= SpawnTime)
        {
            Transform spawnPosition = Director.gameObject.transform.Find("AttachPoint").Find(SpawnPositionAttachPoint);
            Ammo ammo = Object.Instantiate(AmmoPrefab, spawnPosition.position, spawnPosition.rotation);
            ammo.transform.forward = spawnPosition.forward;
            ammo.Initialize();
            _hasSpawned = true;
        }
    }
}