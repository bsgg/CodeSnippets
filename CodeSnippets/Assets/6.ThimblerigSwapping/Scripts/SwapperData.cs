using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThimblerigSwapping
{
    [CreateAssetMenu(fileName = "SwapperData", menuName = "SwapperData/Data", order = 1)]
    public class SwapperData : ScriptableObject
    {
        public int MaximunSwaps = 3;
        public float SwapAnimationTime = 0.6f;
        public int NumberObjectsToInstance = 3;
        public float DistanceBetweenItems = 3.0f;
    }
}
