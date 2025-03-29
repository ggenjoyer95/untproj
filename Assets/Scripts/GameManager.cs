using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerNames;
    [SerializeField] private GameObject gamePiece;

    public bool canClick, hasGameFinished;
    Board myBoard;
    public static GameManager instance;

    string gameState;
    int roll;
    List<GamePiece> result;
    bool hasKilled, hasReachedEnd;

    int numOfPlayers;
    List<Player> players;
    int currentPlayer;
    Dictionary<GamePiece, GameObject> gamePieces;

    public string GetPlayerName(int index)
    {
        return playerNames[index].GetComponent<Text>().text;
    }

    public readonly Dictionary<Player, List<Vector3>> startPos = new Dictionary<Player, List<Vector3>>()
    {
        {
            Player.RED,
            new List<Vector3>()
            {
                new Vector3(2f, -2f, -1f), new Vector3(4f, -2f, -1f),
                new Vector3(2f, -4f, -1f), new Vector3(4f, -4f, -1f)
            }
        },
        {
            Player.BLUE,
            new List<Vector3>()
            {
                new Vector3(11f, -2f, -1f), new Vector3(13f, -2f, -1f),
                new Vector3(11f, -4f, -1f), new Vector3(13f, -4f, -1f)
            }
        },
        {
            Player.YELLOW,
            new List<Vector3>()
            {
                new Vector3(11f, -11f, -1f), new Vector3(13f, -11f, -1f),
                new Vector3(11f, -13f, -1f), new Vector3(13f, -13f, -1f)
            }
        },
        {
            Player.GREEN,
            new List<Vector3>()
            {
                new Vector3(2f, -11f, -1f), new Vector3(4f, -11f, -1f),
                new Vector3(2f, -13f, -1f), new Vector3(4f, -13f, -1f)
            }
        }
    };

    List<GameIndex> safePositions = new List<GameIndex>()
    {
        new GameIndex() { posType = Constants.NORMAL_POS, pos = 0 },
        new GameIndex() { posType = Constants.NORMAL_POS, pos = 13 },
        new GameIndex() { posType = Constants.NORMAL_POS, pos = 26 },
        new GameIndex() { posType = Constants.NORMAL_POS, pos = 39 },
        new GameIndex() { posType = Constants.NORMAL_POS, pos = 9 },
        new GameIndex() { posType = Constants.NORMAL_POS, pos = 22 },
        new GameIndex() { posType = Constants.NORMAL_POS, pos = 35 },
        new GameIndex() { posType = Constants.NORMAL_POS, pos = 48 }
    };

    Dictionary<GameGrid, GameIndex> gridToIndex;
    Dictionary<GameIndex, GameGrid> indexToGrid;
    Dictionary<Player, Dictionary<GameIndex, GameGrid>> indexToGridEnd;
    Dictionary<Player, int> remainingPieces;
    Dictionary<int, Player> playerRank;

    public delegate void UpdateMessage(Player temp);
    public event UpdateMessage Message;

    public void GameQuit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        canClick = true;
        hasGameFinished = false;
        gameState = Constants.ROLL_DIE;
        currentPlayer = 0;

        numOfPlayers = PlayerPrefs.HasKey("players") ? PlayerPrefs.GetInt("players") : 4;

        myBoard = new Board();
        players = new List<Player>();
        gamePieces = new Dictionary<GamePiece, GameObject>();
        remainingPieces = new Dictionary<Player, int>();
        playerRank = new Dictionary<int, Player>();
        gridToIndex = new Dictionary<GameGrid, GameIndex>();
        indexToGrid = new Dictionary<GameIndex, GameGrid>();
        indexToGridEnd = new Dictionary<Player, Dictionary<GameIndex, GameGrid>>();

        for (int i = 0; i < numOfPlayers; i++)
        {
            players.Add((Player)i);
            remainingPieces[(Player)i] = 4;
            playerNames[i].SetActive(true);
            playerNames[i].GetComponent<Text>().text =
                PlayerPrefs.HasKey((i + 1).ToString()) ? PlayerPrefs.GetString((i + 1).ToString()) : "Player" + (i + 1);

            for (int j = 0; j < 4; j++)
            {
                GamePiece tempPiece = new GamePiece() { player = (Player)i, pieceNumber = j };
                GameObject piece = Instantiate(gamePiece);
                piece.transform.position = startPos[(Player)i][j];
                piece.GetComponent<Piece>().SetColor((Player)i);
                gamePieces[tempPiece] = piece;
            }
        }

        InitializeMapping();
    }

    private void Update()
    {
        if (!canClick || hasGameFinished)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            switch (gameState)
            {
                case Constants.ROLL_DIE:
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                    if (!hit) return;

                    if (hit.collider.CompareTag("Die"))
                    {
                        gameState = Constants.MOVE_PLAYER;
                        canClick = false;
                        roll = Random.Range(0, 6) + 1;
                        hit.collider.gameObject.GetComponent<Die>().RollDie(roll);
                        result = myBoard.GetRoll(players[currentPlayer], roll);
                    }
                    break;

                case Constants.MOVE_PLAYER:
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    GameGrid currentGrid = GridMapping.TransformToGrid(mousePos);
                    GameIndex currentIndex = GridIndexer.GetGridIndex(currentGrid, players, currentPlayer, gridToIndex, indexToGridEnd);
                    if (currentIndex.pos == -1) return;

                    List<GamePiece> selectedPieces = myBoard.GetPieceAtIndex(currentIndex);
                    GamePiece movingPiece = GetPieceCommon(selectedPieces, result);
                    if (movingPiece.pieceNumber == -1) return;

                    List<GameIndex> indexes = myBoard.UpdateRoll(movingPiece, roll);
                    List<GameGrid> resultGrid = IndexToGrid(indexes);
                    List<Vector3> resultVectors = ListToVector(resultGrid);
                    gamePieces[movingPiece].GetComponent<Piece>().SetPos(resultVectors);

                    hasKilled = false;
                    GameIndex lastIndex = indexes[indexes.Count - 1];
                    if (lastIndex.posType == Constants.NORMAL_POS && !ContainsSafe(lastIndex))
                    {
                        List<GamePiece> removingPieces = myBoard.GetPieceAtIndex(lastIndex);
                        foreach (GamePiece remove in removingPieces)
                        {
                            if (remove.player != players[currentPlayer])
                            {
                                gamePieces[remove].transform.position = startPos[remove.player][remove.pieceNumber];
                                myBoard.UpdateKill(remove);
                                hasKilled = true;
                            }
                        }
                    }

                    hasReachedEnd = false;
                    if (lastIndex.posType == Constants.END_POS && lastIndex.pos == 5)
                    {
                        hasReachedEnd = true;
                        remainingPieces[players[currentPlayer]] -= 1;
                        if (remainingPieces[players[currentPlayer]] == 0)
                        {
                            playerNames[(int)players[currentPlayer]].GetComponent<Text>().text =
                                (playerRank.Count + 1).ToString() + ". " +
                                playerNames[(int)players[currentPlayer]].GetComponent<Text>().text;
                            playerRank[playerRank.Count + 1] = players[currentPlayer];
                            players.RemoveAt(currentPlayer);

                            if (players.Count == 1)
                                hasGameFinished = true;
                        }
                        currentPlayer %= players.Count;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    bool ContainsSafe(GameIndex index)
    {
        foreach (GameIndex temp in safePositions)
        {
            if (temp.pos == index.pos && temp.posType == index.posType)
                return true;
        }
        return false;
    }

    List<Vector3> ListToVector(List<GameGrid> gridList)
    {
        List<Vector3> result = new List<Vector3>();
        for (int i = 0; i < gridList.Count; i++)
        {
            result.Add(GridMapping.GridToTransform(gridList[i]));
        }
        return result;
    }

    List<GameGrid> IndexToGrid(List<GameIndex> indexes)
    {
        List<GameGrid> result = new List<GameGrid>();
        for (int i = 0; i < indexes.Count; i++)
        {
            if (indexes[i].posType == Constants.NORMAL_POS)
                result.Add(indexToGrid[indexes[i]]);
            else
                result.Add(indexToGridEnd[players[currentPlayer]][indexes[i]]);
        }
        return result;
    }

    GamePiece GetPieceCommon(List<GamePiece> selected, List<GamePiece> result)
    {
        foreach (GamePiece select in selected)
        {
            foreach (GamePiece resultPiece in result)
            {
                if (select.pieceNumber == resultPiece.pieceNumber && select.player == resultPiece.player)
                    return resultPiece;
            }
        }
        return new GamePiece() { pieceNumber = -1, player = Player.BLUE };
    }

    public void RollEnd()
    {
        if (result.Count == 0)
        {
            gameState = Constants.ROLL_DIE;
            currentPlayer++;
            currentPlayer %= players.Count;
            Message(players[currentPlayer]);
        }
        canClick = true;
    }

    public void MoveEnd()
    {
        gameState = Constants.ROLL_DIE;
        canClick = true;
        if (roll == 6 || hasKilled || hasReachedEnd) return;
        currentPlayer++;
        currentPlayer %= players.Count;
        Message(players[currentPlayer]);
    }

    void InitializeMapping()
    {
        var normalCells = NormalCellsGenerator.GenerateNormalCells();
        foreach (var (grid, index) in normalCells)
        {
            gridToIndex[grid] = index;
            indexToGrid[index] = grid;
        }

        foreach (Player p in playerData.Keys)
        {
            indexToGridEnd[p] = new Dictionary<GameIndex, GameGrid>();
            foreach (var (grid, index) in playerData[p])
            {
                gridToIndex[grid] = index;
                indexToGridEnd[p][index] = grid;
            }
        }
    }

    static readonly Dictionary<Player, (GameGrid grid, GameIndex index)[]> playerData = new Dictionary<Player, (GameGrid, GameIndex)[]>()
    {
        {
            Player.RED, new (GameGrid, GameIndex)[]
            {
                (new GameGrid(){ row = 7, col = 1 }, new GameIndex(){ posType = Constants.END_POS, pos = 0 }),
                (new GameGrid(){ row = 7, col = 2 }, new GameIndex(){ posType = Constants.END_POS, pos = 1 }),
                (new GameGrid(){ row = 7, col = 3 }, new GameIndex(){ posType = Constants.END_POS, pos = 2 }),
                (new GameGrid(){ row = 7, col = 4 }, new GameIndex(){ posType = Constants.END_POS, pos = 3 }),
                (new GameGrid(){ row = 7, col = 5 }, new GameIndex(){ posType = Constants.END_POS, pos = 4 }),
                (new GameGrid(){ row = 7, col = 6 }, new GameIndex(){ posType = Constants.END_POS, pos = 5 }),

                (new GameGrid(){ row = 1, col = 1 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 1, col = 2 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 1, col = 3 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 1, col = 4 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 2, col = 1 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 2, col = 2 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 2, col = 3 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 2, col = 4 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 3, col = 1 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 3, col = 2 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 3, col = 3 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 3, col = 4 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 4, col = 1 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 4, col = 2 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 4, col = 3 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 }),
                (new GameGrid(){ row = 4, col = 4 }, new GameIndex(){ posType = Constants.START_POS, pos = 0 })
            }
        },
    };
}
