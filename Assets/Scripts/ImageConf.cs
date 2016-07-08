using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public class ImageConf : MonoBehaviour, IPointerClickHandler {
	private GameObject draggingObject;
	public int attribute = 0,kind = 0;
	public ToggleGroup tg;

	public void Start(){
		tg = GameObject.Find("RadioSet").GetComponent<ToggleGroup>();
	}

	public void OnPointerClick(PointerEventData ped){
		GameObject obj = GameObject.Find("SelectMapTip");
        draggingObject = obj;
		Image draggingImage = draggingObject.GetComponent<Image>();
		ImageConf IC = draggingObject.AddComponent<ImageConf>();
		Image sourceImage = GetComponent<Image>();
		draggingImage.sprite = sourceImage.sprite;
		draggingImage.color = sourceImage.color;
		draggingImage.material = sourceImage.material;
		IC.attribute = int.Parse(tg.ActiveToggles().FirstOrDefault().name);
		IC.kind = kind+1;
	}
}