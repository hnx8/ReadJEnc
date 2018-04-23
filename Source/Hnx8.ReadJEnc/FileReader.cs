using System;
using System.IO;

namespace Hnx8.ReadJEnc
{
    /// <summary>
    /// ReadJEnc ファイル読み出し＆ファイル文字コード種類自動判別(Rev.20170821)
    /// </summary>
    public class FileReader : IDisposable
    {   ////////////////////////////////////////////////////////////////////////
        // <FileReader.cs> ReadJEnc File読出＆文字コード自動判別(Rev.20170821)
        //  Copyright (C) 2014-2018 hnx8(H.Takahashi)
        //  https://github.com/hnx8/ReadJEnc
        //
        //  Released under the MIT license
        //  http://opensource.org/licenses/mit-license.php
        ////////////////////////////////////////////////////////////////////////

        // コンストラクタ／デストラクタ==========================================

        /// <summary>単一ファイル読み出し用にファイルを指定して新しいインスタンスを初期化します。</summary>
        /// <param name="file">読み出すファイル（このファイルのサイズどおりに読み出し領域バッファを確保する）</param>
        public FileReader(FileInfo file)
            : this((file.Length < int.MaxValue ? (int)file.Length : 0)) { }

        /// <summary>複数ファイル連続読み出し用にバッファサイズを指定して新しいインスタンスを初期化します。</summary>
        /// <param name="len">最大読み出しファイルサイズ（領域バッファ確保サイズ）</param>
        public FileReader(int len)
        {
            Bytes = new byte[len];
        }

        /// <summary>ファイル読み出し用のリソースを解放します。</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>ファイル読み出し用のリソースを解放します。</summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {   // 管理（managed）リソースの破棄処理
                Bytes = null;
            }
            // 非管理（unmanaged）リソースの破棄処理：ReadJEncでは特に該当なし
        }

        // 設定カスタマイズ======================================================

        /// <summary>文字コード自動判別対象言語指定（初期状態は日本語ShiftJIS）</summary>
        public ReadJEnc ReadJEnc = ReadJEnc.JP;

        // ファイル読み出し本体==================================================

        /// <summary>ファイルを読み出してファイル文字コード種類を取得します。</summary>
        /// <param name="file">読み出すファイル</param>
        /// <returns>ファイル文字コード種類の判定結果</returns>
        public virtual CharCode Read(FileInfo file)
        {
            this.Length = 0;
            text = null;
            try
            {   // 無用なDiskIOを極力行わないよう、オープン前にもファイルサイズチェック
                if (file.Length == 0) { return FileType.EMPTYFILE; } // ■空ファイル
                if (file.Length > Bytes.Length) { return FileType.HUGEFILE; } // ■巨大ファイル
                CharCode c;
                // ファイルを読み込み、ファイル文字コード種類を把握
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {   // オープン後の実際のファイルサイズによるチェック
                    long filesize = stream.Length;
                    if (filesize == 0) { return FileType.EMPTYFILE; } // ■空ファイル
                    if (filesize > Bytes.Length) { return FileType.HUGEFILE; } // ■巨大ファイル
                    if (filesize > 65536)
                    {   // 一定サイズ以上の大きいファイルなら、BOM/マジックナンバー判定に必要な先頭バイトを読み込み、判断
                        this.Length = stream.Read(Bytes, 0, FileType.GetBinaryType_LEASTREADSIZE);
                        c = GetPreamble(filesize);
                        if (c == null || c is CharCode.Text)
                        {   // 残りの読みこみ（ただし非テキストと確定した場合は省略）
                            this.Length += stream.Read(Bytes, this.Length, (int)filesize - this.Length);
                        }
                    }
                    else
                    {   // 大きくないファイルは一括で全バイト読み込み、判断
                        this.Length = stream.Read(Bytes, 0, (int)filesize);
                        c = GetPreamble(filesize);
                    }
                }
                if (c is CharCode.Text)
                {   // BOMありテキストなら文字列を取り出す（取り出せなかったら非テキスト扱い）
                    if ((text = c.GetString(Bytes, Length)) == null) { c = null; }
                }
                else if (c == null)
                {   // ファイル文字コード種類不明なら、全バイト走査して文字コード確定
                    c = ReadJEnc.GetEncoding(Bytes, Length, out text);
                }
                // ここまでで文字コードが決まらなかったらバイナリファイル扱い
                return (c == null ? FileType.GetBinaryType(Bytes, Length) : c);
            }
            catch (System.IO.IOException) { return FileType.READERROR; } // ■読み取りエラー
            catch (System.UnauthorizedAccessException) { return FileType.READERROR; } // ■読み取りエラー
        }

        /// <summary>Readメソッド呼び出し時にファイルから読み出したテキスト文字列内容を取得します。</summary>
        /// <remarks>ファイルからテキストが取り出せなかった場合はnullとなります。</remarks>
        public string Text { get { return text; } }

        #region 非public処理----------------------------------------------------
        /// <summary>ファイル内容の読み出し先領域</summary>
        protected byte[] Bytes; // コンストラクタで確保,Disposeで廃棄
        /// <summary>現在読み出し済のファイルサイズ</summary><remarks>非読み出し時は0、分割読み出し時は読込済部分のサイズ</remarks>
        protected int Length = 0;
        /// <summary>ファイルから取り出したテキスト文字列</summary>
        protected String text = null;

        /// <summary>読み込んであるバイト配列のプリアンブル（BOMヘッダ／マジックナンバー）からファイル文字コード種類特定を試みる</summary>
        /// <param name="len">ファイルサイズ(未読込部分も含む。読み込み済サイズはthis.Lengthを参照)</param>
        /// <returns>確定した場合、ファイル文字コード種類。確定できなかった場合null</returns>
        protected virtual CharCode GetPreamble(long len)
        {
            // 【0】ファイル先頭バイトからUTF文字コード（BOMつきUTF）を判定
            CharCode ret = CharCode.GetPreamble(this.Bytes, this.Length);
            // BOMテキストファイルと判定できず＆ファイル先頭にバイナリファイル特徴の0x00が登場している場合、追加チェック
            if (ret == null && Array.IndexOf<byte>(this.Bytes, 0x00, 0, this.Length) >= 0)
            {   // UTF16Nの可能性がなければバイナリファイルとみなす
                if (ReadJEnc.SeemsUTF16N(this.Bytes, (int)len) == null)
                {   // ■バイナリ確定（マジックナンバーからファイル種類を決定）
                    return FileType.GetBinaryType(this.Bytes, this.Length);
                }
            }
            return ret; // ■BOMから特定できた場合はBOMつきUTF（特定できなかった場合はnull）
        }
        #endregion
    }
}
