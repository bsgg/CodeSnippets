using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SwapItems
{
    public class SwapItems : MonoBehaviour
    {
        [SerializeField]
        private int m_NumberSwaps = 3;
        [SerializeField] private List<BezierMovement>                  m_ListSwapItems;
        private List<Vector3>                                          m_ListSwapItemsPostions;

        private int m_NumberItemsToSwap = 2;

        void Start()
        {
            if (m_ListSwapItems != null)
            {
                m_ListSwapItemsPostions = new List<Vector3>();
                for (int i = 0; i < m_ListSwapItems.Count; i++)
                {
                    m_ListSwapItemsPostions.Add(m_ListSwapItems[i].transform.localPosition);
                }

                StartRandomSwap();
            }
        }

        /// <summary>
        /// Set random swap movement
        /// </summary>
        private void StartRandomSwap()
        {
            if (m_ListSwapItems != null)
            {
                // Create random array and select two items
                int[] auxListIndex = new int[m_ListSwapItems.Count];
                for (int i = 0; i < m_ListSwapItems.Count; i++)
                {
                    auxListIndex[i] = i;
                }
                Utility.MathUtility.Shuffle<int>(ref auxListIndex);

                // Select the two first items
                int indexA = auxListIndex[0];
                int indexB = auxListIndex[1];

                // Start swaps
                BezierMovement m_ItemSwapItemA = m_ListSwapItems[indexA];
                m_ItemSwapItemA.StartBezierAnimation(m_ListSwapItemsPostions[indexB]);
                m_ItemSwapItemA.OnFinishMove += onFinishBezierMove;
               

                BezierMovement m_ItemSwapItemB = m_ListSwapItems[indexB];
                m_ItemSwapItemB.StartBezierAnimation(m_ListSwapItemsPostions[indexA]);
                m_ItemSwapItemB.OnFinishMove += onFinishBezierMove;

                m_NumberItemsToSwap = 2;

                // Before start movement, swap items
                BezierMovement auxObj = m_ListSwapItems[indexB];
                m_ListSwapItems[indexB] = m_ListSwapItems[indexA];
                m_ListSwapItems[indexA] = auxObj;
            }
        }

        /// <summary>
        /// Event handle when a bezier movement is finished
        /// </summary>
        /// <param name="obj"></param>
        public void onFinishBezierMove(BezierMovement obj)
        {
            obj.OnFinishMove -= onFinishBezierMove;
            m_NumberItemsToSwap -= 1;

            // All items have been swaped
            if (m_NumberItemsToSwap <= 0)
            {
                // New random swap
                m_NumberSwaps--;
                if (m_NumberSwaps > 0)
                {
                    StartRandomSwap();
                }                
            }          
                       
        }
      }
}
