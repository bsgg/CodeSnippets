using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class ScrollInitializer : MonoBehaviour {

    [SerializeField] private UIBase m_ScrollViewParent;
    [SerializeField] private ScrollUI m_ScrollView;
    [SerializeField] private int m_ItemNumberInScroll = 30;

    [SerializeField] private Text m_DebugText;

    

    private void Start()
    {
        List<string> elementList = new List<string>();

        for (int i = 0; i < m_ItemNumberInScroll; i++)
        {
            string title = "Scroll Item " + (i + 1);
            elementList.Add(title);
        }

        StartCoroutine(m_ScrollView.InitScroll(elementList));
        m_ScrollView.OnItemPress += OnScrollItemPress;

    }

    public void OnScrollItemPress(int id)
    {
        m_ScrollViewParent.Hide();
        if ((m_ScrollView.ListElements != null) && (id < m_ScrollView.ListElements.Count))
        {
            m_DebugText.text = m_ScrollView.ListElements[id].GetComponent<TextButton>().Title;
        }
            
    }
}
