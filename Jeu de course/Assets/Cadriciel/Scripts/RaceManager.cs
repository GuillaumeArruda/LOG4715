using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public struct Position
{
    public Position(Transform car, int position, string name)
    {
        _car = car;
        _position = position;
        _name = name;
    }

    public Transform _car;
    public int _position;
    public string _name;
};

public class RaceManager : MonoBehaviour 
{
	[SerializeField]
	private GameObject _carContainer;

	[SerializeField]
	private GUIText _announcement;

	[SerializeField]
	private int _timeToStart;

	[SerializeField]
	private int _endCountdown;

    public List<Position> _positions = new List<Position>();
    public bool update = true;
    public float time = 0;

	// Use this for initialization
	void Awake () 
	{
        DontDestroyOnLoad(transform.gameObject);
		CarActivation(false);

        GameObject cars = GameObject.Find("Cars");
        foreach(Transform child in cars.transform)
        {
            _positions.Add(new Position(child, 0, child.name));
        }
	}
	
	void Start()
	{
		StartCoroutine(StartCountdown());
	}

	IEnumerator StartCountdown()
	{
		int count = _timeToStart;
		do 
		{
			_announcement.text = count.ToString();
			yield return new WaitForSeconds(1.0f);
			count--;
		}
		while (count > 0);
		_announcement.text = "Partez!";
		CarActivation(true);
		yield return new WaitForSeconds(1.0f);
		_announcement.text = "";
	}

	public void EndRace(string winner)
	{
		StartCoroutine(EndRaceImpl(winner));
	}

	IEnumerator EndRaceImpl(string winner)
	{
		CarActivation(false);
		_announcement.fontSize = 20;
		int count = _endCountdown;
		do 
		{
            _announcement.text = "Course terminée!";
			yield return new WaitForSeconds(1.0f);
			count--;
		}
		while (count > 0);

		Application.LoadLevel("end");
	}

	public void Announce(string announcement, float duration = 2.0f)
	{
		StartCoroutine(AnnounceImpl(announcement,duration));
	}

	IEnumerator AnnounceImpl(string announcement, float duration)
	{
		_announcement.text = announcement;
		yield return new WaitForSeconds(duration);
		_announcement.text = "";
	}

	public void CarActivation(bool activate)
	{
		foreach (CarAIControl car in _carContainer.GetComponentsInChildren<CarAIControl>(true))
		{
			car.enabled = activate;
		}
		
		foreach (CarUserControlMP car in _carContainer.GetComponentsInChildren<CarUserControlMP>(true))
		{
			car.enabled = activate;
		}

	}

    public GameObject GetFirstPlaceCar()
    {
        GameObject cars = GameObject.Find("Cars");
        GameObject firstPlaceCar = null;
        float maxProgress = 0.0f;

        foreach(WaypointProgressTracker progressTracker in cars.GetComponentsInChildren<WaypointProgressTracker>())
        {
            if(progressTracker.progressDistance > maxProgress)
            {
                maxProgress = progressTracker.progressDistance;
                firstPlaceCar = progressTracker.gameObject;
            }
        }

        return firstPlaceCar;
    }

    public int GetPositionOfCar(GameObject car)
    {
        GameObject cars = GameObject.Find("Cars");
        int numberOfCarsAhead = 0;
        WaypointProgressTracker tracker = car.GetComponent<WaypointProgressTracker>();
        if(!tracker)
        {
            return int.MaxValue;
        }

        float progress = tracker.progressDistance;
        foreach(WaypointProgressTracker progressTracker in cars.GetComponentsInChildren<WaypointProgressTracker>())
        {
            if (progressTracker.gameObject == car)
            {
                continue;
            }

            if(progressTracker.progressDistance > progress)
            {
                ++numberOfCarsAhead;
            }
        }

        return numberOfCarsAhead + 1;
    }

    void Update()
    {
        if(update)
        {
            time += Time.deltaTime;
            for(int i = 0; i < _positions.Count; ++i)
            {
                _positions[i] = new Position(_positions[i]._car, GetPositionOfCar(_positions[i]._car.gameObject), _positions[i]._name);
            }

            _positions.Sort(delegate (Position x, Position y)
            {
                return x._position.CompareTo(y._position);
            });
        }
    }
}
