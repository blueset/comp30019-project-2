﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public int[] tutorialObjects = {0, 6, 8, 9, 10, 11};
	public GameObject[] objects;
	public float spawnWait;
	public float spawnMostWait;
	public float spawnLeastWait;
	public int startWait;
	public bool stop = false;
	public float threshold;

	private LaneProperties laneProperties;
	private TutorialTextManager tutorialTextManager;
	private int randomObject;
	private float countTime = 0f;
	private int stage = 0;
	private bool beginStage = true;
	
	

	// Use this for initialization
	void Start () {

		threshold = 0.7f;

		laneProperties = GameObject.FindWithTag("GameController")
                      	.GetComponent<LaneProperties>();
		
		tutorialTextManager = GameObject.FindWithTag("GameController")
							  .GetComponent<TutorialTextManager>();

		StartCoroutine(obstacleSpawner());
		
	}
	
	// Update is called once per frame
	void Update () {

		countTime += Time.deltaTime;

		if ((int) countTime / 15 > stage && stage < 5) {
			stage += 1;
			beginStage = true;
		}
		
	}

	IEnumerator obstacleSpawner () {

		yield return new WaitForSeconds(startWait);

		while (!stop) {

			int L, R, C, maxObj = 0;
			Debug.Log("Stage = " + stage);
			do {
				switch (stage) {

					case 0:
						maxObj = 6;
						L = objects.Length;
						R = objects.Length;
						C = Random.Range(0, maxObj);
						// show tutorial text
						Debug.Log("Stage 0: " + L + ", " + C + ", " + R);
						break;
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
						maxObj = tutorialObjects[stage] + 1;
						if (beginStage) {
							L = objects.Length;
							R = objects.Length;
							C = tutorialObjects[stage];
							beginStage = false;
							//show tutorial text
							
						} else {
							L = RandomObject(maxObj);
							R = RandomObject(maxObj);
							C = RandomObject(maxObj);
						}
						break;
					default:
						maxObj = objects.Length;
						L = RandomObject(maxObj);
						R = RandomObject(maxObj);
						C = RandomObject(maxObj);
						break;

				}
			} while (
				!isPassable(L, R, C)
			);

			Vector3 spawnPosition = new Vector3(0, 1, 0);

			if (C != objects.Length) {
				Instantiate(objects[C],
							spawnPosition + transform.TransformPoint(0, 0, 0),
							gameObject.transform.rotation);
			}

			if (L != objects.Length) {
				spawnPosition.z = -laneProperties.laneWidth;
				Debug.Log("L: " + spawnPosition);

				Instantiate(objects[L],
							spawnPosition + transform.TransformPoint(0, 0, 0),
							gameObject.transform.rotation);
			}

			if (R != objects.Length) {
				spawnPosition.z = laneProperties.laneWidth;
				Debug.Log("R: " + spawnPosition);

				Instantiate(objects[R],
							spawnPosition + transform.TransformPoint(0, 0, 0),
							gameObject.transform.rotation);
			}

	
			spawnWait = Random.Range(spawnLeastWait, spawnMostWait);

			Debug.Log(L + ", " + C + ", " + R);

			yield return new WaitForSeconds(spawnWait);

		}

	}

	private int RandomObject (int max) {

		int objectId = 12;

		float randomValue = Random.value;

		if (randomValue < threshold) {

			if (max != objects.Length) {
				return Random.Range(0, max);
			}

			float i = randomValue / threshold;

			if (i < 0.4) {
				// obstacle
				objectId = Random.Range(0, 6);
			} else if (i < 0.6) {
				// tree
				objectId = 6;
			} else if (i < 0.8) {
				// item +
				objectId = Random.Range(7, 10);
			} else {
				// item -
				objectId = Random.Range(10, 12);
			}

		}

		return objectId;
	}

	private bool isPassable (int L, int R, int C) {
		if (L == 6 && R == 6 && C == 6) {
			return false;
		}
		return true;
	}

}
