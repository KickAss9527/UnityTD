using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameScene : MonoBehaviour {
	public GameObject prefTile;
	public GameObject prefEnemy;
	public GameObject prefTower;
	public GameObject prefTower_Slow;
	int curWave = 0;

	Tile objSelectedTile;
	public GameObject panBuild;
	// Use this for initialization
	void Start () {
		
		this.loadGroundTile ();

		this.panBuild.SetActive (false);

		StartCoroutine ("generateEnemies");

	}

	IEnumerator generateEnemies()
	{
		
		for (int i = 0; i < GameManager.Instance.team.Length; i++) 
		{
			yield return new WaitForSeconds(5f);
			string single = GameManager.Instance.team [i];
			for (int j = 0; j < single.Length; j++) 
			{
				string name = single [j]+"";
				this.loadEnemy (name);
				yield return new WaitForSeconds(1f);
			}
		}
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
				cube.transform.SetParent(terrainParent.transform);
				cube.transform.position = new Vector3 ((x - teSize.x/2)*dis, 0, (teSize.y/2 - y)*dis);
				cube.name = GameManager.Instance.convertXY_ToId (new Vector2 (x, y)).ToString ();
			}
		}
	}

	void loadEnemy(string name)
	{
		GameObject terrainParent = GameObject.Find ("terrain");
		int startTag = GameManager.Instance.iStartTag;
		GameObject startTile = terrainParent.transform.Find (startTag.ToString ()).gameObject;

		Enemy en = Instantiate (prefEnemy).GetComponent<Enemy>();
		en.transform.SetParent(GameObject.Find ("enemyParent").transform);
		for (int i = 0; i < GameManager.Instance.enemyConfig.Length; i++) {
			EnemyConfig con = GameManager.Instance.enemyConfig [i];
			if (con.name == name) {
				en.iHealthValue = con.health;
				en.fSpeed = con.speed;
				break;
			}
		}

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
					Tile t = gameObj.GetComponent<Tile> ();
					this.evtSelectTile (t);
				} else {
					this.evtSelectTile (null);
				}
			}
			else{this.evtSelectTile (null);}
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

	public void Click(Transform ts)
	{
		this.objSelectedTile.flgHasTower = true;

		Tower t = null;
		if (ts.name == "0") t = Instantiate (prefTower).GetComponent<Tower> ();
		else if(ts.name == "1") t = Instantiate (prefTower_Slow).GetComponent<Tower> ();

		Vector3 vec3 = this.objSelectedTile.transform.position;
		vec3.y = 5;
		t.transform.SetParent(GameObject.Find ("tower").transform);
		t.transform.position = vec3;

		GameManager.Instance.sendBuilding (int.Parse(this.objSelectedTile.name));
		this.evtSelectTile (null);

	}


		
}
