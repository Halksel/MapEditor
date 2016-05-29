﻿using UnityEngine;
using System.Collections;
using System.Drawing;
using System;

public class MapGenerater{
	private string filepath ;
	private string[] files;
	private int i ;
	private System.Drawing.Bitmap[] images;

	public MapGenerater(){
		filepath = Application.streamingAssetsPath+"/Images/";
		files = System.IO.Directory.GetFiles(Application.streamingAssetsPath+"/Images/","*.png",System.IO.SearchOption.TopDirectoryOnly);
	}
	public void GenerateMap(int[,] kinds, int width,int height,int num){
		i = 0;
		Array.Resize(ref images,files.Length);
		foreach (var n in files) {
			images[i] = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(files[i]);
            GC.Collect();
			++i;
		}
        if (images.Length > 0) {
            int imageW = images[0].Width;
            int imageH = images[1].Height;
            Bitmap img = new Bitmap(width * 40, height * 40);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);

            /*全体を黒で塗りつぶす
            g.FillRectangle(Brushes.Black, g.VisibleClipBounds);
            //黄色い扇形を描画する
            g.DrawPie(Pens.Yellow, 60, 10, 80, 80, 30, 300);
            */

            for (i = 0; i < width; ++i) {
                for (int j = 0; j < height; ++j) {
                    if (kinds[i,j] >0) {
                        g.DrawImage(images[kinds[i,j]-1],i*imageW,j*imageH);
                    }
                }
            }

            //リソースを解放する
            g.Dispose();
            //作成した画像を表示する
            Debug.Log(Application.streamingAssetsPath + "/Map");
            img.Save(Application.streamingAssetsPath + "/Map/Map" + num + ".png", System.Drawing.Imaging.ImageFormat.Png);
            img.Dispose();
            for (i = 0; i < images.Length; ++i) {
                images[i].Dispose();
            }
        }
	}
}