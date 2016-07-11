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
	private string text;

	private bool isOn = false;

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

	public void OnPointerClick(PointerEventData ped){
		GameObject obj = GameObject.Find("Dragging Object");
		draggingObject = obj;
		if(obj){
			Destroy(obj);
		}
		Image draggingImage = draggingObject.AddComponent<Image>();
		ImageConf IC = draggingObject.AddComponent<ImageConf>();
		Image sourceImage = GetComponent<Image>();
		draggingImage.transform.position = ped.position;
		draggingImage.sprite = sourceImage.sprite;
		draggingImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
		draggingImage.color = sourceImage.color;
		draggingImage.material = sourceImage.material;
		draggingObject.transform.position = ped.position;
		IC.attribute = int.Parse(tg.ActiveToggles().FirstOrDefault().name);
		IC.kind = kind+1;
		text = attribute.ToString();
		if(txt){
			txt.text = text;
		}
	}
	public void Visualize(){
		text = attribute.ToString();
		isOn = !isOn;
		txt.enabled = isOn;
		txt.text = text;
		txt.fontSize = 20;
		txt.fontSize = 20;
	}
}
