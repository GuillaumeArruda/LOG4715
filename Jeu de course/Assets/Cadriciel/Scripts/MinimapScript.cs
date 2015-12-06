using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapScript : MonoBehaviour {

	private float minimapWidth;
	private float minimapHeight;

	private float markerWidth;
	private float markerHeight;

	[SerializeField]
	private Texture minimap;
	[SerializeField]
	private Texture redCar;
	[SerializeField]
	private Texture orangeCar;
	[SerializeField]
	private Texture yellowCar;
	[SerializeField]
	private Texture whiteCar;
	[SerializeField]
	private Texture blueCar;
	[SerializeField]
	private Texture greenCar;
	[SerializeField]
	private Texture blackCar;
	
	// Use this for initialization
	void Start () {
		minimapWidth = Screen.width / 5;
		minimapHeight = 2 * Screen.height / 5;

		markerWidth  = Screen.width / 100;
		markerHeight = Screen.height / 100;
	}

	public void OnGUI()
	{
        GameObject gameManager = GameObject.Find("Game Manager");
        Terrain terrain = GameObject.Find("Terrain").GetComponentInChildren<Terrain>();

        RaceManager raceManager = gameManager.GetComponent<RaceManager>();
        List<Position> positions = raceManager._positions;

        GUI.DrawTexture(new Rect(Screen.width - minimapWidth, (Screen.height - minimapHeight), minimapWidth, minimapHeight), minimap, ScaleMode.ScaleToFit);

        foreach (Position pos in positions)
        {
            if (pos._position == int.MaxValue)
            {
                continue;
            }

            Vector3 carPosition = pos._car.position - terrain.transform.position;
            Vector2 terrainCoord = new Vector2(carPosition.x / terrain.terrainData.size.x, carPosition.z / terrain.terrainData.size.z);

            GUI.DrawTexture(new Rect(Screen.width - minimapWidth + terrainCoord.x * minimapWidth - markerWidth/2,
                                    (Screen.height - terrainCoord.y * minimapHeight - markerHeight/2),
                                    markerWidth, markerHeight), GetTextureFromName(pos._name), ScaleMode.ScaleToFit);
        }
	}

    private Texture GetTextureFromName(string name)
    {
        switch (name)
        {
            case "Joueur 1":
                return blackCar;
            case "Voiture blanche":
                return whiteCar;
            case "Voiture jaune":
                return yellowCar;
            case "Voiture rouge":
                return redCar;
            case "Voiture bleue":
                return blueCar;
            case "Voiture verte":
                return greenCar;
            case "Voiture orange":
                return orangeCar;
            default:
                return blackCar;
        }
    }
}
