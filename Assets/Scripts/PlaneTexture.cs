using UnityEngine;

public class PlaneTexture : MonoBehaviour
{
    public Renderer textureRenderer;
    public void DrawTexture(Texture2D texture) 
    {      

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height); 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
