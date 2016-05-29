using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class MapFragConf : MonoBehaviour, IPointerClickHandler {
	private GameObject draggingObject;
	public int attribute = 0,kind = 0;
	public ToggleGroup tg;
	private Text txt;

	private bool isOn = false;

	public void Start(){
		tg = GameObject.Find("RadioSet").GetComponent<ToggleGroup>();
		txt = GetComponentInChildren<Text>();
		if(txt ){
			txt.enabled = isOn;
			txt.transform.localPosition = new Vector3(20,20,0);
			txt.transform.localScale = new Vector3(1,1,0);
			txt.text = attribute.ToString();
			txt.GetComponent<RectTransform>().sizeDelta = new Vector2(40,40);
		}
	}

	void Update(){
		
	}

	public void OnPointerClick(PointerEventData ped){
		GameObject obj = GameObject.Find("SelectMapTip");
		draggingObject = obj;
		//GameObject obj = GameObject.Find("Dragging Object");
		if(obj){
			Destroy(obj);
		}
		draggingObject = new GameObject("Dragging Object");
		Image draggingImage = draggingObject.AddComponent<Image>();
		ImageConf IC = draggingObject.AddComponent<ImageConf>();
		Image sourceImage = GetComponent<Image>();
		draggingImage.transform.position = ped.position;
		draggingImage.sprite = sourceImage.sprite;
		draggingImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
		draggingImage.color = sourceImage.color;
		draggingImage.material = sourceImage.material;
		draggingObject.transform.position = ped.position;
		IC.attribute = tg.ActiveToggles().FirstOrDefault().GetComponent<TogglesConf>().num;
		IC.kind = kind+1;
		if(txt){
			txt.text = attribute.ToString();
		}
	}
	public void Visualize(){
		isOn = !isOn;
		txt.enabled = isOn;
		txt.text = attribute.ToString();
		txt.fontSize = 20;
		txt.fontSize = 20;
	}
}
