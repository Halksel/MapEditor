using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class MapFragConf : MonoBehaviour{
	public int attribute = 0,kind = 0;
	public bool isOn = false;
	public ToggleGroup tg;

	private Text txt;
	private string text;

	public void Start(){
		tg = GameObject.Find("RadioSet").GetComponent<ToggleGroup>();
		txt = GetComponentInChildren<Text>();
		if(txt){
			txt.enabled = isOn;
			txt.transform.localPosition = new Vector3(20,20,0);
			txt.transform.localScale = new Vector3(1,1,0);
			text = attribute.ToString();
			txt.text = text;
			txt.GetComponent<RectTransform>().sizeDelta = new Vector2(40,40);
		}
	}

	public void Visualize(){
		text = attribute.ToString();
		isOn = !isOn;
		txt.enabled = isOn;
		txt.text = text;
		txt.fontSize = 20;
		txt.fontSize = 20;
		if (this.GetComponent<Image> ().sprite != null) {
			Texture2D tmp = this.GetComponent<Image> ().sprite.texture;
			var col = tmp.GetPixel (tmp.width / 2, tmp.height / 2);
			float avg = new List<float>{col.r,col.g,col.b}.Max();
			Debug.Log(avg);
			if(avg < 0.5f){
				col = Color.white;
			}
			else{
				col = Color.black;
			}
			txt.color = col;
		}
	}
}
