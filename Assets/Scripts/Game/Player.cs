using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Plain c# object used to store data between StartView & Main
///</summary>
public class Player 
{
	private readonly int _number;
	private readonly string _name;
	private readonly bool _isCpu;
	private readonly Color _color;

	public int Number { get { return _number; } }
	public string Name { get { return _name; } }
	public bool IsCpu { get { return _isCpu; } }
	public Color Color { get { return _color; } }

	public Player(int number, string name, bool isCPU, Color color)
	{
		this._number = number;
		this._name = name;
		this._isCpu = isCPU;
		this._color = color;
	}

	public override string ToString()
	{
		return Number + ") " + Name + " - " + IsCpu + " - " + Color;
	}
}
