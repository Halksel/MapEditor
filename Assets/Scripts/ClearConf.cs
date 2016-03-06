using UnityEngine;
using System.Collections;

public class ClearConf : MonoBehaviour {
	private MapViewConf MVC;
	public GameObject obj;
	void Start(){
		MVC = obj.GetComponent<MapViewConf>();
	}
	public void ButtonPush() {
		MVC.Clear();
	}
}
