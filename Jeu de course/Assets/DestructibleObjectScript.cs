using UnityEngine;
using System.Collections;

public class DestructibleObjectScript : MonoBehaviour {

    [SerializeField]
	private int numberOfHitToDestroy = 2;
    [SerializeField]
    private Texture HalfHealthTexture;
    [SerializeField]
    private int layerToTakeDamage;

    private int numberOfHitReceived;
    // Use this for initialization
	void Start () {
        numberOfHitReceived = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        if((other.gameObject.layer & LayerMask.NameToLayer("Shell")) > 0)
        {
            ++numberOfHitReceived;
            renderer.material.mainTexture = HalfHealthTexture;
            if (numberOfHitReceived >= numberOfHitToDestroy)
            {
                Destroy(gameObject);
            }
        }

    }
}
