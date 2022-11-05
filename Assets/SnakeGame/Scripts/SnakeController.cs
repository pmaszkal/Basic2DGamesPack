using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class SnakeController : MonoBehaviour
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private SpriteRenderer headSpriteRenderer;
    private GamesControlls gamesControlls;
    private Vector2Int gridStartingPosition = new Vector2Int(10, 10);
    private Vector2Int gridPosition;
    private Direction gridNextMoveDirection;
    private Direction gridCurrentMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimerMax = .5f;

    public V2IntEvent snakeMoveEvent;

    private void Awake()
    {
        headSpriteRenderer = GetComponent<SpriteRenderer>();
        headSpriteRenderer.sprite = SnakeGameAssets.instance.HeadSnakeSprite;

        if (snakeMoveEvent == null)
        {
            snakeMoveEvent = new V2IntEvent();
        }
    }

    private void OnEnable()
    {
        if (gamesControlls == null)
        {
            gamesControlls = new GamesControlls();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        gridPosition = gridStartingPosition;
        gridNextMoveDirection = Direction.Right;
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer > gridMoveTimerMax)
        {
            Vector2Int gridMovementVector = new Vector2Int(0, 0);
            gridCurrentMoveDirection = gridNextMoveDirection;
            switch (gridCurrentMoveDirection)
            {
                case Direction.Up: gridMovementVector = new Vector2Int(0, 1); break;
                case Direction.Down: gridMovementVector = new Vector2Int(0, -1); break;
                case Direction.Left: gridMovementVector = new Vector2Int(-1, 0); break;
                case Direction.Right: gridMovementVector = new Vector2Int(1, 0); break;
                default: break;
            }
            gridPosition += gridMovementVector;

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMovementVector));

            snakeMoveEvent.Invoke(gridPosition);
            gridMoveTimer = 0;
        }
    }

    public void HandleInput(CallbackContext callbackContext)
    {
        if (callbackContext.action.phase == InputActionPhase.Started)
            return;
        if (callbackContext.action.id == gamesControlls.Snake.Up.id && gridCurrentMoveDirection != Direction.Down)
        {
            gridNextMoveDirection = Direction.Up;
        }
        else if (callbackContext.action.id == gamesControlls.Snake.Down.id && gridCurrentMoveDirection != Direction.Up)
        {
            gridNextMoveDirection = Direction.Down;
        }
        else if (callbackContext.action.id == gamesControlls.Snake.Left.id && gridCurrentMoveDirection != Direction.Right)
        {
            gridNextMoveDirection = Direction.Left;
        }
        else if (callbackContext.action.id == gamesControlls.Snake.Right.id && gridCurrentMoveDirection != Direction.Left)
        {
            gridNextMoveDirection = Direction.Right;
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}

[System.Serializable]
public class V2IntEvent : UnityEvent<Vector2Int>
{
}