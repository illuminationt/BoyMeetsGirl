using UnityEngine;



[ExecuteInEditMode]

public class CameraScript : MonoBehaviour

{

    [SerializeField]private  Material mat;



    void OnRenderImage(RenderTexture source, RenderTexture destination)

    {

        Graphics.Blit(source, destination, mat);

    }

}