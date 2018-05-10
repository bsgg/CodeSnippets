using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourPart : MonoBehaviour
{
    [SerializeField] private Camera m_SceneCamera;

    //Image m_ImagePart;

    private void Start()
    {
       // m_ImagePart = GetComponent<Image>();
    }

    /*public void OnPartPress()
    {
        Debug.Log("OnPartPress " + this.name);

        m_ImagePart.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0, 1.0f), 1.0f);
    }*/

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = m_SceneCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Hit Collider: " + hit.transform.name);
                SpriteRenderer sprite = hit.transform.GetComponent<SpriteRenderer>();

                if (sprite != null)
                {
                    Debug.Log("ON HIT SPRITE: " + sprite.name);
                    sprite.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0, 1.0f), 1.0f);
                }
            }
            else
            {
                Debug.Log("No colliders hit from mouse click");
            }
        }



        //if (Input.GetMouseButton(0))
        //{

            



            /*Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);

            Debug.Log("MOSUE: " + Input.mousePosition);

            Ray cursorRay = m_SceneCamera.ScreenPointToRay(cursorPos);



            RaycastHit hit;
            if (Physics.Raycast(cursorRay, out hit, 1000.0f))
            {
                Debug.Log("ON HIT SPRITE: " + hit.collider.name);
                if (hit.collider.tag == "Canvas")
                {
                    SpriteRenderer sprite = hit.transform.GetComponent<SpriteRenderer>();

                    if (sprite != null)
                    {
                        Debug.Log("ON HIT SPRITE: " + sprite.name);
                        sprite.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0, 1.0f), 1.0f);
                    }
                }
            }*/

        //}
    }


    /*void onClick()
    {
        Debug.Log("ON HIT SPRITE: " + gameObject.name);

        /*Vector2 p = Camera.main.ScreenToWorldPoint(pos);
        int hitNum = Physics2D.OverlapPointNonAlloc(p, results, 1 << Layer);
        if (hitNum > 0)
        {
            // Should be A, assuming the camera's z is -10
            Collider2D hit = results[0];
        }*/
   /* }*/

    /*void OnMouseDown()
    {
        Debug.Log("Clicked the Collider!" + gameObject.name);
    }*/
}
