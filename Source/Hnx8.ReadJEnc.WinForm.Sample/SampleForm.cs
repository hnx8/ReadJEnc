using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Hnx8.ReadJEnc.Sample
{
    /// <summary>
    /// ReadJEnc使用サンプル
    /// </summary>
    public partial class SampleForm : Form
    {

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SampleForm());
        }

        public SampleForm()
        {
            InitializeComponent();

            // ReadJEncクラスに定義されている文字エンコード判別オブジェクトの一覧をコンボボックスに設定する
            KeyValuePair<string, ReadJEnc>[] items =
            {
                new KeyValuePair<string, ReadJEnc>("日本語", ReadJEnc.JP),
                new KeyValuePair<string, ReadJEnc>("欧文（西欧）", ReadJEnc.ANSI),
#if (!JPONLY)
                new KeyValuePair<string, ReadJEnc>("繁体字中国語", ReadJEnc.TW),
                new KeyValuePair<string, ReadJEnc>("簡体字中国語", ReadJEnc.CN),
                new KeyValuePair<string, ReadJEnc>("ハングル", ReadJEnc.KR),

                new KeyValuePair<string, ReadJEnc>("中欧東欧言語", ReadJEnc.CP1250),
                new KeyValuePair<string, ReadJEnc>("キリル言語", ReadJEnc.CP1251),
                new KeyValuePair<string, ReadJEnc>("ギリシャ語", ReadJEnc.CP1253),
                new KeyValuePair<string, ReadJEnc>("トルコ語", ReadJEnc.CP1254),
                new KeyValuePair<string, ReadJEnc>("ヘブライ語", ReadJEnc.CP1255),
                new KeyValuePair<string, ReadJEnc>("アラビア語", ReadJEnc.CP1256),
                new KeyValuePair<string, ReadJEnc>("バルト言語", ReadJEnc.CP1257),
                new KeyValuePair<string, ReadJEnc>("ベトナム語", ReadJEnc.CP1258),
                new KeyValuePair<string, ReadJEnc>("タイ語", ReadJEnc.TIS620),
#endif
            };
            foreach (KeyValuePair<string, ReadJEnc> item in items)
            {
                selDefault.Items.Add(item);
            }
            selDefault.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 連続読み出しの最大許容ファイルサイズ指定値
            int maxFileSize = (int)nmuMaxSize.Value;

            string path = txtPath.Text;
            ReadJEnc readJEnc = ((KeyValuePair<String, ReadJEnc>)selDefault.SelectedItem).Value;
            if (File.Exists(path))
            {   // ファイル指定：単一ファイル読み出しの実行例
                System.IO.FileInfo file = new FileInfo(path);
                using (FileReader reader = new FileReader(file))
                {
                    // コンボボックス選択どおりの文字エンコード判別オブジェクトで判別
                    reader.ReadJEnc = readJEnc;
                    // 判別結果の文字エンコードは、Readメソッドの戻り値で把握できます
                    CharCode c = reader.Read(file);
                    // 戻り値の型からファイルの大まかな種類が判定できます、
                    string type =
                        (c is CharCode.Text ? "Text:"
                        : c is FileType.Bin ? "Binary:"
                        : c is FileType.Image ? "Image:"
                        : "");
                    // 戻り値のNameプロパティから文字コード名を取得できます
                    string name = c.Name;
                    // 戻り値のGetEncoding()メソッドで、エンコーディングを取得できます
                    System.Text.Encoding enc = c.GetEncoding();
                    // 実際に読み出したテキストは、Textプロパティから取得できます
                    // （非テキストファイルの場合は、nullが設定されます）
                    string text = reader.Text;

                    txtResult.Text =
                        "【" + type + c.Name + "】" + file.Name + "\r\n"
                        + "-------------------------------------------" + "\r\n"
                        + (text != null ? text : "");
                    btnClipboard.Enabled = true;
                }
            }
            else if (Directory.Exists(path))
            {   // ディレクトリ指定：複数ファイル連続読み出しの実行例
                txtResult.Text = "一括判定中。。。";
                txtResult.Refresh();
                // 最大許容ファイルサイズ指定でオブジェクトを作成する
                using (FileReader reader = new FileReader(maxFileSize))
                {
                    // コンボボックス選択どおりの文字エンコード判別オブジェクトで判別
                    reader.ReadJEnc = readJEnc;
                    // ディレクトリ再帰調査
                    DirectoryInfo dir = new DirectoryInfo(path);
                    rootDir = dir.FullName.TrimEnd('\\') + @"\";
                    txtResult.Text = getFiles(reader, dir);
                    btnClipboard.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("ディレクトリまたはファイルのフルパスを指定してください。", "Path指定エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        string rootDir;

        private string getFiles(FileReader reader, DirectoryInfo dir)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (FileInfo f in dir.GetFiles())
                {   // 各ファイルの文字エンコード判別のみ実施し、StringBuilderに結果をタブ区切りで詰め込む
                    CharCode c = reader.Read(f);
                    sb.Append((f.DirectoryName + @"\").Substring(rootDir.Length));
                    sb.Append("\t");
                    sb.Append(f.Name);
                    sb.Append("\t");
                    sb.Append(c.Name);
                    sb.AppendLine();
                }
                foreach (DirectoryInfo d in dir.GetDirectories())
                {   // サブフォルダについて再帰
                    sb.Append(getFiles(reader, d));
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            return sb.ToString();
        }

        private void btnClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtResult.Text);
            MessageBox.Show("結果をクリップボードにコピーしました。" + Environment.NewLine + "テキストエディタ・Excel等に貼り付けて確認してください。", "ReadJEncSample", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
