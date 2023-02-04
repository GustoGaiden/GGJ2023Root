using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class CollisionLogic : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) => HandleCollision(collision);

    public PlayerController? PlayerController;
    public ResourceNode? ResourceNode;

    private void HandleCollision(Collider2D collision)
    {
        // Debug.Log($"Collision took place for obect {collision.name}");
        if(PlayerController != null)
        {
            // Debug.Log($"Collision handling for player object {collision.name}");
            PlayerController.ResetForNewRun();
        }
        if(ResourceNode != null)
        {
            // Debug.Log("Resource getting activated!");
            ResourceNode.ActivateResource();
        }
    }
}