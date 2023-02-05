using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class FlavorText: MonoBehaviour
{
    private Text textComp;

    private string text = "It's the year 2024. The alien biomass Florbrbps has landed on earth and \n"
        + "now starts diggings its roots into the planet to consume its juicy resources. Yums!";

    private void Start()
    {
        textComp = gameObject.GetComponent<Text>();
        textComp.text = text;
    }

    public void HideFlavorText()
    {
        gameObject.SetActive(false);
    }

    public void ShowFlavorText()
    {
        gameObject.SetActive(true);
    }
}



