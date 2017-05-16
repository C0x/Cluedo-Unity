﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

///<summary>
/// Handles gameboard logic
/// Handles pawn movement
///</summary>
public class TileMap : MonoBehaviour 
{
	[Header("Pawns")]
	public List<GameObject> PawnPrefabs;
	private GameObject SelectedPawn;

	[Header("Map")]
	[SerializeField]
	private int mapSizeX = 25;
	[SerializeField]
	private int mapSizeY = 25;

	private enum ETileType : byte
	{
		DEFAULT = 0,
		SPAWN,
		OUTSIDE,
		DOOR,
		SECRETENTRANCE,
		ROOM
	}

	public TileType[] tileTypes;
	private ETileType[,] tiles;
	
	private Node[,] graph;
	private List<Node> currentPath;

	private List<GameObject> SpawnedPawns;


	void Start()
	{
		SpawnAllPawns();

		//Setup all pawn start variables
		foreach (var pawn in SpawnedPawns)
		{
			pawn.GetComponent<Pawn>().TileX = (int) pawn.transform.position.x;
			pawn.GetComponent<Pawn>().TileY = (int) pawn.transform.position.z;
			pawn.GetComponent<Pawn>().Map = this;
		}

		GenerateMapData();
		GenerateGraph();
		GenerateMapVisual();

		FindPossibleTiles(6);
	}

	void Update()
	{
		DrawLines(); //debug
	}

