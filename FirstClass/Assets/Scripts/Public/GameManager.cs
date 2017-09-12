using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : Singleton<GameManager> {


	public int iStartTag;
	public int iEndTag;
	public string[] team;
	public EnemyConfig[] enemyConfig;
	int len = 20;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int convertXY_ToId(Vector2 vec)
	{
		return (int)(vec.x + vec.y*len);
	}
	public Vector2 convertId_ToXY(int idx)
	{
		return new Vector2 (idx%len, idx/len);
	}
	public void startGame()
	{
		Server.Instance.launch ();
	}
	public void playerReady(bool flgMultiple = false)
	{
		if (flgMultiple) 
		{
			Server.Instance.sendMultipleReady ();
		} else {
			Server.Instance.sendReady ();
		}
	}
	public void setupConfig(int start, int end)
	{;
		this.iStartTag = start;
		this.iEndTag = end;

		SceneManager.LoadScene ("Game");
	}

	public void setupEnemyInfo(string[] team, EnemyConfig[] enemyConfig)
	{
		this.enemyConfig = enemyConfig;
		this.team = team;
	}

	public void recieveBuildingInfo(int tileId, string tower){
		GameScene.Instance.recvBuildMsg (tileId, tower);
	}

	public void sendBuilding(int tileIdx, string tower)
	{
		Server.Instance.sendBuilding (tileIdx, tower);	
	}
	public void sendDeconstrucBuilding(int tileIdx)
	{
		Server.Instance.sendDeconstructBuilding (tileIdx);	
	}

}
