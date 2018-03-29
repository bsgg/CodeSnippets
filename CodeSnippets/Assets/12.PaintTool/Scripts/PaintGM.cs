using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGM : MonoBehaviour {

    public Transform m_BaseDot;

    [SerializeField] private KeyCode m_MouseLeft;

    [SerializeField] private Camera m_PaintCamera;

    public static bool m_Erasing = false;
    public static bool m_Paint = false;

    void Update ()
    {
        if (m_Paint)
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPostion = m_PaintCamera.ScreenToWorldPoint(mousePos);

            if (Input.GetKey(m_MouseLeft))
            {
                Instantiate(m_BaseDot, objPostion, m_BaseDot.rotation);
            }
        }
		
	}

    public void Erase()
    {
        m_Erasing = true;
        m_Paint = false;
    }

    public void Paint()
    {
        m_Paint = true;
        m_Erasing = false;
    }
}
