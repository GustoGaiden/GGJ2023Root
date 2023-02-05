using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MusicOffText: MonoBehaviour
{
    private Text textComp;

    private string text = "Press M to toggle Music on/off!";

    private void Start()
    {
        textComp = gameObject.GetComponent<Text>();
        textComp.text = text;
    }

    public void HideMusicOffText()
    {
        gameObject.SetActive(false);
    }

    public void ShowMusicOffText()
    {
        gameObject.SetActive(true);
    }
}



