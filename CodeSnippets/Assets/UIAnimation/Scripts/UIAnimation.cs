using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MiscCode
{
    public class UIAnimation
    {
        public static IEnumerator EnumeratorString(MonoBehaviour objectToString, params IEnumerator[] actions)
        {
            for (int loop = 0; loop < actions.Length; loop++)
            {
                yield return objectToString.StartCoroutine(actions[loop]);
            }
        }

        #region Transformations

        public static IEnumerator TranslateObj(RectTransform objToMove,
            RectTransform gotoPoint,
            float duration, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            float lerp = 0f;
            Vector2 startP = objToMove.anchoredPosition;

            while (lerp < 1f)
            {
                lerp += Time.deltaTime/duration;
                objToMove.anchoredPosition = Vector2.Lerp(startP, gotoPoint.anchoredPosition, lerp);

                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator TranslateObj(RectTransform objToMove,
            float x, float y,
            float duration, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            float lerp = 0f;
            Vector2 startP = objToMove.anchoredPosition;
            Vector2 gotoPoint = new Vector2(x, y);

            while (lerp < 1f)
            {
                lerp += Time.deltaTime/duration;
                objToMove.anchoredPosition = Vector2.Lerp(startP, gotoPoint, lerp);

                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator ResizeObjToDefined(RectTransform objToEdit,
            float gotoHeight, float gotoWidth,
            float duration, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            float lerp = 0f;
            Vector2 start = objToEdit.sizeDelta;
            Vector2 end = new Vector2(gotoWidth, gotoHeight);

            while (lerp < 1f)
            {
                lerp += Time.deltaTime/duration;
                objToEdit.sizeDelta = Vector2.Lerp(start, end, lerp);

                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator ResizeObjByScale(RectTransform objToEdit,
            float startScale, float gotoScale,
            float duration, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            float lerp = 0f;
            Vector2 start = new Vector2(startScale, startScale);
            Vector2 end = new Vector2(gotoScale, gotoScale);

            while (lerp < 1f)
            {
                lerp += Time.deltaTime/duration;
                objToEdit.localScale = Vector2.Lerp(start, end, lerp);

                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator RotateObj(RectTransform objToRotate,
            Quaternion startRot, Quaternion gotoRot,
            float duration, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            float lerp = 0f;

            while (lerp < 1f)
            {
                lerp += Time.deltaTime/duration;
                objToRotate.rotation = Quaternion.Slerp(startRot, gotoRot, duration);

                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator RotateObj(RectTransform objToRotate,
            Vector3 startRot, Vector3 gotoRot,
            float duration, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            float lerp = 0f;
            Quaternion startQuaternion = Quaternion.Euler(startRot);
            Quaternion gotoQuaternion = Quaternion.Euler(gotoRot);

            while (lerp < 1f)
            {
                lerp += Time.deltaTime/duration;
                objToRotate.rotation = Quaternion.Slerp(startQuaternion, gotoQuaternion, duration);

                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator RotateObjForever(RectTransform objToRotate,
            float rotSpeed, Vector3 axis,
            float delay = 0f)
        {
            yield return new WaitForSeconds(delay);
            while (true)
            {
                objToRotate.Rotate(axis, rotSpeed*Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }
        }

        #endregion

        #region Fading

        public static IEnumerator FadeImage(Image objToFade,
            float startAlpha, float gotoAlpha,
            float duration, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            float lerp = 0f;

            Color start = objToFade.color;
            start.a = startAlpha;

            Color end = objToFade.color;
            end.a = gotoAlpha;

            while (lerp < 1f)
            {
                lerp += Time.deltaTime/duration;
                objToFade.color = Color.Lerp(start, end, lerp);

                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator FadeImageToDeactivate(Image objToFade, GameObject objToSetActive,
            float startAlpha, float gotoAlpha,
            float duration, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            float lerp = 0f;

            Color start = objToFade.color;
            start.a = startAlpha;

            Color end = objToFade.color;
            end.a = gotoAlpha;

            while (lerp < 1f)
            {
                lerp += Time.deltaTime/duration;
                objToFade.color = Color.Lerp(start, end, lerp);

                yield return new WaitForEndOfFrame();
            }

            objToSetActive.SetActive(false);
        }

        public static IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float length, GameObject objToDisable = null, bool disable = false)
        {
            float lerp = 0.0f;
            while (lerp < 1.0f)
            {
                lerp += Time.deltaTime;
                group.alpha = Mathf.Lerp(start, end, lerp);

                yield return new WaitForEndOfFrame();
            }

            if (disable && objToDisable)
                objToDisable.SetActive(false);
        }

        #endregion

        #region UIFeel

        public static IEnumerator TranslateOscillateShake(RectTransform objToShake,
            float horPower, float verPower,
            float shakeSpeed, float length,
            bool dropOff = true, float delay = 0f)
        {
            bool dampHor = (horPower > 0f);
            bool dampVer = (verPower > 0f);

            yield return new WaitForSeconds(delay);

            Vector2 startingPoint = objToShake.anchoredPosition;
            Vector2 newPoint = objToShake.anchoredPosition;

            float xShakePowerDamp = 0.0f;
            float yShakePowerDamp = 0.0f;

            float lerp = 0.0f;
            float pingPong = 0f;
            while (lerp < 1.0f)
            {
                lerp += Time.deltaTime/length;
                pingPong += Time.deltaTime;
                float sine = 0.5f*(1.0f + Mathf.Sin(1.85f*Mathf.PI*Mathf.PingPong(pingPong, 1f)*shakeSpeed));

                float xShake = horPower*sine;
                float yShake = verPower*sine;

                newPoint.x = startingPoint.x + xShake;
                newPoint.y = startingPoint.y + yShake;

                objToShake.anchoredPosition = newPoint;

                if (dropOff)
                {
                    if (dampVer)
                    {
                        verPower = Mathf.SmoothDamp(verPower, 1f, ref xShakePowerDamp, length);
                    }
                    if (dampHor)
                    {
                        horPower = Mathf.SmoothDamp(horPower, 1f, ref yShakePowerDamp, length);
                    }
                }

                yield return new WaitForEndOfFrame();
            }

            lerp = 0.0f;
            while (lerp < 1.0f)
            {
                lerp += Time.deltaTime/0.2f;
                objToShake.anchoredPosition = Vector2.Lerp(objToShake.anchoredPosition, startingPoint, lerp);

                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator SizeDeltaShake(RectTransform objToShake,
            float shakePower, float endPower,
            float shakeSpeed, float length,
            bool dropOff = true, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            Vector2 baseSizeDelta = objToShake.sizeDelta;
            Vector2 gotoSizeDelta = objToShake.sizeDelta*shakePower;
            float shakePowerDamp = 0.0f;

            float lerp = 0.0f;
            while (lerp < 1.0f)
            {
                lerp += Time.deltaTime/length;
                float sine = 0.5f*(1.0f + Mathf.Sin(1.85f*Mathf.PI*lerp*shakeSpeed));

                objToShake.sizeDelta = Vector2.Lerp(baseSizeDelta, gotoSizeDelta, sine);

                if (dropOff)
                {
                    shakePower = Mathf.SmoothDamp(shakePower, endPower, ref shakePowerDamp, length);
                    gotoSizeDelta = objToShake.sizeDelta*shakePower;
                }

                yield return new WaitForEndOfFrame();
            }

            lerp = 0.0f;
            gotoSizeDelta = objToShake.sizeDelta;
            while (lerp < 1.0f)
            {
                lerp += Time.deltaTime/0.2f;
                objToShake.sizeDelta = Vector2.Lerp(gotoSizeDelta, baseSizeDelta, lerp);

                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator ScaleShake(RectTransform objToShake,
            float shakePower, float endPower,
            float shakeSpeed, float length,
            bool dropOff = true, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            Vector2 baseScale = objToShake.localScale;
            Vector2 gotoScale = objToShake.localScale*shakePower;
            float shakePowerDamp = 0.0f;

            float lerp = 0.0f;
            while (lerp < 1.0f)
            {
                lerp += Time.deltaTime/length;
                float sine = 0.5f*(1.0f + Mathf.Sin(1.85f*Mathf.PI*lerp*shakeSpeed));

                objToShake.localScale = Vector2.Lerp(baseScale, gotoScale, sine);

                if (dropOff)
                {
                    shakePower = Mathf.SmoothDamp(shakePower, endPower, ref shakePowerDamp, length);
                    gotoScale = baseScale*shakePower;
                }

                yield return new WaitForEndOfFrame();
            }

            lerp = 0.0f;
            gotoScale = objToShake.localScale;
            while (lerp < 1.0f)
            {
                lerp += Time.deltaTime/0.2f;
                objToShake.localScale = Vector2.Lerp(gotoScale, baseScale, lerp);

                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator RotateShake(RectTransform objToShake,
            float shakePower, float endPower,
            float shakeSpeed, float length,
            bool dropOff = true, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            Quaternion baseRotation = objToShake.localRotation;
            float shakePowerDamp = 0.0f;

            float lerp = 0.0f;
            while (lerp < 1.0f)
            {
                lerp += Time.deltaTime/length;
                float sine = 0.5f*(1.0f + Mathf.Sin(1.85f*Mathf.PI*lerp*shakeSpeed));

                Quaternion newRotation = baseRotation;
                newRotation.z = sine*shakePower;
                objToShake.localRotation = newRotation;

                if (dropOff)
                {
                    shakePower = Mathf.SmoothDamp(shakePower, endPower, ref shakePowerDamp, length);
                }

                yield return new WaitForEndOfFrame();
            }

            lerp = 0.0f;
            Quaternion gotoRotation = objToShake.localRotation;
            while (lerp < 1.0f)
            {
                lerp += Time.deltaTime/0.2f;
                objToShake.localRotation = Quaternion.Slerp(gotoRotation, baseRotation, lerp);

                yield return new WaitForEndOfFrame();
            }
        }

        #endregion

    }
}