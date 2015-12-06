using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndScreenManager : MonoBehaviour 
{
    public GUIText rankings;
    public GUIText winner;

	void Awake()
	{
		Input.simulateMouseWithTouches = true;
        GameObject gameManager = GameObject.Find("Game Manager");
        RaceManager raceManager = gameManager.GetComponent<RaceManager>();
        List<Position> positions = raceManager._positions;
        float time = raceManager.time;

        winner.text =  "Le vainqueur est " + positions[0]._name + "\n";
        winner.text += "Le temps de la course est " + time;

        rankings.text = "";

        for(int i = 1; i < positions.Count; ++i)
        {
            if (positions[i]._position == int.MaxValue)
            {
                continue;
            }

            rankings.text += "Position " + positions[i]._position + " : " + positions[i]._name + "\n";
        }
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
		{
			Application.LoadLevel("course");
		}
	}
}
