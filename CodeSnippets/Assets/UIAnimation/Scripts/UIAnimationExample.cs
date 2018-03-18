using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Utility
{
    public class UIAnimationExample : MonoBehaviour
    {
        [Header("Obj")]
        [SerializeField] private RectTransform          m_RootTransform;

        void Start()
        {
            float startScale = 0.0f;
            float endScale = 1.0f;
            StartCoroutine(UIAnimation.EnumeratorString(
                this,
                UIAnimation.ResizeObjByScale(m_RootTransform, startScale, endScale, 0.4f),
                UIAnimation.ResizeObjByScale(m_RootTransform, endScale, 0.6f, 0.4f)
                )
            );

        }        
    }
}
