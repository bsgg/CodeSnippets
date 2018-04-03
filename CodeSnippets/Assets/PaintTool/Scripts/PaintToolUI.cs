using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utility.PaintTool
{
    public class PaintToolUI : MonoBehaviour
    {
       

        void Update()
        {
           /* Vector2 localPoint = Vector2.zero;

            screenPoint = Input.mousePosition;
            

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, cam, out localPoint);


            rectTransform.GetLocalCorners(localCorners);
            imgWidth = localCorners[3].x - localCorners[0].x;
            imgHeight = localCorners[1].y - localCorners[0].y;

            Vector2 positionNormalizedForTexCoords;
            positionNormalizedForTexCoords.x = localPoint.x / imgWidth + 0.5f;
            positionNormalizedForTexCoords.y = localPoint.y / imgHeight + 0.5f;

            if (Input.GetMouseButton(0))
            {
                if (positionNormalizedForTexCoords.x >= 0 && positionNormalizedForTexCoords.x <= 1 &&
                    positionNormalizedForTexCoords.y >= 0 && positionNormalizedForTexCoords.y <= 1)
                {
                    pickedColor = tex.GetPixelBilinear(positionNormalizedForTexCoords.x, positionNormalizedForTexCoords.y);
                    //print(positionNormalizedForTexCoords + "   pickedColor " + pickedColor);
                    //UpdatePickedColorGameObject(pickedColor);
                }
            }*/

        }

        /*void UpdatePickedColorGameObject(Color color)
        {
            Image img = pickedColorGameObject.GetComponent<Image>();
            img.color = color;
        }*/

    }
}
