using System.Collections.Generic;
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

    private enum State
    {
        alive,
        dead
    }

    private SpriteRenderer headSpriteRenderer;
    private GamesControlls gamesControlls;
    private Vector2Int gridStartingPosition = new Vector2Int(10, 10);
    private Vector2Int gridPosition;
    private Direction gridNextMoveDirection;
    private Direction gridCurrentMoveDirection;
    private float gridMoveTimer;
    private int bodyLength = 0;
    private List<SnakeBodyPart> bodyPartList;
    private List<SnakeMovePosition> movePosistionList;
    [SerializeField] private float gridMoveTimerMax = .2f;
    private GridMapManager gridMapManager;
    private State state;

    public void Init(GridMapManager gridMapManager)
    {
        this.gridMapManager = gridMapManager;
    }

    private void Awake()
    {
        headSpriteRenderer = GetComponent<SpriteRenderer>();
        headSpriteRenderer.sprite = SnakeGameAssets.Instance.HeadSnakeSprite;
        bodyPartList = new List<SnakeBodyPart>();
        movePosistionList = new List<SnakeMovePosition>();
        state = State.alive;
    }

    private void OnEnable()
    {
        if (gamesControlls == null)
        {
            gamesControlls = new GamesControlls();
        }
    }

    private void Start()
    {
        gridPosition = gridStartingPosition;
        gridNextMoveDirection = Direction.Right;
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
    }

    private void Update()
    {
        switch (state)
        {
            case State.alive:
                HandleMovement();
                break;

            case State.dead:
                break;
        }
    }

    private void HandleMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer > gridMoveTimerMax)
        {
            gridMoveTimer = 0;
            Vector2Int gridMovementVector = new Vector2Int(0, 0);
            gridCurrentMoveDirection = gridNextMoveDirection;

            SnakeMovePosition previousSnakeMovePosistion = null;
            if (movePosistionList.Count > 0)
            {
                previousSnakeMovePosistion = movePosistionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosistion, gridPosition, gridCurrentMoveDirection);
            movePosistionList.Insert(0, snakeMovePosition);

            switch (gridCurrentMoveDirection)
            {
                case Direction.Up: gridMovementVector = new Vector2Int(0, 1); break;
                case Direction.Down: gridMovementVector = new Vector2Int(0, -1); break;
                case Direction.Left: gridMovementVector = new Vector2Int(-1, 0); break;
                case Direction.Right: gridMovementVector = new Vector2Int(1, 0); break;
                default: break;
            }

            gridPosition += gridMovementVector;

            gridPosition = gridMapManager.CheckGridConstrains(gridPosition);

            bool snakeAte = gridMapManager.SnakeTryEat(gridPosition);

            if (snakeAte)
            {
                bodyLength++;
                CreateBodyPart();
                SnakeSoundManager.PlaySound(SnakeSoundManager.Sound.SnakeEat);
            }

            if (movePosistionList.Count >= bodyLength + 1)
            {
                movePosistionList.RemoveAt(movePosistionList.Count - 1);
            }

            UpdateBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in bodyPartList)
            {
                if (gridPosition == snakeBodyPart.GetGridPosition())
                {
                    //GAME OVER
                    state = State.dead;
                    SnakeGameManager.Instance.HandleSnakeDeath();
                    SnakeSoundManager.PlaySound(SnakeSoundManager.Sound.SnakeDie);
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMovementVector));
        }
    }

    private void CreateBodyPart()
    {
        bodyPartList.Add(new SnakeBodyPart(bodyPartList.Count));
    }

    private void UpdateBodyParts()
    {
        for (int i = 0; i < bodyPartList.Count; i++)
        {
            bodyPartList[i].SetSnakeMovePosition(movePosistionList[i]);
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

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in movePosistionList)
        {
            gridPositionList.Add(snakeMovePosition.GridPosition);
        }
        return gridPositionList;
    }

    private class SnakeBodyPart
    {
        private SnakeMovePosition SnakeMovePosistion;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = SnakeGameAssets.Instance.BodySnakeSprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            snakeBodyGameObject.transform.localScale = new Vector3(8, 8, 0);
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosistion)
        {
            SnakeMovePosistion = snakeMovePosistion;
            transform.position = new Vector3(snakeMovePosistion.GridPosition.x, snakeMovePosistion.GridPosition.y);

            float angle;

            switch (snakeMovePosistion.Direction)
            {
                default:
                case Direction.Up:
                    switch (snakeMovePosistion.GetPreviousDirection())
                    {
                        default:
                            angle = 90;
                            break;

                        case Direction.Left:
                            angle = 90 + 45;
                            transform.position += new Vector3(.2f, .2f);
                            break;

                        case Direction.Right:
                            angle = 90 - 45;
                            transform.position += new Vector3(-.2f, .2f);
                            break;
                    }
                    break;

                case Direction.Down:
                    switch (snakeMovePosistion.GetPreviousDirection())
                    {
                        default:
                            angle = 270;
                            break;

                        case Direction.Right:
                            angle = 270 + 45;
                            transform.position += new Vector3(-.2f, -.2f);
                            break;

                        case Direction.Left:
                            angle = 270 - 45;
                            transform.position += new Vector3(.2f, -.2f);
                            break;
                    }
                    break;

                case Direction.Left:
                    switch (snakeMovePosistion.GetPreviousDirection())
                    {
                        default:
                            angle = 180;
                            break;

                        case Direction.Down:
                            angle = 180 + 45;
                            transform.position += new Vector3(-.2f, .2f);
                            break;

                        case Direction.Up:
                            angle = 180 - 45;
                            transform.position += new Vector3(-.2f, -.2f);
                            break;
                    }
                    break;

                case Direction.Right:
                    switch (snakeMovePosistion.GetPreviousDirection())
                    {
                        default:
                            angle = 0;
                            break;

                        case Direction.Up:
                            angle = 0 + 45;
                            transform.position += new Vector3(.2f, -.2f);
                            break;

                        case Direction.Down:
                            angle = 0 - 45;
                            transform.position += new Vector3(.2f, .2f);
                            break;
                    }
                    break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetGridPosition()
        {
            return SnakeMovePosistion.GridPosition;
        }
    }

    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        public Vector2Int GridPosition { get; private set; }
        public Direction Direction { get; private set; }

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            GridPosition = gridPosition;
            Direction = direction;
        }

        public Direction? GetPreviousDirection()
        {
            //return previousSnakeMovePosition == null ? Direction.Right : previousSnakeMovePosition.Direction;
            return previousSnakeMovePosition == null ? null : previousSnakeMovePosition.Direction;
        }
    }
}

[System.Serializable]
public class V2IntEvent : UnityEvent<Vector2Int>
{
}