using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public JuiceManager JuiceManager;
    public ResourceNodeManager ResourceNodeManager;

    public GameObject GameMapContainer;
    public GameObject CavernsContainer;
  
    public float MaxPlayerJuice = 100f;
    public float JuiceDepletionRate = 0.01f;
    
    public int ConnectedCaverns;
    
    private void Awake()
    {
        StartNewRun();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckResets();
        JuiceManager.DepleteJuice(JuiceDepletionRate);
        JuiceManager.AddJuiceForActiveResources(ResourceNodeManager.ActiveResources);
        CheckJuiceEmpty();
    }

    void CheckResets()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Backspace))
        {
            StartNewRun();
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
        JuiceManager.resetToMax(MaxPlayerJuice);
        PlayerController.ResetForNewRun();
    }
    
}
