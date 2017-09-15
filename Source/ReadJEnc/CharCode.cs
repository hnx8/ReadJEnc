using System.Text;

namespace Hnx8.ReadJEnc
{
    /// <summary>
    /// ReadJEnc 文字コード種類定義(Rev.20170821)
    /// </summary>
    public abstract class CharCode
    {   ////////////////////////////////////////////////////////////////////////
        // <CharCode.cs> ReadJEnc 文字コード種類定義(Rev.20170821)
        //  Copyright (C) 2014-2017 hnx8(H.Takahashi)
        //  http://hp.vector.co.jp/authors/VA055804/
        //
        //  Released under the MIT license
        //  http://opensource.org/licenses/mit-license.php
        ////////////////////////////////////////////////////////////////////////

        //Unicode系文字コード

        /// <summary>UTF8(BOMあり)</summary>
        public static readonly Text UTF8 = new Text("UTF-8", new UTF8Encoding(true, true)); //BOM : 0xEF, 0xBB, 0xBF
        /// <summary>UTF32(BOMありLittleEndian)</summary>
        public static readonly Text UTF32 = new Text("UTF-32", new UTF32Encoding(false, true, true)); //BOM : 0xFF, 0xFE, 0x00, 0x00
        /// <summary>UTF32(BOMありBigEndian)</summary>
        public static readonly Text UTF32B = new Text("UTF-32B", new UTF32Encoding(true, true, true)); //BOM : 0x00, 0x00, 0xFE, 0xFF
        /// <summary>UTF16(BOMありLittleEndian)</summary><remarks>Windows標準のUnicode</remarks>
        public static readonly Text UTF16 = new Text("UTF-16", new UnicodeEncoding(false, true, true)); //BOM : 0xFF, 0xFE
        /// <summary>UTF16(BOMありBigEndian)</summary>
        public static readonly Text UTF16B = new Text("UTF-16B", new UnicodeEncoding(true, true, true)); //BOM : 0xFE, 0xFF

        /// <summary>UTF16(BOM無しLittleEndian)</summary>
        public static readonly Text UTF16LE = new Text("UTF-16LE", new UnicodeEncoding(false, false, true));
        /// <summary>UTF16(BOM無しBigEndian)</summary>
        public static readonly Text UTF16BE = new Text("UTF-16BE", new UnicodeEncoding(true, false, true));
        /// <summary>UTF8(BOM無し)</summary>
        public static readonly Text UTF8N = new Text("UTF-8N", new UTF8Encoding(false, true));

        //１バイト文字コード

        /// <summary>Ascii</summary><remarks>デコードはUTF8Encodingを流用。Bom情報は転記しない</remarks>
        public static readonly Text ASCII = new Text("ASCII", 0) { Encoding = UTF8N.GetEncoding() };
        /// <summary>1252 ISO8859 西ヨーロッパ言語</summary>
        public static readonly Text ANSI = new Text("ANSI1252", 1252);

        //ISO-2022文字コード

        /// <summary>50221 iso-2022-jp 日本語 (JIS-Allow 1 byte Kana) ※MS版</summary>
        public static readonly Text JIS = new Text("JIS", 50221);
        /// <summary>50222 iso-2022-jp 日本語 (JIS-Allow 1 byte Kana - SO/SI)</summary><remarks>SO/SIによるカナシフトのみのファイルもCP50222とみなす</remarks>
        public static readonly Text JIS50222 = new Text("JIS50222", 50222);
        /// <summary>50221(MS版JIS) + 20932(JIS補助漢字を無理やりデコード)</summary><remarks>JIS補助漢字はデコードのみ対応、エンコードは未対応</remarks>
        public static readonly Text JISH = new JisHText("JIS補漢"); //他クラス定義の50221と20932を利用してデコード
        /// <summary>JISのように見えるがデコード不能な箇所あり、実質非テキストファイル</summary>
        public static readonly Text JISNG = new Text("JIS破損", -50221);
        /// <summary>50225 iso-2022-kr 韓国語(ISO)</summary><remarks>SO/SIカナシフトファイルの判定ロジックに流れ込まないようにするため定義</remarks>
        public static readonly Text ISOKR = new Text("ISO-KR", 50225);

        //日本語文字コード

