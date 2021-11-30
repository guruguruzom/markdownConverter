using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarkdownConverter
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileAccessor fileAccessor;

        public MainWindow()
        {
            InitializeComponent();
            fileAccessor = FileAccessor.Instance();

            

        }

        public void covert(JsonData datas, int depth)
        {

            if (datas == null)
                return;
            if (datas.GetJsonType() == JsonType.Array) {
                covert(datas[0], depth);
                return;
            }

            try
            {
                foreach (JsonData data in datas.Keys)
                {
                    if (datas[data.ToString()].GetJsonType() == JsonType.Int || datas[data.ToString()].GetJsonType() == JsonType.String)
                    {
                        noneKeyNonC(data, datas, depth);
                    }
                    else
                    {
                        noneKeyNonC(data, datas, depth);
                        //noneKey(data, depth);
                        covert(datas[data.ToString()], depth + 1);
                    }

                }
            }
            catch {
                return;
            }
        }

        public void noneKeyNonC(JsonData data, JsonData origin, int depth)
        {
            string oneline = "| 모델 |";
            for (int i = 0; i < depth; i++)
            {
                oneline += "┕";
            }
            string oper = origin[data.ToString()].GetJsonType().ToString();
            if (oper.Contains("Int")) {
                oper = "Number";
            }
            oneline += data.ToString() + "|" + oper + "||\n";

            result.AppendText(oneline);
            //fileAccessor.WriteJson(oneline);
        }

        public void noneKey(JsonData datas, int depth) {
            string oneline = "| 모델 |";
            for (int i = 0; i < depth; i++)
            {
                oneline += "┕";
            }

            string oper = datas.GetJsonType().ToString();
            if (oper.Contains("Int"))
            {
                oper = "Number";
            }

            oneline += datas.ToString() + "|" + oper + "||\n";

            result.AppendText(oneline);
            //fileAccessor.WriteJson(oneline);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string jsonStr = before.Text;
            //            string jsonStr = fileAccessor.ReadJsonConfig();
            try
            {
                JsonData datas = JsonMapper.ToObject(jsonStr);

                result.Clear();
                //result.AppendText("| 항목 | 타입 | 설명 | 필수 여부 |\n");
                //result.AppendText("| --- | --- | --- | :---: |\n");
                result.AppendText("| 구분 | 항목 | 타입 | 설명 | \n");
                result.AppendText("| --- | --- | --- | --- | \n");
                covert(datas, 0);
            }
            catch {
                MessageBox.Show("형식에 맞지않는 json또는 value값에 null이 포함되어 있습니다.");
            }
        }
    }
}
