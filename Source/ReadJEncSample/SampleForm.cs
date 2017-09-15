using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

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

        List<ReadJEnc> list = new List<ReadJEnc>();

        public SampleForm()
        {
            InitializeComponent();

            //ReadJEncクラスに定義されているすべての文字コード判別定義をリフレクションで取り出し、コンボボックスに設定する
            FieldInfo[] fields = typeof(ReadJEnc).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (FieldInfo f in fields)
            {
                ReadJEnc readjenc = (f.GetValue(null) as ReadJEnc);
                if (readjenc != null) 
                {
                    list.Add(readjenc);
                    string text = readjenc.CharCode.ToString();
                    foreach (object description in f.GetCustomAttributes(typeof(DescriptionAttribute), true))
                    {   //属性Descriptionに定義されている言語名 ＋ そのReadJEncインスタンスで主に対象とする文字コードで表示名を組み立て
                        text = (description as DescriptionAttribute).Description + " - " + readjenc.CharCode.ToString();
                    }
                    selDefault.Items.Add(text);
                }
            }
            //ReadJEnc[] list = 
            //{
            //    ReadJEnc.JP,
            //    ReadJEnc.TW,
            //    ReadJEnc.CN,
            //    ReadJEnc.KR,
            //    ReadJEnc.ANSI,
            //};
            //selDefault.Items.AddRange(list.ToArray());
            selDefault.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //連続読み出しの最大許容ファイルサイズ指定値
            int maxFileSize = (int)nmuMaxSize.Value;

            string path = txtPath.Text;
            if (File.Exists(path))
            {   //ファイル指定：単一ファイル読み出しの実行例
                System.IO.FileInfo file = new FileInfo(path);
                using (FileReader reader = new FileReader(file))
                {   
                    //コンボボックス選択どおりの文字コード判別オブジェクトで判別
                    reader.ReadJEnc = list[selDefault.SelectedIndex]; // (ReadJEnc)selDefault.SelectedItem;
                    //判別結果の文字コードは、Readメソッドの戻り値で把握できます
                    CharCode c = reader.Read(file);
                    //戻り値の型からファイルの大まかな種類が判定できます、
                    string type =
                        (c is CharCode.Text ? "Text:"
                        : c is FileType.Bin ? "Binary:"
                        : c is FileType.Image ? "Image:"
                        : "");
                    //戻り値のNameプロパティから文字コード名を取得できます
                    string name = c.Name;
                    //戻り値のGetEncoding()メソッドで、エンコーディングを取得できます
                    System.Text.Encoding enc = c.GetEncoding();
                    //実際に読み出したテキストは、Textプロパティから取得できます
                    //（非テキストファイルの場合は、nullが設定されます）
                    string text = reader.Text;

                    txtResult.Text =
                        "【" + type + c.Name + "】" + file.Name + "\r\n"
                        + "-------------------------------------------" + "\r\n"
                        + (text != null ? text : "");
                    btnClipboard.Enabled = true;
                }
            }
            else if (Directory.Exists(path))
            {   //ディレクトリ指定：複数ファイル連続読み出しの実行例
                txtResult.Text = "一括判定中。。。";
                txtResult.Refresh();
                //最大許容ファイルサイズ指定でオブジェクトを作成する
                using (FileReader reader = new FileReader(maxFileSize))
                {   
                    //コンボボックス選択どおりの文字コード判別オブジェクトで判別
                    reader.ReadJEnc = list[selDefault.SelectedIndex]; // (ReadJEnc)selDefault.SelectedItem;
                    //ディレクトリ再帰調査
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
                {   //各ファイルの文字コード判別のみ実施し、StringBuilderに結果をタブ区切りで詰め込む
                    CharCode c = reader.Read(f);
                    sb.Append((f.DirectoryName + @"\").Substring(rootDir.Length));
                    sb.Append("\t");
                    sb.Append(f.Name);
                    sb.Append("\t");
                    sb.Append(c.Name);
                    sb.AppendLine();
                }
                foreach (DirectoryInfo d in dir.GetDirectories())
                {   //サブフォルダについて再帰
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
