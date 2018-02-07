using UnityEngine;
using System.Collections;

namespace Effects
{
    public class Effect : MonoBehaviour
    {
        [Header("Effect at Start")]

        [SerializeField]  protected float m_DelayToStart;
        [SerializeField]  private bool m_EffectAtStart = false;
        protected bool m_UpdateEffect = false;

        void Awake()
        {
            DoAwake();
        }

        protected virtual void DoAwake()
        { }

        void Start()
        {
            DoStart();            
        }

        protected virtual void DoStart()
        {
            m_UpdateEffect = false;
            if (m_EffectAtStart)
            {
                StartCoroutine(WaitToStart());
            }
        }

        private IEnumerator WaitToStart()
        {
            yield return new WaitForSeconds(m_DelayToStart);
            m_UpdateEffect = true;
            DoEffect();
        }

        void Update()
        {
            if (m_UpdateEffect)
            {
                DoUpdate();
            }
        }
        

        public virtual void DoEffect()
        {}
        
        protected virtual void DoUpdate() {}

    }

}
