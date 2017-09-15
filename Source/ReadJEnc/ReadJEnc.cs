using System;
using System.ComponentModel;

namespace Hnx8.ReadJEnc
{
    /// <summary>
    /// ReadJEnc 文字コード自動判別ライブラリ・処理本体(Rev.20170821)
    /// </summary>
    public class ReadJEnc
    {   ////////////////////////////////////////////////////////////////////////
        // <ReadJEnc.cs> ReadJEnc 文字コード自動判別処理本体(Rev.20170821)
        //  Copyright (C) 2014-2017 hnx8(H.Takahashi)
        //  http://hp.vector.co.jp/authors/VA055804/
        //
        //  Released under the MIT license
        //  http://opensource.org/licenses/mit-license.php
        ////////////////////////////////////////////////////////////////////////

        //言語別文字コード自動判別オブジェクト==================================

        /// <summary>日本語文字コード判別(SJIS/EUCJP)</summary>
        [Description("日本語")]
        public static readonly ReadJEnc JP = new SJIS();

        /// <summary>ANSI文字コード判別(CP1252)</summary>
        [Description("欧文（西欧）")]
        public static readonly ReadJEnc ANSI = new ReadJEnc(CharCode.ANSI, null);

#if (!JPONLY) //漢字圏文字コード判別対象言語（日本語判別以外使用しないなら定義省略可）
        /// <summary>繁体字中国語文字コード判別(BIG5/EUCTW)</summary>
        [Description("繁体字中国語")]
        public static readonly ReadJEnc TW = new BIG5TW();

        /// <summary>簡体字中国語文字コード判別(GB18030)</summary>
        [Description("簡体字中国語")]
        public static readonly ReadJEnc CN = new GB18030();

        /// <summary>ハングル文字コード判別(UHCKR)</summary>
        [Description("ハングル")]
        public static readonly ReadJEnc KR = new UHCKR();
#endif

#if (!JPONLY) //その他の１バイト系文字コード判別対象言語（日本語判別以外使用しないなら定義省略可）

        //アルファベットと混在させずに使う文字種があればSBCSクラスで、なければReadJEncクラスで、判別解析を行う。
        // ※チェック用定義値は未定義文字コードのポイント通り。具体的には以下のようになる
        //               FEDC BA98 7654 3210         FEDC BA98 7654 3210
        //               ---- ---- ---- ----         ---- ---- ---- ----
        // CP1250 (0x9#) 0000 0001 0000 0001  (0x8#) 0000 0001 0000 1010 
        // CP1251 (0x9#) 0000 0001 0000 0000  (0x8#) 0000 0000 0000 0000 ※0xC0以降を特別扱い
        // CP1253 (0x9#) 1111 0101 0000 0001  (0x8#) 1111 0101 0000 0010 ※0xC1以降を特別扱い、0xAA/D2/FFは無効
        // CP1254 (0x9#) 0110 0000 0000 0001  (0x8#) 1110 0000 0000 0010
        // CP1255 (0x9#) 1111 0100 0000 0001  (0x8#) 1111 0100 0000 0010 ※0xC0以降を特別扱い、0xCA/D9-DF/FB-FC/FFは無効
        // CP1256 (0x9#) 0000 0000 0000 0000  (0x8#) 0000 0000 0000 0000 ※0xC0以降を特別扱い、無効文字なし
        // CP1257 (0x9#) 1001 0101 0000 0001  (0x8#) 0001 0101 0000 1010 （0xA1/A5は妥協する。最終的にデコードエラーとなるはず）
        // CP1258 (0x9#) 0110 0100 0000 0001  (0x8#) 1110 0100 0000 0010
        // TIS620 (0x9#) 1111 1111 0000 0001  (0x8#) 1111 1111 1101 1110 ※OxA1以降を特別扱い、0xDB-DE/FC-FFは無効

        /// <summary>Windows1250(iso-8859-2) 中央ヨーロッパ言語(チェコ語等)判別 </summary>
        [Description("中欧東欧言語")]
        public static readonly ReadJEnc CP1250 = new ReadJEnc(CharCode.CP1250, 0x0101010A);

        /// <summary>Windows1251 キリル言語(ロシア語等)＆ANSI判別 </summary>
        [Description("キリル言語")]
        public static readonly ReadJEnc CP1251 = new SBCS(CharCode.CP1251, 0xC0, 0x01000000);

        /// <summary>Windows1253(iso-8859-7) ギリシャ語＆ANSI判別 </summary>
        [Description("ギリシャ語")]
        public static readonly ReadJEnc CP1253 = new SBCS(CharCode.CP1253, 0xC1, 0xF501F502, 0x00000400, 0x00040000, 0x80000000);

        /// <summary>Windows1254(iso-8859-9) トルコ語判別 </summary>
        [Description("トルコ語")]
        public static readonly ReadJEnc CP1254 = new ReadJEnc(CharCode.CP1254, 0x6001E002);

        /// <summary>Windows1255(iso-8859-8) ヘブライ語＆ANSI判別 </summary>
        [Description("ヘブライ語")]
        public static readonly ReadJEnc CP1255 = new SBCS(CharCode.CP1255, 0xC0, 0xF401F402, 0x00000000, 0xFE000400, 0X98000000);

        /// <summary>Windows1256 アラビア語＆ANSI判別 </summary>
        [Description("アラビア語")]
        public static readonly ReadJEnc CP1256 = new SBCS(CharCode.CP1256, 0xC0);

        /// <summary>Windows1257(iso-8859-13) バルト言語判別 </summary>
        [Description("バルト言語")]
        public static readonly ReadJEnc CP1257 = new ReadJEnc(CharCode.CP1257, 0x9501150A);

        /// <summary>Windows1258 ベトナム語判別 </summary>
        [Description("ベトナム語")]
        public static readonly ReadJEnc CP1258 = new ReadJEnc(CharCode.CP1258, 0x6401E402);

