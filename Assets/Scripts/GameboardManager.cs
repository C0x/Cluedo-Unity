using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameboardManager : MonoBehaviour 
{
    private const float TILE_SIZE = .8f;
    private const float TILE_OFFSET = 0.5f;

    private const int NUMBER_OF_TILES_X = 25;
    private const int NUMBER_OF_TILES_Y = 25;

	public Pawn WhitePrefab;
	public Pawn GreenPrefab;
	public Pawn BluePrefab;
	public Pawn PurplePrefab;
	public Pawn YellowPrefab;
	public Pawn RedPrefab;

    public GameObject InvisibleDefaultTilePrefab;
    public List<GameObject> SpawnPoints;
    public List<GameObject> DoorTiles;
    public List<GameObject> SecretEntrances;
    
    private List<GameObject> DefaultTiles;
    private List<Pawn> PawnsInGame;
	private Pawn CurrentPlayer;
    private List<GameObject> AccessibleTiles;

    void Start() 
    {
        InitializeDefaultTiles();
        AccessibleTiles = DoorTiles
                            .Concat(DoorTiles)
                            .Concat(SecretEntrances)
                            .Concat(DefaultTiles)
                            .ToList();
    }

    void Update() 
    {
        DrawGameBoard(); //debug purpose only

        if (Input.GetKeyDown("space"))
		{
			//Simulate dice roll here
			int die1 = Random.Range(1, 7);
			int die2 = Random.Range(1, 7);

            Debug.Log("Die 1 rolled: " + die1);
            Debug.Log("Die 2 rolled: " + die2);

			if (!CurrentPlayer.InSpawn) 
			{
				CurrentPlayer.SearchAvailableMoves(AccessibleTiles, die1 + die2);
			} 
			else 
			{
				GameObject curTile = null;

				switch (CurrentPlayer.Id)
				{
					case 1: curTile = GameObject.Find("WhiteSpawn"); break;
					case 2: curTile = GameObject.Find("GreenSpawn"); break;
					case 3: curTile = GameObject.Find("BlueSpawn"); break;
					case 4: curTile = GameObject.Find("PurpleSpawn"); break;
					case 5: curTile = GameObject.Find("YellowSpawn"); break;
					case 6: curTile = GameObject.Find("RedSpawn"); break;
				}

				Debug.Assert(curTile != null, "Current tile can't be null!");

				CurrentPlayer.SearchAvailableMoves(curTile, AccessibleTiles, die1 + die2);
			}			
		}

    }

    ///<summary>
    /// Spawns empty gameobjects that represent "DefaultTiles"
    ///<summary/>
    private void InitializeDefaultTiles()
    {
        this.DefaultTiles = new List<GameObject>();

        for (int i = 0; i < NUMBER_OF_TILES_Y; i++) 
        {
            for (int j = 0; j < NUMBER_OF_TILES_X; j++) 
            {
                #region skip loop for rooms

                if (j >= 0 && j <= 5 && i >= 0 && i <= 7) continue;         //lounge
                if (j >= 9 && j <= 14 && i >= 0 && i <= 8) continue;        //diner
                if (j == 15 && i >= 0 && i <= 4) continue;                  //diner
                if (j >= 18 && j <= 23 && i >= 0 && i <= 5) continue;       //kitchen
                if (j >= 0 && j <= 7 && i >= 10 && i <= 15) continue;       //hall   
                if (j >= 17 && j <= 22 && i >= 8 && i <= 16) continue;      //ball
                if (j == 23 && i >= 11 && i <= 13) continue;                //ball
                if (j >= 1 && j <= 3 && i == 18) continue;                  //study
                if (j >= 0 && j <= 3 && i >= 19 && i <= 24) continue;       //study
                if ((j >= 7 && j <= 9) && (i == 18 || i == 24)) continue;   //library
                if (j >= 6 && j <= 10 && i >= 19 && i <= 23) continue;      //library
                if (j >= 12 && j <= 16 && i >= 19 && i <= 24) continue;     //billiart
                if ((j >= 20 && j <= 23) && (i == 19 || i == 24)) continue; //conservatory
                if (j >= 19 && j <= 23 && i >= 20 && i <= 23) continue;     //conservatory          

                #endregion

                #region skip loop for spawns

                if (j == 24 && i == 9) continue;    //white
                if (j == 24 && i == 15) continue;   //green
                if (j == 18 && i == 24) continue;   //blue
                if (j == 7 && i == 0) continue;     //yellow
                if (j == 5 && i == 24) continue;    //purple
                if (j == 0 && i == 8) continue;     //red
                               
                #endregion

                #region skip loop for outside tiles (including cellar)

                if (i == 0 && j != 17) continue;
                if (j == 24) continue;
                if (i == 24) continue;
                if (i != 17 && j == 0) continue;
                if ((i == 6 || i == 10 || i == 14 || i == 18) && j == 23) continue;
                if (j >= 9 && j <= 14 && i >= 11 && i <= 15) continue; //cellar

                #endregion

                GameObject tile = (GameObject) Instantiate(InvisibleDefaultTilePrefab, new Vector3(i + TILE_OFFSET, 0.7f, j + TILE_OFFSET), Quaternion.identity);
                tile.name = "DefaultTile" + i + "-" + j;
                tile.transform.parent = this.transform;
                this.DefaultTiles.Add(tile);
            }
        }
    }

    public void SpawnPawns(List<int> pawnIds)
    {
        PawnsInGame = new List<Pawn>(pawnIds.Count);

        for (int i = 0; i < pawnIds.Count; i++) 
        {
            switch(pawnIds[i])
            {
                case 1: 
                    PawnsInGame.Add(WhitePrefab);
                    break;
                case 2:
                    PawnsInGame.Add(GreenPrefab);
                    break;
                case 3:
                    PawnsInGame.Add(BluePrefab);
                    break;
                case 4:
                    PawnsInGame.Add(PurplePrefab);
                    break;
                case 5:
                    PawnsInGame.Add(YellowPrefab);
                    break;
                case 6:
                    PawnsInGame.Add(RedPrefab);
                    break;
                default:
                    Debug.LogError("Pawn ID not existing!");
                    break;
            }

            Instantiate(PawnsInGame[i], PawnsInGame[i].transform.position, PawnsInGame[i].transform.rotation);
        }

        CurrentPlayer = PawnsInGame[0];
    }

    ///<summary>
    /// Debug method to draw lines of the gameboard in the inspector
    ///<summary/>
    private void DrawGameBoard() 
    {
        Vector3 widthLine = Vector3.right * NUMBER_OF_TILES_X;
        Vector3 heightLine = Vector3.forward * NUMBER_OF_TILES_Y;

        for (int i = 0; i < NUMBER_OF_TILES_Y; i++) 
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);

            for (int j = 0; j < NUMBER_OF_TILES_X; j++) 
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }
    }
}