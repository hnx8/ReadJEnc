# ReadJEnc
ReadJEnc C#(.NET)用ファイル文字コード種類自動判別ライブラリ

[English version](./README.en.md)

## 概要
C#(.NET Framework)向けテキストファイル文字コード自動判別＆読出ライブラリです。  
自作のgrepツール（TresGrep/HNXgrep）の文字コード自動判別機能をC#のライブラリとして切り出したものです。

## DLL一式の入手方法
 * NuGetから入手
	* https://www.nuget.org/packages/ReadJEnc/
	* NuGetパッケージとしてインストールできます。
	* Compatible: .Net 2.0/3.5/4.0/4.5/4.6/4.6.1, .Net Core 2.0, .Net Standard 2.0
 * Vectorから入手
	* http://www.vector.co.jp/soft/winnt/util/se506899.html
	* dllおよびソースコード・動作サンプルを同梱したzipファイルをダウンロードできます。

## Readme
 * https://github.com/hnx8/ReadJEnc/blob/master/ReadJEnc_Readme.txt

## 使用方法
 * Readme、あるいは、ReadJEncSampleのSampleForm.csを参照してください。
 * ほか、バイト配列の内容から文字コードを判定するサンプルが@ITに掲載されています。
	* @IT「[.NET TIPS：ReadJEncを使って文字エンコーディングを推定するには？](http://www.atmarkit.co.jp/ait/articles/1501/20/news073.html)」 

## その他補足
ソースコード主要部分については、以下のblog記事で簡単に解説しています。  
 * [テキストファイル文字コード自動判別(2014年版)（BLOG記事：2014.08.24）](http://d.hatena.ne.jp/hnx8/20140824/1408844344)

## ライブラリ組み込み例
 * TresGrep 
	* http://hp.vector.co.jp/authors/VA055804/TresGrep/
 * HNXgrep 
	* http://hp.vector.co.jp/authors/VA055804/HNXgrep/
 * などなど

## 連絡先
このライブラリに関する不具合・疑問点・感想などありましたら、作者BLOGの適当な記事へフィードバックをお寄せください。  
 * [hnx8 開発室(作者のBLOG) カテゴリ「ReadJEnc」の記事一覧 ](http://d.hatena.ne.jp/hnx8/archive?word=%2A%5BReadJEnc%5D)

## License
 - MIT License
