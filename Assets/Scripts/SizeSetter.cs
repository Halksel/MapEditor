using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SizeSetter : MonoBehaviour {

	public InputField x,y;
	[SerializeField]
	private float width,height;

	public void SetSize(float f,float f2){
		width = f;
		height = f2;
		x.text = width.ToString();
		y.text = height.ToString();
	}
}
