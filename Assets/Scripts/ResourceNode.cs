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
    public float MaxJuiceIncrease = 30f;

    public void ActivateResource()
    {
        if (State == ResourceNodeState.AVAILABLE)
        {
            State = ResourceNodeState.ACTIVE;
        }
    }
}