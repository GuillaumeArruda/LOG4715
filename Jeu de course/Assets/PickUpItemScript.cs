using UnityEngine;
using System.Collections;

public class PickUpItemScript : MonoBehaviour {
    private enum TypeOfPickUp
    {
        GreenShell,
        RedShell,
        BlueShell,
        Repair,
        Nitro
    }
    [SerializeField]
    private int timeToRespawn = 10;
    [SerializeField]
    private int probabilityToBeGreenShell = 20;
    [SerializeField]
    private int probabilityToBeRedShell = 15;
    [SerializeField]
    private int probabilityToBeBlueShell = 1;
    [SerializeField]
    private int probabilityToBeRepair = 20;
    [SerializeField]
    private int probabilityToBeNitro = 20;
    [SerializeField]
    Textures textures;

    [System.Serializable]
    private class Textures
    {
        public Texture GreenShellTexture;
        public Texture RedShellTexture;
        public Texture BlueShellTexture;
        public Texture RepairTexture;
        public Texture NitroTexture;
    }

    private int totalProbability;
    private TypeOfPickUp type;
	// Use this for initialization
	void Start () {
        totalProbability = probabilityToBeBlueShell + probabilityToBeGreenShell + probabilityToBeNitro + probabilityToBeRedShell + probabilityToBeRepair;
        ChooseType();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void ChooseType()
    {
        int value = (int)(totalProbability * Random.value);
        if(value <= probabilityToBeGreenShell)
        {
            Type = TypeOfPickUp.GreenShell;
            return;
        }
        value -= probabilityToBeGreenShell;
        if(value <= probabilityToBeRedShell)
        {
            Type = TypeOfPickUp.RedShell;
            return;
        }

        value -= probabilityToBeRedShell;
        if (value <= probabilityToBeBlueShell)
        {
            Type = TypeOfPickUp.BlueShell;
            return;
        }

        value -= probabilityToBeBlueShell;
        if (value <= probabilityToBeRepair)
        {
            Type = TypeOfPickUp.Repair;
            return;
        }

        value -= probabilityToBeRepair;
        if (value <= probabilityToBeNitro)
        {
            Type = TypeOfPickUp.Nitro;
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Vehicles"))
        {
            if(other.gameObject.name == "Joueur1")
            {
                switch (type)
                {
                    case TypeOfPickUp.GreenShell:
                        other.gameObject.GetComponentInParent<CarProjectileLauncher>().GreenShellAmmo++;
                        break;
                    case TypeOfPickUp.RedShell:
                        other.gameObject.GetComponentInParent<CarProjectileLauncher>().RedShellAmmo++;
                        break;
                    case TypeOfPickUp.BlueShell:
                        other.gameObject.GetComponentInParent<CarProjectileLauncher>().BlueShellAmmo++;
                        break;
                    case TypeOfPickUp.Nitro:
                        other.gameObject.GetComponentInParent<NitroScript>().FillNitro();
                        break;
                    case TypeOfPickUp.Repair:
                        other.gameObject.GetComponentInParent<DamageScript>().ResetHealth();
                        break;
                }

            }
            StartCoroutine("WaitThenRenable");
        }
    }

    private IEnumerator WaitThenRenable()
    {
        enabled = false;
        renderer.enabled = false;
        yield return new WaitForSeconds(timeToRespawn);
        renderer.enabled = true;
        enabled = true;
        ChooseType();
    }

    void SetAppropriateTexture()
    {
        switch(type)
        {
            case TypeOfPickUp.GreenShell:
                renderer.material.mainTexture = textures.GreenShellTexture;
                break;
            case TypeOfPickUp.RedShell:
                renderer.material.mainTexture = textures.RedShellTexture;
                break;
            case TypeOfPickUp.BlueShell:
                renderer.material.mainTexture = textures.BlueShellTexture;
                break;
            case TypeOfPickUp.Repair:
                renderer.material.mainTexture = textures.RepairTexture;
                break;
            case TypeOfPickUp.Nitro:
                renderer.material.mainTexture = textures.NitroTexture;
                break;
        }
    }
    private TypeOfPickUp Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
            SetAppropriateTexture();
        }
    }

    
}