        /// <summary>TIS620/Windows874(iso-8859-11) タイ語＆ANSI判別 </summary>
        [Description("タイ語")]
        public static readonly ReadJEnc TIS620 = new SBCS(CharCode.TIS620, 0xA1, 0xFF01FFDE, 0x00000000, 0x78000000, 0xF0000000);
#endif

        #region 基本クラス定義--------------------------------------------------
        /// <summary>バイナリと判定するDEL文字コード、兼、ASCII/非ASCIIの境界文字コード</summary>
        const byte DEL = (byte)0x7F;
        /// <summary>非テキストファイルと判定する制御文字コードの最大値</summary>
        const byte BINARY = (byte)0x03; //0x01-0x07位の範囲で調整。0x08(BS)はTeraTerm等ログで出る。0x09(TAB)は普通にテキストで使う。0x03くらいにするのがよい。HNXgrep/TresGrepでは0x03を採用

        /// <summary>このインスタンスでおもに判別対象とするデフォルト文字コード</summary>
        public readonly CharCode CharCode;
        /// <summary>EUCデコードで用いる文字コード（無指定null時はEUCの判別を行わない）</summary>
        protected readonly CharCode EUC = null;
        /// <summary>CP1252系で判別対象とする文字コード（デフォルトはANSI=CP1252）</summary>
        protected readonly CharCode CP125X = CharCode.ANSI;
        /// <summary>CP1252系の判別にあたり、0x80-0x9Fのうち文字コード未定義の箇所をbitで表現した値</summary>
        /// <remarks>
        /// デフォルトの非CP1252チェック用定義値(0x2001A002)＝未定義の81,8D,8F,90,9Dに対応するビットがON
        ///        FEDC BA98 7654 3210         FEDC BA98 7654 3210
        ///        ---- ---- ---- ----         ---- ---- ---- ----
        /// (0x9#) 0010 0000 0000 0001  (0x8#) 1010 0000 0000 0010
        /// </remarks>
        protected readonly uint NODEF = 0x2001A002;

        /// <summary>引数指定文字コード/CP1252/EUCを判別対象とする際のコンストラクタ</summary>
        /// <param name="CharCode">このインスタンスで判別対象とするデフォルト文字コード</param>
        /// <param name="EUC">EUCの文字コード</param>
        protected ReadJEnc(CharCode CharCode, CharCode EUC)
        {
            this.CharCode = CharCode;
            this.EUC = EUC;
        }
        /// <summary>CP1252系文字コードのみを判別対象とする際のコンストラクタ</summary>
        /// <param name="CP125X">CP1252系の判別対象デフォルト文字コード(CP1252以外を指定)</param>
        /// <param name="NODEF">0x80-0x9Fのうち文字コード未定義の箇所をbitで表現した値</param>
        protected ReadJEnc(CharCode CP125X, uint NODEF)
        {
            this.CharCode = CP125X;
            this.CP125X = CP125X;
            this.NODEF = NODEF;
        }
        /// <summary>オブジェクト文字列表現として、自動判別デフォルト文字コードの名前を返す</summary>
        public override string ToString()
        {
            return this.CharCode.Name;
        }
        #endregion

        //文字コード判別メソッド================================================

