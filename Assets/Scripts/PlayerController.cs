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
    private List<Vector2> history = new List<Vector2>();
    private State state = State.Exploring;
    private Vector2 startingPosition = new Vector2(0, 0);
    private int historyIndex = 0;

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
        MovePlayer();
        DebugFun();
    }

    void Update()
    {
        UpdateMovementDirection();
        CheckReset();

    }

    void MovePlayer()
    {
        if (state == State.Traversing)
        {
            if (historyIndex < history.Count - 1)
            {
                historyIndex++;
                transform.position = history[historyIndex];
            }
            else
            {
                state = State.Exploring;
            }
        }
        if (state == State.Exploring)
        {
            transform.Translate(movementDirection);
            history.Add(transform.position);

        }

    }

    void UpdateMovementDirection()
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
        movementDirection = Mathf.RoundToInt(Vector2.Angle(newDirection, Vector2.down)) <= maxAngle ? newDirection : movementDirection;
    }
    void CheckReset()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            state = State.Traversing;
            historyIndex = 0;
            transform.position = history[historyIndex];

        }
    }


    void DebugFun()
    {
        if (Input.GetKey(KeyCode.P))
        {
            foreach (Vector2 point in history)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Instantiate(cube, point, Quaternion.identity);
            }
        }
    }
}