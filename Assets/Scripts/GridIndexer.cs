using System.Collections.Generic;
using UnityEngine;

public static class GridIndexer
{
    public static Dictionary<GameIndex, GameGrid> indexToGrid = new Dictionary<GameIndex, GameGrid>();

    public static GameIndex GetGridIndex(GameGrid grid, List<Player> players, int currentPlayer, 
        Dictionary<GameGrid, GameIndex> gridToIndex, Dictionary<Player, Dictionary<GameIndex, GameGrid>> indexToGridEnd)
    {
        Dictionary<GameGrid, GameIndex> temp = new Dictionary<GameGrid, GameIndex>()
        {
            { new GameGrid() { row = 6, col = 1 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 0 } },
            { new GameGrid() { row = 6, col = 2 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 1 } },
            { new GameGrid() { row = 6, col = 3 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 2 } },
            { new GameGrid() { row = 6, col = 4 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 3 } },
            { new GameGrid() { row = 6, col = 5 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 4 } },
            { new GameGrid() { row = 5, col = 6 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 5 } },
            { new GameGrid() { row = 4, col = 6 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 6 } },
            { new GameGrid() { row = 3, col = 6 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 7 } },
            { new GameGrid() { row = 2, col = 6 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 8 } },
            { new GameGrid() { row = 1, col = 6 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 9 } },
            { new GameGrid() { row = 0, col = 6 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 10 } },
            { new GameGrid() { row = 0, col = 7 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 11 } },
            { new GameGrid() { row = 0, col = 8 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 12 } },
            { new GameGrid() { row = 1, col = 8 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 13 } },
            { new GameGrid() { row = 2, col = 8 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 14 } },
            { new GameGrid() { row = 3, col = 8 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 15 } },
            { new GameGrid() { row = 4, col = 8 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 16 } },
            { new GameGrid() { row = 5, col = 8 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 17 } },
            { new GameGrid() { row = 6, col = 9 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 18 } },
            { new GameGrid() { row = 6, col = 10}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 19 } },
            { new GameGrid() { row = 6, col = 11}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 20 } },
            { new GameGrid() { row = 6, col = 12}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 21 } },
            { new GameGrid() { row = 6, col = 13}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 22 } },
            { new GameGrid() { row = 6, col = 14}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 23 } },
            { new GameGrid() { row = 7, col = 14}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 24 } },
            { new GameGrid() { row = 8, col = 14}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 25 } },
            { new GameGrid() { row = 8, col = 13}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 26 } },
            { new GameGrid() { row = 8, col = 12}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 27 } },
            { new GameGrid() { row = 8, col = 11}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 28 } },
            { new GameGrid() { row = 8, col = 10}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 29 } },
            { new GameGrid() { row = 8, col = 9 }, new GameIndex() { posType = Constants.NORMAL_POS, pos = 30 } },
            { new GameGrid() { row = 9 , col = 8}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 31 } },
            { new GameGrid() { row = 10, col = 8}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 32 } },
            { new GameGrid() { row = 11, col = 8}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 33 } },
            { new GameGrid() { row = 12, col = 8}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 34 } },
            { new GameGrid() { row = 13, col = 8}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 35 } },
            { new GameGrid() { row = 14, col = 8}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 36 } },
            { new GameGrid() { row = 14, col = 7}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 37 } },
            { new GameGrid() { row = 14, col = 6}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 38 } },
            { new GameGrid() { row = 13, col = 6}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 39 } },
            { new GameGrid() { row = 12, col = 6}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 40 } },
            { new GameGrid() { row = 11, col = 6}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 41 } },
            { new GameGrid() { row = 10, col = 6}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 42 } },
            { new GameGrid() { row = 9 , col = 6}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 43 } },
            { new GameGrid() { row = 8 , col = 5}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 44 } },
            { new GameGrid() { row = 8,  col = 4}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 45 } },
            { new GameGrid() { row = 8,  col = 3}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 46 } },
            { new GameGrid() { row = 8,  col = 2}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 47 } },
            { new GameGrid() { row = 8,  col = 1}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 48 } },
            { new GameGrid() { row = 8,  col = 0}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 49 } },
            { new GameGrid() { row = 7,  col = 0}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 50 } },
            { new GameGrid() { row = 6,  col = 0}, new GameIndex() { posType = Constants.NORMAL_POS, pos = 51 } },
        };

        foreach (KeyValuePair<GameGrid, GameIndex> pair in temp)
        {
            gridToIndex[pair.Key] = pair.Value;
            indexToGrid[pair.Value] = pair.Key;
        }
        GameIndex outresult;
        if (temp.TryGetValue(grid, out outresult))
        {
            return outresult;
        }

        switch (players[currentPlayer])
        {
            case Player.RED:
                temp = new Dictionary<GameGrid, GameIndex>()
                {
                    { new GameGrid() { row = 7, col = 1}, new GameIndex() { posType = Constants.END_POS, pos = 0 } },
                    { new GameGrid() { row = 7, col = 2}, new GameIndex() { posType = Constants.END_POS, pos = 1 } },
                    { new GameGrid() { row = 7, col = 3}, new GameIndex() { posType = Constants.END_POS, pos = 2 } },
                    { new GameGrid() { row = 7, col = 4}, new GameIndex() { posType = Constants.END_POS, pos = 3 } },
                    { new GameGrid() { row = 7, col = 5}, new GameIndex() { posType = Constants.END_POS, pos = 4 } },
                    { new GameGrid() { row = 7, col = 6}, new GameIndex() { posType = Constants.END_POS, pos = 5 } },
                    { new GameGrid() { row = 1, col = 1}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 1, col = 2}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 1, col = 3}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 1, col = 4}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 2, col = 1}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 2, col = 2}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 2, col = 3}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 2, col = 4}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 3, col = 1}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 3, col = 2}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 3, col = 3}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 3, col = 4}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 4, col = 1}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 4, col = 2}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 4, col = 3}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 4, col = 4}, new GameIndex() { posType = Constants.START_POS, pos = 0 } }
                };
                indexToGridEnd[Player.RED] = new Dictionary<GameIndex, GameGrid>();
                foreach (KeyValuePair<GameGrid, GameIndex> pair in temp)
                {
                    gridToIndex[pair.Key] = pair.Value;
                    indexToGridEnd[Player.RED][pair.Value] = pair.Key;
                }
                if (temp.TryGetValue(grid, out outresult))
                {
                    return outresult;
                }
                break;

            case Player.BLUE:
                temp = new Dictionary<GameGrid, GameIndex>()
                {
                    { new GameGrid() { row = 1, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 0 } },
                    { new GameGrid() { row = 2, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 1 } },
                    { new GameGrid() { row = 3, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 2 } },
                    { new GameGrid() { row = 4, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 3 } },
                    { new GameGrid() { row = 5, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 4 } },
                    { new GameGrid() { row = 6, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 5 } },
                    { new GameGrid() { row = 1, col = 10}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 1, col = 11}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 1, col = 12}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 1, col = 13}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 2, col = 10}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 2, col = 11}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 2, col = 12}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 2, col = 13}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 3, col = 10}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 3, col = 11}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 3, col = 12}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 3, col = 13}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 4, col = 10}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 4, col = 11}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 4, col = 12}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 4, col = 13}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                };
                indexToGridEnd[Player.BLUE] = new Dictionary<GameIndex, GameGrid>();
                foreach (KeyValuePair<GameGrid, GameIndex> pair in temp)
                {
                    gridToIndex[pair.Key] = pair.Value;
                    indexToGridEnd[Player.BLUE][pair.Value] = pair.Key;
                }
                if (temp.TryGetValue(grid, out outresult))
                {
                    return outresult;
                }
                break;

            case Player.YELLOW:
                temp = new Dictionary<GameGrid, GameIndex>()
                {
                    { new GameGrid() { row = 7, col = 13}, new GameIndex() { posType = Constants.END_POS, pos = 0 } },
                    { new GameGrid() { row = 7, col = 12}, new GameIndex() { posType = Constants.END_POS, pos = 1 } },
                    { new GameGrid() { row = 7, col = 11}, new GameIndex() { posType = Constants.END_POS, pos = 2 } },
                    { new GameGrid() { row = 7, col = 10}, new GameIndex() { posType = Constants.END_POS, pos = 3 } },
                    { new GameGrid() { row = 7, col = 9}, new GameIndex() { posType = Constants.END_POS, pos = 4 } },
                    { new GameGrid() { row = 7, col = 8}, new GameIndex() { posType = Constants.END_POS, pos = 5 } },
                    { new GameGrid() { row = 10, col = 10}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 10, col = 11}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 10, col = 12}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 10, col = 13}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 11, col = 10}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 11, col = 11}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 11, col = 12}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 11, col = 13}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 12, col = 10}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 12, col = 11}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 12, col = 12}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 12, col = 13}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 13, col = 10}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 13, col = 11}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 13, col = 12}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 13, col = 13}, new GameIndex() { posType = Constants.START_POS, pos = 0 } }
                };
                indexToGridEnd[Player.YELLOW] = new Dictionary<GameIndex, GameGrid>();
                foreach (KeyValuePair<GameGrid, GameIndex> pair in temp)
                {
                    gridToIndex[pair.Key] = pair.Value;
                    indexToGridEnd[Player.YELLOW][pair.Value] = pair.Key;
                }
                if (temp.TryGetValue(grid, out outresult))
                {
                    return outresult;
                }
                break;

            case Player.GREEN:
                temp = new Dictionary<GameGrid, GameIndex>()
                {
                    { new GameGrid() { row = 13, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 0 } },
                    { new GameGrid() { row = 12, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 1 } },
                    { new GameGrid() { row = 11, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 2 } },
                    { new GameGrid() { row = 10, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 3 } },
                    { new GameGrid() { row = 9, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 4 } },
                    { new GameGrid() { row = 8, col = 7}, new GameIndex() { posType = Constants.END_POS, pos = 5 } },
                    { new GameGrid() { row = 10, col = 1}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 10, col = 2}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 10, col = 3}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 10, col = 4}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 11, col = 1}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 11, col = 2}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 11, col = 3}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 11, col = 4}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 12, col = 1}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 12, col = 2}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 12, col = 3}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 12, col = 4}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 13, col = 1}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 13, col = 2}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 13, col = 3}, new GameIndex() { posType = Constants.START_POS, pos = 0 } },
                    { new GameGrid() { row = 13, col = 4}, new GameIndex() { posType = Constants.START_POS, pos = 0 } }
                };
                indexToGridEnd[Player.GREEN] = new Dictionary<GameIndex, GameGrid>();
                foreach (KeyValuePair<GameGrid, GameIndex> pair in temp)
                {
                    gridToIndex[pair.Key] = pair.Value;
                    indexToGridEnd[Player.GREEN][pair.Value] = pair.Key;
                }
                if (temp.TryGetValue(grid, out outresult))
                {
                    return outresult;
                }
                break;
        }
        return new GameIndex() { pos = -1 };
    }

    public static GameGrid TransformToGrid(Vector3 temp)
    {
        return new GameGrid() { row = -(int)temp.y, col = (int)temp.x };
    }

    public static Vector3 GridToTransform(GameGrid temp)
    {
        return new Vector3() { x = temp.col + 0.5f, y = -(temp.row + 0.5f), z = -1 };
    }
}
