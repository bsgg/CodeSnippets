using UnityEngine;
using System.Collections;

namespace Effects
{
    /// <summary>
    /// Fade In Fade Out Material effect. The object needs a material with Fade render option 
    /// </summary>
    public class FadeMaterialEffect : Effect
    {       
        public delegate void FadeEndAction(TypeFade tFade);
        public event FadeEndAction OnFadeActionEnd;

        public enum TypeFade { NONE, FADEIN, FADEOUT };

        [Header("Alpha Effect")]
        [SerializeField] private TypeFade   m_TypeFade;
        [SerializeField] private bool       m_LoopEffect;

        [SerializeField] private float      m_RateFade;
        private MeshRenderer                m_Mesh;

        protected override void DoStart()
        {
            m_Mesh = transform.GetComponent<MeshRenderer>();

            InitializeFadeEffect(m_TypeFade);

            base.DoStart();
        }

        public override void DoEffect()
        {
            base.DoEffect();

            if (m_LoopEffect)
            {
                OnFadeActionEnd += OnFadeEnd;
            }
            if (m_TypeFade == TypeFade.FADEIN)
            {
                StartCoroutine(FadeInRoutine(m_DelayToStart));
            }

            if (m_TypeFade == TypeFade.FADEOUT)
            {
                StartCoroutine(FadeOutRoutine(m_DelayToStart));
            }

        }

        public void DoEffect(TypeFade effect, float delay, bool loop)
        {
            if (loop)
            {
                OnFadeActionEnd += OnFadeEnd;
            }
            if (m_TypeFade == TypeFade.FADEIN)
            {
                StartCoroutine(FadeInRoutine(delay));
            }

            if (m_TypeFade == TypeFade.FADEOUT)
            {
                StartCoroutine(FadeOutRoutine(delay));
            }
        }

        private void InitializeFadeEffect(TypeFade tFade)
        {
            Color color = m_Mesh.material.color;
            if (tFade == TypeFade.FADEIN)
            {
                color.a = 0.0f;

            }else if (tFade == TypeFade.FADEOUT)
            {
                color.a = 1.0f;
            }
            m_Mesh.material.color = color;
        }

        /// <summary>
        /// Routine FadeIn (0.0f to 1.0f)
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeInRoutine(float delay)
        {
            m_TypeFade = TypeFade.FADEIN;
            yield return new WaitForSeconds(delay);

            Color color = m_Mesh.material.color;
            color.a = 0.0f;
            m_Mesh.material.color = color;
            while (color.a < 1.0f)
            {
                color.a += (m_RateFade * Time.deltaTime);
                m_Mesh.material.color = color;
                yield return new WaitForEndOfFrame();
            }
            color.a = 1.0f;
            m_Mesh.material.color = color;

            if (OnFadeActionEnd != null)
            {
                OnFadeActionEnd(m_TypeFade);
            }
        }
       

        /// <summary>
        /// Routine FadeIn (0.0f to 1.0f)
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeOutRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            m_TypeFade = TypeFade.FADEOUT;            
            Color color = m_Mesh.material.color;
            color.a = 1.0f;
            m_Mesh.material.color = color;            
            while (color.a > 0.0f)
            {
                color.a -= (m_RateFade * Time.deltaTime);
                m_Mesh.material.color = color;
                yield return new WaitForEndOfFrame();
            }
            color.a = 0.0f;
            m_Mesh.material.color = color;

            if (OnFadeActionEnd != null)
            {
                OnFadeActionEnd(m_TypeFade);
            }
        }

        public void OnFadeEnd(TypeFade tFade)
        {
            if (tFade == TypeFade.FADEIN)
            {
                StartCoroutine(FadeOutRoutine(0.0f));
            }
            else if (tFade == TypeFade.FADEOUT)
            {
                StartCoroutine(FadeInRoutine(0.0f));
            }
        }
    }
}
