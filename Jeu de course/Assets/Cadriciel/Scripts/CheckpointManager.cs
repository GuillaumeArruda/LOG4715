﻿using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour 
{

	[SerializeField]
	private GameObject _carContainer;

	[SerializeField]
	private int _checkPointCount;
	[SerializeField]
	private int _totalLaps;

	private bool _finished = false;
	
	private Dictionary<CarController,PositionData> _carPositions = new Dictionary<CarController, PositionData>();

	private class PositionData
	{
		public int lap;
		public int checkPoint;
		public int position;
	}

	// Use this for initialization
	void Awake () 
	{
		foreach (CarController car in _carContainer.GetComponentsInChildren<CarController>(true))
		{
			_carPositions[car] = new PositionData();
		}
	}
	
	public void CheckpointTriggered(CarController car, int checkPointIndex)
	{

		PositionData carData = _carPositions[car];

		if (!_finished)
		{
			if (checkPointIndex == 0)
			{
				if (carData.checkPoint == _checkPointCount-1)
				{
					carData.checkPoint = checkPointIndex;
					carData.lap += 1;
					Debug.Log(car.name + " lap " + carData.lap);
					if (IsPlayer(car))
					{
						GetComponent<RaceManager>().Announce("Tour " + (carData.lap+1).ToString());
					}

					if (carData.lap >= _totalLaps)
					{
						_finished = true;
						GetComponent<RaceManager>().update = false;
						GetComponent<RaceManager>().EndRace(car.name);
					}
				}
			}
			else if (carData.checkPoint == checkPointIndex-1) //Checkpoints must be hit in order
			{
				carData.checkPoint = checkPointIndex;
			}
		}
	}

	bool IsPlayer(CarController car)
	{
		return car.GetComponent<CarUserControlMP>() != null;
	}

    /*
    public GameObject GetCarInFirstPlace()
    {
        bool first = true;
        CarController firstPlaceCar = new CarController();
        PositionData firstPlacePosData = new PositionData();
        firstPlacePosData.lap = 0;
        firstPlacePosData.checkPoint = 0;

        foreach(KeyValuePair<CarController, PositionData> item in _carPositions)
        {
            if(first)
            {
                firstPlaceCar = item.Key;
                firstPlacePosData = item.Value;
                first = false;
                continue;
            }

            if(item.Value.lap > firstPlacePosData.lap)
            {
                firstPlaceCar = item.Key;
                firstPlacePosData = item.Value;
                continue;
            }
            else if(item.Value.lap < firstPlacePosData.lap)
            {
                continue;
            }

            if(item.Value.checkPoint > firstPlacePosData.checkPoint)
            {
                firstPlaceCar = item.Key;
                firstPlacePosData = item.Value;
                continue;
            }
            else if(item.Value.checkPoint < firstPlacePosData.checkPoint)
            {
                continue;
            }
        }

        return firstPlaceCar.gameObject;
    }
    */
}
