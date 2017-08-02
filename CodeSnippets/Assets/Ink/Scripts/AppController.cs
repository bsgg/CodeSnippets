using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

namespace InkExample
{ 
    public class AppController : MonoBehaviour
    {
       [SerializeField] private InkExample m_InkExample;

        [SerializeField]
        private TextAsset m_InkJSONAsset;
        private Story m_Story;

        private bool m_RestartStory = false;


        private void Start()
        {
            StartStory();
        }

        private void StartStory()
        {
            m_Story = new Story(m_InkJSONAsset.text);
            m_RestartStory = false;
            UpdateStory();
        }

        public void UpdateStory()
        {
            m_InkExample.Description = "";
            // While the story continue (there are text and not options)
            while (m_Story.canContinue)
            {
                string text = m_Story.Continue().Trim();
                m_InkExample.Description += text + "\n";
            }

            m_InkExample.DisableAllChoices();
            // Check current choices
            if (m_Story.currentChoices.Count > 0)
            {
                for (int i = 0; i < m_Story.currentChoices.Count; i++)
                {
                    m_InkExample.ActiveChoice(i,true);

                    Choice choice = m_Story.currentChoices[i];

                    m_InkExample.SetChoice(i, choice.text.Trim());
                }
            }
            else
            {
                m_InkExample.SetChoice(0, "Restart story?");
                m_RestartStory = true;
            }
        }


        public void OnChoiceSelected(int index)
        {
            if (!m_RestartStory)
            {
                m_Story.ChooseChoiceIndex(index);
                UpdateStory();
            }
            else
            {
                StartStory();
            }
        }


    }
}
