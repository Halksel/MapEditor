<<<<<<< HEAD
﻿using UnityEngine;using System.Collections;using UnityEngine.EventSystems;using UnityEngine.UI;using System.Linq;public class ImageConf : MonoBehaviour, IPointerClickHandler {	private GameObject draggingObject;	public int attribute = 0,kind = 0;	public ToggleGroup tg;	public void Start(){		tg = GameObject.Find("RadioSet").GetComponent<ToggleGroup>();	}	public void OnPointerClick(PointerEventData ped){		GameObject obj = GameObject.Find("SelectMapTip");        draggingObject = obj;		Image draggingImage = draggingObject.GetComponent<Image>();		ImageConf IC = draggingObject.AddComponent<ImageConf>();		Image sourceImage = GetComponent<Image>();		draggingImage.sprite = sourceImage.sprite;		draggingImage.color = sourceImage.color;		draggingImage.material = sourceImage.material;		IC.attribute = tg.ActiveToggles().FirstOrDefault().GetComponent<TogglesConf>().num;		IC.kind = kind+1;	}}
=======
﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class ImageConf : MonoBehaviour, IPointerClickHandler {
	private GameObject draggingObject;
	public int attribute = 0,kind = 0;
	public ToggleGroup tg;

	public void Start(){
		tg = GameObject.Find("RadioSet").GetComponent<ToggleGroup>();
	}

	public void OnPointerClick(PointerEventData ped){
		GameObject obj = GameObject.Find("Dragging Object");
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
	}
}
>>>>>>> 07dbdf0... First Commit
