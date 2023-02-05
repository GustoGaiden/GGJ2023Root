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

    void Start()
    {
        GlobalVars.GameManager = this;
    }
    
    private void Awake()
    {
        StartNewRun();
        DirtManager.RegenerateWorld();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckManualResets();
        DepleteJuice();
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
        ResetJuiceToMax();
        PlayerController.ResetForNewRun();
    }

    public void DepleteJuice() {
        float costPerSecond = DirtManager.GetCurrentJuiceCostPerSecond();
        float costCurrentTime = costPerSecond * Time.fixedDeltaTime;
        Juice.CurrentJuice = Mathf.Max(Juice.CurrentJuice - costCurrentTime, 0f);
    }
    
    public void ResetJuiceToMax()
    {
        float newMaxValue = Juice.InitialMaxJuice + ResourceNodeManager.GetMaxJuiceIncrease();
        Juice.MaxJuice = newMaxValue;
        Juice.CurrentJuice = Juice.MaxJuice;
        JuiceDisplay.ResetToMax();
    }
}
