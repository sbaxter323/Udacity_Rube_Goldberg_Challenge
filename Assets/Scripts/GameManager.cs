using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject playerObject;
    public GameObject platformPrefab;
    public GameObject ballPrefab;

    private GameObject platformInstance;
    private GameObject ballInstance;

	// Use this for initialization
	void Start () {
        platformInstance = Instantiate(platformPrefab) as GameObject;
        platformInstance.transform.position = new Vector3(0f, 2f, -5f);
        playerObject.SetActive(true);
        playerObject.transform.position = new Vector3(0f, 3.5f, -4.5f);
        ballInstance = Instantiate(ballPrefab) as GameObject;
        ballInstance.transform.position = new Vector3(0f, 3f, -4.25f);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
