using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiMover : MonoBehaviour {

public float speed = 1f;



void FixedUpdate () 
	{
	transform.Translate (Vector3.forward * speed);
	}
}

