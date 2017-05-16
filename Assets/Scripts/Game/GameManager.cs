using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[HideInInspector]
	public int Round = 0;

	public List<CanvasGroup> CollapsablePanels;
	public List<RawImage> Images;
	public Camera camera;



	private PersistentData persistentData;
	private int currentIndex;

	private Player[] players;
	private int numberOfPlayers;

	private TileMap gameBoard;

	private enum CameraView : byte {
		MAIN = 0,
		DICE = 1
	}
	private CameraView cameraView = CameraView.MAIN;
	private bool cameraIsMoving = false;
	private List<Vector3> cameraViewCoords;


	void Start () {
		LoadPersistentData();
		InitGui();
		InitCamera();

		currentIndex = 0;
		ToggleCurrentPlayerPanels();
	}

	void Update()
	{

		//debug
		if (Input.GetKeyDown("space"))
		{
			//ToggleCurrentPlayerPanels();
			cameraIsMoving = true;

			if (cameraView == CameraView.DICE)
			{
				cameraView = CameraView.MAIN;
			} else {
				cameraView = cameraView = CameraView.DICE;
			}			
		}


		if (cameraIsMoving)
		{
			switch(cameraView)
			{
				case CameraView.MAIN: MoveCameraToMain(); break;
				case CameraView.DICE: MoveCameraToDice(); break;
			}
		}
	}

	///<summary>
	///	Loads persistent data into local player array
	///</summary>
	private void LoadPersistentData()
	{
		GameObject persistentGO = GameObject.Find("PersistentData");
		players = null;

		if (Debug.isDebugBuild && persistentGO == null)
		{
			players = DebugPlayers();
		}
		else
		{
			persistentData = persistentGO.GetComponent<PersistentData>();
			players = persistentData.Players;
			Destroy(persistentGO, 1f);
		}

		numberOfPlayers = players.Length;
			
		for (int i = 0; i < players.Length; i++)
		{
			Debug.Log(players[i].ToString());
		}
	}

	///<summary>
	///	Loads GUI with values of persistent data
	/// NOTE: if scene loads instantly without passing by StartGame, all "playing as" is gonna be scarlett (due to lazyness)
	///</summary>
	private void InitGui()
	{
		//Hide unused images
		for (int i = numberOfPlayers; i < Images.Count; i++)
		{
			Images[i].enabled = false;
		}

		for (int i = 0; i < numberOfPlayers; i++)
		{
			GameObject t1GO = GameObject.Find("PlayerName" + (i + 1));
			t1GO.GetComponent<Text>().text = players[i].Name;
			if (players[i].IsCpu)
			{
				t1GO.GetComponent<Text>().text += "\t(CPU)";
			}

			GameObject t2GO = GameObject.Find("PlayingAs" + (i + 1));

			if (players[i].Color == Color.white)
			{
				t2GO.GetComponent<Text>().text = "Mrs. White";
			} 
			else if (players[i].Color == Color.green)
			{
				t2GO.GetComponent<Text>().text = "Mr. Green";
			} 
			else if (players[i].Color == Color.blue)
			{
				t2GO.GetComponent<Text>().text = "Mrs. Peacock";
			} 
			else if (players[i].Color == Color.yellow)
			{
				t2GO.GetComponent<Text>().text = "Col. Mustard";
			}
			else if (players[i].Color == Color.magenta)
			{
				t2GO.GetComponent<Text>().text = "Prof. Plum";
			}
			else if (players[i].Color == Color.red)
			{
				t2GO.GetComponent<Text>().text = "Mrs. Scarlett";
			}	
		}
	}

	///<summary>
	///	Loads GUI with values of persistent data
	/// NOTE: if scene loads instantly without passing by StartGame, all "playing as" is gonna be scarlett (due to lazyness)
	///</summary>
	private void InitCamera()
	{
		cameraViewCoords = new List<Vector3>();
		cameraViewCoords.Add(new Vector3(16f, 45f, -3f)); //Main camera coords
		cameraViewCoords.Add(new Vector3(-4f, 45f, -3f)); //Dice camera coords
	}

	///<summary>
	///	Show panel of current player
	///</summary>
	private void ToggleCurrentPlayerPanels()
	{
		if (currentIndex == numberOfPlayers)
		{
			currentIndex = 0;
			CollapsablePanels[numberOfPlayers - 1].alpha = 0;
		}					

		if (currentIndex > 0)
			CollapseAllPanels();
		CollapsablePanels[currentIndex].alpha = 1;

		for (int i = currentIndex + 1; i < numberOfPlayers; i++) 
		{
			Images[i].transform.position += new Vector3(185f, 0, 0);
		}

		currentIndex++;
	}

	///<summary>
	///	Hides all panels (of current players)
	///</summary>
	private void CollapseAllPanels()
	{
		CollapsablePanels.ForEach(p => p.alpha = 0);

		for (int i = currentIndex; i < CollapsablePanels.Count; i++)
		{		
			Images[i].transform.position -= new Vector3(185f, 0, 0);
		}
	}

	#region eventhandlers

	public void ThrowDice()
	{
		Debug.Log("Throw Dice");
		//gameBoard.ThrowDice();
	}

	public void ShowCards()
	{
		Debug.Log("Show Cards");
		//gameBoard.ShowCards();
	}

	public void ShowNotebook()
	{
		Debug.Log("Show Notebook");
		//gameBoard.ShowNotebook();
	}

	public void MakeGuess()
	{
		Debug.Log("Make guess");
		//gameBoard.MakeGuess();
	}

	public void EndTurn()
	{
		Debug.Log("End turn");
		//gameBoard.EndTurn();
	}

	#endregion

	#region cameraSlider

	///<summary>
	///	Slide camera to mainview
	///</summary>
	private void MoveCameraToMain()
	{
		camera.transform.position = Vector3.Lerp(camera.transform.position, 
												 cameraViewCoords[(int)CameraView.MAIN], 
												 7f * Time.deltaTime);
												 
		if (camera.transform.position == cameraViewCoords[(int)CameraView.MAIN])
			cameraIsMoving = false;
	}

	///<summary>
	///	Slide camera to diceview
	///</summary>
	private void MoveCameraToDice()
	{	
		camera.transform.position = Vector3.Lerp(camera.transform.position, 
												 cameraViewCoords[(int)CameraView.DICE], 
												 7f * Time.deltaTime);

		if (camera.transform.position == cameraViewCoords[(int)CameraView.DICE])
			cameraIsMoving = false;
	}

	#endregion

	#region debug

	///<summary>
	///	Debug methud to create a player array if the scene is directly started without going via gamestart
	///</summary>
	private Player[] DebugPlayers()
	{
		Player[] players = new Player[6]; 

		for (int i = 0; i < players.Length; i++)
		{
			players[i] = new Player((i + 1),
									"Player " + (i + 1),
									Random.value > 0.5f,	//random boolean
									Color.red);
		}


		return players;
	}

	#endregion

}
