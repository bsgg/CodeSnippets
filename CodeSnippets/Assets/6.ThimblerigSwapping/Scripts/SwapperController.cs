using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ThimblerigSwapping
{
    public class SwapperController : MonoBehaviour
    {
        [Header("Swapper Settings")]
        [SerializeField] private SwapperData m_data;

        [SerializeField] private BezierMovement m_itemPrefab;      

        [SerializeField] private Transform m_itemParent;

        private List<BezierMovement> m_itemList;

        private int m_currentMaximunSwaps;  

        private int m_itemsToSwapSimultaneously = 2;

        void Start()
        {
            InitializeSwapper();
        }

        private void DestroyItems()
        {
            if (m_itemList != null)
            {
                for (int i = (m_itemList.Count -1); i >= 0; i--)
                {
                    Destroy(m_itemList[i].gameObject);
                }
            }
        }
        private void InitializeSwapper()
        {
            if ((m_data == null) || (m_itemPrefab == null))
            {
                Debug.Log("[SwapperController.InitializeSwapper] No data found");

                return;
            }

            DestroyItems();

            m_itemList = new List<BezierMovement>();

            Vector3 origPosition = Vector3.zero;
            for (int i = 0; i< m_data.NumberObjectsToInstance; i++)
            {
                BezierMovement bezierObj = Instantiate(m_itemPrefab, Vector3.zero, Quaternion.identity, m_itemParent) as BezierMovement;

                if (bezierObj != null)
                {
                    bezierObj.transform.localPosition = origPosition;

                    bezierObj.BezierActionCompleted += OnBezierAnimationCompleted;

                    m_itemList.Add(bezierObj);
                }

                origPosition.x += m_data.DistanceBetweenItems;
            }

            m_currentMaximunSwaps = m_data.MaximunSwaps;

            RandomizeItems();
        }

        private void RandomizeItems()
        {
            if (m_itemList != null)
            {
                // Create random array and select two items
                int[] randomIndexList = new int[m_itemList.Count];
                for (int i = 0; i < m_itemList.Count; i++)
                {
                    randomIndexList[i] = i;
                }

                // Shufle indeces
                Shuffle(ref randomIndexList);

                // Select the first 2 items
                int indexA = randomIndexList[0];
                int indexB = randomIndexList[1];

                //Select those elements
                BezierMovement itemA = m_itemList[indexA];
                //itemA.MoveTo(m_itemPostionList[indexB], m_data.SwapAnimationTime);
                itemA.MoveTo(m_itemList[indexB].transform.localPosition, m_data.SwapAnimationTime);

                BezierMovement itemB = m_itemList[indexB];
                //itemB.MoveTo(m_itemPostionList[indexA], m_data.SwapAnimationTime);
                itemB.MoveTo(m_itemList[indexA].transform.localPosition, m_data.SwapAnimationTime);

                m_itemsToSwapSimultaneously = 2;

                // Before start movement, swap items
                BezierMovement auxObj = m_itemList[indexB];
                m_itemList[indexB] = m_itemList[indexA];
                m_itemList[indexA] = auxObj;
            }
        }

        public void Shuffle<T>(ref T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int idx = Random.Range(i, array.Length);
                //swap elements
                T tmp = array[i];
                array[i] = array[idx];
                array[idx] = tmp;
            }
        }

        public void OnBezierAnimationCompleted(BezierMovement a_obj)
        {
            m_itemsToSwapSimultaneously -= 1;

            // Check if all items have been swapped
            if (m_itemsToSwapSimultaneously <= 0)
            {
                // New random swap until all swaps have finished
                m_currentMaximunSwaps--;
                if (m_currentMaximunSwaps > 0)
                {
                    RandomizeItems();
                }
            }
        }
    }
}
