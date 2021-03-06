﻿using UnityEngine;
using System.Collections;

public class CarAccelerator : MonoBehaviour {
    [SerializeField]
    private float AccelerationBonus = 20.0f;
    [SerializeField]
    private float AccelerationDuration = 3.0f;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Vehicles"))
        {
            Rigidbody car = collision.gameObject.GetComponent<Rigidbody>();
            StartCoroutine("AcceleratorCoroutine", car);
        }
    }

    IEnumerator AcceleratorCoroutine(Rigidbody car)
    {
        double time = 0.0;
        int layerMask = LayerMask.GetMask("Track");
        
        while(time < AccelerationDuration)
        {
            time += Time.fixedDeltaTime;
            RaycastHit groundCheck;
            Physics.Raycast(car.position, -Vector3.up, out groundCheck, Mathf.Infinity, layerMask);
            if (groundCheck.distance < 0.1)
            {
                car.AddForce(car.rotation * new Vector3(0.0f, 0.0f, AccelerationBonus));
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
