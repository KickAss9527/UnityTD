using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour {
	public GameObject prefTile;
	public GameObject prefEnemy;
	// Use this for initialization
	void Start () {
		
		this.loadGroundTile ();
		this.loadEnemy ();
	}

	void loadGroundTile()
	{
		string[] terrainStr = GameManager.Instance.strTerrain;
		Vector2 teSize = new Vector2 (terrainStr[0].Length, terrainStr.Length);

		GameObject terrainParent = GameObject.Find ("terrain");
		GameObject tt = Instantiate (prefTile);
		float dis = tt.transform.localScale.x*1.02f;
		Destroy (tt);
		for (int x = 0; x < teSize.x; x++) {
			for (int y = 0; y < teSize.y; y++) {
				if (terrainStr [y] [x] == 'X')
					continue;
				GameObject cube = GameObject.Instantiate (tt);
				cube.transform.parent = terrainParent.transform;
				cube.transform.position = new Vector3 ((x - teSize.x/2)*dis, 0, (teSize.y/2 - y)*dis);
				cube.name = GameManager.Instance.convertXY_ToId (new Vector2 (x, y)).ToString ();
			}
		}
	}

	void loadEnemy()
	{
		GameObject terrainParent = GameObject.Find ("terrain");
		int startTag = GameManager.Instance.iStartTag;
		GameObject startTile = terrainParent.transform.Find (startTag.ToString ()).gameObject;

		Enemy en = Instantiate (prefEnemy).GetComponent<Enemy>();
		en.transform.parent = GameObject.Find ("enemyParent").transform;
		en.move ();
	}
	// Update is called once per frame
	void Update () {

	}
		
}
