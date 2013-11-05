using UnityEngine;
using System.Collections;

public class FloorWalker
{
	private Vector2 RIGHT { get{ return new Vector2(1,0);  } }
	private Vector2 LEFT  { get{ return new Vector2(-1,0); } }
	private Vector2 DOWN  { get{ return new Vector2(0,-1); } }
	private Vector2 UP    { get{ return new Vector2(0,1);  } }
	
	private Vector2 position;  // where we are
	private Vector2 direction; // where we face
	private int movesLeft;     // moves left until we're done
	
	// percent chances for these to occur
	private int turnLeftChance = 20;
	private int turnRightChance = 20;
	private int turnAroundChance = 20;
	
	public FloorWalker(Vector2 position)
	{
		this.position = position;
		direction = RIGHT;
		movesLeft = 100;
	}
	
	public Vector2 getPosition()
	{
		return position;
	}
	
	public bool HasMovesLeft()
	{
		return (movesLeft <= 0) ? false : true;
	}
	
	public void Move()
	{
		// get new direction
		direction = getNewDirection(direction);
		// move us in direction
		position += direction;
		// minus one move
		movesLeft--;
	}
	
	// check if we should change direction
	private Vector2 getNewDirection(Vector2 direction)
	{
		int r = Random.Range(1,100);
		if      (ShouldTurnLeft(r))   return TurnAround(direction);
		else if (ShouldTurnRight(r))  return TurnLeft(direction);
		else if (ShouldTurnAround(r)) return TurnRight(direction);
		else return direction;
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