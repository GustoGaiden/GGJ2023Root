using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class TutorialText : MonoBehaviour
{
    private Text textComp;

    private int currentTutIndex = 0;
    private bool show;

    private List<string> TutorialTexts = new List<string>()
    {
        "Press RIGHT ARROW for TUTORIAL\nPress M to toggle Music.",
        "Story (1/6)\nYou are aan alien Biomass.\nEat caverns.\nGet paid.",
        "Starting Out (2/6)\npress SPACEBAR to Start\n press A and D keys to Steer your Tendril",
        "Caverns & Juice (3/6)\nExplore the earth to find Caverns.\nCaverns increase your maximum Juice.\nExploring Deeper costs more Juice.",
        "Traversing (4/6)\nYou can Traverse your old roots!\n Press A or D to continue down a Branch.\n Traversing costs less Juice.",
        "Branching Out (5/6)\n While Traversing: \nPress SHIFT with the A or D keys to Branch off\nand and Explore new areas.",
        "Debug Commands (6/6)\n Press BACKSPACE to end your run early."
    };

    private void Start()
    {
        textComp = gameObject.GetComponent<Text>();
        textComp.text = TutorialTexts[currentTutIndex];
    }

    public void HideTutorial()
    {
        gameObject.SetActive(false);
    }

    public void ShowTutorial()
    {
        currentTutIndex = 0;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentTutIndex = math.max(0, currentTutIndex - 1);
            textComp.text = TutorialTexts[currentTutIndex];
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentTutIndex = math.min(TutorialTexts.Count - 1, currentTutIndex + 1);
            textComp.text = TutorialTexts[currentTutIndex];
        }
    }
}