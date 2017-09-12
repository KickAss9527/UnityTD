using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.AI;
public class GameScene : Singleton<GameScene> {
	public GameObject prefTile;
	public GameObject prefEnemy;
	public GameObject prefTower;
	public GameObject prefTower_Slow;

	GameObject pathTrail;
	Sequence pathTrailSeq;
	int curWave = 0;

	Tile objSelectedTile;
	Tower objSelectedTower;
	public GameObject panBuild;
	public GameObject UITower;
	// Use this for initialization

	float trailY = 1f;
	public Vector3 convertXYToPos(Vector2 vec)
	{
		return new Vector3 ((vec.x - 10) * 10 + 5f, 0f, (10 - vec.y) * 10 - 5f);
	}

	public Vector3 getStartPos()
	{
		Vector2 vec = GameManager.Instance.convertId_ToXY (GameManager.Instance.iStartTag);
		Vector3 v = convertXYToPos (vec);
		v.y = 2f;
		return v;
	}
	public Vector3 getTargetPos(){
		Vector2 vec = GameManager.Instance.convertId_ToXY (GameManager.Instance.iEndTag);
		Vector3 v = convertXYToPos (vec);
		v.y = 2f;
		return v;
	}

	void runPathTrail()
	{
		NavMeshAgent agent = pathTrail.GetComponent<NavMeshAgent> ();
//		agent.isStopped = true;
		this.pathTrail.transform.position = getStartPos ();
		agent.SetDestination (getTargetPos ());
//		agent.isStopped = false;

		GameObject.Find ("enemy").transform.position = getStartPos ();
		GameObject.Find ("enemy").GetComponent<NavMeshAgent> ().SetDestination (getTargetPos ());

	}
	void Start () {
		
		this.loadGroundTile ();

		this.panBuild.SetActive (false);
		this.UITower.SetActive (false);
		StartCoroutine ("generateEnemies");
		this.pathTrail = GameObject.Find ("Trail");

		this.runPathTrail ();
	}


	public float getTileScale(){
		return prefTile.transform.localScale.x;
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
		GameObject terrainParent = GameObject.Find ("terrain");
		Transform ground = terrainParent.transform.Find ("ground");


		for (int x = 0; x < ground.localScale.x; x++) {
			for (int y = 0; y < ground.localScale.z; y++) 
			{
				Tile tile = Instantiate (prefTile).GetComponent<Tile> ();
				tile.name = (x + y * ground.localScale.x).ToString();
				tile.transform.position = new Vector3 ((x-10)*10 + 5f,  0f, (10-y)*10 - 5f);
				tile.transform.SetParent (terrainParent.transform);
				BoxCollider bc = tile.gameObject.GetComponent<BoxCollider> ();
//				bc.isTrigger = true;

			}
		}

		for (int i = 0; i < ground.childCount; i++) {
//			ground.GetChild (i).gameObject.SetActive (false);
		}
	}

	void loadEnemy(string name)
	{
		Vector3 dst = getTargetPos ();
		Vector3 src = getStartPos ();
		src.y = 1.4f;

		GameObject terrainParent = GameObject.Find ("terrain");
		int startTag = GameManager.Instance.iStartTag;
		GameObject startTile = terrainParent.transform.Find (startTag.ToString ()).gameObject;

		Enemy en = Instantiate (prefEnemy, src,  terrainParent.transform.rotation).GetComponent<Enemy>();
		en.transform.SetParent(GameObject.Find ("enemyParent").transform);

		for (int i = 0; i < GameManager.Instance.enemyConfig.Length; i++) {
			EnemyConfig con = GameManager.Instance.enemyConfig [i];
			if (con.name == name) {
				en.iHealthValue = con.health;
				en.fSpeed = con.speed;
//				en.transform.position = src;
				en.GetComponent<NavMeshAgent> ().SetDestination (dst);
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
				if (gameObj.tag == "Tile") 
				{
					Debug.Log (gameObj.name);
					Tile t = gameObj.GetComponent<Tile> ();
					this.evtSelectTile (t);
					this.evtSelectTower (null);
				}
				else if (gameObj.tag == "Tower")
				{
					Tower t = gameObj.GetComponent<Tower> ();
					this.evtSelectTower (t);
					this.evtSelectTile (null);
				}
				else {
					this.evtSelectTile (null);
					this.evtSelectTower (null);
				}
			}
			else{
				this.evtSelectTile (null);
				this.evtSelectTower (null);
			}
		}

		NavMeshAgent agent = pathTrail.GetComponent<NavMeshAgent> ();
		float distance = Vector3.Distance (pathTrail.transform.position, agent.destination);
//		Debug.Log (distance <= agent.stoppingDistance );
		if (distance <= agent.stoppingDistance) 
		{
//			agent.isStopped = true;
			this.runPathTrail ();
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

	void evtSelectTower(Tower t)
	{
		if (objSelectedTower)
			this.objSelectedTower.unSelect ();
		if (t == null) 
		{
			this.UITower.SetActive (false);

			return;
		}
		t.evtSelect ();
		this.objSelectedTower = t;

		this.UITower.SetActive (true);
	}

	public void Click(Transform ts)
	{
		string towerType="";
		if (ts.name == "0") {
			towerType = "Tower";
		} else if (ts.name == "1") {
			towerType = "Tower_Slower";
		}
		else if (ts.name == "sell") {

			GameManager.Instance.sendDeconstrucBuilding (objSelectedTower.tileId);
			Destroy (objSelectedTower.gameObject);
			this.UITower.SetActive (false);
			this.objSelectedTower = null;
			return;
		}
			
		this.buildTower (towerType, int.Parse (objSelectedTile.name));
		GameManager.Instance.sendBuilding(int.Parse(this.objSelectedTile.name), towerType);
		this.evtSelectTile (null);

	}

	void buildTower(string type, int tileId)
	{
		Tile tile = GameObject.Find ("terrain").transform.Find(tileId.ToString()).GetComponent<Tile>();
		tile.flgHasTower = true;

		GameObject gobj = type == "Tower" ? prefTower : prefTower_Slow;
		Tower t = Instantiate (gobj).GetComponent<Tower> ();
		Vector3 pos = tile.transform.position;
		pos.y = 5;
		t.transform.SetParent (GameObject.Find ("tower").transform);
		t.transform.position = pos;
		t.tileId = tileId;
	}

	public void recvBuildMsg(int tileId, string tower)
	{
		if(tower != null)
		{
			this.buildTower (tower, tileId);
			Debug.Log ("building");
		}
		else
		{
			Debug.Log ("kill building");
			Transform towerParent = GameObject.Find ("tower").transform;
			for(int i=0; i<towerParent.childCount; i++)
			{
				Tower t = towerParent.GetChild(i).GetComponent<Tower>();
				if(t.tileId == tileId)
				{
					Destroy(t.gameObject);
					break;
				}
			}
		}
	}

		
}
