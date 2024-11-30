using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using ProjectHH;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public enum InputType: uint
{
    Attack01 = 0,
    Attack02 = 1,
    SoulAbility = 2
}

[Serializable]
public enum ComboSkillType : uint
{
    Attack01_02 = 0,
}

[Serializable]
[ShowOdinSerializedPropertiesInInspector]
public class CheckContinuePlayableAsset : PlayableAsset, ISerializationCallbackReceiver, ISupportsPrefabSerialization
{
    // Factory method that generates a playable based on this asset
    [Title("技能延续")]
    [LabelText("是否移动打断")]public bool MoveInterrupt = true;
    [LabelText("当前技能输入类型")]public InputType InputType;
    [LabelText("技能延续跳转时间")]public float JumpTime;
    [LabelText("移动打断开始时间")]public float MoveInterruptStartTime = 0;
    
    [PropertySpace]
    [Title("技能跳转")]
    [LabelText("连招跳转timeline")]public Dictionary<InputType,ComboSkillType> AllowedInputCombos;
    [LabelText("技能切换timeline")]public List<InputType> AllowedInputTransforms;
    
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<CheckContinuePlayableBehavior>.Create(graph);
        var behavior = playable.GetBehaviour();
        behavior.Character = go.GetComponent<TestCharacter>();
        behavior.CheckInputType = InputType;
        behavior.MoveInterrupt = MoveInterrupt;
        behavior.MoveInterruptTime = MoveInterruptStartTime;
        behavior.JumpTime = JumpTime;
        behavior.Director = go.GetComponent<PlayableDirector>();
        behavior.AllowedInputCombos = AllowedInputCombos;
        behavior.AllowedInputTransforms = AllowedInputTransforms;
        return playable;
    }
    
    // 序列化相关代码
    [SerializeField,HideInInspector]
    private SerializationData serializationData;
    
    SerializationData ISupportsPrefabSerialization.SerializationData { get => this.serializationData; set => this.serializationData = value; }
    public void OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
    }

    public void OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData);
    }
}
