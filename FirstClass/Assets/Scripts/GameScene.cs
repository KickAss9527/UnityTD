using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour {

	// Use this for initialization
	void Start () {

//		string[] terrainStr = GameManager.Instance.strTerrain;
//		Vector2 teSize = new Vector2 (terrainStr[0].Length, terrainStr.Length);
//
//		GameObject terrainParent = GameObject.Find ("terrain");
//		GameObject tt = GameObject.Find ("groundTile");
//		float dis = tt.transform.localScale.x*1.02f;
//		for (int x = 0; x < teSize.x; x++) {
//			for (int y = 0; y < teSize.y; y++) {
//				if (terrainStr [y] [x] == 'X')
//					continue;
//				GameObject cube = GameObject.Instantiate (tt);
//				cube.transform.parent = terrainParent.transform;
//				cube.transform.position = new Vector3 ((teSize.x/2 - x)*dis, 0, (y - teSize.y/2)*dis);
//			}
//		}
//		GameObject.Destroy (tt);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
}
