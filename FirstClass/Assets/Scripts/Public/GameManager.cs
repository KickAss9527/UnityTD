using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : Singleton<GameManager> {

	public string[] strTerrain;
	private int iStartTag;
	private int iEndTag;
	private int[] arrPath;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void startGame()
	{
		Server.Instance.launch ();
	}
	public void playerReady()
	{
		Server.Instance.sendReady ();
	}
	public void setupConfig(string[] t, int start, int end, int[] path)
	{
		this.strTerrain = t;
		this.iStartTag = start;
		this.iEndTag = end;
		this.arrPath = path;
		SceneManager.LoadScene ("Game");
	}
}
