using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SizeSetter : MonoBehaviour {

	public InputField x,y;
	[SerializeField]
	private float width,height;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetSize(float f,float f2){
		width = f;
		height = f2;
		x.text = width.ToString();
		y.text = height.ToString();
	}
}
