using UnityEngine;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour 
{
	public GameObject tile;
	
	private List<Tile> tileList;
	private FloorWalker walker;
	
	void Start()
	{	
		tileList = new List<Tile>();
		walker = new FloorWalker(Vector2.zero);
	}
	
	void Update()
	{
		if (walker.HasMovesLeft()) 
		{
			AddTile(walker.getPosition());
			walker.Move();
		}
	}
	
	void AddTile(Vector2 position) 
	{
		// create tile and add to list
		Tile t = new Tile(position);
		if (!tileList.Contains(t)) tileList.Add(t);
		
		// create it in unity and set it as child object to World
		GameObject newTile = (GameObject)Instantiate(tile, t.getPosition(), Quaternion.identity);
		newTile.transform.parent = gameObject.transform;
	}
}
