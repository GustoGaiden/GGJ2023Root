using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    private Texture2D texture;
    private int resolution = 1000;

    public Material mat;

    public Shader shader;

    private void Awake()
    {
        texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
        texture.name = "Procedural Texture";
        FillTexture();

        GetComponent<SpriteRenderer>().material.mainTexture = texture;
        mat = new Material(shader: shader);
        mat.mainTexture = texture;
        GetComponent<SpriteRenderer>().material = mat;


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FillTexture()
    {
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                texture.SetPixel(x, y, Color.green);
            }
        }
        texture.Apply();
    }
}
