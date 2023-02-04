using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public ResourceNodeManager ResourceNodeManager;
    public DirtManager DirtManager;
    public RootTextureController RootTextureController;
    public GameObject GameMapContainer;
    public GameObject CavernsContainer;

    public JuiceDisplay JuiceDisplay;
    
    private void Awake()
    {
        StartNewRun();
        DirtManager.RegenerateWorld();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckManualResets();
        DepleteJuice(Juice.JuiceDepletionRate);
        CheckJuiceEmpty();
    }

    void CheckManualResets()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Backspace))
        {
            StartNewRun();
        }

        if (Input.GetKey(KeyCode.R))
        {
            DirtManager.RegenerateWorld();
        }
    }

    void CheckJuiceEmpty()
    {
        if (Juice.CurrentJuice <= 0f)
        {
            StartNewRun();
        }
    }
    
    public void StartNewRun()
    {
        ResetJuiceToMax(Juice.InitialMaxJuice + ResourceNodeManager.GetMaxJuiceIncrease());
        PlayerController.ResetForNewRun();
    }

    public void DepleteJuice(float JuiceDepletionRate) {
        Juice.CurrentJuice = Mathf.Max(Juice.CurrentJuice - JuiceDepletionRate, 0f);
    }
    public void ResetJuiceToMax(float newMaxValue)
    {
        Debug.Log($"Current Juice: ${Juice.CurrentJuice}");
        Debug.Log($"Reset to max. Old Max = {Juice.MaxJuice} and New Max = {newMaxValue}");
        Juice.MaxJuice = newMaxValue;
        Juice.CurrentJuice = Juice.MaxJuice;
        JuiceDisplay.ResetToMax();
    }
}
