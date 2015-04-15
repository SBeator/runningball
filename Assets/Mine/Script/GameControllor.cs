using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameControllor : MonoBehaviour 
{
	public Road[] possibleRoads;
	public Road[] initRoads;
	public IList<Road> roads;
	public Player player;
	public ScoreManager DistanceText;
	public Animator gameOverAniator;

	float removeDelay = 2f;
	public IList<Road> pendingRemoveRoadList;
	int minRoadsNumber = 3;
	bool gameOver;
	bool canReset;

	public void CanReset()
	{
		this.canReset = true;
	}
	
	void Start () 
	{
		this.InitializeRoad();

		this.pendingRemoveRoadList = new List<Road>();

		this.gameOver = false;
		this.canReset = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.gameOver) 
		{
			if (this.canReset && Input.GetKeyDown(KeyCode.R)) 
			{
				Application.LoadLevel (Application.loadedLevel);
			}

			return;
		}

		if (this.CheckIfGameOver()) 
		{
			this.GameOver();
		}

		foreach (var road in this.roads) 
		{
			if (road.PlayerOut() || (road.LastRoad == null && !road.PlayerIn()) ) 
			{
				this.pendingRemoveRoadList.Add(road);
			}
			else
			{
				var turnTransform = road.NeedTurn();
				if (turnTransform != null) 
				{
					player.Turn(turnTransform);
				}
			}
		}

		foreach (var removeRoad in this.pendingRemoveRoadList) 
		{
			StartCoroutine (this.RemoveRoad(removeRoad));
		}

		this.pendingRemoveRoadList.Clear();

		this.RefreshRoads();

		this.RefreshScore();
	}

	bool CheckIfGameOver()
	{
		return this.player.IsOver();
	}

	void GameOver()
	{
		this.player.Die();
		this.gameOver = true;
		this.gameOverAniator.SetTrigger("GameOver");
	}

	void InitializeRoad()
	{
		this.roads = new List<Road>();

		foreach (var road in this.initRoads) 
		{
			this.roads.Add(road);
		}
	}

	void AddNewRoad()
	{
		var lastRoads = this.GetLastRoads();
		foreach (var lastRoad in lastRoads) 
		{
			var newRoads = CreateNewRoad(lastRoad.endTriggers.Length);
			lastRoad.SetAlongNewRoads(newRoads);

			foreach (var newRoad in newRoads) 
			{
				roads.Add(newRoad);
			}
		}
	}

	Road[] CreateNewRoad(int number)
	{
		Road[] newRoads = new Road[number];
		for (int i = 0; i < number; i++) 
		{
			newRoads[i] = Instantiate (this.CreateNewRoad());
		}

		return newRoads;
	}

	Road CreateNewRoad()
	{
		var index = Random.Range (0, possibleRoads.Length);
		return possibleRoads[index];
	}

	IEnumerable<Road> GetLastRoads()
	{
		return this.roads.Where(t => t.NextRoads.Length == 0).ToList();
	}

	IEnumerator RemoveRoad(Road road)
	{
		this.roads.Remove(road);

		foreach (var nextRoad in road.NextRoads) 
		{
			nextRoad.LastRoad = null;
		}

		yield return new WaitForSeconds(this.removeDelay);

		Destroy(road.gameObject);
	}

	void RefreshRoads()
	{
		if (this.roads.Count < this.minRoadsNumber) 
		{
			this.AddNewRoad();
		}
	}

	void RefreshScore()
	{
		this.DistanceText.score = (int)this.player.Distance;
	}
}
