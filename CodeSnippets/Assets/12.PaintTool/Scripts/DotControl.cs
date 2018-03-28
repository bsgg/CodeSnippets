using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotControl : MonoBehaviour
{

    private void OnMouseOver()
    {
        if (PaintGM.m_Erasing)
        {
            Destroy(gameObject);
        }
    }
}
