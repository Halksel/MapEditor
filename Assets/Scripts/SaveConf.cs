using UnityEngine;
using System.Collections;

public class SaveConf : MonoBehaviour {
	private MapViewConf MVC;
	public GameObject obj;
	void Start(){
		MVC = obj.GetComponent<MapViewConf>();
	}
	public void ButtonPush() {
		if(MVC != null){
			MVC.SaveMap();
		}
	}
}
