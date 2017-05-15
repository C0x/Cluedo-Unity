using System.Collections.Generic;
using UnityEngine;

	public class Node
	{
		[HideInInspector]
		public List<Node> Neighbours;
		[HideInInspector]
		public int X;
		[HideInInspector]
		public int Y;

		public Node()
		{
			Neighbours = new List<Node>();
		}

		public float DistanceToNode(Node n)
		{
			Debug.Assert(n != null, "Node can't be null!");

			return Vector2.Distance(
				new Vector2(X, Y),
				new Vector2(n.X, n.Y)
			);
		}
	}
