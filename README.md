# ReadJEnc
ReadJEnc C#(.NET)用ファイル文字コード種類自動判別ライブラリ

[English version](./README.en.md)

## 概要
C#(.NET Framework)向けテキストファイル文字コード自動判別＆読出ライブラリです。  
自作のgrepツール（TresGrep/HNXgrep）の文字コード自動判別機能をC#のライブラリとして切り出したものです。
 * 対応フレームワーク： .NET 2.0/3.5/4.0/4.5/4.6/4.7, .NET Core 1.0/1.1/2.0, .NET Standard 1.3/2.0

## DLL一式の入手方法
 * NuGetからインストール
	* https://www.nuget.org/packages/ReadJEnc/
 * GitHub(Release)から入手
	* https://github.com/hnx8/ReadJEnc/releases
	* ReadJEnc_(yyyymmdd).zip をダウンロードしてください。
 * Vectorから入手
	* http://www.vector.co.jp/soft/winnt/util/se506899.html

## Readme
 * https://github.com/hnx8/ReadJEnc/blob/master/ReadJEnc_Readme.txt
	* ライブラリ仕様（特徴・対応エンコーディング・ソースコード概要）は、上記のReadme.txtを参照してください。

## 使用方法
テキストファイル読み出し・文字エンコード判別の基本的な手順ですが、  
 1) FileReaderオブジェクトを生成  
 2) Read()メソッドを呼び出し、ファイルのエンコードを把握  
 3) Textプロパティより、実際に読み出したテキストを取得  
 という流れになります。
```cs
	// 文字エンコードを判別したいファイルを、FileInfoオブジェクトなどで指定
	void Example(System.IO.FileInfo file) 
	{
	    // ファイル自動判別読み出しクラスを生成
	    using (Hnx8.ReadJEnc.FileReader reader = new FileReader(file))
	    {
	        // 判別読み出し実行。判別結果はReadメソッドの戻り値で把握できます
	        Hnx8.ReadJEnc.CharCode c = reader.Read(file);
	        // 戻り値のNameプロパティから文字コード名を取得できます
	        string name = c.Name;
	        Console.WriteLine("【" + name + "】" + file.Name);
	        // GetEncoding()を呼び出すと、エンコーディングを取得できます
	        System.Text.Encoding enc = c.GetEncoding(); 
	        // 実際に読み出したテキストは、Textプロパティから取得できます
	        // ※非テキストファイルの場合は、nullが設定されます
	        string text = reader.Text;
	        // 戻り値の型からは、該当ファイルの大まかな分類を判断できます
	        if (c is CharCode.Text) 
	        {
	            Console.WriteLine("-------------------------------------");
	            Console.WriteLine(text);
	        }
	    }
	}
```
 * 実際に動くサンプルとしては、`Hnx8.ReadJEnc.WinForm.Sample`のプロジェクトをご覧ください。
 * APIリファレンスは特に用意していません。オブジェクトブラウザーを参照するか、ライブラリのソースコードを直接読んでください。
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

[Issues](https://github.com/hnx8/ReadJEnc/issues)、[Pull requests](https://github.com/hnx8/ReadJEnc/pulls)でも連絡を受け付けます。

## 謝辞
[英語版README.md](./README.en.md)の整備、ならびにターゲットフレームワークの複数化対応は、[Nordes Ménard-Lamarre](https://github.com/Nordes)さんが行ってくださいました。
厚くお礼申し上げます。

## License
 - [MIT License](https://github.com/hnx8/ReadJEnc/blob/master/LICENSE)
