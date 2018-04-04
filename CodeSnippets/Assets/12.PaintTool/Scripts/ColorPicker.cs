using System.Collections;
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
        public Color pickedColor = new Color(0.5f,0.5f,0.5f,1.0f);

        public float imgWidth2, imgHeight2;

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

            imgWidth2 = tex.width;
            imgHeight2 = tex.height;



        }

        public Vector2 localPoint;
        public Vector2 positionNormalizedForTexCoords;
        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 pos1 = eventData.position;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pos1, null, out localPoint)) return;



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
