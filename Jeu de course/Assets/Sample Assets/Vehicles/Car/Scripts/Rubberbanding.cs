using UnityEngine;

class Rubberbanding : MonoBehaviour
{
    [SerializeField] private float maxSpeedRubberBandingMultiplier = 1.10f;         // the maximum speed multiplier when trailing behind(in meters per second!)
    [SerializeField] private int rubberBandingHelpPosition = 5;                     // Position at which the rubberbanding starts

    void Update()
    {
        GameObject gameManager = GameObject.Find("Game Manager");
        int positionOfCar = gameManager.GetComponent<RaceManager>().GetPositionOfCar(gameObject);
        if(positionOfCar >= rubberBandingHelpPosition)
        {
            MaxSpeedMultiplier = maxSpeedRubberBandingMultiplier;
        }
        else
        {
            MaxSpeedMultiplier = 1.0f;
        }
    }

    public float MaxSpeedMultiplier
    {
        get;
        set;
    }
}
