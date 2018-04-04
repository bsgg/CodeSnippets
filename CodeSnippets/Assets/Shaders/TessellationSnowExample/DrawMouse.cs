using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMouse : MonoBehaviour
{
    public Camera _camera;
    public Shader _drawShader;

    private RenderTexture _splatmap;
    private Material _snowMaterial;
    private Material _drawMaterial;

    [Range(1,500)]
    public float _brushSize;
    [Range(0, 1)]
    public float _brushStrength;

    private RaycastHit _hit;

	void Start ()
    {
        _drawMaterial = new Material(_drawShader);
        _drawMaterial.SetVector("_Color", Color.red);

        _snowMaterial = GetComponent<MeshRenderer>().material;
        _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _snowMaterial.SetTexture("_Splat", _splatmap);
    }

	void Update ()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            

            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                //Debug.Log("TextureCoord (" + _hit.textureCoord.x + "," + _hit.textureCoord.y + ")");
               // Debug.Log("Input.mousePosition (" + Input.mousePosition.x + "," + Input.mousePosition.y + ")");

                // Send coordinates to the material where the input was donw
                _drawMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y));
                _drawMaterial.SetFloat("_Strength", _brushStrength);
                _drawMaterial.SetFloat("_Size", _brushSize);

                RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatmap, temp);
                // use draw material only when we click on a certain position on a mesh
                Graphics.Blit(temp, _splatmap, _drawMaterial);

                // Release temporary render texture
                RenderTexture.ReleaseTemporary(temp);
            }
        }
		
	}

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatmap, ScaleMode.ScaleToFit, false, 1);
    }
}
