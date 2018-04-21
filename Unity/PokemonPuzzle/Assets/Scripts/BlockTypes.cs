using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockTypes", menuName = "Blocks/BlockTypes", order = 1)]
public class BlockTypes : ScriptableObject
{
    public GameBlock[] defaultBlockPrefabs; 
}