        /// <summary>932 shift_jis 日本語 (シフト JIS) ※MS独自</summary>
        public static readonly Text SJIS = new Text("ShiftJIS", 932);
        /// <summary>EUC補助漢字(0x8F)あり ※MS-CP20932を利用し強引にデコードする</summary><remarks>エンコードするとファイルが壊れるので注意</remarks>
        public static readonly Text EUCH = new EucHText("EUC補漢"); //20932
        /// <summary>51932 euc-jp 日本語 (EUC) ※MS版</summary>
        public static readonly Text EUC = new Text("EUCJP", 51932);

#if (!JPONLY) 
        
        //漢字圏テキスト文字コード各種（日本語判別以外使用しないなら定義省略可）

        /// <summary>950 big5 繁体字中国語 (BIG5)</summary>
        public static readonly Text BIG5TW = new Text("Big5", 950);
        /// <summary>20000 x-Chinese-CNS 繁体字中国語(EUC-TW)</summary>
        public static readonly Text EUCTW = new Text("EUC-TW", 20000);

        /// <summary>54936 GB18030 簡体字中国語 (GB2312/GBKの拡張)</summary>
        public static readonly Text GB18030 = new Text("GB18030", 54936);
        //→EUC-CN(GB2312)はGB18030として取り扱うほうが妥当であるため定義をコメントアウト抹消
        ///// <summary>51936 EUC-CN 簡体字中国語 (=GB2312)</summary>
        //public static readonly Text EUCCN = new Text("EUC(中)", 51936);

        /// <summary>949 ks_c_5601-1987 韓国語 (UHC=EUC-KRの拡張)</summary>
        public static readonly Text UHCKR = new Text("UHC", 949);
        //→EUC-KRはUHCとして取り扱うほうが妥当であるため定義をコメントアウト抹消
        ///// <summary>51949 euc-kr 韓国語 (=KSX1001)</summary>
        //public static readonly Text EUCKR = new Text("EUC(韓)", 51949);


        //ISO8859などのテキスト文字コード自動判別（日本語判別以外使用しないなら定義省略可）
        
        /// <summary>Windows1250 中央ヨーロッパ言語(チェコ語等) iso-8859-2</summary>
        public static readonly Text CP1250 = new Text("CP1250", 1250);
        /// <summary>Windows1251 キリル言語(ロシア語等) </summary>
        public static readonly Text CP1251 = new Text("CP1251", 1251);
        /// <summary>Windows1253 ギリシャ語 iso-8859-7</summary>
        public static readonly Text CP1253 = new Text("CP1253", 1253);
        /// <summary>Windows1254 トルコ語 iso-8859-9</summary>
        public static readonly Text CP1254 = new Text("CP1254", 1254);
        /// <summary>Windows1255 ヘブライ語 iso-8859-8</summary>
        public static readonly Text CP1255 = new Text("CP1255", 1255);
        /// <summary>Windows1256 アラビア語 </summary>
        public static readonly Text CP1256 = new Text("CP1256", 1256);
        /// <summary>Windows1257 バルト言語 iso-8859-13</summary>
        public static readonly Text CP1257 = new Text("CP1257", 1257);
        /// <summary>Windows1258 ベトナム語</summary>
        public static readonly Text CP1258 = new Text("CP1258", 1258);
        /// <summary>TIS-620/Windows874 タイ語 iso-8859-11</summary>
        public static readonly Text TIS620 = new Text("TIS-620", 874);
#endif

        // 文字コード（ファイル種類）判定メソッド

        /// <summary>引数で指定されたbyte配列がBOMありUTFファイルと判定できる場合、その文字コードを返します。</summary>
        /// <param name="bytes">判定対象のバイト配列</param>
        /// <param name="read">バイト配列先頭の読み込み済バイト数（LEASTREADSIZEのバイト数以上読み込んでおくこと）</param>
        /// <returns>BOMから判定できた文字コード種類、合致なしの場合null</returns>
        public static CharCode GetPreamble(byte[] bytes, int read)
        {   //BOM一致判定
            return GetPreamble(bytes, read,
                UTF8, UTF32, UTF32B, UTF16, UTF16B);
        }

