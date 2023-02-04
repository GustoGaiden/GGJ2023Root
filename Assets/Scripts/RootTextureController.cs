using System;
using UnityEngine;

public class RootTextureController : MonoBehaviour
{
    //public Sprite[] textures;

    //public Texture2D atlas;    // Just to see on editor nothing to add from editor
    //public Material testMaterial;
    //public SpriteRenderer testSpriteRenderer;

    //int textureWidthCounter = 0;
    //int width,height;
    private void Start () {
        // width = 0;
        // height = 0;
        //
        // foreach(var  t in textures) {
        //     width += t.texture.width;
        //
        //     if (t.texture.height > height)
        //         height = t.texture.height;
        // }
        //
        // atlas = new Texture2D(width,height, TextureFormat.RGBA32,false);
        //
        // for (int i = 0; i < textures.Length; i++)
        // {
        //     int y = 0;
        //
        //     while (y < atlas.height) {
        //         int x = 0;
        //
        //         while (x < textures[i].texture.width ) {
        //             if (y < textures[i].texture.height) 
        //                 atlas.SetPixel(x + textureWidthCounter, y, textures[i].texture.GetPixel(x, y));  // Fill your texture
        //             else atlas.SetPixel(x + textureWidthCounter, y,new Color(0f,0f,0f,0f));  // Add transparency
        //             x++;
        //         }
        //         y++;
        //     }
        //     atlas.Apply();
        //     textureWidthCounter +=  textures[i].texture.width;
        // }
        //
        // // For normal renderers
        // if (testMaterial != null)
        //     testMaterial.mainTexture = atlas;
        //
        // // for sprite renderer just make  a sprite from texture
        // var s = Sprite.Create(atlas, new Rect(0f, 0f, atlas.width, atlas.height), new Vector2(0.5f, 0.5f));
        // testSpriteRenderer.sprite = s;
        //
        // // add your polygon collider
        // testSpriteRenderer.gameObject.AddComponent<PolygonCollider2D>();
    }

}
