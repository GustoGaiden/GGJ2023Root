using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject PlayerController;
    public List<GameObject> Caverns;
    public int ConnectedCaverns;
    public GameObject GameMapContainer;


    public JuiceManager JuiceManager;

    public float maxJuice = 100;

    // Start is called before the first frame update

    private void Awake()
    {
        StartNewRun();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewRun()
    {
        JuiceManager.resetToMax(maxJuice);
    }
}
