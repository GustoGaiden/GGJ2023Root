using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    Exploring,
    Traversing,
}

public class PlayerController : MonoBehaviour
{
    private Vector2 movementDirection = Vector2.down;
    private List<Vector2> travelHistory = new List<Vector2>();
    [Tooltip("Speed in units/second")]
    public float movementSpeed;
    [Tooltip("Rotational speed in degrees per second")]
    public float steeringSpeed;
    [Tooltip("Max angle in degrees")]
    public int maxAngle;

    void Awake()
    {
        movementDirection = movementDirection.normalized * movementSpeed;

    }

    void FixedUpdate()
    {
        transform.Translate(movementDirection);
        UpdateHistory();
        DebugFun();
    }

    void Update()
    {
        movementDirection = UpdateDirection();

    }

    Vector2 UpdateDirection()
    {
        Vector2 newDirection = movementDirection;
        if (Input.GetKey(KeyCode.A))
        {
            newDirection = Quaternion.AngleAxis(-steeringSpeed * Time.deltaTime, Vector3.forward) * movementDirection;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newDirection = Quaternion.AngleAxis(steeringSpeed * Time.deltaTime, Vector3.forward) * movementDirection;
        }
        return Mathf.RoundToInt(Vector2.Angle(newDirection, Vector2.down)) <= maxAngle ? newDirection : movementDirection;
    }

    void UpdateHistory()
    {
        travelHistory.Add(transform.position);
    }

    void DebugFun()
    {
        if (Input.GetKey(KeyCode.P))
        {
            foreach (Vector2 point in travelHistory)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Instantiate(cube, point, Quaternion.identity);
            }
        }
    }
}