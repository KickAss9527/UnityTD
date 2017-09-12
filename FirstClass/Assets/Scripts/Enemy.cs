using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AI;
public class Enemy : MonoBehaviour {
	public float fSpeed = 10.5f;
	int iCurIdx = 0;
	int iTargetTileID;
	Vector3 vTargetPos;
	public int iHealthValue = 100;
	int iHealthCur = 0;
	public GameObject preBar;
	Sequence mySeq;
	Slider objSlider;

	// Use this for initialization

	void Awake () 
	{
		
		this.iCurIdx = 0;
//		this.transform.position = GameScene.Instance.getStartPos ();
	}

	void Start()
	{
		this.iHealthCur = this.iHealthValue;
		this.objSlider = Instantiate (preBar).GetComponent<Slider> ();
		this.objSlider.transform.SetParent(GameObject.Find ("CanvasMain").transform);
		this.updateHealthBar ();

		move ();
	}


	void Update () 
	{
//		if (checkMoveNext ()) {
//		}
//		else 
//		{
//			this.escapse ();
//		}
		NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent> ();
		float distance = Vector3.Distance (transform.position, agent.destination);
//		Debug.Log (distance <= agent.stoppingDistance );
		if (distance <= agent.stoppingDistance) 
		{
			//			agent.isStopped = true;
//			this.runPathTrail ();
		}
		Vector2 player2DPos = Camera.main.WorldToScreenPoint (transform.position);
		player2DPos.y += 20f;
		this.objSlider.transform.position = player2DPos;
	}

	void OnTriggerEnter(Collider collider)
	{
		Bullet bul = collider.transform.GetComponent<Bullet> ();
		if (!bul)//撞到建筑物了
			return;
		this.iHealthCur -= bul.iDmg;
		Destroy (bul.gameObject);

		this.updateHealthBar ();
		this.checkDeath ();
	}

	void updateHealthBar()
	{
		float p = iHealthCur * 1f / iHealthValue;
		this.objSlider.value = p;
		Color co = Color.Lerp (Color.red, Color.green, p);
		this.objSlider.fillRect.transform.GetComponent<Image> ().color = co;
	}

	void checkDeath()
	{
		if (iHealthCur > 0)
			return;
		this.goDie ();

	}
	public void move()
	{
		NavMeshAgent agent = GetComponent<NavMeshAgent> ();
		Debug.Log (GameScene.Instance.getTargetPos ());

		agent.SetDestination (GameScene.Instance.getTargetPos());
//		agent.isStopped = false;
	}
	void goDie(){
		Destroy (this.objSlider.gameObject);
		Destroy (this.gameObject);
	}

	void escapse()
	{	
		this.goDie ();
	}
}
