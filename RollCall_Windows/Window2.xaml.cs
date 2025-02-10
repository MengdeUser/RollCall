using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Word.Application;
using Application1 = Microsoft.Office.Interop.Excel.Application;

namespace RollCall_Windows
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : System.Windows.Window
    {
        public Window2()
        {
            InitializeComponent();
            string filePath = "name.txt"; // 指定TXT文件路径
            if (File.Exists(filePath)) // 检查文件是否存在
            {
                try
                {
                    // 按行读取文件内容
                    string[] lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
                    foreach (string line in lines)
                    {
                        ListBoxTextLines.Items.Add(line); // 将每行内容添加到ListBox中
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"读取文件时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error); // 显示错误信息
                    ListBoxTextLines.Items.Clear();
                }
            }
            else
            {
                ListBoxTextLines.Items.Clear(); // 提示文件不存在
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string filePath = "name.txt"; // 获取文件路径
            ExportTextToTxt(filePath); // 调用方法导出文本
        }

        // 导出文本到TXT文件
        private void ExportTextToTxt(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8)) // 创建StreamWriter
                {
                    foreach (var item in ListBoxTextLines.Items) // 遍历ListBox中的每一项
                    {
                        writer.WriteLine(item.ToString()); // 写入TXT文件
                    }
                }
                System.Windows.MessageBox.Show("导出成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information); // 提示用户
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"导出失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error); // 显示错误信息
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string userInput = TextBoxInput.Text.Trim(); // 获取用户输入并去除首尾空格
            if (!string.IsNullOrEmpty(userInput)) // 检查输入是否为空
            {
                ListBoxTextLines.Items.Add(userInput); // 将用户输入添加到 ListBox
                TextBoxInput.Clear(); // 清空 TextBox
                TextBoxInput.Focus(); // 将焦点返回到 TextBox
            }
            else
            {
                System.Windows.MessageBox.Show("请输入一些内容！", "提示", MessageBoxButton.OK, MessageBoxImage.Information); // 提示用户输入内容
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (ListBoxTextLines.SelectedItem != null) // 检查是否有选中的项
            {
                ListBoxTextLines.Items.Remove(ListBoxTextLines.SelectedItem); // 从 ListBox 中删除选中的项
                
            }
            else
            {
                System.Windows.MessageBox.Show("请选择要删除的项！", "提示", MessageBoxButton.OK, MessageBoxImage.Information); // 提示用户选择要删除的项
            }
        }

        private void Button_Click_1(object sender, object e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Word文档 (*.doc;*.docx)|*.doc;*.docx|Excel文档 (*.xls;*.xlsx)|*.xls;*.xlsx" // 设置文件过滤器
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) // 如果用户选择了文件
            {
                string filePath = openFileDialog.FileName; // 获取文件路径
                string fileExtension = Path.GetExtension(filePath).ToLower(); // 获取文件扩展名

                // 根据文件扩展名导入内容
                if (fileExtension == ".doc" || fileExtension == ".docx")
                {
                    ImportFromWord(filePath); // 导入Word文档内容
                }
                else if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    ImportFromExcel(filePath); // 导入Excel文档内容
                }
                else
                {
                    System.Windows.MessageBox.Show("不支持的文件类型！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 从Word文档导入内容
        private void ImportFromWord(string filePath)
        {
            Application wordApp = new Application(); // 创建Word应用程序实例
            Document wordDoc = null;

            try
            {
                wordDoc = wordApp.Documents.Open(filePath); // 打开Word文档
                string wordText = wordDoc.Content.Text; // 获取文档内容
                string[] lines = wordText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); // 按行分割文本

                // 将每行文本添加到ListBox中
                ListBoxTextLines.Items.Clear(); // 清空ListBox
                foreach (string line in lines)
                {
                    ListBoxTextLines.Items.Add(line); // 添加到ListBox
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"导入Word文档时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error); // 显示错误信息
            }
            finally
            {
                if (wordDoc != null)
                {
                    wordDoc.Close(false); // 关闭Word文档
                }
                wordApp.Quit(); // 退出Word应用程序
            }
        }

        // 从Excel文档导入内容
        private void ImportFromExcel(string filePath)
        {
            Application1 excelApp = new Application1(); // 创建Excel应用程序实例
            Workbook workbook = null;
            Worksheet worksheet = null;

            try
            {
                workbook = excelApp.Workbooks.Open(filePath); // 打开Excel工作簿
                worksheet = workbook.Sheets[1] as Worksheet; // 获取第一个工作表

                // 读取工作表中的内容
                Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange; // 获取工作表的使用范围
                ListBoxTextLines.Items.Clear(); // 清空ListBox

                for (int row = 1; row <= usedRange.Rows.Count; row++)
                {
                    string cellValue = usedRange.Cells[row, 1].Text; // 获取每行第一列的值
                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        ListBoxTextLines.Items.Add(cellValue); // 添加到ListBox
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"导入Excel文档时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error); // 显示错误信息
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(false); // 关闭工作簿
                }
                excelApp.Quit(); // 退出Excel应用程序
            }
        }
    }
}
