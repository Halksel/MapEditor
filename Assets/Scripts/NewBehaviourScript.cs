using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {

	public Texture2D t;
	private GameObject obj;
	// Use this for initialization
	void Start () {
		obj = (GameObject)Instantiate(new GameObject(),new Vector3(0,0,0),transform.rotation);
		Sprite spr = Sprite.Create(t,new Rect(0,0,40,40),Vector2.zero);
		obj.AddComponent<Image>();
		obj.GetComponent<Image>().sprite = spr;
		obj.name = "str";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
