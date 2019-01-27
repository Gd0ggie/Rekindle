﻿using UnityEngine;
using UnityEngine.Tilemaps;

public class Application : MonoBehaviour
{
	public static Application Inst;
	public ScreenSpaceDarkness ScreenSpaceDarkness;
	public bool EnableScreenSpaceDarkness = false;

	public World World;

	public Player PlayerOne;
	public Player PlayerTwo;

	public TileFinder TileFinder;

	public Tilemap Walls;
	public Tilemap Floors;

#if USE_XB1_CONTROLLERS
	readonly ControllerManager ControllerManager = new XboxControllerManager();
#else
	readonly ControllerManager ControllerManager = new PCControllerManager();
#endif
	
	void Start()
	{
		if (Inst != null)
		{
			Destroy(this);
		}
		else
		{
			Inst = this;
			InitializeGame();
		}
	}

	void InitializeGame()
	{
		Debug.Assert(ScreenSpaceDarkness != null, "No screen space darkness script attached to Application");
		ControllerManager.BothControllersInitializedEvent += OnBothControllersInitialized;

		Debug.Assert(Walls != null, "Walls Tilemap not assigned in Application");
		Debug.Assert(Floors != null, "Floors Tilemap not assigned in Application");
		World = new World(TileFinder, Walls, Floors);

		World.CreateRoom(Vector2Int.zero, Vector2Int.one * 5, new Vector2Int[] {new Vector2Int(2, 4), new Vector2Int(4, 2)});
	}

	void OnBothControllersInitialized()
	{
		PlayerOne.Initialize(ControllerManager.PlayerOneController);
		PlayerTwo.Initialize(ControllerManager.PlayerTwoController);
	}

	void Update()
	{
		ControllerManager.Update();

		PlayerOne.UpdatePlayerWithInput();
		PlayerTwo.UpdatePlayerWithInput();

		UpdateScreenSpaceShader();
	}

	void UpdateScreenSpaceShader()
	{
		if (EnableScreenSpaceDarkness)
		{
			if (!ScreenSpaceDarkness.gameObject.activeSelf)
			{
				ScreenSpaceDarkness.gameObject.SetActive(true);
			}
			ScreenSpaceDarkness.UpdateTextureWithLights();
		}
		else if (ScreenSpaceDarkness.gameObject.activeSelf)
		{
			ScreenSpaceDarkness.gameObject.SetActive(false);
		}
	}

		void Loader()
	{
		string loadedWorld = JsonSaver.Load("");
		int dataSeparator = loadedWorld.IndexOf("+", 0);
		string mapData = loadedWorld.Substring (0, dataSeparator);
		string characterData = loadedWorld.Substring (dataSeparator, loadedWorld.Length - dataSeparator);
	}
}
