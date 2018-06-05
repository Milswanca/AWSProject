using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelector : GridObject
{
    private GameGrid gameGrid = null;

    private Vector2Int downIndex2D;
    private bool makingMove = false;

    private void Start()
    {
        gameGrid = MasterManager.instance.GameGrid;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            makingMove = true;
            downIndex2D = gameGrid.GetIndex2DFromWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButton(0) && makingMove)
        {
            Vector2Int newIndex = gameGrid.GetIndex2DFromWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            newIndex.y = downIndex2D.y;

            Debug.Log(newIndex);

            if (newIndex.x != downIndex2D.x)
            {
                gameGrid.MakeMove(gameGrid.Index2DTo1D(downIndex2D), gameGrid.Index2DTo1D(newIndex));
                makingMove = false;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            makingMove = false;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Swap();
        }
    }

    void Swap()
    {
        gameGrid.MakeMove(Index, Index + 1);
    }
}
