using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SnippetsCode
{
    public class NormalizeRangeExample : MonoBehaviour
    {
        [SerializeField] private Image circleTimer;

        [SerializeField] private Text circleText;

        [SerializeField] private float startTime = 30.0f;

        private float currentTime;

        private bool updateTime;

        private void Start()
        {
            currentTime = startTime;
            circleTimer.fillAmount = 1.0f;
            circleText.text = (int)currentTime + "s";

            updateTime = true;
        }

        private void Update()
        {
            if (updateTime)
            {
                currentTime -= Time.deltaTime;

                if (currentTime <= 0.0f)
                {
                    updateTime = false;
                    currentTime = 0.0f;
                }

                circleText.text = (int)currentTime + "s";

                float normalizedValue = Mathf.Clamp(currentTime / startTime, 0.0f, 1.0f);
                
                circleTimer.fillAmount = normalizedValue;

            }
        }
    }
}
