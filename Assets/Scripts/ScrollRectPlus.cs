using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ScrollRectPlus : ScrollRect {	// ScrollRectを継承
	private Image scrollHandle; // スクロールバーのHandle(Image)
	private bool isScroll = false, isDrag = false; // フラグ

	protected override void Awake (){
		base.Awake ();
		// HandleのImageを取得
		scrollHandle = verticalScrollbar.gameObject.GetComponentInChildren<Image>();
	}
	
	protected override void OnEnable(){
		base.OnEnable ();
		scrollHandle.CrossFadeAlpha (0.0f, 0.0f, true);
	}
	
	void Update(){
		if (Mathf.Abs(velocity.y) < 10f) {
			if(!isDrag && isScroll){
				scrollHandle.CrossFadeAlpha (0.0f, 0.25f, true); // フェードアウト
				isScroll = false;
			}
		}
	}

	public override void OnBeginDrag(PointerEventData eventData){
		base.OnBeginDrag (eventData);
		isDrag = true;
		isScroll = true;
		
		scrollHandle.CrossFadeAlpha (0.5f, 0.25f, true);  // フェードイン
	}

	public override void OnEndDrag(PointerEventData eventData){
		base.OnEndDrag (eventData);
		isDrag = false;
	}
}