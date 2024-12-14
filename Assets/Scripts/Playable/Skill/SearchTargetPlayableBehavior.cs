using ProjectHH;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SearchTargetPlayableBehavior : PlayableBehaviour
{
    public TestCharacter Character;
    public Vector3 HitBoxCenterOffset;
    public Vector3 HitBoxExtent;

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Vector3 hitBoxCenter = Character.transform.position + Character.transform.rotation * HitBoxCenterOffset;
        var result = Physics.OverlapBox(hitBoxCenter, HitBoxExtent, Quaternion.identity, LayerMask.GetMask("Enemy"));
        foreach (var r in result)
        {
            var gameObject = r.gameObject;
            var enemy = r.GetComponent<TestEnemy>();
            if (enemy != null)
            {
                enemy.SetHealth(enemy.CurrentHealth - 10);
            }
        }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
    }
}