        /// <summary>バイト配列を全走査し、文字コードを自動判別する</summary>
        /// <param name="bytes">判定対象のバイト配列</param>
        /// <param name="len">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
        /// <param name="text">out 判別した文字コードにより取り出したテキスト文字列（非テキストならnull）</param>
        /// <returns>文字コード判別結果（非テキストならnull）</returns>
        public CharCode GetEncoding(byte[] bytes, int len, out string text)
        {
            if(len==0)
            {   //■空ファイルにつき非テキストと判断
                text = null;
                return null;
            }

            byte b1 = bytes[0]; //汎用バイトデータ読み取り変数初期化

            //【1】7bit文字コードの範囲の走査(ASCII判定/非ASCII文字開始位置把握)、およびUTF16N/JISチェック
            JIS escapeSequenceChecker = null; //JISエスケープシーケンス評価
            int asciiEndPos = 0; //ループ変数、兼、非ASCII文字を初めて検出した位置
            while (b1 < DEL) //非ASCII文字が出現したらループ脱出：b1にはあらかじめ読み込み済
            {
                if (b1 <= BINARY)
                {   //バイナリ文字検出：先頭２バイトでの検出ならUTF16Nの可能性をチェック、否ならバイナリ確定
                    CharCode ret = (asciiEndPos < 2 ? SeemsUTF16N(bytes, len) : null);
                    if (ret != null && (text = ret.GetString(bytes, len)) != null)
                    {   //UTF16Nデコード成功：非テキスト文字混入チェック
                        int i;
                        for (i = -3; i <= BINARY; i++)
                        {   //0xFFFD,0xFFFE,0xFFFF,0～BINARY、DELが混入している場合は非テキストとみなす
                            if (text.IndexOf((char)i, 0, text.Length) != -1) { break; }
                        }
                        if (i > BINARY && text.IndexOf((char)DEL, 0, text.Length) == -1)
                        {   //■UTF16N確定（非テキスト文字混入なし）
                            return ret;
                        }
                    }
                    text = null;
                    return null; //■バイナリ確定
                }
                if (b1 == 0x1B)
                {   //エスケープシーケンス判定(エスケープコード内容を読み飛ばす)
                    if (escapeSequenceChecker == null) { escapeSequenceChecker = new JIS(bytes, len, asciiEndPos); }
                    asciiEndPos += escapeSequenceChecker.GetEncoding(asciiEndPos);
                }
                //次の文字へ
                if ((++asciiEndPos) >= len)
                {   //全文字チェック完了：非ASCII文字未検出、JISもしくはASCII
                    if (escapeSequenceChecker != null)
                    {   //エスケープシーケンスに基づく文字コードが取得できるか確認
                        CharCode ret = escapeSequenceChecker.GetEncoding(out text);
                        if (ret != null)
                        {   //■エスケープシーケンスに基づく文字コードで確定
                            return ret;
                        }
                    }
                    else if (JIS.hasSOSI(bytes, len))
                    {   //SO,SIによるエスケープを検出した場合は、半角カナJISの可能性を判定
                        if (escapeSequenceChecker == null && (text = CharCode.JIS50222.GetString(bytes, len)) != null)
                        {   //■エスケープシーケンスなし、半角カナSOSIのみを使用したJISで確定
                            return CharCode.JIS50222;
                        }
                    }
                    //■ASCII確定（ただしデコード失敗時はバイナリ）
                    return ((text = CharCode.ASCII.GetString(bytes, len)) != null) ? CharCode.ASCII : null;
                }
                b1 = bytes[asciiEndPos];
            }

            //【2】非ASCII文字を含む範囲の走査、CP1252系/UTF8/EUCチェック、JIS残チェック
            byte b2;
            int cp1252Score = 0; //いずれも、可能性が否定されたらint.MinValueが設定される
            int utfScore = 0;
            int eucScore = (this.EUC == null ? int.MinValue : 0); //EUC検出対象なしなら最初からチェック対象外
            int sjisScore = 0;
            bool existsEUC0x8F = false; //EUC補助漢字を見つけたらtrueを設定
            uint NODEF = this.NODEF; //パフォーマンス改善のためローカル変数におろす

            for (int cp1252Pos = asciiEndPos; cp1252Pos < len; ) //cp1252Posの加算はロジック途中で随時実施
            {
                if (b1 == DEL)
                {   //制御文字0x7F登場なら、ごくわずかなJISの可能性以外全消滅。JISの可能性を消しきれるか判定
                    cp1252Score = int.MinValue;
                    utfScore = int.MinValue;
                    eucScore = int.MinValue;
                    sjisScore = int.MinValue;
                    if (escapeSequenceChecker == null || (cp1252Pos++) >= len || (b1 = bytes[cp1252Pos]) < 0x21 || b1 >= DEL)
                    {   //JISエスケープ未出現 or ファイル末尾で2バイト目なし or 2バイト目が0x21-0x7E範囲外ならJISの可能性も否定
                        text = null;
                        return null; //■バイナリ確定
                    }
                }
                //CP1252系チェック＆0x80以上の文字範囲の把握(notAsciiStartPos～cp1252Pos)。b1読込済
                int notAsciiStart = cp1252Pos;
                switch (cp1252Score)
                {
                    case int.MinValue: //CP1252系の可能性否定済み、非ASCII文字のスキップのみ実施
                        while (b1 > DEL && (++cp1252Pos) < len) { b1 = bytes[cp1252Pos]; }
                        break;
                    default: //CP1252系可能性あり、定義外文字混入チェック＆ポイント加算
                        while (b1 > DEL)
                        {   // そのバイト値が未定義コードかどうかチェック（bit表現値とかみあうようビットシフトして照合）
                            if (b1 <= 0x9F && (NODEF & (1u << (b1 % 32))) != 0)
                            {   // ビットが立ってる＝未定義コード、可能性消滅
                                cp1252Score = int.MinValue;
                                goto case int.MinValue; //非ASCII文字スキップへ
                            }
                            if ((++cp1252Pos) >= len) { break; }
                            b1 = bytes[cp1252Pos];
                        }
                        //非ASCII文字範囲終了、評価ポイント加算
                        //１バイトのみ出現時（SJISよりもCP1252系の可能性が高い）、SJIS漢字1文字目と同評価・SJISカナよりも高評価となるようポイント加算
                        if (cp1252Pos == notAsciiStart + 1) { cp1252Score += 2; }
                        else if (cp1252Pos == notAsciiStart + 2 && (b2 = bytes[cp1252Pos - 1]) >= 0xC0)
                        {   //２バイトのみ出現時、ダイアクリティカルマーク（発音記号等）つきアルファベットなら配点補正
                            if (b2 == (b2 = bytes[cp1252Pos - 2])) { cp1252Score += 5; } //同一文字重ねはかなり特徴的(SJISカナより可能性高)
                            else if (b2 >= 0xC0)
                            {   //続きor直前のASCII文字がアルファベットっぽければ、SJISカナより可能性が高くなるよう補正                               
                                if (b1 > 0x40 || (notAsciiStart > 0 && bytes[notAsciiStart - 1] > 0x40)) { cp1252Score += 5; }
                                else { cp1252Score += 3; } //どちらでもなければ、EUCよりは可能性高とする
                            }
                            else { cp1252Score++; } //否ならば低めの加算とする
                        }
                        else { cp1252Score++; } //いずれにも該当しなければやや低めの加算とする
                        break;
                }
                //notAsciiStartPos～cp1252Pos範囲のUTF8チェック
                if (utfScore >= 0)
                {
                    bool prevIsKanji = false;
                    for (int utfPos = notAsciiStart; utfPos < cp1252Pos; utfPos++)
                    {
                        b1 = bytes[utfPos]; //※1バイト目は厳密にチェック、2バイト目以降は（デコード時にチェックアウトできる前提で）冗長なエンコードやサロゲート等を許容している
                        //1バイト目・２バイト目(ともに0x80以上であることは確認済み)をチェック
                        if (b1 < 0xC2 || (++utfPos) >= cp1252Pos || bytes[utfPos] > 0xBF) { utfScore = int.MinValue; break; } //UTF8可能性消滅
                        else if (b1 < 0xE0)
                        {   //２バイト文字OK（半角文字とみなして評価）
                            if (prevIsKanji == false) { utfScore += 6; } else { utfScore += 2; prevIsKanji = false; }
                        }
                        //3バイト目(0x80以上であることは確認済み)をチェック
                        else if ((++utfPos) >= cp1252Pos || bytes[utfPos] > 0xBF) { utfScore = int.MinValue; break; } //UTF8可能性消滅
                        else if (b1 < 0xF0)
                        {   //３バイト文字OK（全角文字とみなして評価）
                            if (prevIsKanji == true) { utfScore += 8; } else { utfScore += 4; prevIsKanji = true; }
                        }
                        //4バイト目(0x80以上であることは確認済み)をチェック
                        else if ((++utfPos) >= cp1252Pos || bytes[utfPos] > 0xBF) { utfScore = int.MinValue; break; } //UTF8可能性消滅
                        else if (b1 < 0xF5)
                        {   //４バイト文字OK（全角文字とみなして評価）
                            if (prevIsKanji == true) { utfScore += 12; } else { utfScore += 6; prevIsKanji = true; }
                        }
                        else { utfScore = int.MinValue; break; } //UTF8可能性消滅(0xF5以降はUTF8未定義)
                    }
                }
                //notAsciiStartPos～cp1252Pos範囲のEUCチェック ※EUCの文字コード範囲はEUC-JP/TW/CN/KRでほぼ共通
                if (eucScore >= 0)
                {   //前の文字との連続性チェック用定数定義
                    const int PREV_KANA = 1; //直前文字は半角カナ
                    const int PREV_ZENKAKU = 2; //直前文字は全角
                    int prevChar = 0; //前の文字はKANAでもZENKAKUでもない
                    for (int eucPos = notAsciiStart; eucPos < cp1252Pos; eucPos++)
                    {   //１バイト目(0xA1-0xFE,0x8E,0x8F)・２バイト目(１バイト目に応じ範囲が異なる)のチェック
                        b1 = bytes[eucPos];
                        if (b1 == 0xFF || (++eucPos) >= cp1252Pos) { eucScore = int.MinValue; break; } //EUC可能性消滅
                        b2 = bytes[eucPos];
                        if (b1 >= 0xA1)
                        {   //１バイト目＝全角文字指定、２バイト全角文字チェック
                            if (b2 < 0xA1 || b2 == 0xFF) { eucScore = int.MinValue; break; } //EUC可能性消滅
                            //２バイト文字OK（全角）
                            if (prevChar == PREV_ZENKAKU) { eucScore += 5; } else { eucScore += 2; prevChar = PREV_ZENKAKU; }
                        }
                        else if (b1 == 0x8E)
                        {   //１バイト目＝EUC-JPのかな文字(orEUC-TWの４バイト文字)指定。２バイトの半角カナ文字チェック
                            if (b2 < 0xA1 || b2 > 0xDF) { eucScore = int.MinValue; break; } //EUC可能性消滅
                            //検出OK,EUC文字数を加算（半角文字）
                            if (prevChar == PREV_KANA) { eucScore += 6; }
#if (!JPONLY)
                            //漢字圏テキスト文字コードのうちEUC-TWに限り全角文字相当の扱いとする(0x8E,0xA2-0xB0,0xA1-0xFE,0xA1-0xFEの４バイト文字の判定に流用)
                            else if (this.EUC == CharCode.EUCTW) { if (prevChar == PREV_ZENKAKU) { eucScore += 6; } else { eucScore += 2; prevChar = PREV_ZENKAKU; } }
#endif
                            else { eucScore += 2; prevChar = PREV_KANA; }
                        }
                        else if (b1 == 0x8F
                            && b2 >= 0xA1 && b2 < 0xFF
                            && (++eucPos) < cp1252Pos
                            && (b2 = bytes[eucPos]) >= 0xA1 && b2 < 0xFF)
                        {   //残る可能性は３バイト文字：検出OKならEUC文字数を加算（全角文字、補助漢字）
                            if (prevChar == PREV_ZENKAKU) { eucScore += 8; } else { eucScore += 3; prevChar = PREV_ZENKAKU; }
                            existsEUC0x8F = true; //※補助漢字有
                        }
                        else { eucScore = int.MinValue; break; } //EUC可能性消滅
                    }
                }

                //ASCII文字範囲の読み飛ばし＆バイナリチェック＆JISチェック、b1に非ASCII文字出現位置のバイト値を格納
                while (cp1252Pos < len && (b1 = bytes[cp1252Pos]) < DEL)
                {
                    if (b1 <= BINARY)
                    {   //■バイナリ確定
                        text = null;
                        return null;
                    }
                    if (b1 == 0x1B)
                    {   //エスケープシーケンス判定(エスケープコード内容を読み飛ばす)
                        if (escapeSequenceChecker == null) { escapeSequenceChecker = new JIS(bytes, len, cp1252Pos); }
                        cp1252Pos += escapeSequenceChecker.GetEncoding(cp1252Pos);
                    }
                    cp1252Pos++;
                }
            }

            //【3】SJISなどの各国語文字コードチェック（非ASCII登場位置からチェック開始:ただしDEL検出時などは可能性なし）
            if (sjisScore != int.MinValue)
            {
                sjisScore = GetEncoding(bytes, asciiEndPos, len);
            }

            //【4】ポイントに応じ文字コードを決定（実際にそのエンコーディングで読み出し成功すればOKとみなす）
            if (escapeSequenceChecker != null)
            {   //JIS系可能性高：エスケープシーケンスに基づく文字コードが取得できるか確認
                CharCode ret = escapeSequenceChecker.GetEncoding(out text);
                if (ret != null) { return ret; } //■エスケープシーケンスに基づく文字コードで確定
            }
            if (eucScore > 0 && eucScore > sjisScore && eucScore > utfScore)
            {   //EUC可能性高
                if (cp1252Score > eucScore)
                {   //ただし可能性が高ければCP1252系を先にチェック
                    if ((text = this.CP125X.GetString(bytes, len)) != null) { return this.CP125X; } //■CP1252系で読みこみ成功
                }
                if (existsEUC0x8F && (text = CharCode.EUCH.GetString(bytes, len)) != null) { return CharCode.EUCH; }//■EUC補助漢字読みこみ成功
                if ((text = this.EUC.GetString(bytes, len)) != null) { return this.EUC; } //■EUCで読みこみ成功
            }
            if (utfScore > 0 && utfScore >= sjisScore)
            {   //UTF可能性高
                if ((text = CharCode.UTF8N.GetString(bytes, len)) != null) { return CharCode.UTF8N; } //■UTF-8Nで読みこみ成功
            }
            if (sjisScore >= 0)
            {   //SJISなどの各国語指定に合致したなら、そのコードでの読み出しを試みる(ただし可能性が高ければCP1252系を先にチェック)
                if (cp1252Score > sjisScore && (text = this.CP125X.GetString(bytes, len)) != null) { return this.CP125X; } //■CP1252系で読みこみ成功
                if ((text = this.CharCode.GetString(bytes, len)) != null) { return this.CharCode; } //■各国語文字コードで読みこみ成功
            }
            if (cp1252Score > 0)
            {   //CP1252系の可能性のみ残っているのでチェック
                if ((text = this.CP125X.GetString(bytes, len)) != null) { return this.CP125X; } //■CP1252系で読みこみ成功
            }
            //■いずれにも該当しなかった場合は、バイナリファイル扱いとする
            text = null;
            return null;
        }