	///<summary>
	///	Generates the map (assigns different kinds of tiles)
	///</summary>
	private void GenerateMapData()
	{
		tiles = new ETileType[mapSizeX, mapSizeY];

		//Assign tiles
		for (int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				//Assign room tiles
				if (
					y >= 0 && y <= 5 && x >= 0 && x <= 7 ||					//lounge
					y >= 9 && y <= 14 && x >= 0 && x <= 8 ||				//diner
					y == 15 && x >= 0 && x <= 4 ||							//diner
					y >= 18 && y <= 23 && x >= 0 && x <= 5 ||				//kitchen
					y >= 0 && y <= 7 && x >= 10 && x <= 15 ||				//hall
					y >= 17 && y <= 22 && x >= 8 && x <= 16 ||				//ball
					y == 23 && x >= 11 && x <= 13 ||						//ball
					y >= 1 && y <= 3 && x == 18 ||							//study
					y >= 0 && y <= 3 && x >= 19 && x <= 24 ||				//study
					(y >= 7 && y <= 9) && (x == 18 || x == 24) ||			//library
					y >= 6 && y <= 10 && x >= 19 && x <= 23 ||				//library
					y >= 12 && y <= 16 && x >= 19 && x <= 24 ||				//billiart
					(y >= 20 && y <= 23) && (x == 19 || x == 24) ||			//conservatory	
					y >= 19 && y <= 23 && x >= 20 && x <= 23				//conservatory
				) {

					tiles[x,y] = ETileType.ROOM;
					continue;
				}

				//TODO: assign secret entrance tiles
				//TODO: assign door tiles

				//Assign spawn tiles
				if (
					y == 24 && x == 9 ||		//white
					y == 24 && x == 15 ||		//green
					y == 18 && x == 24 ||		//blue
					y == 7 && x == 0 ||			//yellow
					y == 5 && x == 24 ||		//purple
					y == 0 && x == 8 			//red
				) {

					tiles[x,y] = ETileType.SPAWN;
					continue;
				}

				//Assign outside tiles (incl. basement)
				if (
					y != 17 && x == 0 ||
					y == 24 ||
					x == 24 ||
					y == 0 && x != 17 ||
					(y == 23 && (x == 6 || x == 10 || x == 14 || x == 18)) ||
					y >= 9 && y <= 14 && x >= 11 && x <= 15 					//basement
 				) {

					tiles[x, y] = ETileType.OUTSIDE;
					continue;
				}

				//other tiles are default tiles
				tiles[x, y] = ETileType.DEFAULT;
			}
		}
	}

	///<summary>
	///	Generates graph used for pathfinding
	///</summary>
	private void GenerateGraph()
	{
		graph = new Node[mapSizeX, mapSizeY];

		//initialize with nodes
		for(int x=0; x < mapSizeX; x++) 
		{
			for(int y=0; y < mapSizeX; y++) 
			{
				graph[x,y] = new Node();
				graph[x,y].X = x;
				graph[x,y].Y = y;
			}
		}

		//Calculate 4-way neighbours
		for(int x=0; x < mapSizeX; x++) 
		{
			for(int y=0; y < mapSizeX; y++) 
			{
				if (x > 0)
					graph[x,y].Neighbours.Add( graph[x - 1, y] );
				if(x < mapSizeX-1)
					graph[x,y].Neighbours.Add( graph[x + 1, y] );
				if(y > 0)
					graph[x,y].Neighbours.Add( graph[x, y - 1] );
				if(y < mapSizeY-1)
					graph[x,y].Neighbours.Add( graph[x, y  +1] );
			}
		}
	}

	///<summary>
	///	Used to generate (invisible) tiles that the user can click & used in pathfinding
	///</summary>
	private void GenerateMapVisual()
	{
		for (int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				TileType tt = tileTypes[ (int)tiles[x,y] ];
				GameObject go = (GameObject)Instantiate( tt.InvisiblePrefab, new Vector3(x, 0, y), Quaternion.identity );

				ClickableTile ct = go.GetComponent<ClickableTile>();
				ct.TileX = x;
				ct.TileY = y;
				ct.Map = this;
				ct.transform.parent = GameObject.Find("GameBoard").transform;
				ct.transform.name = "Tile" + x + "-" + y;
				ct.GetComponent<Renderer>().enabled = false; //debug == true
			}
		}
	}

	///<summary>
	///	BFS algorithm
	/// Highlights possible tiles
	///</summary>
	public void FindPossibleTiles(int diceValue)
	{
		Debug.Log("Dicevalue: " + diceValue);

		//reset depth of all nodes
		foreach(Node n in graph)
		{
			n.Depth = 0;
		}

		Node root = graph[ SelectedPawn.GetComponent<Pawn>().TileX,
						   SelectedPawn.GetComponent<Pawn>().TileY ];

		HashSet<Node> set = new HashSet<Node>();
		Queue<Node> queue = new Queue<Node>();
		List<Node> possibleNodes = new List<Node>();

		set.Add(root);
		queue.Enqueue(root);
		queue.Enqueue(null);
		int depth = 1;

		while (queue.Count > 0)
		{
			Node current = queue.Dequeue();

			if (current == null)
			{
				depth++;
				queue.Enqueue(null);
				if (queue.First() == null) break; //two nulls in a row
				else continue;
			}

			if (depth > diceValue)
				break;

			foreach(Node n in current.Neighbours)
			{
				if (!set.Contains(n) && PawnCanEnterTile(n.X, n.Y))
				{
					set.Add(n);
					n.Depth = depth;
					queue.Enqueue(n);
				}
			}
		}

		foreach(Node n in set)
		{
			//Debug.Log(n.X + "-" + n.Y + " => " + n.Depth);
			
			if (IsDoorTile(n.X, n.Y))
			{
				possibleNodes.Add(n);
				HighlightTile(n.X, n.Y);
				continue;
			}

			if (n.Depth == diceValue)
			{
				HighlightTile(n.X, n.Y);
				possibleNodes.Add(n);
			}
		}
	}

	///<summary>
	///	Dijkstra's algorithm
	/// Assigns current pawn the current path
	///</summary>
	public void CalculatePathTo(int x, int y)
	{
		SelectedPawn.GetComponent<Pawn>().CurrentPath = null;

		if (!PawnCanEnterTile(x, y))
			return;

		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

		List<Node> unvisited = new List<Node>();

		Node source = graph[ SelectedPawn.GetComponent<Pawn>().TileX,
							 SelectedPawn.GetComponent<Pawn>().TileY ];
		Node target = graph[ x, y];

		dist[source] = 0;
		prev[source] = null;

		//Initialize everything with INF
		foreach(Node v in graph)
		{
			if (v != source)
			{
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			unvisited.Add(v);
		}

		while (unvisited.Count > 0)
		{
			//Search unvisited node with smallest distance
			Node u = null;

			foreach(Node possibleU in unvisited)
			{
				if (u == null || dist[possibleU] < dist[u])
					u = possibleU;
			}

			//Reached target
			if (u == target)
				break;
			
			unvisited.Remove(u);

			//Search neighbours
			foreach(Node v in u.Neighbours)
			{
				//float alt = dist[u] + u.DistanceToNode(v);
				float alt = dist[u] + CostToEnterTile(u.X, u.Y, v.X, v.Y);
				if (alt < dist[v])
				{
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

		//No route found
		if (prev[target] == null)
			return;
		
		//Build path from target to source
		List<Node> currentPath = new List<Node>();
		Node cur = target;

		while (cur != null)
		{
			currentPath.Add(cur);
			cur = prev[cur];
		}

		//Switch path from source to target
		currentPath.Reverse();

		//Add path to pawn
		SelectedPawn.GetComponent<Pawn>().CurrentPath = currentPath;
	}

	///<summary>
	///	Determine 'Cost' to enter tile (1 if posible, infinite if inpossible)
	///</summary>
	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
	{
		if (!PawnCanEnterTile(targetX, targetY))
			return Mathf.Infinity;

		return 1f;
	}

	///<summary>
	///	Convert x and y to Vector3
	///</summary>
	public Vector3 TileCoordToWorldCoord(int x, int y)
	{
		return new Vector3(x, 0, y);
	}

	///<summary>
	///	Check if pawn can enter tile with given coordinates
	///</summary>
	public bool PawnCanEnterTile(int x, int y)
	{
		if (tiles[x,y] == ETileType.DEFAULT)
		{
			return tileTypes[ (int)tiles[x,y] ].CanBeEntered;
		} 
		else if (tiles[x,y] == ETileType.DOOR)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	///<summary>
	///	Check if tile is doortile or not
	///</summary>
	private bool IsDoorTile(int x, int y)
	{
		return (tiles[x, y] == ETileType.DOOR);
	}

	///<summary>
	///	Sets tile visible with colorfader enabled
	///</summary>
	private void HighlightTile(int x, int y)
	{
		GameObject t = GameObject.Find("Tile" + x + "-" + y);
		ClickableTile ct = t.GetComponent<ClickableTile>();
		ct.Enabled = true;
		ct.GetComponent<Renderer>().enabled = true;
		ct.GetComponent<ColorFader>().IsEnabled = true;
	}

	#region Debug methods

	///<summary>
	///	Debug method to draw grid
	///</summary>
    private void DrawLines() 
    {
		Vector3 offset = new Vector3(.5f, 0, .5f);

		Vector3 widthLine = Vector3.right * (float) mapSizeX;
        Vector3 heightLine = Vector3.forward * (float) mapSizeY	;

        for (int x = 0; x <= mapSizeX; x++) 
        {
            Vector3 start = Vector3.right * x;
			start -= offset;
            Debug.DrawLine(start, start + heightLine, Color.red);

            for (int y = 0; y <= mapSizeY; y++) 
            {
                start = Vector3.forward * y;
				start -= offset;
                Debug.DrawLine(start, start + widthLine, Color.red);
            }
        }
    }

	///<summary>
	/// Debug method to spawn all pawns
	///</summary>
	private void SpawnAllPawns()
	{
		SpawnedPawns = new List<GameObject>();

		foreach (var pawn in PawnPrefabs)
		{
			GameObject go = (GameObject) Instantiate(pawn, pawn.transform.position, pawn.transform.rotation);
			SpawnedPawns.Add(go);
		}

		SelectedPawn = SpawnedPawns[0];
	}

	#endregion

}
