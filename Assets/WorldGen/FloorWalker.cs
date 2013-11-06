using UnityEngine;
using System.Collections;

public class FloorWalker
{
	private Vector2 RIGHT { get{ return new Vector2(1,0);  } }
	private Vector2 LEFT  { get{ return new Vector2(-1,0); } }
	private Vector2 DOWN  { get{ return new Vector2(0,-1); } }
	private Vector2 UP    { get{ return new Vector2(0,1);  } }
	
	private Vector2 position;     // where we are
	private Vector2 direction;    // where we face
	private Vector2 newDirection; // where we will face before move
	private int movesLeft;        // moves left until we're done
	
	// percent chances for these to occur
	private int turnLeftChance = 20;
	private int turnRightChance = 20;
	private int turnAroundChance = 10;
	
	public FloorWalker(Vector2 position, int moves)
	{
		this.position = position;
		direction = RIGHT;
		movesLeft = moves;
	}
	
	public Vector2 getPosition()
	{
		return position;
	}
	
	public int getMovesLeft()
	{
		return movesLeft;
	}
	
	public bool HasMovesLeft()
	{
		return (movesLeft <= 0) ? false : true;
	}
	
	public int Move()
	{
		// get new direction to return (0:none, 1:left, 2:right:, 3:around)
		int d = getNewDirection(direction);
		// set new direction
		direction = newDirection;
		// move us in direction
		position += direction;
		// minus one move
		movesLeft--;
		
		return d;
	}
	
	// check if we should change direction
	private int getNewDirection(Vector2 direction)
	{
		int r = Random.Range(1,100);
		
		if (ShouldTurnLeft(r)) 
		{
			newDirection = TurnAround(direction);
			return 1;
		}
		else if (ShouldTurnRight(r))
		{
			newDirection = TurnLeft(direction);
			return 2;
		}
		else if (ShouldTurnAround(r))
		{
			newDirection = TurnRight(direction);
			return 3;
		}
		else 
		{
			newDirection = direction;
			return 0;
		}
	}
	
	// helper methods to change direction
	private Vector2 TurnAround(Vector2 direction)
	{
		if (direction == RIGHT) return LEFT;
		if (direction == LEFT)  return RIGHT;
		if (direction == UP)    return DOWN;
		if (direction == DOWN)  return UP;
		return RIGHT;
	}
	private Vector2 TurnRight(Vector2 direction)
	{
		if (direction == RIGHT) return DOWN;
		if (direction == LEFT)  return UP;
		if (direction == UP)    return RIGHT;
		if (direction == DOWN)  return LEFT;
		return RIGHT;
	}
	private Vector2 TurnLeft(Vector2 direction)
	{
		if (direction == RIGHT) return UP;
		if (direction == LEFT)  return DOWN;
		if (direction == UP)    return LEFT;
		if (direction == DOWN)  return RIGHT;
		return RIGHT;
	}
	
	// helper methods to determine random number falling in range
	private bool ShouldTurnLeft(int random)
	{
		return (random <= turnLeftChance);
	}
	private bool ShouldTurnRight(int random)
	{
		int sum = turnLeftChance + turnRightChance;
		return (random > turnLeftChance && random <= sum);
	}
	private bool ShouldTurnAround(int random)
	{
		int sum = turnLeftChance + turnRightChance;
		int sum2 = turnLeftChance + turnRightChance + turnAroundChance;
		return (random > sum && random <= sum2);
	}
}