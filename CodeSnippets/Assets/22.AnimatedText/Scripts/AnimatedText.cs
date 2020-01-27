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
        [SerializeField] private float delayBettweenLetters = 0.1f;            

        private bool updateAnimation = false;

        private string textToAnimate;
        private string currentText;

        private float currentDeltaTime;
        private int indexLetter;        

        public void AnimateText(float startDelay, string text)
        {
            textToAnimate = text;
            indexLetter = 0;
            currentText = "";

            displayedText.text = currentText;
            StartCoroutine(StartAnimation(startDelay));
        }

        private IEnumerator StartAnimation(float startDelay)
        {
            yield return new WaitForSeconds(startDelay);

            // Set first letter
            if (indexLetter < textToAnimate.Length)
            {
                currentText += textToAnimate[indexLetter];

                displayedText.text = currentText; 
            }

            currentDeltaTime = 0.0f;
            updateAnimation = true;
        }
       
        void Update()
        {
            if (!updateAnimation) return;

            currentDeltaTime += Time.deltaTime;
            if (currentDeltaTime >= delayBettweenLetters)
            {
                indexLetter += 1;
                currentDeltaTime = 0.0f;
                if (indexLetter < textToAnimate.Length)
                {
                    currentText += textToAnimate[indexLetter];

                    displayedText.text = currentText;
                }else
                {
                    displayedText.text = textToAnimate;

                    if (OnAnimatedTextCompleted != null)
                    {
                        OnAnimatedTextCompleted();
                    }
                }
            }
        }
    }
}
