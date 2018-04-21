================================================================================
==
== ReadJEnc C#(.NET)用ファイル文字エンコード自動判別ライブラリ
==   Ver 1.3.1.1 (2018/04/21)
==
==  GitHub         [ https://github.com/hnx8/ReadJEnc ]
==  NuGet          [ https://www.nuget.org/packages/Hnx8.ReadJEnc/ ]
==  Vector         [ http://www.vector.co.jp/soft/winnt/util/se506899.html ]
==
==   Copyright (C) 2014-2018 hnx8(H.Takahashi) 
==   Blog [ http://d.hatena.ne.jp/hnx8/ ]
==
== Released under the MIT license
== https://github.com/hnx8/ReadJEnc/blob/master/LICENSE
==
================================================================================

【１】ReadJEncについて

C#(.NET Framework)向けテキストファイル文字エンコード自動判別＆読出ライブラリです。

※自作grepソフト(TresGrep/HNXgrep)のエンコード判別処理をライブラリ化したものです。
　TresGrepは http://hp.vector.co.jp/authors/VA055804/TresGrep/ から入手できます。
　HNXgrepは http://hp.vector.co.jp/authors/VA055804/HNXgrep/ から入手できます。


＜特徴＞

(1)アプリケーションへの組み込みに適したコンパクトなライブラリ
 ・DLL版サイズ17KB、NuGetからのインストールも可能
 ・ソースコードはC#(.cs)４ファイルのみ、ソースコードを直接取り込んで使用してもOK

(2)BOMあり/BOMなしUTF、ShiftJIS、EUC/JIS(補助漢字可)のほか、ANSI(CP1252)も判別
　 非テキストファイル(バイナリファイル)の種類判別にも対応
 ※モード切替により、日本語以外（欧米各国・中国語等）のテキストファイルも判別可

(3)軽量高速のわりに高精度な文字エンコード判定
 ・複数の文字エンコードでデコード可能な場合、どの文字コードとみなすのがより妥当か
　 直前に出てきた文字との整合性をもとに判定、
　 ShiftJIS半角カナやANSIなどの誤判定が起こる可能性を低減
 ・複数ファイル連続読み出し時は、非テキストであることが明らかなファイルの
　 読み込み・判別をスキップしたりと、高速化のための各種チューニングを実施

(4)ファイル読み出し～stringテキスト取り出しまで一括実行
 ・ファイルオープンエラー／テキストのデコード失敗等はライブラリ内でチェック、
　 呼び出しアプリケーション側でのエラーハンドリング処理は原則不要
 ・ファイルではなくByte配列に対してのエンコード判別も可能


＜判別可能な文字エンコード・ファイル種類＞

(1)BOMつきUnicode(UTF-8/UTF-16/UTF-16B/UTF-32/UTF-32B)
(2)BOMなしUnicode(UTF-8N、およびASCII文字始まりのUTF-16BE/UTF-16LE)
(3)ASCII    : 日本語などの非ASCII文字が１文字も登場しないテキストファイル
(4)ANSI1252 : 欧米版WindowsのISO-8859-1(CP1252)
(5)ShiftJIS : MS版(CP932)
(6)EUCJP    : MS版(CP51932)／0x8F補助漢字ありEUC(CP20932相当)の２種類を識別
(7)JIS      : MS版(CP50221/CP50222)／JIS90補助漢字(CP20932相当)にも対応
(8)ISO2022KR(CP50225)

(9)文字エンコード自動判別オブジェクトを切り替えた場合、
　 　ShiftJIS/EUC-JPの代わりに以下のエンコードについて判別

  ・ANSI  [欧文（西欧）]: (UTF/ASCII/ANSI/ISO2022の判定のみ行い、ShiftJIS/EUC等は無視)
  ・TW    [繁体字中国語]: Big5(CP950)／EUC-TW(CP20000)
  ・CN    [簡体字中国語]: GB18030(CP54936)
  ・KR    [ハングル]    : UHC(CP949)

  ・CP1250[中欧東欧言語]: ISO-8859-2(CP1250)  ※ANSIは判定しなくなります
  ・CP1251[キリル言語]  : (CP1251)            ※ISO-8859-5/KOI-8は未対応
  ・CP1253[ギリシャ語]  : ISO-8859-7(CP1253)
  ・CP1254[トルコ語]    : ISO-8859-9(CP1254)  ※ANSIは判定しなくなります
  ・CP1255[ヘブライ語]  : ISO-8859-8(CP1255)
  ・CP1256[アラビア語]  : (CP1256)            ※ISO-8859-6/ASMO-449は未対応
  ・CP1257[バルト言語]  : ISO-8859-13(CP1257) ※ANSIは判定しなくなります
  ・CP1258[ベトナム語]  : TCVN-5712  (CP1258) ※ANSIは判定しなくなります。VISCIIは未対応
  ・TIS620[タイ語]      : TIS-620(CP874)

(10)非テキストファイル(以下のものを識別)
　・画像ファイル(BMP/GIF/JPEG/PNG/TIFF/ICON)
　・圧縮ファイル(ZIP/GZIP/7z/RAR/CAB/BZIP2/Z)
　・PDFファイル
　・Javaバイナリ(classファイル)
　・Windowsバイナリ(exe,dll等)
　・Windowsショートカットファイル
　・上記いずれにも該当しない非テキストファイル
　・空ファイル、巨大ファイル、読み出しエラーとなったファイル


＜対応.NET Frameworkバージョン＞

 .NET Framework 2.0以降(2.0/3.5、4.0/4.5.x/4.6.x/4.7.x)
 .Net Core 2.0以降
 .Net Standard 1.3以降、2.0以降

 ※DLLはVisualStudio2017（もしくはVSCode）でビルドできます。
   
 ※同梱している動作サンプルは.NET Framework 4.0以降の環境で動作します。
 

＜補足＞

 ReadJEncは、「りーどじぇんく」と読みます。


【２】ライブラリの使い方

◎まず、ReadJEncライブラリを、各自のVisualStudioプロジェクトに組み込みます。
  
 (a)NuGetからインストールする場合

 ・Visual StudioのNuGetパッケージマネージャを起動し、
   オンライン(nuget.org)より「ReadJEnc」を検索してインストールしてください。

 (b)GitHubのソースを利用する場合

 ・ReadJEncのGitHubリポジトリ [ https://github.com/hnx8/ReadJEnc ]より
   「Source/Hnx8.ReadJEnc」フォルダ内のソースコード４ファイル
   （CharCode.cs、FileType.cs、FileReader.cs、ReadJEnc.cs）を取得し、
   Visual Studioプロジェクトに追加してください。

 (c)GitHub/Vectorで公開されているzip形式のファイルを入手して利用する場合

 ・GitHub [ https://github.com/hnx8/ReadJEnc/releases ]もしくは
   Vector [ http://www.vector.co.jp/soft/winnt/util/se506899.html ]より
   zipファイルをダウンロードしてください。
   （ソースコード、各ターゲットフレームワークのDLL、動作サンプルが含まれています）

 ・DLL版を使用する場合は、zipファイル内の「dll」フォルダより
   対象フレームワークに応じたサブフォルダの「Hnx8.ReadJEnc.dll」を取り出し、
   Visual Studioプロジェクトの参照設定に追加してください。

 ・C#ソースコード版を使用する場合は、zipファイル内「Source\Hnx8.ReadJEnc」
   フォルダから、CharCode.cs、FileType.cs、FileReader.cs、ReadJEnc.csの
   ４ファイルを取り出し、Visual Studioプロジェクトに追加してください。


◎テキストファイル読み出し・文字エンコード判別の基本的な手順ですが、
    1) FileReaderオブジェクトを生成
    2) Read()メソッドを呼び出し、ファイル文字エンコードを把握
    3) Textプロパティより、実際に読み出したテキストを取得
  という流れになります。

	--ソースコード例--------------------------------------------------------
	// 文字エンコードを判別したいファイルを、FileInfoオブジェクトなどで指定
	void Test(System.IO.FileInfo file) 
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
	------------------------------------------------------------------------

◎Byte配列の内容から文字エンコードを判別する場合は、
  ReadJEncクラスのインスタンスメソッド『GetEncoding』を呼び出します。
  （引数1：Byte配列、引数2：Byte長を指定します。
    戻り値で文字コード判別結果が、out引数3で取り出せたテキスト内容が返ります）

  HttpClientクラスでWebページの文字エンコードを推定する具体的なサンプルが
  「@IT」のInsider.NET技術情報にて詳細に解説されています。

  .NET TIPS：ReadJEncを使って文字エンコーディングを推定するには？［C#、VB］
  http://www.atmarkit.co.jp/ait/articles/1501/20/news073.html


【３】サンプルアプリケーションについて

zipファイルのSampleフォルダに入っている「Hnx8.ReadJEnc.WinForm.Sample.exe」は、
実際にReadJEncライブラリを使用したサンプルアプリケーションです。
（ロジック部100行ちょっとの小さなアプリケーションです。
　ソースは「Source\Hnx8.ReadJEncWinForm.Sample」にあります）

ファイルまたはフォルダのフルパスを「Path」に入力、「実行」ボタンを押すと
画面下段に文字コード判別結果が表示されます。

⇒ファイル指定時は、文字エンコード判別結果・読み出したテキストが表示されます。
  フォルダ指定時は、そのフォルダ配下の全ファイルの文字エンコード判別結果が
  タブ区切りで一覧表示されます。

⇒「最大サイズ」「判別対象言語」を切り替えることもできます。

⇒「結果をクリップボードにコピー」ボタンをクリックし、
   テキストエディタやExcelに結果を貼り付けることもできます。


【４】ライブラリの仕様について

 本ライブラリのソースコードは、以下の４ファイルで構成されています。

 (1) FileReader.cs : ファイル読み出し＆文字エンコード自動判別クラス

    ⇒ファイル読み込み、文字エンコード判別・テキスト取り出しの機能を提供します。
      （内部でReadJEncクラスなどの呼び出しを行うための入口となるクラスです）

 (2) ReadJEnc.cs   : 文字エンコード自動判別処理クラス本体

    ⇒バイト配列から文字エンコード判別を行う機能を提供します。
      （各国語のエンコード判定用内部クラス：SJIS/BIG5TW/GB18030/UHCKR/SBCS、
        およびJIS判定支援クラスを内包しています）
      実際に各国語の文字エンコードを判別するオブジェクトは、
      static(readonly)フィールドJP/ANSI/TW/CN/KR・CP1250～CP1258・TIS620として
      定義されています。

 (3) CharCode.cs   : 文字エンコード種類定義クラス（列挙相当）

    ⇒文字エンコードの名称/対応するEncoding、BOMプリアンブル情報を保持します。
      本ライブラリで判別する文字エンコード全種類がstatic(readonly)フィールドで
      定義されており、デコード機能などが利用できます。

 (4) FileType.cs   : 非テキストファイル種類定義クラス（列挙相当）

    ⇒FileReaderクラスで読み込んだファイルがテキストファイルではない場合、
      ファイルの先頭バイト(マジックナンバー)からファイル種類を識別するための
      定義情報を保持し、ファイル種類判別機能を提供します。
      （CharCodeクラスの機能を流用しています）

  ※ファイルからの読込機能を利用しない場合は、FileReader.cs/FileType.csは
    不要です。ReadJEnc.cs/CharCode.cs の２ファイルだけ使用してください。

  ※条件付コンパイルシンボル「JPONLY」を設定すると、
    日本語以外の文字エンコード定義／判定処理がコンパイル対象外となります。
    （コンパイルサイズがさらにコンパクトになります）

 ライブラリのAPI/列挙相当フィールド定義などについては、
 同梱の「Hnx8.ReadJEnc.xml」を参照ください。（ドキュメントコメントどおりです）

 仕様詳細ならびに文字エンコード判定の基準/考え方(アルゴリズム)については、
 ソースコードを直接参照願います。
 （ソースを読み解く助けとなるよう、コメントは多めにしてあります。
   実行速度向上などのため一部トリッキーな書き方をしている箇所もありますが、
   C#特有の構文などは極力使わないようにしており、
   読み解くのも、他言語への移植等も、比較的容易かと思っております）


【５】著作権・使用規定

ReadJEnc、および同梱サンプルアプリケーション(ReadJEncSample)について、
著作権は作者hnx8(H.Takahashi)が有しています。
MITライセンス[ https://github.com/hnx8/ReadJEnc/blob/master/LICENSE ]です。
ライセンスの範囲内であれば無償で自由にお使いいただけます。
（要点は、利用・改造・再配布時に著作権表示を残す、無保証、の２点です）

利用・再配布や改良については、何らかの形でフィードバック連絡をいただけると
助かります。

MITライセンスの範囲を超えた使用（たとえば著作権表記を消す、など）については
現状のところ想定しておりません。
そのような場合には事前に連絡・相談のほどお願いいたします。


本ライブラリは無償で利用できますが寄付も受け付けています。
2015年時点では、このライブラリを利用したGrepソフト「HNXgrep」の補足ページ
（ http://hp.vector.co.jp/authors/VA055804/HNXgrep/#ReadJEnc ）などから
送金できます。
またBLOGなどにてひっそりと「ほしい物リスト」を公開したりしなかったりしてるかも
しれません。


【６】制限事項

※ReadJEncの文字エンコード識別アルゴリズムは自作ですが、テキストへのデコードには
  Windows(.NET Framework)標準のエンコーディングをそのまま使用しています。
  MS互換ではないShiftJIS,EUC,JIS（csShiftJIS,IBM版CP932など）は、
  正しくマッピングされないことがあります。

※ShiftJIS,EUC,JISの方言を識別する機能は備わっておりません。

※不当な文字コードが混入していないかの最終的なチェックも、
  Windows(.NET Framework)標準のエンコーディングに一任しています。
  文字コードとしておかしいにもかかわらずWindowsの仕様としてデコードできるなら、
  妥当な文字コードであるとみなしてしまいます。

※日本語以外の文字エンコードについてはあまりテストが行えておらず、
  判別精度が低いかもしれません。


【７】サポート・連絡先

連絡、ご意見ご感想、不具合報告などについては、
・GitHubのIssue [ https://github.com/hnx8/ReadJEnc/issues ]またはPullRequest
・作者が開設しているBLOG [ http://d.hatena.ne.jp/hnx8/ ]
などへコメント等いただければと思います。


【８】更新履歴

■2018.04.21(Ver 1.3.1.1)
・ターゲットフレームワークにNetStandard1.3以降を追加
（この対応のため、ReadJEnc.csで使用していたDescriptionAttributeを抹消）
・Readme記述内容／ソースコード構成等を見直し最新化

■2018.04.17(Ver 1.3.1.0)
・ターゲットフレームワーク複数化(by Nordes Ménard-Lamarre さん)
  ※2.0のみ⇒2.0/3.5/4.0/4.5/4.6/4.6.1、およびNetCore2.0、NetStandard2.0
    サンプルプロジェクトは4.0環境で動作


□2017.09.15
・GitHub[ https://github.com/hnx8/ReadJEnc ]にも登録、
  登録に伴いこのReadmeの記述を一部手直し

□2017.08.23
・NuGetギャラリー[ https://www.nuget.org/packages/ReadJEnc/ ]にも登録、
  登録に伴いこのReadmeの記述を一部手直し

■2017.08.21(Ver 1.3.0.0821)
（自作grepツール「TresGrep」開発にあたり機能強化）
・CP1250～CP1258の各言語、および、TIS-620/CP874のタイ語に対応
  これらの言語のため解読クラスSBCSを新設し各クラスのフィールド等を調整
・EUC-KR/EUC-CNの定義を抹消、それぞれUHC/GB18030に一本化
  （別々の文字コードとして定義する意義が薄いため）
・バイナリファイル種類BZIP2/Z(compress)を追加
・JISH(補助漢字使用ありISO-2022-JP)のデコードクラスをCharCodeクラス内に移動
・文字コード定義/自動判別オブジェクトの定義順を微調整

■2015.03.09(Ver 1.2.2.0309)
・Readmeに「＠IT」解説記事へのリンクを追加、その他Readmeの一部手直し
・補助漢字ありJISのCharCode定義を独立させる。また判定ロジックを微調整
・メソッド引数名の大文字始まり(バイナリサイズ削減効果を狙ったもの)を一部取止め

■2014/12/06(Ver 1.2.1.1206)
・JIS(ISO-2022-JP)デコード時にJISX0212補助漢字がまったくデコードできていなかった
  不具合を修正、JIS判定ロジック関連の改善

■2014/08/18(Ver 1.2.0.0818)
・EUC補助漢字がまったくデコードできていなかった不具合を修正
  (0x8FのEUC補助漢字について、CP20932コードテーブルで対応可能な範囲で対応)
・JISX0201のエスケープシーケンスなし7bit半角カナファイルに対応
・ISO-2022-KRの判別に対応
・バイト配列の文字コード自動判別メソッドをprivate⇒public化
・「優先EUC」の機能を「デフォルト文字コード」機能に全面変更
・初回起動時(おそらくコードテーブル読み出しに)時間がかかる問題への対策を実施
・これらに伴う全面的なソースコード構造改善

■2014/07/13(Ver 1.1.2014.0713)
・他プロセスがロックしているファイルが読み出しできない不具合を修正
・ファイル文字コード種類定義にプリアンブル(BOM/マジックナンバー)情報を
  持たせるよう構造変更、プリアンブルのチェック仕様を改善。
・UTF16N判定処理をBOM判定処理内に移動
・判別ファイル種類にWindowsショートカット、Windowsアイコン・TIFF、7z・CABを追加
・その他全般的なソースコード構造改善、
  日本語以外の文字コード定義・文字コード判定処理を条件付コンパイル化

■2014/06/07(Ver 1.0.2014.0607)
・初公開
　(自作grepツール「HNXgrep」Ver1.4で採用しているものとほぼ同一内容)

(以下、HNXgrep開発時の更新履歴)

■2013年末～2014年前半頃
・HNXgrep過去バージョンのソース・開発知見をもとに、
　文字コード自動判別ライブラリを抜本的に作り直し

□2012年後半～2013年前半頃
・自作grepツール向けの文字コード自動判定処理につき中規模改善
　(HNXgrep Ver1.2/Ver1.3にて採用、ソースコードは未公開)

□2012年1月～2月頃
・自作grepツール向けの文字コード自動判定処理として、DOBON.NET様の
　「文字コードを判別する(http://dobon.net/vb/dotnet/string/detectcode.html)」の
　ソースを流用・改善し、以下のURLにて公開
　http://d.hatena.ne.jp/hnx8/20120225/1330157903
　(HNXgrep Ver1.0にて採用)


【９】参考にしたサイト等（メモ、敬称略）

(1)文字コードに関する情報全般
・書籍「文字コード「超」研究」 深沢 千尋/著、ISBN:4899770510
・とほほのWWW入門/漢字コード
   http://www.tohoho-web.com/ex/draft/kanji.htm
・noocyte のプログラミング研究室/文字コードに関する覚え書きと実験
   http://www5d.biglobe.ne.jp/~noocyte/Programming/CharCode.html
・しいしせねっと/文字コードの墓場
   http://siisise.net/charset.html
・euc.JP/文字コードの話
   http://euc.jp/i18n/charcode.ja.html

(2)文字コード体系
・RFC http://tools.ietf.org/html
・通信用語の基礎知識 http://www.wdic.org/
・Wikipedia http://ja.wikipedia.org/wiki
・文字コード表 http://charset.uic.jp/
・ASHホームページ/文字コード体系 http://ash.jp/code/index.htm
・CyberLibrarian/文字コード http://www.asahi-net.or.jp/~ax2s-kmtn/character/

(3)JIS/EUCに関する詳細情報
・森山 将之のホームページ/JIS X 0201 片仮名
   http://www2d.biglobe.ne.jp/~msyk/charcode/jisx0201kana/index.html
・めらんこーど地階/Encode::JP::JIS7 のJIS X 0201片仮名対応
   http://d.hatena.ne.jp/anon_193/20090108/1231428802

・Legacy Encoding Project/[LE-talk-ja 3] ISO-2022-JP-MS について(CP5022xに関する特異なコード範囲)
   http://sourceforge.jp/projects/legacy-encoding/lists/archive/talk-ja/2006-March/000002.html

・汁ムゴ魚/EUC対応について(#1-#7)
   http://wantech.ikuto.com/2006/0219/0037/euc%E5%AF%BE%E5%BF%9C%E3%81%AB%E3%81%A4%E3%81%84%E3%81%A6.htm 等
・コードページ932(森山 将之)/eucJP-msとCP51932の違い
   http://msyk.at.webry.info/200511/article_2.html

(4).NET FrameworkのEncoding関連ソースコード
・Encoding.cs
   http://referencesource.microsoft.com/#mscorlib/system/text/encoding.cs
・ISO2022Encoding.cs（大変素敵な実装になっています・・・・・コメントを読んでのけぞってください）
   http://referencesource.microsoft.com/#mscorlib/system/text/iso2022encoding.cs
・EUCJPEncoding.cs（これもまた大変素敵な実装になっています・・・・・コメントを読んでのけぞってください）
   http://referencesource.microsoft.com/#mscorlib/system/text/eucjpencoding.cs

(5)文字コード判別ソースコード
・DOBON.NET 文字コードを判別する
   http://dobon.net/vb/dotnet/string/detectcode.html
・雅階凡の C# プログラミング C#2008 文字コードの判定【リンク切れ】
   http://www.geocities.jp/gakaibon/tips/csharp2008/charset-check.html

 ほか、沢山。

