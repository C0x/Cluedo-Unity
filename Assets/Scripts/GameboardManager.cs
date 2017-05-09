using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboardManager : MonoBehaviour 
{
    private const float TILE_SIZE = .8f;
    private const float TILE_OFFSET = 0.5f;

    private const int NUMBER_OF_TILES_X = 25;
    private const int NUMBER_OF_TILES_Y = 25;

    private int selectionX = -1;
    private int selectionY = -1;

    public Transform InvisibleDefaultTilePrefab;
    public List<GameObject> SpawnPoints;
    public List<GameObject> AccessibleTiles;

    void Start() 
    {
        InitializeDefaultTiles();        
    }

    void Update() 
    {
        DrawGameBoard(); //debug purpose only
    }

    private void InitializeDefaultTiles()
    {
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

                Transform tile = (Transform) Instantiate(InvisibleDefaultTilePrefab, new Vector3(i + TILE_OFFSET, 0.7f, j + TILE_OFFSET), Quaternion.identity);
                tile.name = "DefaultTile" + i + "-" + j;
                tile.parent = this.transform;
            }
        }
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