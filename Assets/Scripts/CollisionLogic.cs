using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLogic : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) => LogCollision(collision);
    void OnTriggerStay2D(Collider2D collision) => LogCollision(collision);
    void OnTriggerExit2D(Collider2D collision) => LogCollision(collision);

    void LogCollision(Collider2D collision)
    {
        Debug.Log(
            $"{collision.gameObject.name} detected a collision"
            );
    }

        // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"collision logic started for {name}");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update happened");
    }
}