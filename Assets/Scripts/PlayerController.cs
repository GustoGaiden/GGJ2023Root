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
    public List<GameObject> pathElements;
    public Node left;
    public Node right;
    public int pathIndex;

    public Node()
    {
        path = new List<Vector2>();
        pathElements = new List<GameObject>();
        left = null;
        right = null;
        pathIndex = 0;
    }
    private Node(List<Vector2> path, Node left, Node right, int pathIndex, List<GameObject> pathElements)
    {
        this.path = path;
        this.left = left;
        this.right = right;
        this.pathIndex = pathIndex;
        this.pathElements = pathElements;
    }
    public bool IsPathNotEmpty()
    {
        return (pathIndex < path.Count - 1);
    }
    public Vector2 GetLatestPosition()
    {
        return path[(pathIndex - 1) < 0 ? 0 : (pathIndex - 1)];
    }
    public void AddPosition(Vector2 position, GameObject rootPart)
    {
        path.Add(position);
        pathElements.Add(rootPart);
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
    public bool HasBothLeaves()
    {
        return (HasLeft() && HasRight());
    }
    public Node Copy()
    {
        return new Node(path, left, right, pathIndex, pathElements);
    }
}
public class Tree
{
    public Node root;
    public Node current;

    public PlayerInput bufferedInput;

    private List<GameObject> highlightedBranch;

    public Tree()
    {
        root = new Node();
        current = root;
        bufferedInput = PlayerInput.Neither;
        highlightedBranch = new List<GameObject>();
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
        current.pathIndex++;
    }
    public void BufferInput()
    {
        if (Input.GetKey(KeyCode.A)) { bufferedInput = PlayerInput.Left; }
        if (Input.GetKey(KeyCode.D)) { bufferedInput = PlayerInput.Right; }
        ResetHighlightedBranch();
        HighlightBranch();
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

    public void ResetHighlightedBranch()
    {
        foreach (GameObject pathElement in highlightedBranch)
        {
            pathElement.GetComponent<SpriteRenderer>().color = Color.white;
        }
        highlightedBranch.Clear();
    }


    public void HighlightBranch()
    {
        int highlightLength = 10;
        if (current.HasBothLeaves())
        {
            int splittingPoint = Mathf.Max((current.pathElements.Count - 1 - highlightLength), 0);
            int availableTail = Mathf.Min(highlightLength, current.pathElements.Count - 1 - splittingPoint);
            int availableHead = 0;
            List<GameObject> pathToHighlight = current.pathElements.GetRange(splittingPoint, highlightLength);
            switch (bufferedInput)
            {
                case PlayerInput.Left:
                    availableHead = Mathf.Min(highlightLength, current.left.pathElements.Count - 1);
                    pathToHighlight.AddRange(current.left.pathElements.GetRange(0, availableHead));
                    break;
                case PlayerInput.Right:
                    availableHead = Mathf.Min(highlightLength, current.right.pathElements.Count - 1);
                    pathToHighlight.AddRange(current.right.pathElements.GetRange(0, availableHead));
                    break;

            }
            foreach (GameObject pathElement in pathToHighlight)
            {

                pathElement.GetComponent<SpriteRenderer>().color = Color.black;
            }
            highlightedBranch = pathToHighlight;
        }
    }

}

public class PlayerController : MonoBehaviour
{
    private Vector2 movementDirection = Vector2.down;
    private Tree treeHistory = new Tree();
    private State state = State.Exploring;
    private int rootCounter = 0;


    public GameObject playerHeadExploring;
    public GameObject playerHeadTraversing;
    [Tooltip("Speed in units/second")]
    public float movementSpeed;
    [Tooltip("Rotational speed in degrees per second")]
    public float steeringSpeed;
    [Tooltip("Max angle in degrees")]
    public int maxAngle;
    [Tooltip("Texture to draw a root")]
    public GameObject rootPrefab;

    void Awake()
    {
        movementDirection = movementDirection.normalized * movementSpeed;
        playerHeadTraversing.GetComponent<SpriteRenderer>().enabled = false;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void Update()
    {
        TriggerSplitBranch();
        UpdateMovementDirection();
        Debug.Log(state);

    }
    void SetState(State newState)
    {
        state = newState;
        switch (newState)
        {
            case State.Exploring:
                SetExploringHead(true);
                SetTraversingHead(false);
                break;
            case State.Traversing:
                SetExploringHead(false);
                SetTraversingHead(true);
                break;
        }
    }
    void SetTraversingHead(bool enabled)
    {
        playerHeadTraversing.GetComponent<SpriteRenderer>().enabled = enabled;

    }
    void SetExploringHead(bool enabled)
    {
        SpriteRenderer[] sprites = playerHeadExploring.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in sprites)
        {
            sprite.enabled = enabled;
        }
    }

    // All player movement happens here
    // It is differentiated between two modes:
    //      - travelling on previous tendrils aka 'Traversing'
    //      - create a new tendril aka 'Exploring'
    void MovePlayer()
    {
        // Traversing the network has multiple modes as well:
        if (state == State.Traversing)
        {
            treeHistory.BufferInput();
            // You are currently in the midst of following an old tendril, so continue doing that
            if (treeHistory.current.IsPathNotEmpty())
            {
                treeHistory.ContinueOnBranch();
                transform.position = treeHistory.current.GetLatestPosition();
                // if (treeHistory.current.HasLeaves())
                // {
                //     Debug.Log("Highlighting");
                //     treeHistory.HighlightBranch();
                // }
            }
            // If you aren't in the midst of that, check if other old tendrils are below your current one and switch to them
            else if (treeHistory.current.HasLeaves())
            {
                treeHistory.SwitchBranch();
                transform.position = treeHistory.current.GetLatestPosition();
            }
            // And finally, if there are no old tendrils below your current one, you followed an old tendril to its end
            // In that case, create a new branch at the end of it
            else
            {
                movementDirection = Vector2.down * movementSpeed;
                treeHistory.CreateBranch();
                SetState(State.Exploring);
            }
        }
        if (state == State.Exploring)
        {
            transform.Translate(movementDirection);
            GameObject instance = Instantiate(rootPrefab, transform.position, Quaternion.identity);
            treeHistory.current.AddPosition(transform.position, instance);
            playerHeadExploring.transform.up = movementDirection;
        }
    }
    void TriggerSplitBranch()
    {
        if ((Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftShift)) || Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift) && state == State.Traversing)
        {
            SetState(State.Exploring);
            treeHistory.SplitBranch(PlayerInput.Left);

        }
        if ((Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftShift)) || Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift) && state == State.Traversing)
        {
            SetState(State.Exploring);
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
        SetState(State.Traversing);
    }
}