        /// <summary>デフォルト文字コードの判定スコア算出（判定開始位置～ファイル末尾までの範囲を対象）</summary>
        /// <param name="bytes">判定対象のバイト配列</param>
        /// <param name="pos">判定開始位置(非ASCII文字コードが初めて登場した位置)</param>
        /// <param name="len">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
        /// <returns>判定スコア算出結果</returns>
        protected virtual int GetEncoding(byte[] bytes, int pos, int len)
        {   //デフォルト(ANSI)の場合は常に評価なしとする。オーバーライドで算出ロジックを書き換えること
            return int.MinValue;
        }

        /// <summary>BOMなしUTF16の可能性があるか(先頭文字がASCIIか否かをもとに)判定</summary>
        /// <param name="bytes">判定対象のバイト配列</param>
        /// <param name="len">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
        /// <returns>UTF16Nと思われる場合はその文字コード、否ならnull</returns>
        public static CharCode SeemsUTF16N(byte[] bytes, int len)
        {
            if (len >= 2 && len % 2 == 0)
            {
                if (bytes[0] == 0x00)
                {
                    if (bytes[1] > BINARY && bytes[1] < DEL && (len == 2 || bytes[2] == 0))
                    {   //▲UTF16BigEndianの可能性あり
                        return CharCode.UTF16BE;
                    }
                }
                else if (bytes[1] == 0x00)
                {
                    if (bytes[0] > BINARY && bytes[0] < DEL && (len == 2 || bytes[3] == 0))
                    {   //▲UTF16LittleEndianの可能性あり
                        return CharCode.UTF16LE;
                    }
                }
            }
            return null; //UTF16Nの可能性はないと判断
        }

