using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class CollisionLogic : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("COLLIDED WITH SOMETHING : ME " + collision.collider.gameObject.name);
        Debug.Log("COLLIDED WITH SOMETHING : OTHER " + collision.otherCollider.gameObject.name);
        if (collision.collider.tag == "Cavern")
        {
            CavernDisplay cavern = collision.gameObject.GetComponent<CavernDisplay>();
            cavern.isConnected = true;
            Debug.Log($"cavern {cavern.name} is now active" );
        }
        // if(ResourceNode != null)
        // {
        //     Debug.Log("Resource getting activated!");
        //     ResourceNode.ActivateResource();
        // }
    }
}