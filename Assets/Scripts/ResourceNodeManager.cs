using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeManager : MonoBehaviour
{
    private List<ResourceNode> AllResources = new List<ResourceNode>();

    public List<ResourceNode> ActiveResources
    {
        get
        {
            return AllResources.FindAll((resource) => resource.State == ResourceNodeState.DEPLETED);
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

    private void Awake()
    {
        ResourceNode[] resources = FindObjectsOfType<ResourceNode>();
        // Debug.Log($"Found {resources.Length} resources on awake.");
        foreach (ResourceNode resource in resources)
        {
            AllResources.Add(resource);
        }

    }
}