        #region JIS/SJIS判定処理各種--------------------------------------------
        /// <summary>
        /// JIS判定関連処理クラス
        /// </summary>
        private class JIS
        {
            //※MS版JISでは、ISO/IEC 2022に準じていない0x7F以上のコード(外字や8bitカナなど)が混入してもOKである。
            //  このため、非ASCII文字登場後もエスケープシーケンスのチェックを行う。
            //  CP5022x(CP50220/CP50221/CP50222)の特異なコード体系については、
            //  http://sourceforge.jp/projects/legacy-encoding/lists/archive/talk-ja/2006-March/000002.html も参照

            // また、エスケープシーケンスを伴わない(文字コードSO/SIのみによる)半角カナ切り替えが含まれている場合も、
            // JISとみなしてデコードするほうが妥当であるため、ASCII文字のみ出現の場合にもSOSI有無チェックを行う。

            /// <summary>SO/SI文字出現チェック</summary>
            /// <param name="bytes">判定対象のバイト配列</param>
            /// <param name="len">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
            /// <returns>出現していればtrue、否ならfalse</returns>
            internal static bool hasSOSI(byte[] bytes, int len)
            {
                return (Array.IndexOf<byte>(bytes, 0x0E, 0, len) >= 0 && Array.IndexOf<byte>(bytes, 0x0F, 0, len) >= 0);
            }

            #region JIS判定用のインスタンスメンバ／メソッド--------------------
            byte[] bytes;
            int len;
            /// <summary>JIS補助漢字エスケープシーケンス有無(有ならtrue)</summary>
            bool JISH = false;
            /// <summary>ISO-2022-KRエスケープシーケンス有無(有ならtrue)</summary>
            bool ISOKR = false;
            /// <summary>JIS評価値(JISとしてデコードすべきなら正値、否ならマイナス値)</summary>
            int c = 0;

            /// <summary>JISエスケープシーケンス判定オブジェクト初期化（およびISO-2022-KR判定）</summary>
            /// <param name="bytes">判定対象のバイト配列</param>
            /// <param name="len">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
            /// <param name="pos">エスケープ(0x1B)出現位置</param>
            internal JIS(byte[] bytes, int len, int pos)
            {
                this.bytes = bytes;
                this.len = len;
                //ISO-2022-KR判定特殊ロジック。初出のエスケープシーケンスのみをもとに判定
                this.ISOKR = (pos >= 0 && pos < len - 4
                            && bytes[pos + 1] == '$'
                            && bytes[pos + 2] == ')'
                            && bytes[pos + 3] == 'C');
            }

