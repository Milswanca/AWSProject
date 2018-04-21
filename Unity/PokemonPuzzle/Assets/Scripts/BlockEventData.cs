using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockEventData", menuName = "Blocks/BlockEventData", order = 1)]
public class BlockEventData : ScriptableObject
{
    [Header("Swap Event Data")]
    public AnimationCurve SwapScaleAnimCurve;
    public float SwapEventTime = 0.2f;

    [Header("Fall Event Data")]
    public float FallEventSpeed = 6.0f;
}
