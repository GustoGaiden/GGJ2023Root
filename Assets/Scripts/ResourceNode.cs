using System.Collections.Generic;
using UnityEngine;


public enum ResourceNodeState {
    AVAILABLE,
    ACTIVE,
    STONE
}

public class ResourceNode : MonoBehaviour
{
    public ResourceNodeState State = ResourceNodeState.AVAILABLE;
    public float MaxJuiceIncrease = 0.3f*Juice.InitialMaxJuice;

    public void ActivateResource()
    {
        if (State == ResourceNodeState.AVAILABLE)
        {
            Debug.Log($"Resource {name} is now active");
            State = ResourceNodeState.ACTIVE;
            GlobalVars.GameManager.ResetJuiceToMax();
        }
    }
}