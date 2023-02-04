using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PlayerDisplay;
    public GameObject PlayerObject;
    public Vector3 Direction;
    
    public Vector2 currentMovementDirection = Vector2.down;
    public Vector2 StartAngle = Vector2.left;
    
    [Tooltip("Speed in units/second")]
    public float movementSpeed;
    
    [Tooltip("Rotational speed in degrees per second")]
    public float steeringSpeed;
    
    [Tooltip("Max angle in degrees")]
    public int maxAngle;
    
    void Awake()
    {
        currentMovementDirection = currentMovementDirection.normalized * movementSpeed;
    }

    void Update()
    {
        currentMovementDirection = UpdateDirection();
    }
    
    void FixedUpdate()
    {
        PlayerObject.transform.Translate(currentMovementDirection);
        PlayerDisplay.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(currentMovementDirection, StartAngle), Vector3.forward);
    }
    
    Vector2 UpdateDirection()
    {
        Vector2 newDirection = currentMovementDirection;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow))
        {
            newDirection = Quaternion.AngleAxis(-steeringSpeed * Time.deltaTime, Vector3.forward) * currentMovementDirection;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
        {
            newDirection = Quaternion.AngleAxis(steeringSpeed * Time.deltaTime, Vector3.forward) * currentMovementDirection;
        }
        return Mathf.RoundToInt(Vector2.Angle(newDirection, Vector2.down)) <= maxAngle ? newDirection : currentMovementDirection;

    }
}
