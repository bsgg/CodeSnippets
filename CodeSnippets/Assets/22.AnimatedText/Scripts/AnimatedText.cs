using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SnippetsCode.AnimatedDialogText
{
    public class AnimatedText : MonoBehaviour
    {
        public delegate void AnimatedTextCompleted();
        public static event AnimatedTextCompleted OnAnimatedTextCompleted;

        [SerializeField] private Text displayedText;
        [SerializeField] private float delayBettweenWord = 0.1f;            
               
        private string textToAnimate;
        private string currentText;        

        private string[] splitMessage;
        private int indexWord = 0;
        private string previousWord;

        public void AnimateText(float startDelay, string text)
        {
            textToAnimate = text;

            displayedText.text = "";

            splitMessage = text.Split(' ');

            StartCoroutine(StartAnimation(startDelay));
        }

        private IEnumerator StartAnimation(float startDelay)
        {
            yield return new WaitForSeconds(startDelay);

            indexWord = 0;
            previousWord = splitMessage[indexWord]; // Save previous word with no style   
            currentText = "";

            displayedText.text = "<i><color=orange>" + splitMessage[indexWord] + "</color></i>";

            indexWord += 1;
            while (indexWord < splitMessage.Length)
            {
                yield return new WaitForSeconds(delayBettweenWord);
                // Add to current text previous word (no style) + next one with style
                currentText += previousWord + " ";
                displayedText.text = currentText + "<i><color=orange>" + splitMessage[indexWord] + "</color></i>";

                // Save previous word with no style and add 1
                previousWord = splitMessage[indexWord];
                indexWord += 1;
                
            }

            yield return new WaitForSeconds(delayBettweenWord);
            displayedText.text = textToAnimate;

            if (OnAnimatedTextCompleted != null)
            {
                OnAnimatedTextCompleted();
            }
        }
        
    }
}
