using System.Collections.Generic;
using UnityEngine;


public enum State
{
    Exploring,
    Traversing,
}
public enum BufferedInput
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
    }
    private Node(List<Vector2> path, Node left, Node right)
    {
        this.path = path;
        this.left = left;
        this.right = right;
    }
    public bool IsPathNotEmpty()
    {
        return (pathIndex < path.Count - 1);
    }
    public Vector2 GetLatestPosition()
    {
        Debug.Log("----");
        Debug.Log(path.Count);
        Debug.Log(pathIndex);
        return path[pathIndex - 1];
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
        return right != null && right.path.Count != 1;
    }
    public bool HasLeaves()
    {
        return (HasLeft() || HasRight());
    }
    public Node Copy()
    {
        return new Node(path, left, right);
    }

}
public class Tree
{
    public Node root;
    public Node current;

    public BufferedInput bufferedInput;

    public Tree()
    {
        root = new Node();
        current = root;
        bufferedInput = BufferedInput.Neither;
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
        if (Input.GetKey(KeyCode.A)) { bufferedInput = BufferedInput.Left; }
        if (Input.GetKey(KeyCode.D)) { bufferedInput = BufferedInput.Right; }
    }
    public void SwitchBranch()
    {
        if (bufferedInput == BufferedInput.Left)
        {
            current = current.left;
            current.Reset();
        }
        else if (bufferedInput == BufferedInput.Right)
        {
            current = current.right;
            current.Reset();
        }
        else if (bufferedInput == BufferedInput.Neither)
        {
            current = current.left;
            current.Reset();
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

    public void SplitBranch(BufferedInput input)
    {
        Node originalNode = current;
        List<Vector2> previousPath = current.path.GetRange(0, current.pathIndex);
        originalNode.path = previousPath;

        Node shortenedNode = current.Copy();
        List<Vector2> remainingPath = current.path.GetRange(current.pathIndex, current.path.Count - current.pathIndex);
        shortenedNode.path.RemoveRange(0, current.pathIndex - 1);

        if (input == BufferedInput.Left)
        {
            originalNode.right = shortenedNode;
            originalNode.left = new Node();
            current = originalNode.left;
        }
        else if (input == BufferedInput.Right)
        {
            originalNode.left = shortenedNode;
            originalNode.right = new Node();
            current = originalNode.right;
        }
    }

}

public class PlayerController : MonoBehaviour
{
    private Vector2 movementDirection = Vector2.down;
    private Tree treeHistory = new Tree();
    private State state = State.Exploring;
    private Vector2 startingPosition = new Vector2(0, 0);

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintPaths(treeHistory.root, 0);
        }
    }

    void Update()
    {
        UpdateMovementDirection();
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
                treeHistory.CreateBranch();
                state = State.Exploring;
            }
        }
        if (state == State.Exploring)
        {
            transform.Translate(movementDirection);
            treeHistory.current.path.Add(transform.position);

        }
    }

    void UpdateMovementDirection()
    {
        Vector2 newDirection = movementDirection;
        //FIXME
        // if ((Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftShift)) || Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     treeHistory.SplitBranch(BufferedInput.Left);
        //     state = State.Exploring;
        // }
        // if ((Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftShift)) || Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     treeHistory.SplitBranch(BufferedInput.Right);
        //     state = State.Exploring;
        // }
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

    public void ResetForNewRun()
    {
        state = State.Traversing;
        treeHistory.ResetToRoot();
        //transform.position = history[historyIndex];
    }


    void PrintPaths(Node node, int i)
    {
        if (node.HasLeft())
        {
            Debug.Log($"{i} -has Left");
            PrintPaths(node.left, i++);
        }
        if (node.HasRight())
        {
            Debug.Log($"{i} -has Right");
            PrintPaths(node.right, i++);
        }
        if (!node.HasLeaves())
        {
            Debug.Log($"{i} -has Nothing");
            foreach (Vector2 point in node.path)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Instantiate(cube, point, Quaternion.identity);
            }
        }
    }
}