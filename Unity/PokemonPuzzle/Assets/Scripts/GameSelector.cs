using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelector : GridObject
{
    private GameGrid gameGrid = null;

    private void Start()
    {
        gameGrid = MasterManager.instance.GameGrid;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Vector2Int.up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector2Int.down);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Swap();
        }

        transform.position = gameGrid.GetWorldPositionOfIndex(Index);
    }

    void Move(Vector2Int _dir)
    {
        int newIndexLeft = Index + gameGrid.Index2DTo1D(_dir);
        int newIndexRight = newIndexLeft + 1;

        if (!gameGrid.IsIndexValid(newIndexLeft) || !gameGrid.IsIndexValid(newIndexRight))
        {
            return;
        }

        if (!gameGrid.AreIndexsOnSameRow(newIndexLeft, newIndexRight))
        {
            return;
        }

        Index = newIndexLeft;
    }

    void Swap()
    {
        gameGrid.MakeMove(Index, Index + 1);
    }
}
