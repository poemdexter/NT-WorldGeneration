using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour 
{
	public GameObject floorTile;
	public GameObject treasureTile;
	public GameObject spawnTile;
	
	private List<Tile> tileList;
	private List<Tile> treasureList;
	private List<FloorWalker> walkerList;
	private List<FloorWalker> deadWalkerList;
	private List<FloorWalker> childWalkerList;
	
	private bool generating = false;
	private int walkerSteps = 150;
	private Tile treasureSpawn;
	
	void Start()
	{	
		tileList = new List<Tile>();
		treasureList = new List<Tile>();
		walkerList = new List<FloorWalker>();
		deadWalkerList = new List<FloorWalker>();
		childWalkerList = new List<FloorWalker>();
		walkerList.Add(new FloorWalker(Vector2.zero, walkerSteps));
		
		GenerateWorld();
	}
	
	void GenerateWorld()
	{
		generating = true;
		
		// do spawnpoint
		GenerateSpawnpoint();
		
		// generate world
		while (generating)
		{
			foreach(FloorWalker walker in walkerList)
			{
			    if (walker.HasMovesLeft()) 
				{
					int actionNum = walker.Move();
					
					// 50% chance of 2x2 room
					if (Random.Range(0,2) == 0)
					{
						CreateTwoByTwoRoom(walker.getPosition(), actionNum);
					}
					else
					{
						AddTile(walker.getPosition(), actionNum);
					}
					
					// small chance of spawning another walker
					TrySpawnAnotherWalker(walker.getPosition(), walker.getMovesLeft());
				}
				else
				{
					deadWalkerList.Add(walker);
				}
			}
			
			// remove dead walkers from walkerList
			foreach(FloorWalker deadWalker in deadWalkerList)
			{
				walkerList.Remove(deadWalker);
			}
			deadWalkerList.Clear();
			
			// add in any child walkers
			walkerList.AddRange(childWalkerList);
			childWalkerList.Clear();
			
			if (walkerList.Count() == 0)
				generating = false;
		}
		
		KeepSingleChest();
	}
	
	void GenerateSpawnpoint()
	{
		GameObject newTile = (GameObject)Instantiate(spawnTile, Vector2.zero, Quaternion.identity);
		Tile t = new Tile(Vector2.zero);
	    tileList.Add(t);
		newTile.transform.parent = gameObject.transform;
	}
	
	// remove all chests but furthest
	void KeepSingleChest()
	{
		// find furthest chest from origin
		Tile furthestTile = treasureList[0];
		float furthestDist = Vector2.Distance(Vector2.zero, furthestTile.getPosition());
		foreach(Tile t in treasureList)
		{
			float dist = Vector2.Distance(Vector2.zero, t.getPosition());
			if (furthestDist < dist)
			{
				furthestTile = t;
				furthestDist = dist;
			}
		}
		
		// set treasure spawn and draw tile
		treasureSpawn = furthestTile;
		Vector3 pos = new Vector3(furthestTile.getPosition().x, furthestTile.getPosition().y, -0.001f);
		GameObject newTile = (GameObject)Instantiate(treasureTile, pos, Quaternion.identity);
		newTile.transform.parent = gameObject.transform;
		
		// clear treasure list
		treasureList.Clear();
	}
	
	void Update()
	{
		// regenerate on keypress
		if (!generating && Input.GetKeyDown(KeyCode.R))
		{
			// remove all child objects from world
			foreach(Transform child in gameObject.transform)
			{
				Destroy(child.gameObject);
			}
			
			// reset tileList
			tileList.Clear();
			
			// reset walkerList
			walkerList.Clear();
			walkerList.Add(new FloorWalker(Vector2.zero, walkerSteps));
			
			GenerateWorld();
		}
	}
	
	void AddTile(Vector2 position, int actionNum) 
	{
		if (!tileListContainsTileWithPosition(position)) 
		{
			// create tile object for adding to list
		    Tile t = new Tile(position);
			
			// instantiate tile prefab in unity and set it as child object to World
			GameObject newTile = (GameObject)Instantiate(floorTile, t.getPosition(), Quaternion.identity);
			tileList.Add(t);
			newTile.transform.parent = gameObject.transform;
			
			// based on walker action, create different tile
			// (0:none, 1:left, 2:right:, 3:around)
			switch(actionNum)
			{
			case 3:
				treasureList.Add(t);
				break;
			default:
				break;
			}
		}
	}
	
	bool tileListContainsTileWithPosition(Vector2 position)
	{
		var matches = tileList.Where(x => x.getPosition() == position);
		return (matches.Count() > 0);
	}

	void CreateTwoByTwoRoom(Vector2 position, int action)
	{
		// x x
		// p x
		// only apply action to position of walker
		AddTile(position, action);
		AddTile(position + new Vector2(1,0), 0);
		AddTile(position + new Vector2(1,1), 0);
		AddTile(position + new Vector2(0,1), 0);
	}
	
	void TrySpawnAnotherWalker(Vector2 position, int parentMovesLeft)
	{
		// depends on how many walkers exist
		// should have less moves so we aren't walking forever
		
		// 10% chance to spawn walker
		if (Random.Range(0,10) == 0)
		{
			childWalkerList.Add(new FloorWalker(position, Mathf.CeilToInt(parentMovesLeft / 4)));
		}
	}
}
