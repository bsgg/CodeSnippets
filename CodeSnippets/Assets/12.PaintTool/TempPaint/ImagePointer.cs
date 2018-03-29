using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImagePointer : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Texture2D texture;

    private void Start()
    {
        texture = GetComponent<Image>().sprite.texture;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localCursor;
        var rect1 = GetComponent<RectTransform>();
        var pos1 = eventData.position;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect1, pos1,
            null, out localCursor))
            return;

        int xpos = (int)(localCursor.x);
        int ypos = (int)(localCursor.y);

        if (xpos < 0) xpos = xpos + (int)rect1.rect.width / 2;
        else xpos += (int)rect1.rect.width / 2;

        if (ypos > 0) ypos = ypos + (int)rect1.rect.height / 2;
        else ypos += (int)rect1.rect.height / 2;

        Debug.Log("Correct Cursor Pos: " + xpos + " " + ypos);

        int xCoord = (xpos * texture.width);
        int yCoord = (ypos * texture.height);

        Debug.Log("Coords: " + xCoord + " " + yCoord);

        texture.SetPixel(xCoord, yCoord, Color.red);
        texture.Apply();

        //Vector2 position;
        //GetPositionOnImage01(GetComponent<Image>(), localCursor, out position);

        //Debug.Log("GetPositionOnImage01 Pos: " + position.x + " " + position.y);
    }


    // Take an image and a screenPosition. Return the ptLocationRelativeToImage01 relative to the image (where x,y between 0.0 and 1.0 are on the image, values below 0.0 or above 1.0 are outside the image).
    public bool GetPositionOnImage01(
        UnityEngine.UI.Image uiImageObject,
        Vector2 screenPosition,
        out Vector2 ptLocationRelativeToImage01)
    {
        ptLocationRelativeToImage01 = new Vector2();
        RectTransform uiImageObjectRect = uiImageObject.GetComponent<RectTransform>();
        Vector2 localCursor = new Vector2();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiImageObjectRect,
            screenPosition,
            null,
            out localCursor))
        {
            Vector2 ptPivotCancelledLocation = new Vector2(localCursor.x - uiImageObjectRect.rect.x, localCursor.y - uiImageObjectRect.rect.y);
            Vector2 ptLocationRelativeToImageInScreenCoordinates = new Vector2();
            // How do we get the location of the image? Calculate the size of the image, then use the pivot information.
            if (uiImageObject.preserveAspect)
            {
                // If we are preserving the aspect ratio of the image, then we need to do some additional calculations
                // Figure out if the image constrained by height or by width.
                float fImageAspectRatio = uiImageObject.sprite.rect.height / uiImageObject.sprite.rect.width;
                float fRectAspectRatio = uiImageObjectRect.rect.height / uiImageObjectRect.rect.width;
                Rect imageRectInLocalScreenCoordinates = new Rect();
                if (fImageAspectRatio > fRectAspectRatio)
                {
                    // The image is constrained by its height.
                    float fImageWidth = (fRectAspectRatio / fImageAspectRatio) * uiImageObjectRect.rect.width;
                    float fExcessWidth = uiImageObjectRect.rect.width - fImageWidth;
                    imageRectInLocalScreenCoordinates.Set(uiImageObjectRect.pivot.x * fExcessWidth, 0, uiImageObjectRect.rect.height / fImageAspectRatio, uiImageObjectRect.rect.height);
                }
                else
                {
                    // The image is constrained by its width.
                    float fImageHeight = (fImageAspectRatio / fRectAspectRatio) * uiImageObjectRect.rect.height;
                    float fExcessHeight = uiImageObjectRect.rect.height - fImageHeight;
                    imageRectInLocalScreenCoordinates.Set(0, uiImageObjectRect.pivot.y * fExcessHeight, uiImageObjectRect.rect.width, fImageAspectRatio * uiImageObjectRect.rect.width);
                }
                ptLocationRelativeToImageInScreenCoordinates.Set(ptPivotCancelledLocation.x - imageRectInLocalScreenCoordinates.x, ptPivotCancelledLocation.y - imageRectInLocalScreenCoordinates.y);
                ptLocationRelativeToImage01.Set(ptLocationRelativeToImageInScreenCoordinates.x / imageRectInLocalScreenCoordinates.width, ptLocationRelativeToImageInScreenCoordinates.y / imageRectInLocalScreenCoordinates.height);
            }
            else
            {
                ptLocationRelativeToImageInScreenCoordinates.Set(ptPivotCancelledLocation.x, ptPivotCancelledLocation.y);
                ptLocationRelativeToImage01.Set(ptLocationRelativeToImageInScreenCoordinates.x / uiImageObjectRect.rect.width, ptLocationRelativeToImageInScreenCoordinates.y / uiImageObjectRect.rect.height);
            }
            return true;
        }
        return false;
    }

}
