using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	public GameObject test;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void PlayerEnter(GameObject player)
	{
		test.SetActive (true);
	}

	protected void PlayerExit(GameObject player)
	{
		test.SetActive (true);
	}

}