        #region 基本クラス定義--------------------------------------------------
        /// <summary>ファイル文字コード種類名</summary>
        public readonly string Name;
        /// <summary>先頭バイト識別データ（BOM/マジックナンバー）</summary>
        protected readonly byte[] Bytes = null;
        /// <summary>エンコーディング</summary>
        private Encoding Encoding;
        /// <summary>コードページ番号(Unicode以外のローカルなエンコーディングを使用するものについて設定あり。遅延初期化用の退避変数を兼ねる)</summary>
        public readonly int CodePage = 0;

        /// <summary>基本コンストラクタ</summary>
        /// <param name="Name">ファイル文字コード種類名を定義する</param>
        /// <param name="CodePage">デコード時に使用するCodePageを指定(正値ならDecoderExceptionFallback、マイナス値ならDecoderReplacementFallBackを設定)</param>
        /// <param name="Bytes">先頭バイト識別データを指定する</param>
        protected CharCode(string Name, int CodePage, byte[] Bytes)
        {
            this.Name = Name;
            this.CodePage = CodePage;
            //GetEncoding(); //Encodingを実際に使用するまで初期化を遅らせる
            this.Bytes = Bytes;
        }
        /// <summary>基本コンストラクタ</summary>
        /// <param name="Name">ファイル文字コード種類名を定義する</param>
        /// <param name="Encoding">デコード時に使用するEncodingを指定する</param>
        /// <param name="Bytes">先頭バイト識別データを指定する</param>
        protected CharCode(string Name, Encoding Encoding, byte[] Bytes)
        {
            this.Name = Name;
            this.Encoding = Encoding;
            this.Bytes = Bytes;
        }

        /// <summary>このファイル文字コード種類のEncodingオブジェクトを取得します。</summary>
        public Encoding GetEncoding()
        {
            if (this.Encoding == null)
            {   //Encodingオブジェクトがまだ用意されていなければ初期化する
                this.Encoding =
                    (CodePage > 0 ? System.Text.Encoding.GetEncoding(CodePage, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback)
                    : CodePage < 0 ? System.Text.Encoding.GetEncoding(-CodePage, EncoderFallback.ExceptionFallback, DecoderFallback.ReplacementFallback)
                    : null);
            }
            return Encoding;
        }

        /// <summary>引数のバイト配列から文字列を取り出します。失敗時はnullが返ります。</summary>
        /// <param name="bytes">判定対象のバイト配列</param>
        /// <param name="len">ファイルサイズ(バイト配列先頭からの先頭からのデコード対象バイト数)</param>
        public virtual string GetString(byte[] bytes, int len)
        {
            Encoding enc = GetEncoding();
            if (enc == null) { return null; }
            try
            {   //BOMサイズを把握し、BOMを除いた部分を文字列として取り出す
                int bomBytes = (this.Bytes == null ? 0 : this.Bytes.Length);
                return enc.GetString(bytes, bomBytes, len - bomBytes);
            }
            catch (DecoderFallbackException)
            {   //読み出し失敗(マッピングされていない文字があった場合など)
                return null;
            }
        }

        /// <summary>このファイル文字コード種類の名前を取得します。</summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>判定対象のファイル文字コード種類一覧から、BOM/マジックナンバーが一致するものを探索して返す</summary>
        /// <param name="bytes">判定対象のバイト配列</param>
        /// <param name="read">バイト配列先頭の読み込み済バイト数（LEASTREADSIZEのバイト数以上読み込んでおくこと）</param>
        /// <param name="arr">判定対象とするファイル文字コード種類の一覧</param>
        /// <returns>先頭バイトが一致したファイル文字コード種類、合致なしの場合null</returns>
        protected static CharCode GetPreamble(byte[] bytes, int read, params CharCode[] arr)
        {
            foreach (CharCode c in arr)
            {   //読み込み済バイト配列内容をもとにファイル種類の一致を確認
                byte[] bom = c.Bytes;
                int i = (bom != null ? bom.Length : int.MaxValue); //BOM・マジックナンバー末尾から調べる
                if (read < i) { continue; } //そもそもファイルサイズが小さい場合は不一致
                do
                {   //全バイト一致ならその文字コードとみなす
                    if (i == 0) { return c; }
                    i--;
                } while (bytes[i] == bom[i]); //BOM・マジックナンバー不一致箇所ありならdo脱出
            }
            return null; //ファイル種類決定できず
        }

