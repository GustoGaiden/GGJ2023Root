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
}