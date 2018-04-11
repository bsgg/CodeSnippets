using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Utility
{
    public class ScrollUI : MonoBehaviour
    {
        public delegate void ItemPressAction(int indexButton);
        public ItemPressAction OnItemPress;

        [Header("Prefab Item")]
        [SerializeField] protected GameObject m_ItemScrollPrefab;

        [Header("Prefab Item")]
        [SerializeField]  protected RectTransform m_ContentRecTransform;

        [Header("Grid content layout")]
        [SerializeField]  protected GridLayoutGroup m_GridContent;
        [SerializeField] protected int m_TopPadding = 25;
        [SerializeField] protected int m_SidePadding = 25;

        [SerializeField]
        protected ScrollRect m_ScrollObject;

        /// <summary>
        /// List of objects in content panel
        /// </summary>
        protected List<GameObject> m_ListElements;

        public List<GameObject> ListElements
        {
            get { return m_ListElements; }
        }

        /// <summary>
	    /// Method to init menu
	    /// </summary>
	    /// <param name="data">Data to fill the scroll</param>
        public IEnumerator InitScroll(List<string> data)
        {
            // Clear list elements
            ClearListElements();
            m_ListElements = new List<GameObject>();


            m_GridContent.cellSize = new Vector2(m_ContentRecTransform.rect.size.x - m_SidePadding, m_GridContent.cellSize.y);
            m_GridContent.padding.top = m_TopPadding;

            yield return new WaitForEndOfFrame();

            // Update height of the content rect transfrom      
            float hContent = (m_GridContent.cellSize.y * data.Count) + (m_GridContent.spacing.y * (data.Count - 1)) + m_GridContent.padding.top + m_GridContent.padding.bottom;
            m_ContentRecTransform.sizeDelta = new Vector2(m_ContentRecTransform.sizeDelta.x, hContent);

            yield return new WaitForEndOfFrame();

            for (int i = 0; i < data.Count; i++)
            {
                GameObject element = Instantiate(m_ItemScrollPrefab) as GameObject;
                element.name = "Item_" + i.ToString();
                m_ListElements.Add(element);
                element.transform.parent = m_ContentRecTransform.transform;
                element.transform.localScale = new Vector3(1, 1, 1);

                TextButton buttonText = element.GetComponent<TextButton>();
                if (buttonText != null)
                {
                    buttonText.Set(data[i], i, ButtonText_OnButtonClicked);
                }
                            
            }

            yield return new WaitForEndOfFrame();
        }

        private void ButtonText_OnButtonClicked(TextButton button)
        {
            if (OnItemPress != null)
            {
                OnItemPress(button.IdButton);
            }
        }

        protected void ClearListElements()
        {
            if (m_ListElements != null)
            {
                for (int i = 0; i < m_ListElements.Count; i++)
                {
                    Destroy(m_ListElements[i]);
                }
            }
        }

        public void ResetPosition()
        {
            m_ScrollObject.verticalNormalizedPosition = 1.0f;
        }        

        /*private void OnItemButtonPress(int id)
        {
            if (OnItemPress != null)
            {
                OnItemPress(id);
            }
        }*/
    }
}