            /// <summary>JISエスケープシーケンス妥当性チェック</summary>
            /// <param name="pos">エスケープ(0x1B)出現位置</param>
            /// <returns>エスケープシーケンスとして妥当ならpos加算値、そうでなければゼロ</returns>
            internal int GetEncoding(int pos)
            {
                if (pos + 2 < len)
                {   //ESC(0x1B):有効なエスケープシーケンスかチェック
                    c++; //加算前提
                    switch (bytes[pos + 1])
                    {   //２バイト目で分岐
                        case 0x24: //ESC$
                            switch (bytes[pos + 2])
                            {   //３バイト目で分岐
                                case 0x40: //<ESC>$@ : JISエスケープ(78JIS)     - ISO-2022-JP
                                case 0x42: //<ESC>$B : JISエスケープ(83JIS)     - ISO-2022-JP
                                    return 2;
                                case 0x28: //<ESC>$(D: JISエスケープ(90補助漢字)- ISO-2022-JP-1 
                                    //ただしCP5022Xではデコードできないため別途CP20932を用いてデコードすること
                                    if (pos + 3 < len && bytes[pos + 3] == 0x44)
                                    {
                                        JISH = true;
                                        return 3;
                                    }
                                    break;
                            }
                            break;
                        case 0x28: //ESC(
                            switch (bytes[pos + 2])
                            {   //３バイト目で分岐
                                case 0x42: //<ESC>(B : JISエスケープ(ASCII)    - ISO-2022-JP
                                case 0x48: //<ESC>(H : JISエスケープ(swedish)  - (ISO-2022-JP規定外)
                                case 0x49: //<ESC>(I : JISエスケープ(カタカナ) - (ISO-2022-JP規定外 http://tools.ietf.org/html/rfc1468 だがCP5022Xでデコード可能)
                                case 0x4A: //<ESC>(J : JISエスケープ(ローマ字) - ISO-2022-JP
                                    return 2;
                            }
                            break;
                        // 以下、MS版JIS(CP50220/CP50221/CP50222)でデコードできないエスケープシーケンスは、チェックしない。
                        //    <ESC>&@<ESC>$B  : JISエスケープ(90JIS)   - (ISO-2022-JP規定外。90JISで追加された「凜・熙」の２文字は、CP5022xでは更新シーケンス「<ESC>&@」を付けなくてもデコード可能)
                        //    <ESC>$(O : JISエスケープ(2000JIS#1)      - ISO-2022-JP-3
                        //    <ESC>$(P : JISエスケープ(2000JIS#2)      - ISO-2022-JP-3
                        //    <ESC>$(Q : JISエスケープ(2004JIS#1)      - ISO-2022-JP-4
                        // ほか、多言語対応のエスケープシーケンスもチェック外とする。
                        // エスケープシーケンスの一覧／うちMS版JISで有効なものは、
                        // http://ja.wikipedia.org/wiki/ISO/IEC_2022#.E5.BF.9C.E7.94.A8.E4.BE.8B
                        // http://referencesource.microsoft.com/#mscorlib/system/text/iso2022encoding.cs
                        // http://www.wdic.org/w/WDIC/Microsoft%20Windows%20Codepage%20%3A%2050221 なども参照
                    }
                }
                c -= 4; //非JIS：ペナルティ的に評価値を減算する
                return 0;
            }

            /// <summary>JISと判定された場合の文字コードを確定する</summary>
            /// <param name="text">out 判別した文字コードにより取り出したテキスト文字列（非テキストならnull）</param>
            /// <returns>カナSO/SI有無やデコード不能箇所有無に応じたJIS文字コード種類</returns>
            internal CharCode GetEncoding(out string text)
            {
                byte[] bytes = this.bytes;
                int len = this.len;
                if (ISOKR && hasSOSI(bytes, len))
                {   //■KSエスケープシーケンスあり、ISO-2022-KRで確定(半角カナJISではない)
                    text = CharCode.ISOKR.GetString(bytes, len);
                    return (text != null ? CharCode.ISOKR : null);
                }
                if (c <= 0)
                {   //JIS評価値がマイナスないしゼロならばJISではないと判断
                    text = null;
                    return null;
                }

                if (JISH)
                {   //補助漢字のエスケープシーケンスあり、補助漢字を考慮してデコードする
                    text = CharCode.JISH.GetString(bytes, len);
                    if (text != null)
                    {   //■デコード成功ならJISH確定
                        return CharCode.JISH;
                    }
                }
                else
                {   //補助漢字なしJISとしてデコードする
                    text = CharCode.JIS.GetString(bytes, len);
                    if (text != null)
                    {   //■デコード成功なら、カナのSO/SI切り替えがあればCP50222、なければCP50221とみなす。
                        //(なお、CP50221/CP50222どちらを使ってもデコード結果は同一となる)
                        return hasSOSI(bytes, len) ? CharCode.JIS50222 : CharCode.JIS;
                    }
                }
                //■デコード不能箇所有、JIS破損ファイルとみなす
                text = CharCode.JISNG.GetString(bytes, len);
                return CharCode.JISNG;
            }
            #endregion
        }

        /// <summary>
        /// SJIS評価クラス
        /// </summary>
        private class SJIS : ReadJEnc
        {   /// <summary>日本語ShiftJIS・EUCJP・CP1252を判別対象とする際のコンストラクタ</summary>
            internal SJIS() : base(CharCode.SJIS, CharCode.EUC) { }

            protected override int GetEncoding(byte[] bytes, int pos, int len)
            {
                int score = 0; //初期値ゼロからReadJEnc評価を始める
                byte b1 = bytes[pos];
                byte b2;
                while (pos < len)
                {   //前の文字との連続性チェック用定数定義
                    const int PREV_KANA = 1; //直前文字は半角カナ
                    const int PREV_ZENKAKU = 2; //直前文字は全角
                    int prevChar = 0; //前の文字はKANAでもZENKAKUでもない
                    while (b1 > DEL)
                    {
                        if (b1 >= 0xA1 && b1 <= 0xDF)
                        {   //１バイト半角カナ：OK（連続はEUCやCP1252よりも高配点とする）
                            if (prevChar == PREV_KANA) { score += 3; } else { score += 1; prevChar = PREV_KANA; }
                        }
                        // 非CP932チェック用定数(0x00000061,0xE0009800)：CP932ではデコード不能な未定義文字のビットを１
                        //        FEDC BA98 7654 3210         FEDC BA98 7654 3210
                        //        ---- ---- ---- ----         ---- ---- ---- ----
                        // (0x9#) 0000 0000 0000 0000  (0x8#) 0000 0000 0110 0001 - 80(A0判定でも流用):定義外、85,86:未使用(shift_jis2004などでは使用ありだがCP932ではデコード不能)
                        // (0xF#) 1110 0000 0000 0000  (0xE#) 1001 1000 0000 0000 - FD,FE,FF:定義外、EB,EC,EF:未使用 (F0-F9:外字は許容。HNXgrepなど外字不許容とする場合はビットを立てること)
                        else if (((b1 < 0xE0 ? 0x00000061 : 0xE0009800) & 1u << (b1 % 32)) != 0
                            || (++pos) >= len
                            || (b2 = bytes[pos]) < 0x40 || b2 > 0xFC)
                        {   //１バイト目がおかしい(SJIS定義外/未使用) or ２バイト目把握不能 or ２バイト目SJIS定義外
                            return int.MinValue; //可能性消滅
                        }
                        else
                        {   //全角文字数を加算(EUCよりは可能性を低めに見積もっておく)
                            if (prevChar == PREV_ZENKAKU) { score += 4; }
                            else
                            {   //（ただし唐突に0x98以降の第二水準文字が出てきた場合は、UTF-8/EUC/CP1252の可能性が高いのでプラス値なしとする）
                                score += (b1 > 0x98 ? 0 : 2);
                                prevChar = PREV_ZENKAKU;
                            }
                        }
                        //各国語全コード共通：さらに次の文字へ
                        if ((++pos) >= len) { break; }
                        b1 = bytes[pos];
                    }
                    //各国語全コード共通：半角文字の範囲を読み飛ばし
                    while (b1 <= DEL && (++pos) < len) { b1 = bytes[pos]; }
                }
                return score;
            }
        }
        #endregion

