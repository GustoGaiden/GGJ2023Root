using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public ResourceNodeManager ResourceNodeManager;
    public DirtManager DirtManager;
    public RootTextureController RootTextureController;
    public GameObject GameMapContainer;
    public GameObject CavernsContainer;

    public JuiceDisplay JuiceDisplay;

    public GameMode CurrentMode;
    public TutorialText TutorialTextUI;
    public FlavorText FlavorTextUI;
    
    public enum GameMode {
        RunStart, // Waiting for player input
        RunStarting, // Start Animation Playing.
        RunActive,
        RunEnd
    }
    
    void Start()
    {
        GlobalVars.GameManager = this;
        CurrentMode = GameMode.RunStart;
    }
    
    private void Awake()
    {
        StartNewRun();
        DirtManager.RegenerateWorld();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ManageGameMode();
        CheckManualResets();
        DepleteJuice();
        CheckJuiceEmpty();
    }

    void CheckManualResets()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            EndCurrentRun();
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
            EndCurrentRun();
        }
    }
    
    public void StartNewRun()
    {
        CurrentMode = GameMode.RunStart;
        ResetJuiceToMax();
        PlayerController.ResetForNewRun();
        TutorialTextUI.ShowTutorial();
        FlavorTextUI.ShowFlavorText();
    }
    
    public void DepleteJuice() {
        float costPerSecond = DirtManager.GetCurrentJuiceCostPerSecond();
        float costCurrentTime = costPerSecond * Time.fixedDeltaTime;
        Juice.CurrentJuice = Mathf.Max(Juice.CurrentJuice - costCurrentTime, 0f);
    }

    public void ResetJuiceToMax()
    {
        float newMaxValue = Juice.InitialMaxJuice + ResourceNodeManager.GetMaxJuiceIncrease();
        // Debug.Log($"Current Juice: ${Juice.CurrentJuice}");
        // Debug.Log($"Reset to max. Old Max = {Juice.MaxJuice} and New Max = {newMaxValue}");
        
        Juice.MaxJuice = newMaxValue;
        Juice.CurrentJuice = Juice.MaxJuice;
        JuiceDisplay.ResetToMax();
    }

    public void ManageGameMode()
    {
        CheckGameStart();
    }
    
    public void CheckGameStart()
    {
        // Waiting for Player to press START, and begin a run.
        if (CurrentMode == GameMode.RunStart && Input.GetKey(KeyCode.Space))
        {
            CurrentMode = GameMode.RunActive;
            TutorialTextUI.HideTutorial();
            FlavorTextUI.HideFlavorText();
        }
    }

    public void EndCurrentRun()
    {
        StartCoroutine(TransitionFromEndToStart());
    }
    
    // https://docs.unity3d.com/Manual/Coroutines.html
    // Coroutine : After a delay
    IEnumerator TransitionFromEndToStart()
    {
        CurrentMode = GameMode.RunEnd;
        Debug.Log("Ending Wait Begins");
        yield return new WaitForSeconds(2f);
        Debug.Log("Ending Wait Complete");
        StartNewRun();
    }
}
