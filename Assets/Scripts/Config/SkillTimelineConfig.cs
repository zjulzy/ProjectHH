using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "HimeTimelineConfig", menuName = "Scriptable Objects/HimeTimelineConfig")]
public class SkillTimelineConfig : SerializedScriptableObject
{
    [ShowInInspector] public Dictionary<InputType, TimelineAsset> SkillMap;
}
