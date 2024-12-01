using ProjectHH;
using UnityEngine;
using UnityEngine.Timeline;

[TrackClipType(typeof(BlockMovePlayableAsset)), TrackClipType(typeof(CheckContinuePlayableAsset)), TrackClipType(typeof(MoveInterruptPlayableAsset))]
public class CharacterControlTrackAsset : TrackAsset
{
}