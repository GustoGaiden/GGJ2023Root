using System.Collections.Generic;
using UnityEngine;


public enum ResourceNodeState {
    AVAILABLE,
    ACTIVE,
    DEPLETED
}

public class ResourceNode : MonoBehaviour
{
    public ResourceNodeState State = ResourceNodeState.AVAILABLE;
    // float AvailableJuice = 30f;
    public float JuiceFlowRate = 0.0035f;

    public void ActivateResource()
    {
        if (State == ResourceNodeState.AVAILABLE)
        {
            State = ResourceNodeState.ACTIVE;
        }
    }
}