using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

using UnityEngine;

public class server : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TcpClient tcpClient = new TcpClient ();
		tcpClient.Connect ("192.168.1.101", 8888);
		Debug.Log ("hello");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
