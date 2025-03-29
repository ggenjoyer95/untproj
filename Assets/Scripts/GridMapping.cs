using UnityEngine;

public static class GridMapping
{
    public static GameGrid TransformToGrid(Vector3 worldPos)
    {
        return new GameGrid() { row = -(int)worldPos.y, col = (int)worldPos.x };
    }

    public static Vector3 GridToTransform(GameGrid grid)
    {
        return new Vector3(grid.col - 0.5f, -(grid.row + 0.5f), -1);
    }
}
