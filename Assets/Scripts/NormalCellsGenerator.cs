using System.Collections.Generic;
using UnityEngine;

public static class NormalCellsGenerator
{
    public static List<(GameGrid grid, GameIndex index)> GenerateNormalCells()
    {
        var result = new List<(GameGrid, GameIndex)>();
        int pos = 0;
        GameGrid current = new GameGrid() { row = 6, col = 1 };
        result.Add((new GameGrid() { row = current.row, col = current.col },
                    new GameIndex() { posType = Constants.NORMAL_POS, pos = pos++ }));

        var segments = new List<(int count, Vector2 initial, Vector2 repeat)>()
        {
            (5, new Vector2(1, 0), new Vector2(1, 0)),
            (6, new Vector2(1, -1), new Vector2(0, -1)),
            (2, new Vector2(1, 0), new Vector2(1, 0)),
            (5, new Vector2(0, 1), new Vector2(0, 1)),
            (6, new Vector2(1, 1), new Vector2(1, 0)),
            (2, new Vector2(0, 1), new Vector2(0, 1)),
            (5, new Vector2(-1, 0), new Vector2(-1, 0)),
            (6, new Vector2(-1, 1), new Vector2(0, 1)),
            (2, new Vector2(-1, 0), new Vector2(-1, 0)),
            (5, new Vector2(0, -1), new Vector2(0, -1)),
            (6, new Vector2(-1, -1), new Vector2(-1, 0)),
            (2, new Vector2(0, -1), new Vector2(0, -1))
        };

        foreach (var seg in segments)
        {
            current = new GameGrid() 
            { 
                row = current.row + (int)seg.initial.y, 
                col = current.col + (int)seg.initial.x 
            };
            result.Add((new GameGrid() { row = current.row, col = current.col },
                        new GameIndex() { posType = Constants.NORMAL_POS, pos = pos++ }));
            for (int i = 1; i < seg.count; i++)
            {
                current = new GameGrid() 
                { 
                    row = current.row + (int)seg.repeat.y, 
                    col = current.col + (int)seg.repeat.x 
                };
                result.Add((new GameGrid() { row = current.row, col = current.col },
                            new GameIndex() { posType = Constants.NORMAL_POS, pos = pos++ }));
            }
        }
        return result;
    }
}
