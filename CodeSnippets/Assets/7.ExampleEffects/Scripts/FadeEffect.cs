using UnityEngine;
using System.Collections;

namespace MisCode
{
	public class CanvasFadeEffect : MonoBehaviour
	{
		public delegate void DelFadeEnd();
		public static event DelFadeEnd OnFadeInEnd;
		public static event DelFadeEnd OnFadeOutEnd;

		public enum FADETYPE { FADEIN, FADEOUT };

		[Header("Fade Object")]
		[SerializeField]
		private CanvasGroup m_FadeGroup;

		[Header("Fade Settings")]
		[SerializeField]
		private float m_AlphaSpeed = 1.0f;
		[SerializeField]
		private FADETYPE m_TFade = FADETYPE.FADEIN;
		[SerializeField]
		private bool m_DoOnStart = false;

		void Start()
		{
			if (m_DoOnStart)
			{
				if (m_TFade == FADETYPE.FADEIN)
				{
					DoFadeIn();

				}
				else if (m_TFade == FADETYPE.FADEOUT)
				{
					DoFadeOut();
				}
			}
		}

		public void DoFadeIn()
		{
			StartCoroutine(RoutineFadeIn());
		}

		public void DoFadeOut()
		{
			StartCoroutine(RoutineFadeOut());
		}

		private IEnumerator RoutineFadeIn()
		{
			float alpha = 0.0f;
			alpha = 0.0f;
			m_FadeGroup.alpha = alpha;
			while (alpha < 1.0f)
			{
				alpha += (m_AlphaSpeed * Time.deltaTime);
				m_FadeGroup.alpha = alpha;
				yield return new WaitForEndOfFrame();
			}
			// End alpha and invoque 
			alpha = 1.0f;
			m_FadeGroup.alpha = alpha;
			if (OnFadeInEnd != null)
			{
				OnFadeInEnd();
			}
		}

		private IEnumerator RoutineFadeOut()
		{
			float alpha = 1.0f;
			alpha = 1.0f;
			m_FadeGroup.alpha = alpha;
			while (alpha > 0.0f)
			{
				alpha -= (m_AlphaSpeed * Time.deltaTime);
				m_FadeGroup.alpha = alpha;
				yield return new WaitForEndOfFrame();
			}
			alpha = 0.0f;
			m_FadeGroup.alpha = alpha;
			if (OnFadeOutEnd != null)
			{
				OnFadeOutEnd();
			}
		}
	}
}
