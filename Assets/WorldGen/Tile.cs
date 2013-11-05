using UnityEngine;
using System.Collections;

public class Tile 
{
	private Vector2 position;
	
	public Tile(Vector2 position)
	{
		this.position = position;
	}
	
	public Vector2 getPosition()
	{
		return this.position;
	}
}
