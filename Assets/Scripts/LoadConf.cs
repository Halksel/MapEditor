using UnityEngine;
using System.Collections;

public class LoadConf : MonoBehaviour {
	private MapViewConf MVC;
	public GameObject obj;
	void Start(){
		MVC = obj.GetComponent<MapViewConf>();
	}
	public void ButtonPush() {
		MVC.LoadMap();
	}
}
