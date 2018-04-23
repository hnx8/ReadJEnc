namespace Hnx8.ReadJEnc
{
    /// <summary>
    /// ReadJEnc ファイル種類定義(Rev.20170821)
    /// </summary>
    public class FileType : CharCode
    {   ////////////////////////////////////////////////////////////////////////
        // <FileType.cs> ReadJEnc ファイル種類定義(Rev.20170821)
        //  Copyright (C) 2014-2018 hnx8(H.Takahashi)
        //  https://github.com/hnx8/ReadJEnc
        //
        //  Released under the MIT license
        //  http://opensource.org/licenses/mit-license.php
        ////////////////////////////////////////////////////////////////////////

        // ※テキストファイルの定義は、CharCode.csを参照のこと

        // 読込対象外ファイル

        /// <summary>読み込み失敗</summary>
        public static readonly FileType READERROR = new FileType("読込不能");
        /// <summary>空ファイル</summary>
        public static readonly FileType EMPTYFILE = new FileType("空File");
        /// <summary>巨大ファイル</summary>
        public static readonly FileType HUGEFILE = new FileType("巨大File");

        // バイナリその他非テキストファイル

        /// <summary>バイナリ</summary>
        public static readonly Bin BINARY = new Bin("$BINARY", null);

        /// <summary>Javaバイナリ</summary>
        public static readonly Bin JAVABIN = new Bin(-65001, "$JavaBin", 0xCA, 0xFE, 0xBA, 0xBE); // 内部文字コードはUTF8。HNXgrep用設定
        /// <summary>Windowsバイナリ</summary>
        public static readonly Bin WINBIN = new Bin("$WinExec", 0x4D, 0x5A);
        /// <summary>Windowsショートカット</summary>
        public static readonly Bin SHORTCUT = new Bin("$WinLnk", 0x4C, 0x00, 0x00, 0x00, 0x01, 0x14, 0x02, 0x00);
        /// <summary>PDF</summary>
        public static readonly Bin PDF = new Bin("%PDF", (byte)'%', (byte)'P', (byte)'D', (byte)'F', (byte)'-');

        /// <summary>Zip圧縮</summary>
        public static readonly Bin ZIP = new ZipBinary("$ZIP", 0x50, 0x4B, 0x03, 0x04);
        /// <summary>GZip圧縮</summary>
        public static readonly Bin GZIP = new ZipBinary("$GZIP", 0x1F, 0x8B);
        /// <summary>7zip圧縮</summary>
        public static readonly Bin SEVENZIP = new ZipBinary("$7ZIP", (byte)'7', (byte)'z', 0xBC, 0xAF, 0x27, 0x1C);
        /// <summary>Rar圧縮</summary>
        public static readonly Bin RAR = new ZipBinary("$RAR", (byte)'R', (byte)'a', (byte)'r', (byte)'!');
        /// <summary>Cab圧縮</summary>
        public static readonly Bin CABINET = new ZipBinary("$CAB", (byte)'M', (byte)'S', (byte)'C', (byte)'F', 0x00, 0x00, 0x00, 0x00);
        /// <summary>BZip2圧縮</summary>
        public static readonly Bin BZIP2 = new ZipBinary("$BZIP2", (byte)'B', (byte)'Z', (byte)'h');
        /// <summary>Z(compress)圧縮</summary>
        public static readonly Bin ZLZW = new ZipBinary("$Z-LZW", 0x1F, 0x9D);
        // TAR/LHA(LZHファイル)については先頭バイトが不定であり判別対応しずらいため対応外とする。

        /// <summary>BMP画像</summary>
        public static readonly Image BMP = new Image("%BMP", (byte)'B', (byte)'M');
        /// <summary>GIF画像</summary>
        public static readonly Image GIF = new Image("%GIF", (byte)'G', (byte)'I', (byte)'F', (byte)'8');
        /// <summary>JPEG画像</summary>
        public static readonly Image JPEG = new Image("%JPEG", 0xFF, 0xD8, 0xFF);
        /// <summary>PNG画像</summary>
        public static readonly Image PNG = new Image("%PNG", 0x89, (byte)'P', (byte)'N', (byte)'G', 0x0D, 0x0A, 0x1A, 0x0A);
        /// <summary>TIFF画像</summary>
        public static readonly Image TIFF = new Image("%TIFF", 0x49, 0x49, 0x2A, 0x00); // IE9以降ならimgタグで表示可能
        /// <summary>Windowsアイコン画像</summary><remarks>マジックナンバーのほか追加チェックあり</remarks>
        public static readonly Image IMGICON = new Image("%ICON", 0x00, 0x00, 0x01, 0x00);


        // ファイル種類判定用定数・判定メソッド

        /// <summary>BOM/マジックナンバー一致判定にあたり、最低限読み込みを済ませておく必要がある先頭バイト数です。</summary>
        public const int GetBinaryType_LEASTREADSIZE = 32;

        /// <summary>引数で指定されたbyte配列についてバイナリファイルの種類を判定します。</summary>
        /// <param name="bytes">判定対象のバイト配列</param>
        /// <param name="read">バイト配列先頭の読み込み済バイト数（LEASTREADSIZEのバイト数以上読み込んでおくこと）</param>
        /// <returns>バイナリファイル種類判定結果（どれにも該当しなければ一般バイナリと判定）</returns>
        public static CharCode GetBinaryType(byte[] bytes, int read)
        {   // 定義済みマジックナンバーすべてを対象に一致判定
            CharCode ret = GetPreamble(bytes, read,
                BMP, GIF, JPEG, PNG, TIFF, IMGICON,
                JAVABIN, WINBIN, SHORTCUT, PDF,
                ZIP, GZIP, SEVENZIP, RAR, CABINET, BZIP2, ZLZW);
            // ファイル種類に応じた追加判定
            if (ret == IMGICON && (read < 23 || bytes[4] == 0 || bytes[5] != 0)) { ret = null; } // ICONの誤判別防止用（アイコン個数チェック）            
            // 判定できたファイル種類を返す（どれにも該当しなければ一般バイナリと判定）
            return (ret != null ? ret : BINARY);
        }

        #region 継承クラス定義--------------------------------------------------
        private FileType(string Name) : base(Name, 0, null) { }

        /// <summary>ファイル文字コード種類：バイナリ
        /// </summary>
        public class Bin : CharCode
        {
            internal Bin(string Name, params byte[] Bytes) : base(Name, 0, Bytes) { }
            internal Bin(int Encoding, string Name, params byte[] bytes) : base(Name, Encoding, bytes) { }
        }
        /// <summary>ファイル文字コード種類：Zipバイナリ
        /// </summary>
        public class ZipBinary : Bin
        {
            internal ZipBinary(string Name, params byte[] Bytes) : base(Name, Bytes) { }
        }
        /// <summary>ファイル文字コード種類：画像
        /// </summary>
        public class Image : CharCode
        {
            internal Image(string Name, params byte[] Bytes) : base(Name, 0, Bytes) { }
        }
        #endregion
    }
}
