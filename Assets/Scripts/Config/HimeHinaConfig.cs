using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Sirenix.OdinInspector;
using ProjectHH;

[CreateAssetMenu(fileName = "HimeHinaConfig", menuName = "Scriptable Objects/HimeHinaConfig")]
public class HimeHinaConfig : SerializedScriptableObject
{
    [FoldoutGroup("Input"), LabelText("允许的输入映射")]
    public Dictionary<KeyCode, ControlAction> AllowedControls;

    [FoldoutGroup("Hero"), TabGroup("Hero/Tab", "Hime")]
    public CharacterBase HimePrefab;

    [FoldoutGroup("Hero"), TabGroup("Hero/Tab", "Hime")]
    public Vector3 HimeSpawnPosition;

    [FoldoutGroup("Hero"), TabGroup("Hero/Tab", "Hina")]
    public CharacterBase HinaPrefab;

    [FoldoutGroup("Character")] public Dictionary<string, float> CharacterConfig = new()
    {
        {"WalkSpeed", 2.0f},
        {"FirstJumpForce", 6.0f},
        {"SecondJumpForce", 5.0f},
        {"Gravity", 9.8f},
        {"GroundDetectionDistance", 0.01f}
    };
}