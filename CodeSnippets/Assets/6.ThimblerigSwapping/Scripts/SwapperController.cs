using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ThimblerigSwapping
{
    public class SwapperController : MonoBehaviour
    {
        [SerializeField] private int m_MaxSwaps = 3;
        [SerializeField] private List<BezierMovement>                  m_ItemList;
        [SerializeField] private float m_AnimationTotalTime = 0.6f;

        private List<Vector3>                                          m_ItemPostionList;

        private int m_ItemsToSwapNumber = 2;

        void Start()
        {
            if (m_ItemList != null)
            {
                m_ItemPostionList = new List<Vector3>();
                for (int i = 0; i < m_ItemList.Count; i++)
                {
                    m_ItemPostionList.Add(m_ItemList[i].transform.localPosition);
                    // Add event
                    m_ItemList[i].OnEndMovement += OnEndBezierMove;
                }

                RandomizeItems();
            }
        }

        /// <summary>
        /// Set random swap movement
        /// </summary>
        private void RandomizeItems()
        {
            if (m_ItemList != null)
            {
                // Create random array and select two items
                int[] randomIndexList = new int[m_ItemList.Count];
                for (int i = 0; i < m_ItemList.Count; i++)
                {
                    randomIndexList[i] = i;
                }

                // Shufle indeces
                Utility.MathUtility.Shuffle(ref randomIndexList);

                // Select the first 2 items
                int indexA = randomIndexList[0];
                int indexB = randomIndexList[1];

                //Select those elements
                BezierMovement itemA = m_ItemList[indexA];
                itemA.MoveTo(m_ItemPostionList[indexB], m_AnimationTotalTime);
             

                BezierMovement itemB = m_ItemList[indexB];
                itemB.MoveTo(m_ItemPostionList[indexA], m_AnimationTotalTime);

                m_ItemsToSwapNumber = 2;

                // Before start movement, swap items
                BezierMovement auxObj = m_ItemList[indexB];
                m_ItemList[indexB] = m_ItemList[indexA];
                m_ItemList[indexA] = auxObj;

            }
        }

        /// <summary>
        /// Event handle when a bezier movement is finished
        /// </summary>
        /// <param name="obj"></param>
        public void OnEndBezierMove(BezierMovement obj)
        {
            //obj.OnEndMovement -= OnEndBezierMove;
            m_ItemsToSwapNumber -= 1;

            // All items have been swaped
            if (m_ItemsToSwapNumber <= 0)
            {
                // New random swap until all swaps have finished
                m_MaxSwaps--;
                if (m_MaxSwaps > 0)
                {
                    RandomizeItems();
                }                
            }         
        }
      }
}