        #endregion

        #region テキスト基本クラス定義------------------------------------------
        /// <summary>文字コード種類：テキスト
        /// </summary>
        public class Text : CharCode
        {
            internal Text(string Name, Encoding Encoding) : base(Name, Encoding, Encoding.GetPreamble()) { }
            internal Text(string Name, int CodePage) : base(Name, CodePage, null) { }
        }
        #endregion

        #region JIS補助漢字対応デコーダ-----------------------------------------
        /// <summary>
        /// EUC補助漢字特殊処理(MS版CP20932の特異なコード体系によりデコードする)
        /// </summary>
        private class EucHText : Text
        {
            internal EucHText(string Name) : base(Name, 20932) { }

            public override string GetString(byte[] bytes, int len)
            {
                byte[] bytesForCP20932 = new byte[len]; //CP20932でのデコード用にバイト配列を補正
                int cp20932Len = 0;
                int shiftPos = int.MinValue;
                byte b;
                for (int i = 0; i < len; i++)
                {
                    if ((b = bytes[i]) == 0x8F)
                    {   //3byteの補助漢字を検出、補正箇所を把握(0x8Fは読み捨て、補正後配列には設定しない)
                        shiftPos = i + 2;
                    }
                    else
                    {   //補助漢字3byte目ならば0x21-0x7Eへシフト(CP20932におけるEUCの2byte目として設定)
                        bytesForCP20932[cp20932Len] = (i == shiftPos ? (byte)(b & 0x7F) : b);
                        cp20932Len++;
                    }
                }
                try
                {   //補正後配列を用い、CP20932でのデコードを試みる
                    return GetEncoding().GetString(bytesForCP20932, 0, cp20932Len);
                }
                catch (DecoderFallbackException)
                {   //読み出し失敗(マッピングされていない文字があった場合など)
                    return null;
                }
            }
        }
        /// <summary>
        /// JIS補助漢字特殊処理(MS版CP20932の特異なコード体系によりデコードする)
        /// </summary>
        private class JisHText : Text
        {
            internal JisHText(string Name) : base(Name, 0) { }

            public override string GetString(byte[] bytes, int len)
            {
                try
                {
                    StringBuilder ret = new StringBuilder(len);
                    int pos = 0;
                    while (pos < len)
                    {   //JIS補助漢字エスケープ以外の範囲を把握
                        int start = pos;
                        while (pos < len)
                        {
                            if (bytes[pos] == 0x1B && pos + 3 < len &&
                                bytes[pos + 1] == 0x24 &&
                                bytes[pos + 2] == 0x28 &&
                                bytes[pos + 3] == 0x44)
                            {   //JIS補助漢字エスケープシーケンスを検出、ループ脱出
                                break;
                            }
                            pos++;
                        }
                        if (start < pos)
                        {   //通常のCP5022Xでデコードする
                            ret.Append(JIS.GetEncoding().GetString(bytes, start, pos - start));
                        }
                        //JIS補助漢字エスケープ部分の処理
                        if (pos < len)
                        {   //JIS補助漢字エスケープシーケンス除去、補助漢字範囲特定
                            pos = pos + 4;
                            start = pos;
                            while (pos < len && bytes[pos] != 0x1B) { pos++; }
                            if (start < pos)
                            {
                                byte[] bytesForCP20932 = new byte[pos - start];
                                for (int i = 0; i < bytesForCP20932.Length; i++)
                                {   //CP20932のコード体系に合わせ、１バイト目は0xA1-0XFE,２バイト目は0x21-0x7Eとなるようにする
                                    bytesForCP20932[i] = bytes[start + i];
                                    if (i % 2 == 0) { bytesForCP20932[i] |= 0x80; }
                                }
                                //EUC補助漢字のCP20932を用いてデコードする
                                ret.Append(EUCH.GetEncoding().GetString(bytesForCP20932, 0, bytesForCP20932.Length));
                            }
                        }
                    }
                    return ret.ToString();
                }
                catch (DecoderFallbackException)
                {   //読み出し失敗(マッピングされていない文字があった場合など)
                    return null;
                }
            }
        }
        #endregion
    }
}