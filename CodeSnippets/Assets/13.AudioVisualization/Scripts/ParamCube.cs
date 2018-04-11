using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour {

    [SerializeField] private int m_Band;
    [SerializeField] private float m_StartScale;
    [SerializeField] private float m_ScaleMultiplier;

	void Update ()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
            (AudioPeer.m_FrequenceBands[m_Band] * m_ScaleMultiplier) + m_StartScale,
            transform.localScale.z);
		
	}
}
