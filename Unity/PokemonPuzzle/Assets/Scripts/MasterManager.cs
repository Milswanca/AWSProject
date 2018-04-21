using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour {

    [SerializeField]
    private GameGrid gameGrid = null;
    public GameGrid GameGrid { get { return gameGrid; } private set { gameGrid = value; } }

    [SerializeField]
    private DatabaseHandler databaseHandler = null;
    public DatabaseHandler DatabaseHandler { get { return databaseHandler; } private set { databaseHandler = value; } }

    [SerializeField]
    private UICanvas uiCanvas = null;
    public UICanvas UICanvas { get { return uiCanvas; } private set { uiCanvas = value; } }

    [SerializeField]
    private PanelManager panelManager = null;
    public PanelManager PanelManager { get { return panelManager; } private set { panelManager = value; } }

    [SerializeField]
    private PopupManager popupManager = null;
    public PopupManager PopupManager { get { return popupManager; } private set { popupManager = value; } }

    [SerializeField]
    private GameObject gameRoot = null;
    public GameObject GameRoot { get { return gameRoot; } private set { gameRoot = value; } }

    [SerializeField]
    private GameObject menuRoot = null;
    public GameObject MenuRoot { get { return menuRoot; } private set { menuRoot = value; } }

    [SerializeField]
    private string blockTypesAsset = "";
    private BlockTypes blockTypes = null;
    public BlockTypes BlockTypes { get { return blockTypes; } private set { blockTypes = value; } }

    [SerializeField]
    private string blockEventAsset = "";
    private BlockEventData blockEvents = null;
    public BlockEventData BlockEvents { get { return blockEvents; } private set { blockEvents = value; } }

    public EGameState GameState { get; private set; }

    public static MasterManager instance {get; private set;}

    void Awake()
    {
        if(instance != null)
        {
            throw new UnityException("Managers master already exists!");
        }

        instance = this;

        BlockTypes = Resources.Load<BlockTypes>(blockTypesAsset);
		BlockEvents = Resources.Load<BlockEventData>(blockEventAsset);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeGameState(EGameState _state)
    {
        if(GameState == _state) { return; }

        GameState = _state;

        switch(GameState)
        {
            case EGameState.GS_Game:
                MenuRoot.SetActive(false);
                GameRoot.SetActive(true);
                break;

            case EGameState.GS_Menu:
                GameRoot.SetActive(false);
                MenuRoot.SetActive(true);
                break;
        }
    }
}
