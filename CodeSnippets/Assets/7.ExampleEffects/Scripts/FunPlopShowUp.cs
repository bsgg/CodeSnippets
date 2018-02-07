using UnityEngine;
using System.Collections;
namespace Misc
{
    public class FunPlopShowUp : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(PlopLerp());
        }


        private IEnumerator PlopLerp()
        {
           // transform.localScale = Vector3.zero;

            Vector3 startPosition = transform.localPosition;
            //transform.localScale = Vector3.zero;

            float lerp = 0.0f;
            while (lerp < 5.0f)
            {
                yield return new WaitForEndOfFrame();

                lerp += (Time.deltaTime / 0.4f);
                transform.localPosition = startPosition + ((Vector3.up * 15.0f)/* * Mathf.Sin(lerp * 3.0f)*/);
               // transform.localScale = Vector3.one * lerp;
            }

            transform.localPosition = startPosition;
            //transform.localScale = Vector3.one;
        }

    }
}
