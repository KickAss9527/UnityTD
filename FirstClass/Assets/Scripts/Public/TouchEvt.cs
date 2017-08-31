using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEvt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("st");	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void onEnable()
	{
		
	}

	public void onDrag(PointerEventData baseEventData)
	{
		
		Debug.Log (baseEventData.delta.x);

	}
}
