﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiSpawner : MonoBehaviour {

public GameObject taxi;
GameObject clone;
float Timer = 0;



void FixedUpdate ()
	{
	 Timer -= Time.deltaTime;

	 if (Timer <= 0f) {

	 Vector3 StartPos = transform.position;
			clone = Instantiate (taxi, StartPos, Quaternion.identity);
	 Destroy (clone, 13);
	 Timer = Random.Range(0.1f, 1.5f);

		}
	}
}