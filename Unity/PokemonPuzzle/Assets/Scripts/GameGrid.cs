using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
	public int width = 9;

	public int height = 20;

	public int gridSpaceSize = 1;

	[SerializeField]
	private int startingBlocks = 30;

	[SerializeField]
	private int numberOfDifferentBlocks = 4;

	[SerializeField]
	private float raiseTime = 7;

	[SerializeField]
	private float speedIncreaseEverySecond = 0.1f;

	[SerializeField]
	GameSelector gameSelector = null;

    [SerializeField]
    Transform BlocksRoot = null;

    private UIGame uiGame = null;

    public static GameGrid Get()
    {
        return MasterManager.instance.GameGrid;
    }

	private bool raiseUp
	{
		get
		{
			return raiseCounter == 0;
		}
		set
		{
			raiseCounter += (value) ? -1 : 1;
		}
	}

	private int raiseCounter = 0;

	private int score = 0;
	private int chain = 0;

	private GameBlock[] playGrid;
	private GameBlock[] defaultGameBlocks = null;
	private List<GameBlock> dirtyBlocks = null;
	private List<GameBlock> blocksInChain = null;
	private float raisedOffset = 0.0f;

	void Awake()
	{
		playGrid = new GameBlock[width * height];
		dirtyBlocks = new List<GameBlock>();
		blocksInChain = new List<GameBlock>();

        defaultGameBlocks = new GameBlock[numberOfDifferentBlocks];
        System.Array.Copy(MasterManager.instance.BlockTypes.defaultBlockPrefabs, defaultGameBlocks, numberOfDifferentBlocks);
        uiGame = PanelManager.Get().GetScreen(EGameScreens.GS_Game).GetComponent<UIGame>();
    }

    // Use this for initialization
    void OnEnable()
	{
        Reset();
    }

	private void Update()
	{
		RaiseUp();
		CleanDirtyBlocks();
		UpdateBlocks();
	}

	void UpdateBlocks()
	{
		float delta = Time.deltaTime;

		//Update from bottom up
		for(int i = 0; i < playGrid.Length; ++i)
		{
			if(playGrid[i])
			{
				playGrid[i].GridUpdate(delta);
			}
		}
	}

	private void RaiseUp()
	{
		raiseTime -= Time.deltaTime * speedIncreaseEverySecond;

		if (Input.GetKey(KeyCode.LeftControl))
		{
			raisedOffset += Time.deltaTime / 0.1f;
		}
		if (raiseUp)
		{
			raisedOffset += Time.deltaTime / raiseTime;
		}

		if (raisedOffset > gridSpaceSize)
		{
			raisedOffset -= gridSpaceSize;
			for (int i = playGrid.Length - 1; i >= 0; --i)
			{
				if (i + width >= width * height) { continue; }
				if (!playGrid[i]) { continue; }

				if(i / width >= height - 2)
				{
					GameOver();
                    return;
				}

				SwapIndex(i, i + width);
			}

			gameSelector.Index += width;

			MarkRowDirty(1);
			GenerateNewRow();
		}
	}

	public void MakeMove(int _swapIndex, int _withIndex)
	{
        if(!AreIndexsOnSameRow(_swapIndex, _withIndex)) { return; }
        if(Index1DTo2D(_swapIndex).y == 0 || Index1DTo2D(_withIndex).y == 0) { return; }

		GameBlock swap = GetGameBlock(_swapIndex);
		GameBlock with = GetGameBlock(_withIndex);

		if (!swap && !with) { return; }
		if ((swap && !swap.CanInteract) || (with && !with.CanInteract)) { return; }

		//Swap the two indexs
		if (playGrid[_swapIndex])
		{
			playGrid[_swapIndex].Swap(_withIndex);
		}
		if (playGrid[_withIndex])
		{
			playGrid[_withIndex].Swap(_swapIndex);
		}

		SwapIndex(_swapIndex, _withIndex);

		RecursiveFall(_swapIndex + width);
		RecursiveFall(_withIndex + width);
	}

	public bool IsBlockBelow(int _index)
	{
		//Am i on the bottom index
		if (_index / width == 1) { return true; }

		GameBlock blockBelow = GetGameBlock(_index - width);

		return blockBelow != null;
	}

	public void MarkBlockDirty(GameBlock _block)
	{
		if (!_block) { return; }

		if (!dirtyBlocks.Contains(_block))
		{
			dirtyBlocks.Add(_block);
		}
	}

	private void CleanDirtyBlocks()
	{
		List<GameBlock> matches = new List<GameBlock>();

		bool countsAsChain = false;

		//First matches start the chain
		if(chain == 0)
		{
			countsAsChain = true;
		}

		while (dirtyBlocks.Count > 0)
		{
			CheckMatches(ref matches, dirtyBlocks[0]);

			if(blocksInChain.Contains(dirtyBlocks[0]))
			{
				blocksInChain.Remove(dirtyBlocks[0]);
				countsAsChain = true;
			}

			dirtyBlocks.RemoveAt(0);
		}

		if (matches.Count > 0)
		{
			StartCoroutine(PopMatches(matches, countsAsChain));
		}
		else if (countsAsChain && blocksInChain.Count == 0)
		{
			EndChain();
		}

		dirtyBlocks.Clear();
	}

	private IEnumerator PopMatches(List<GameBlock> _matches, bool _countsAsChain)
	{
		raiseUp = false;

		_matches.Sort((p1, p2) => p2.Index.CompareTo(p1.Index));

		if(_countsAsChain)
		{
			IncrementChain();
		}

		//TODO: 4 Loop is real bad
		foreach (GameBlock i in _matches)
		{
			i.QueuePopEvent();
		}

		yield return new WaitForSeconds(0.3f);

		foreach (GameBlock i in _matches)
		{
			i.Pop();
			AddScore(i.ScoreValue);
			yield return new WaitForSeconds(0.1f);
		}

		foreach (GameBlock i in _matches)
		{
			playGrid[i.Index] = null;
		}

		blocksInChain.Clear();

		foreach (GameBlock i in _matches)
		{
			RecursiveFall(i.Index + width, true);
			Destroy(i);
		}

		if (_countsAsChain && blocksInChain.Count == 0)
		{
			EndChain();
		}

		raiseUp = true;
	}

	void IncrementChain()
	{
		chain++;
        uiGame.SetTextChain(chain);
	}

	void EndChain()
	{
		chain = 0;
        uiGame.SetTextChain(chain);
	}

	public void AddScore(int _score)
	{
		score += _score * chain;
        uiGame.SetTextScore(score);
	}

	private void CheckMatches(ref List<GameBlock> _matches, GameBlock _block)
	{
		List<GameBlock> horMatches = new List<GameBlock>();
		List<GameBlock> vertMatches = new List<GameBlock>();

		//Check in both Directions
		RecursiveCheckMatchBranch(ref horMatches, _block.Index, Vector2Int.up);
		RecursiveCheckMatchBranch(ref horMatches, _block.Index, Vector2Int.down);
		RecursiveCheckMatchBranch(ref vertMatches, _block.Index, Vector2Int.left);
		RecursiveCheckMatchBranch(ref vertMatches, _block.Index, Vector2Int.right);

		if (horMatches.Count >= 3) { _matches.AddRange(horMatches); }
		if (vertMatches.Count >= 3) { _matches.AddRange(vertMatches); }
	}

	private void RecursiveCheckMatchBranch(ref List<GameBlock> _matches, int _runningIndex, Vector2Int _dir)
	{
		if (!IsIndexValid(_runningIndex)) { return; }

		GameBlock me = GetGameBlock(_runningIndex);
		if (me == null) { return; }

		_matches.AddUnique(me);

		int nextIndex = _runningIndex + Index2DTo1D(_dir);

		//If im checking left and right and im about to exceed over to the next line, quit
		if (_dir.x != 0 && !AreIndexsOnSameRow(_runningIndex, nextIndex))
		{
			return;
		}

		GameBlock next = GetGameBlock(nextIndex);

		if (next != null && next.BlockType == me.BlockType && next.BlockState == EBlockState.BS_Idle)
		{
			RecursiveCheckMatchBranch(ref _matches, nextIndex, _dir);
		}
	}

	private void RecursiveFall(int _runningIndex, bool _countsAsChain = false)
	{
		GameBlock me = GetGameBlock(_runningIndex);

		if (!me) { return; }
		if (me.BlockState != EBlockState.BS_Idle) { return; }

		//If im grounded then return
		if (IsBlockBelow(_runningIndex)) { return; }

		if (_countsAsChain)
		{
			blocksInChain.Add(playGrid[_runningIndex]);
		}

		me.Fall();

		//If theres a block above me, make that fall too
		RecursiveFall(_runningIndex + width, _countsAsChain);
	}

	public void SwapIndex(int _fromIndex, int _toIndex)
	{
		GameBlock from = GetGameBlock(_fromIndex);
		GameBlock to = GetGameBlock(_toIndex);

		if (!from && !to) { return; }

		if (playGrid[_fromIndex] != null)
		{
			playGrid[_fromIndex].Index = _toIndex;
		}

		if (playGrid[_toIndex] != null)
		{
			playGrid[_toIndex].Index = _fromIndex;
		}

		UtilityFunctionLibrary.Swap(ref playGrid, _fromIndex, _toIndex);
	}

	public void BlockStateChanged(GameBlock _block, EBlockState _oldState, EBlockState _newState)
	{
		if (_oldState == EBlockState.BS_Swap)
		{
			RecursiveFall(_block.Index);
		}

		if (_newState == EBlockState.BS_Idle)
		{
			MarkBlockDirty(_block);
		}
	}

	private void PlaceStartingBlocks()
	{
		//+ width for creating the bottom hidden row
		GameBlock[] blocks = new GameBlock[defaultGameBlocks.Length];
		System.Array.Copy(defaultGameBlocks, blocks, blocks.Length);
		for (int i = width; i < startingBlocks + width; ++i)
		{
			if (blocks.Length <= 2)
			{
				System.Array.Resize(ref blocks, defaultGameBlocks.Length);
				System.Array.Copy(defaultGameBlocks, blocks, blocks.Length);
			}

			GameBlock toPlace = UtilityFunctionLibrary.SpliceRandom(ref blocks);
			playGrid[i] = Instantiate(toPlace, BlocksRoot);
			playGrid[i].Move(i);

			MarkBlockDirty(playGrid[i]);
		}

		GenerateNewRow();
	}

	private void GenerateNewRow()
	{
		//+ width for creating the bottom hidden row
		GameBlock[] blocks = new GameBlock[defaultGameBlocks.Length];
		System.Array.Copy(defaultGameBlocks, blocks, blocks.Length);
		for (int i = 0; i < width; ++i)
		{
			if (blocks.Length <= 2)
			{
				System.Array.Resize(ref blocks, defaultGameBlocks.Length);
				System.Array.Copy(defaultGameBlocks, blocks, blocks.Length);
			}

			GameBlock toPlace = UtilityFunctionLibrary.SpliceRandom(ref blocks);
			playGrid[i] = Instantiate(toPlace, BlocksRoot);
			playGrid[i].Move(i);

			MarkBlockDirty(playGrid[i]);
		}
	}

	private void MarkRowDirty(int _row)
	{
		for (int i = 0; i < playGrid.Length; ++i)
		{
			//Mark the new row as dirty
			if (i / width == _row)
			{
				MarkBlockDirty(playGrid[i]);
			}
		}
	}

	void Shuffle()
	{

	}

	void GameOver()
	{
        raiseUp = false;
        gameSelector.gameObject.SetActive(false);
        StartCoroutine(KillBlocks());
	}

    void Reset()
    {
        raiseCounter = 0;
        score = 0;
        chain = 0;

        uiGame.SetTextScore(score);
        uiGame.SetTextChain(chain);

        gameSelector.gameObject.SetActive(true);
        gameSelector.Index = width;

        PlaceStartingBlocks();
    }

	IEnumerator KillBlocks()
	{
		for(int i = 0; i < playGrid.Length; ++i)
		{
			if(playGrid[i])
			{
				playGrid[i].Die();
				yield return new WaitForEndOfFrame();
			}
		}

        yield return new WaitForSeconds(2.0f);

        OnAllBlocksKilled();
	}

    void OnAllBlocksKilled()
    {
        DatabaseHandler.Get().PostScore(score, ScorePosted);
    }

    void ScorePosted(bool _success, DatabaseHandler.ErrorResult _error)
    {
        ReturnToMenu();
    }

    void ReturnToMenu()
    {
        BlocksRoot.gameObject.DeleteChildren();
        PanelManager.Get().ChangePanels(EGameScreens.GS_ViewHighscores);
        MasterManager.instance.ChangeGameState(EGameState.GS_Menu);
    }

	public GameBlock GetGameBlock(int _index)
	{
		return playGrid[_index];
	}

	public GameBlock GetGameBlock(int _horIndex, int _vertIndex)
	{
		return playGrid[_horIndex * width + _vertIndex];
	}

	public int Index2DTo1D(Vector2Int _2DPoint)
	{
		return _2DPoint.x + (_2DPoint.y * width);
	}

	public Vector2Int Index1DTo2D(int _1DPoint)
	{
		return new Vector2Int(_1DPoint % width, _1DPoint / width);
	}

	public int GetIndexBelow(int _index)
	{
		return _index - width;
	}

    public Vector3 GetWorldPositionOfIndex(int _index)
    {
        return new Vector3((_index % width) * gridSpaceSize, ((_index / width) * gridSpaceSize) + raisedOffset, 0);
    }

    public int GetIndexFromWorldPosition(Vector3 _pos)
    {
        _pos.x += gridSpaceSize * 0.5f;
        _pos.y += -raisedOffset + (gridSpaceSize * 0.5f);
        int x = Mathf.FloorToInt(_pos.x / gridSpaceSize);
        int y = Mathf.FloorToInt(_pos.y / gridSpaceSize);
        return Index2DTo1D(new Vector2Int(x, y));
    }

    public Vector2Int GetIndex2DFromWorldPosition(Vector3 _pos)
    {
        _pos.x += gridSpaceSize * 0.5f;
        _pos.y += -raisedOffset + (gridSpaceSize * 0.5f);
        int x = Mathf.FloorToInt(_pos.x / gridSpaceSize);
        int y = Mathf.FloorToInt(_pos.y / gridSpaceSize);
        return new Vector2Int(x, y);
    }

    public bool AreIndexsOnSameRow(int _indexA, int _indexB)
	{
		return _indexA / width == _indexB / width;
	}

	public bool IsIndexValid(int _index)
	{
		return _index >= width && _index < playGrid.Length;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		float xExtent = width * gridSpaceSize;
		float yExtent = height * gridSpaceSize;
		Gizmos.DrawCube(transform.position + new Vector3(xExtent * 0.5f, yExtent * 0.5f, 1), new Vector3(xExtent, yExtent, 0));
	}
#endif
}