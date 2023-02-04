using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public JuiceManager JuiceManager;
    public ResourceNodeManager ResourceNodeManager;
    public DirtManager DirtManager;
    public RootTextureController RootTextureController;
    public GameObject GameMapContainer;
    public GameObject CavernsContainer;
  
    public float InitialMaxJuice = 100f;
    public float JuiceDepletionRate = 0.01f;
    
    public int ConnectedCaverns;
    
    private void Awake()
    {
        StartNewRun();
        DirtManager.RegenerateWorld();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckManualResets();
        JuiceManager.DepleteJuice(JuiceDepletionRate);
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
        if (JuiceManager.isOutOfJuice())
        {
            StartNewRun();
        }
    }
    
    public void StartNewRun()
    {
        JuiceManager.resetToMax(InitialMaxJuice + ResourceNodeManager.GetMaxJuiceIncrease());
        PlayerController.ResetForNewRun();
    }
    
}