        #region 各国語文字コード評価クラス--------------------------------------
#if (!JPONLY) 
        //漢字圏テキスト文字コード各種（日本語判別以外使用しないなら定義省略可）
        /// <summary>
        /// BIG5TW評価クラス
        /// </summary>
        private class BIG5TW : ReadJEnc
        {   /// <summary>中国語繁体字・EUC-TW・CP1252を判別対象とする際のコンストラクタ</summary>
            internal BIG5TW() : base(CharCode.BIG5TW, CharCode.EUCTW) { }

            protected override int GetEncoding(byte[] bytes, int pos, int len)
            {   //※配点未調整（EUC/UTF8はおそらく正しく識別可能、CP1252は一部誤判別する可能性あり）
                int score = 0; //初期値ゼロからReadJEnc評価を始める
                byte b1 = bytes[pos];
                byte b2;
                while (pos < len)
                {   //前の文字との連続性チェック用定数定義
                    const int PREV_ZENKAKU = 2; //直前文字は全角
                    int prevChar = 0; //前の文字はKANAでもZENKAKUでもない
                    while (b1 > DEL)
                    {
                        if (b1 < 0x81 || b1 > 0xF9 || b1 == 0xC7 || b1 == 0xC8
                            || (++pos) >= len
                            || (b2 = bytes[pos]) < 0x40 || (b2 < 0xA1 && b2 > 0x7E) || b2 > 0xFE)
                        {   //１バイト目がBig5定義外 or ２バイト目把握不能 or ２バイト目がBig5定義外
                            return int.MinValue; //可能性消滅
                        }
                        else
                        {   //全角文字数を加算(EUCよりは可能性を低めに見積もっておく)
                            if (prevChar == PREV_ZENKAKU) { score += 4; }
                            else
                            {   //ただし唐突に外字・次常用字(第二水準)が出てきた場合は配点を低めにする
                                score += (b1 < 0xA1 || b1 > 0xC8 ? 1 : 2);
                                prevChar = PREV_ZENKAKU;
                            }
                        }
                        //各国語全コード共通：さらに次の文字へ
                        if ((++pos) >= len) { break; }
                        b1 = bytes[pos];
                    }
                    //各国語全コード共通：半角文字の範囲を読み飛ばし
                    while (b1 <= DEL && (++pos) < len) { b1 = bytes[pos]; }
                }
                return score;
            }
        }

        /// <summary>
        /// GB18030評価クラス
        /// </summary>
        private class GB18030 : ReadJEnc
        {   /// <summary>中国語簡体字・CP1252を判別対象とする際のコンストラクタ</summary>
            internal GB18030() : base(CharCode.GB18030, CharCode.GB18030) { }

            protected override int GetEncoding(byte[] bytes, int pos, int len)
            {   //※配点未調整（EUC/UTF8はおそらく正しく識別可能、CP1252は誤判別の可能性大）
                int score = 0; //初期値ゼロからReadJEnc評価を始める
                byte b1 = bytes[pos];
                byte b2;
                while (pos < len)
                {   //前の文字との連続性チェック用定数定義
                    const int PREV_ZENKAKU = 2; //直前文字は全角
                    int prevChar = 0; //前の文字はKANAでもZENKAKUでもない
                    while (b1 > DEL)
                    {
                        if (b1 < 0x81 || b1 > 0xFE || (++pos) >= len)
                        {   //１バイト目がおかしい(GB定義外) or ２バイト目把握不能
                            return int.MinValue; //可能性消滅
                        }
                        else if ((b2 = bytes[pos]) >= 0x40 && b2 <= 0xFE)
                        {   //２バイト文字：全角文字数を加算(EUCよりは可能性を低めに見積もっておく)
                            if (prevChar == PREV_ZENKAKU) { score += 4; } else { score += 2; prevChar = PREV_ZENKAKU; }
                        }
                        else if (b2 >= 0x30 && b2 <= 0x39
                            && (++pos) < len
                            && (b2 = bytes[pos]) >= 0x81 && b2 <= 0xFE
                            && (++pos) < len
                            && (b2 = bytes[pos]) >= 0x30 && b2 <= 0x39)
                        {   //４バイト全角文字：全角文字数を加算（かなりGB18030の決め手になる特徴なので、ポイントを高めに）
                            if (prevChar == PREV_ZENKAKU) { score += 16; } else { score += 8; prevChar = PREV_ZENKAKU; }
                        }
                        else
                        {   //GB18030定義外
                            return int.MinValue; //可能性消滅
                        }
                        //各国語全コード共通：さらに次の文字へ
                        if ((++pos) >= len) { break; }
                        b1 = bytes[pos];
                    }
                    //各国語全コード共通：半角文字の範囲を読み飛ばし
                    while (b1 <= DEL && (++pos) < len) { b1 = bytes[pos]; }
                }
                return score;
            }
        }

