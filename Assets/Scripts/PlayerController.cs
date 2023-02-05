using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public enum State
{
    Exploring,
    Traversing,
}
public enum PlayerInput
{
    Left,
    Right,
    Neither,
}

public class Node
{
    public List<Vector2> path;
    public Node left;
    public Node right;
    public int pathIndex;

    public Node()
    {
        path = new List<Vector2>();
        left = null;
        right = null;
        pathIndex = 0;
    }
    private Node(List<Vector2> path, Node left, Node right, int pathIndex)
    {
        this.path = path;
        this.left = left;
        this.right = right;
        this.pathIndex = pathIndex;
    }
    public bool IsPathNotEmpty()
    {
        return (pathIndex < path.Count - 1);
    }
    public Vector2 GetLatestPosition()
    {
        return path[pathIndex - 1];
    }
    public void AddPosition(Vector2 position)
    {
        path.Add(position);
        pathIndex++;
    }
    public void Reset()
    {
        pathIndex = 0;
    }
    public bool HasLeft()
    {
        return left != null && left.path.Count != 0;
    }
    public bool HasRight()
    {
        return right != null && right.path.Count != 0;
    }
    public bool HasLeaves()
    {
        return (HasLeft() || HasRight());
    }
    public Node Copy()
    {
        return new Node(path, left, right, pathIndex);
    }
    public Node GetOnlyChild()
    {
        return left == null ? right : left;
    }

}
public class Tree
{
    public Node root;
    public Node current;

    public PlayerInput bufferedInput;

    public Tree()
    {
        root = new Node();
        current = root;
        bufferedInput = PlayerInput.Neither;
    }
    public bool IsCurrentRoot()
    {
        return current == root;
    }
    public void ResetToRoot()
    {
        current = root;
        current.Reset();
    }
    public void ContinueOnBranch()
    {
        // historyIndex++;
        current.pathIndex++;
    }
    public void BufferInput()
    {
        if (Input.GetKey(KeyCode.A)) { bufferedInput = PlayerInput.Left; }
        if (Input.GetKey(KeyCode.D)) { bufferedInput = PlayerInput.Right; }
    }
    public void SwitchBranch()
    {
        switch (bufferedInput)
        {
            case PlayerInput.Left:
                current = current.left;
                current.Reset();
                break;
            case PlayerInput.Right:
                current = current.right;
                current.Reset();
                break;
            case PlayerInput.Neither:
                current = current.left;
                current.Reset();
                break;
        }
    }
    public void CreateBranch()
    {
        if (!current.HasLeft())
        {
            current.left = new Node();
        }
        else if (!current.HasRight())
        {
            current.right = new Node();
        }
    }

    public void SplitBranch(PlayerInput input)
    {
        List<Vector2> previousPath = current.path.GetRange(0, current.pathIndex);
        List<Vector2> remainingPath = current.path.GetRange(current.pathIndex, current.path.Count - current.pathIndex - 1);
        current.path = previousPath;

        Node shortenedNode = current.Copy();
        shortenedNode.path = remainingPath;

        if (input == PlayerInput.Left)
        {
            current.right = shortenedNode;
            current.left = new Node();
            current = current.left;
        }
        else if (input == PlayerInput.Right)
        {
            current.left = shortenedNode;
            current.right = new Node();
            current = current.right;
        }
    }

}

public class PlayerController : MonoBehaviour
{
    private Vector2 movementDirection = Vector2.down;
    private Tree treeHistory = new Tree();
    private State state = State.Exploring;
    private int rootCounter = 0;

    [Tooltip("Speed in units/second")]
    public float movementSpeed;
    [Tooltip("Rotational speed in degrees per second")]
    public float steeringSpeed;
    [Tooltip("Max angle in degrees")]
    public int maxAngle;
    [Tooltip("Texture to draw a root")]
    public RootBehaviour root;

    void Awake()
    {
        movementDirection = movementDirection.normalized * movementSpeed;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void Update()
    {
        TriggerSplitBranch();
        UpdateMovementDirection();
        PrintPaths();
    }

    void MovePlayer()
    {
        if (state == State.Traversing)
        {
            treeHistory.BufferInput();
            if (treeHistory.current.IsPathNotEmpty())
            {
                treeHistory.ContinueOnBranch();
                transform.position = treeHistory.current.GetLatestPosition();
            }
            else if (treeHistory.current.HasLeaves())
            {
                treeHistory.SwitchBranch();
                transform.position = treeHistory.current.GetLatestPosition();
            }
            else
            {
                movementDirection = Vector2.down * movementSpeed;
                treeHistory.CreateBranch();
                state = State.Exploring;
            }
        }
        if (state == State.Exploring)
        {
            transform.Translate(movementDirection);
            // List<Vector2> path = treeHistory.current.path;
            treeHistory.current.AddPosition(transform.position);
            RootBehaviour instance = Instantiate<RootBehaviour>(root, transform.position, Quaternion.identity);
        }
    }
    void TriggerSplitBranch()
    {
        if ((Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftShift)) || Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift) && state == State.Traversing)
        {
            state = State.Exploring;
            treeHistory.SplitBranch(PlayerInput.Left);

        }
        if ((Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftShift)) || Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift) && state == State.Traversing)
        {
            state = State.Exploring;
            treeHistory.SplitBranch(PlayerInput.Right);

        }
    }

    void UpdateMovementDirection()
    {
        Vector2 newDirection = movementDirection;
        //FIXME

        if (Input.GetKey(KeyCode.A) && state == State.Exploring)
        {
            newDirection = Quaternion.AngleAxis(-steeringSpeed * Time.deltaTime, Vector3.forward) * movementDirection;
        }
        if (Input.GetKey(KeyCode.D) && state == State.Exploring)
        {
            newDirection = Quaternion.AngleAxis(steeringSpeed * Time.deltaTime, Vector3.forward) * movementDirection;
        }
        movementDirection = Mathf.RoundToInt(Vector2.Angle(newDirection, Vector2.down)) <= maxAngle ? newDirection : movementDirection;
    }

    public void ResetForNewRun()
    {
        treeHistory.ResetToRoot();
        state = State.Traversing;
    }
}