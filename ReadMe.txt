事前準備
"MapEditor/MapEditor_Data/StreamingAssets/Images/"の中に、マップチップとして使いたい画像データを入れてください
画像データの大きさは統一してください

操作説明

同梱の.exeで起動します

キーコンフィグ
  S : Saveのドロップダウンを開きます、セーブスロットを一つ選んでください、新規作成も可能です、その場合は一番下を選択してください
  D           : Loadのドロップダウンを開きます、ロードスロットを一つ選んでください
  F           : マップを全てクリアします、サイズは変わりません
  V           : マップチップに付随している、各々の属性を表示してくれます
  G           : 現在描画されているマップチップから一枚の画像を生成します
  左クリック  : マップチップビューからマップチップを選択、マップビューではチップの設置が出来ます
  右クリック  : マップビューのチップを削除できます

右側のいろいろ
  W : 表示されている数字は、マップの現在の横サイズです、数字を入力することで横サイズを変更できます
  H : Wの横ではなく縦ver.
  X : 現在のマップチップの縦サイズです、変更はできません
  Y : マップチップの横サイズ

右下のラジオボタン達
  マップに任意の属性（あるいはタグ）をつけることができます。
  使い方としては、
  0 : 進入不可
  1 : 進入可能
  2 : エネミー出現
  3 : ダメージ床
  のような感じになりますが、あくまでも吐き出すデータに書き込まれるだけなので、実際にどう使うのかはプログラマー次第です

出力形式
  "MapEditor/MapEditor_Data/StreamingAssets/Map/"の中に生成されたマップ画像が、"MapEditor/MapEditor_Data/StreamingAssets/Data/"の中に、以下の形式でマップのデータが書き込まれています。

保存形式
  一行目 X:Y,W:H,K -- kはImages/の中のチップの総数
  二行目 チップの名前が列挙されています
  三行目から
  A:B,
  形式のデータがW*Hこ並んでいます、A = 属性, B = チップの通し番号です

