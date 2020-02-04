using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnippetsCode.AnimatedDialogText
{
    public class AnimatedTextTest : MonoBehaviour
    {
        [SerializeField] private AnimatedText animatedText;

        [SerializeField] private string TextToAnimate;

        void Start()
        {
            animatedText.AnimateText(2.0f, TextToAnimate);
            AnimatedText.OnAnimatedTextCompleted += AnimatedText_OnAnimatedTextCompleted;
        }

        private void AnimatedText_OnAnimatedTextCompleted()
        {
            AnimatedText.OnAnimatedTextCompleted -= AnimatedText_OnAnimatedTextCompleted;

            Debug.Log("AnimatedText_OnAnimatedTextCompleted");
        }
        
    }
}
