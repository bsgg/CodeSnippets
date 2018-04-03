﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utility.PaintTool
{
    public class ColorPicker : MonoBehaviour, IPointerClickHandler
    {
        public RectTransform rectTransform;

        public float imgWidth, imgHeight;
        public Vector3[] localCorners = new Vector3[4];

        public Texture2D tex;

        public Image image;
        public Color pickedColor;

        private void Start()
        {
            StartCoroutine(Init());

        }

        private IEnumerator Init()
        {
            yield return new WaitForEndOfFrame();

            tex = image.sprite.texture;

            localCorners = new Vector3[4];
            rectTransform.GetLocalCorners(localCorners);
            imgWidth = localCorners[3].x - localCorners[0].x;
            imgHeight = localCorners[1].y - localCorners[0].y;
        }

        public Vector2 localPoint;
        public Vector2 positionNormalizedForTexCoords;
        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 pos1 = eventData.position;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pos1, null, out localPoint)) return;

            /*int xpos = (int)(localPoint.x);
            int ypos = (int)(localPoint.y);

            if (xpos < 0) xpos = xpos + (int)rectTransform.rect.width / 2;
            else xpos += (int)rectTransform.rect.width / 2;

            if (ypos > 0) ypos = ypos + (int)rectTransform.rect.height / 2;
            else ypos += (int)rectTransform.rect.height / 2;*/


            //positionNormalizedForTexCoords.x = xpos / imgWidth + 0.5f;
            //positionNormalizedForTexCoords.y = ypos / imgHeight + 0.5f;


            positionNormalizedForTexCoords.x = localPoint.x / imgWidth + 0.5f;
            positionNormalizedForTexCoords.y = localPoint.y / imgHeight + 0.5f;

            // Aspect ratio

            if (positionNormalizedForTexCoords.x >= 0 && positionNormalizedForTexCoords.x <= 1 &&
                    positionNormalizedForTexCoords.y >= 0 && positionNormalizedForTexCoords.y <= 1)
            {
                pickedColor = tex.GetPixelBilinear(positionNormalizedForTexCoords.x, positionNormalizedForTexCoords.y);

            }

        }

    }
}
