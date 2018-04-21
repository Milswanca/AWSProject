using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBlockState
{
	BS_Idle,
	BS_Disabled,
	BS_Swap,
	BS_Pop,
	BS_Fall,
	BS_Death
}

public class GameBlock : GridObject
{
    [SerializeField]
    private EBlockType blockType;
    public EBlockType BlockType { get { return blockType; } private set { blockType = value; } }

	[SerializeField]
	private int scoreValue = 100;
	public int ScoreValue { get { return scoreValue; } private set { scoreValue = value; } }

	private GameGrid gameGrid = null;

	private BlockEvent currentEvent = null;
	private SpriteRenderer spriteRenderer = null;

	[SerializeField]
	private Sprite greyedSprite = null;
	[SerializeField]
	private Sprite normalSprite = null;
	[SerializeField]
	private Sprite poppedSprite = null;
	[SerializeField]
	private Sprite deathSprite = null;

	public bool CanInteract { get { return BlockState != EBlockState.BS_Death && BlockState != EBlockState.BS_Disabled && BlockState != EBlockState.BS_Pop && BlockState != EBlockState.BS_Fall; } }

	private EBlockState blockState;
	public EBlockState BlockState
	{
		get
		{
			return blockState;
		}
		set
		{
			EBlockState oldState = blockState;

			blockState = value;

			switch(blockState)
			{
				case EBlockState.BS_Disabled:
					spriteRenderer.sprite = greyedSprite;
					break;

				case EBlockState.BS_Pop:
					spriteRenderer.sprite = poppedSprite;
					break;

				case EBlockState.BS_Death:
					spriteRenderer.sprite = deathSprite;
					break;

				default:
					spriteRenderer.sprite = normalSprite;
					break;
			}

			gameGrid.BlockStateChanged(this, oldState, blockState);
		}
	}

	public override int Index
	{
		get
		{
			return base.Index;
		}

		set
		{
			base.Index = value;

			if (BlockState != EBlockState.BS_Idle)
			{
				return;
			}

			if (gameGrid.Index1DTo2D(Index).y == 0)
			{
				spriteRenderer.sprite = greyedSprite;
			}
			else
			{
				spriteRenderer.sprite = normalSprite;
			}
		}
	}

	void Awake()
    {
        gameGrid = MasterManager.instance.GameGrid;
		spriteRenderer = GetComponent<SpriteRenderer>();
		BlockState = EBlockState.BS_Idle;
    }

    public void GridUpdate(float _deltaTime)
    {
        ProcessEvent(_deltaTime);
    }

    public void Move(int _index)
    {
        Index = _index;
    }

	public void PlayPopBegin()
	{
		spriteRenderer.sprite = poppedSprite;
	}

    public void Pop()
    {
        gameObject.SetActive(false);
    }

    public void Swap(int _withIndex)
    {
		EndCurrentEvent();

        SwapEvent swapEvent = new SwapEvent();
		swapEvent.TargetLocation = gameGrid.GetWorldPositionOfIndex(_withIndex);
        swapEvent.SizeCurve = MasterManager.instance.BlockEvents.SwapScaleAnimCurve;
        swapEvent.Time = MasterManager.instance.BlockEvents.SwapEventTime;
		currentEvent = swapEvent;
		currentEvent.Start(this);
	}

	public void Fall()
	{
		EndCurrentEvent();

		FallEvent fallEvent = new FallEvent();
		fallEvent.Speed = MasterManager.instance.BlockEvents.FallEventSpeed;
		fallEvent.GameGrid = gameGrid;
		currentEvent = fallEvent;
		currentEvent.Start(this);
	}

	public void EndCurrentEvent()
	{
		if(currentEvent != null)
		{
			currentEvent.OnEnded();
			currentEvent = null;
			BlockState = EBlockState.BS_Idle;
		}
	}

	public void QueuePopEvent()
	{
		BlockState = EBlockState.BS_Pop;
	}

	public void Die()
	{
		BlockState = EBlockState.BS_Death;
	}

	private void ProcessEvent(float _deltaTime)
    {
        if(currentEvent != null)
        {
			currentEvent.Update(_deltaTime);

			if (currentEvent.ShouldEnd())
            {
				EndCurrentEvent();
			}
        }
        else
        {
            transform.position = gameGrid.GetWorldPositionOfIndex(Index);
        }
    }
}

[SerializeField]
public class BlockEvent
{
	protected float timeEventStarted = 0.0f;
	protected float elapsedEventTime { get { return Time.fixedTime - timeEventStarted; } }
	protected GameBlock gameBlock = null;

	public virtual void Start(GameBlock _block)
	{
		gameBlock = _block;
		timeEventStarted = Time.fixedTime;
	}

	public virtual void Update(float _deltaTime) { }
	public virtual bool ShouldEnd() { return true; }
	public virtual void OnEnded() { }
}

public class SwapEvent : BlockEvent
{
	public float Time;
	public Vector3 TargetLocation;
	public AnimationCurve SizeCurve;

	private Vector3 animStartLocation;
	private Vector3 startScale;

	public override void Start(GameBlock _block)
	{
		base.Start(_block);

		gameBlock.BlockState = EBlockState.BS_Swap;

		startScale = gameBlock.transform.localScale;
		animStartLocation = gameBlock.transform.position;
	}

	public override void Update(float _deltaTime)
	{
		base.Update(_deltaTime);

		float scale = SizeCurve.Evaluate(elapsedEventTime / Time);
		gameBlock.transform.localScale = startScale * scale;

		gameBlock.transform.position = Vector3.Lerp(animStartLocation, TargetLocation, elapsedEventTime / Time);
	}

	public override bool ShouldEnd() { return elapsedEventTime >= Time; }

	public override void OnEnded()
	{
		base.OnEnded();
		gameBlock.transform.localScale = startScale;
		gameBlock.transform.position = TargetLocation;
	}
}

public class FallEvent : BlockEvent
{
	public float Speed;
	public GameGrid GameGrid;

	private float timePerGridChange = 0.0f;
	private float timeTillGridChange = 0.0f;

	public override void Start(GameBlock _block)
	{
		base.Start(_block);

		if(ShouldEnd())
		{
			gameBlock.EndCurrentEvent();
			return;
		}

		timePerGridChange = GameGrid.gridSpaceSize / Mathf.Abs(Speed);
		timeTillGridChange = timePerGridChange;

		GameGrid.SwapIndex(gameBlock.Index, gameBlock.Index - GameGrid.width);

		gameBlock.BlockState = EBlockState.BS_Fall;
	}

	public override void Update(float _deltaTime)
	{
		base.Update(_deltaTime);

		timeTillGridChange -= _deltaTime;

		gameBlock.transform.position -= Vector3.down * _deltaTime * -Mathf.Abs(Speed);

		while (gameBlock.transform.position.y < GameGrid.GetWorldPositionOfIndex(gameBlock.Index - GameGrid.width).y && !ShouldEnd())
		{
			GameGrid.SwapIndex(gameBlock.Index, gameBlock.Index - GameGrid.width);
			timeTillGridChange += timePerGridChange;
		}
	}

	public override bool ShouldEnd()
	{
		GameBlock below = GameGrid.GetGameBlock(gameBlock.Index - GameGrid.width);

		if(below)
		{
			return below.BlockState != EBlockState.BS_Fall;
		}

		return false;
	}

	public override void OnEnded()
	{
		base.OnEnded();

		gameBlock.transform.position = GameGrid.GetWorldPositionOfIndex(gameBlock.Index);
	}
}