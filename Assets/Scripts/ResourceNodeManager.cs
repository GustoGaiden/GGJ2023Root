using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeManager : MonoBehaviour
{
    public List<ResourceNode> AllResources;

    public List<ResourceNode> ActiveResources
    {
        get
        {
            return AllResources.FindAll((resource) => resource.State == ResourceNodeState.ACTIVE);
        }
    }

    public float GetMaxJuiceIncrease()
    {
        float output = 0;
        foreach(ResourceNode resource in ActiveResources)
        {
            output = output + resource.MaxJuiceIncrease;
        }
        return output;
    }
}