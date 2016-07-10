using UnityEngine;
using System.Collections;
using System.Drawing;
using System;

public class MapCreater {
	private string filepath ;
	private string[] files;
	private int i ;
	private System.Drawing.Bitmap[] images;

	public MapCreater(){
		filepath = Application.streamingAssetsPath+"/Images/";
		files = System.IO.Directory.GetFiles(Application.streamingAssetsPath+"/Images/","*.png",System.IO.SearchOption.TopDirectoryOnly);
		i = 0;
		Array.Resize(ref images,files.Length);
		foreach (var n in files) {
			images[i] = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(files[i]);
			++i;
		}
	}
	public void CreateMap(int[] kinds, int width,int height,int num){
		i = 0;
		Array.Resize(ref images,files.Length);
		foreach (var n in files) {
			images[i] = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(files[i]);
			++i;
		}
		Bitmap img = new Bitmap(width * images[0].Width, height * images[0].Height);
		//ImageオブジェクトのGraphicsオブジェクトを作成する
		System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
		//全体を黒で塗りつぶす
		g.FillRectangle(Brushes.Black, g.VisibleClipBounds);
		//リソースを解放する
		g.Dispose();
		//作成した画像を表示する
		img.Save(Application.streamingAssetsPath+"/Data/Map" + num + ".png",System.Drawing.Imaging.ImageFormat.Png);
		img.Dispose();
		for(i = 0; i < images.Length;++i){
			images[i].Dispose();
		}
	}
}
