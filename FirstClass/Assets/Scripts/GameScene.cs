﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameScene : MonoBehaviour {
	public GameObject prefTile;
	public GameObject prefEnemy;
	public GameObject prefTower;
	Tile objSelectedTile;
	public GameObject panBuild;
	// Use this for initialization
	void Start () {
		
		this.loadGroundTile ();
		this.loadEnemy ();
		this.panBuild.SetActive (false);
		
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
		if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended)) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast (ray, out hitInfo)) {
				GameObject gameObj = hitInfo.collider.gameObject;
				if (gameObj.tag == "Tile") {
					Debug.Log (gameObj.name);
					Tile t = gameObj.GetComponent<Tile>();
					this.evtSelectTile (t);
				}
			}
		}
	}

	void evtSelectTile(Tile t)
	{
		if (objSelectedTile) 
		{
			this.objSelectedTile.evtUnselect ();
			this.panBuild.SetActive (false);
		}
		this.objSelectedTile = t;
		if (t) {
			this.panBuild.SetActive (true);
			this.objSelectedTile.evtSelect ();
		} 
		else 
		{
			this.objSelectedTile = null;
		}

	}

	public void Click()
	{
		this.objSelectedTile.flgHasTower = true;

		Tower t = Instantiate (prefTower).GetComponent<Tower> ();
		Vector3 vec3 = this.objSelectedTile.transform.position;
		vec3.y = 5;
		t.transform.parent = GameObject.Find ("tower").transform;
		t.transform.position = vec3;

		GameManager.Instance.sendBuilding (int.Parse(this.objSelectedTile.name));
		this.evtSelectTile (null);

	}
		
}