        /// <summary>
        /// UHCKR評価クラス
        /// </summary>
        private class UHCKR : ReadJEnc
        {   /// <summary>ハングルUHC・CP1252を判別対象とする際のコンストラクタ</summary>
            internal UHCKR() : base(CharCode.UHCKR, CharCode.UHCKR) { }

            protected override int GetEncoding(byte[] bytes, int pos, int len)
            {   //※配点未調整（EUC/UTF8はおそらく正しく識別可能、CP1252は誤判別の可能性大）
                int score = 0; //初期値ゼロからReadJEnc評価を始める
                byte b1 = bytes[pos];
                byte b2;
                while (pos < len)
                {   //前の文字との連続性チェック用定数定義
                    const int PREV_ZENKAKU = 2; //直前文字は全角
                    int prevChar = 0; //前の文字はKANAでもZENKAKUでもない
                    while (b1 > DEL)
                    {
                        if (b1 < 0x81 || b1 > 0xFE
                            || (++pos) >= len
                            || (b2 = bytes[pos]) < 0x41 || (b2 < 0x61 && b2 > 0x5A) || (b2 < 0x81 && b2 > 0x7A) || b2 > 0xFE)
                        {   //１バイト目がおかしい(UHC定義外) or ２バイト目把握不能 or ２バイト目がUHC定義外
                            return int.MinValue; //可能性消滅
                        }
                        else
                        {   //全角文字数を加算(EUCよりは可能性を低めに見積もっておく)
                            if (prevChar == PREV_ZENKAKU) { score += 4; } else { score += 2; prevChar = PREV_ZENKAKU; }
                        }
                        //各国語全コード共通：さらに次の文字へ
                        if ((++pos) >= len) { break; }
                        b1 = bytes[pos];
                    }
                    //各国語全コード共通：半角文字の範囲を読み飛ばし
                    while (b1 <= DEL && (++pos) < len) { b1 = bytes[pos]; }
                }
                return score;
            }
        }

        //ISO8859などのテキスト文字コード自動判別（日本語判別以外使用しないなら定義省略可）
        /// <summary>
        /// １バイト文字コード汎用評価クラス
        /// タイ語ほかアルファベットと混在させない文字種の文字コード判別で共通仕様
        /// </summary>
        private class SBCS : ReadJEnc
        {
            /// <summary>その文字コード体系固有のコードの開始位置</summary>
            private int BOUND;
            /// <summary>0x80-0xFFの文字コード未定義の箇所をbitで表現した値</summary>
            private new readonly uint[] NODEF;

            /// <summary>引数指定文字コードとCP1252を判別対象とする際のコンストラクタ</summary>
            /// <param name="CharCode">このインスタンスで判別対象とするデフォルト文字コード</param>
            /// <param name="BOUND">通常文字／この文字コード固有の文字種範囲の境界</param>
            /// <param name="NODEF">
            /// 文字コード未定義の箇所をbitで表現した値
            /// <remarks>
            /// [0]=0x80-0x9Fの未定義箇所
            /// [1]=0xA0-0xBFの未定義箇所
            /// [2]=0xC0-0xDFの未定義箇所
            /// [3]=0xE0-0xFFの未定義箇所
            /// </remarks></param>
            internal SBCS(CharCode CharCode, int BOUND, params uint[] NODEF)
                : base(CharCode, null)
            {
                this.BOUND = BOUND;
                this.NODEF = NODEF;
            }

            protected override int GetEncoding(byte[] bytes, int pos, int len)
            {   //※配点未調整（EUC/UTF8はおそらく正しく識別可能、CP1252はひょっとすると誤判別の可能性あり）
                int score = 0; //初期値ゼロからReadJEnc評価を始める
                byte b1 = bytes[pos];

                //配列から展開してローカル変数におろす
                uint undefined_0x80_0x9F = (NODEF.Length > 0 ? NODEF[0] : 0);
                uint undefined_0xA0_0xBF = (NODEF.Length > 1 ? NODEF[1] : 0);
                uint undefined_0xC0_0xDF = (NODEF.Length > 2 ? NODEF[2] : 0);
                uint undefined_0xE0_0xFF = (NODEF.Length > 3 ? NODEF[3] : 0);

                while (pos < len)
                {   //前の文字との連続性チェック用定数定義
                    bool prevOwnChar = false; //直前文字がその文字コード体系固有のものであればtrue、連続していれば配点高めとする
                    while (b1 > DEL)
                    {   //未定義文字チェック
                        uint undefinedCheck = (b1 < 0xC0)
                            ? (b1 < 0xA0) ? undefined_0x80_0x9F : undefined_0xA0_0xBF
                            : (b1 < 0xE0) ? undefined_0xC0_0xDF : undefined_0xE0_0xFF;
                        //そのバイト値が未定義コードかどうかチェック（bit表現値とかみあうようビットシフトして照合）
                        if ((undefinedCheck & (1u << (b1 % 32))) != 0)
                        {   //ビットが立ってる＝未定義コード、可能性消滅
                            return int.MinValue;
                        }
                        if (b1 >= BOUND)
                        {   //その文字コード体系における固有文字種：連続はEUCやCP1252よりも高配点とする
                            if (prevOwnChar) { score += 3; } else { score += 1; prevOwnChar = true; }
                        }
                        else
                        {   //通常のASCII文字と一緒に使われるような文字種：ASCIIとの連続をポイント高めに評価
                            if (prevOwnChar) { score += 1; prevOwnChar = false; } else { score += 2; }
                        }
                        //各国語全コード共通：さらに次の文字へ
                        if ((++pos) >= len) { break; }
                        b1 = bytes[pos];
                    }
                    //各国語全コード共通：半角文字の範囲を読み飛ばし
                    while (b1 <= DEL && (++pos) < len) { b1 = bytes[pos]; }
                }
                return score;
            }
        }
#endif
        #endregion
    }
}
