using System;
using UnityEngine;
using Random = UnityEngine.Random;


public enum ResourceNodeState {
    AVAILABLE,
    DEPLETED,
    STONE
}

public class ResourceNode : MonoBehaviour
{
    public ResourceNodeState State = ResourceNodeState.AVAILABLE;
    public float MaxJuiceIncrease = 0.3f*Juice.InitialMaxJuice;

    public void Start()
    {
        if (State == ResourceNodeState.STONE)
        {
            gameObject.transform.rotation = Quaternion.AngleAxis( Random.Range(0f,356f), Vector3.forward);            
        }
    }

    public void ConsumeResource()
    {
        if (State == ResourceNodeState.AVAILABLE)
        {
            // Debug.Log($"Resource {name} is now active");
            State = ResourceNodeState.DEPLETED;
            GlobalVars.GameManager.ResetJuiceToMax();
        }

        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
    }
}