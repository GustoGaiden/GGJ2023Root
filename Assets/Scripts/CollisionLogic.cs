using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLogic : MonoBehaviour
{
    public PlayerController PlayerController;
    void OnCollisionEnter2D(Collision2D collision) {
        // Debug.Log("Collision.collider:" + collision.collider.gameObject.name);
        // Debug.Log("Collision.collider.other" + collision.otherCollider.gameObject.name);
        if (collision.collider.tag == "Cavern")
        {
            ResourceNode cavern = collision.gameObject.GetComponent<ResourceNode>();
            cavern.ConsumeResource();
            // Debug.Log($"cavern {cavern.name} is now active" );
            GlobalVars.GameManager.EndCurrentRun();
        }
        
    }
}