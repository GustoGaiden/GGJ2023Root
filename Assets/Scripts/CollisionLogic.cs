using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLogic : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) => HandleCollision(collision);

    public PlayerController? PlayerController;

    private void HandleCollision(Collider2D collision)
    {
        if(PlayerController != null)
        {
            // Debug.Log($"Collision handling for player object {collision.name}");
            PlayerController.ResetForNewRun();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}