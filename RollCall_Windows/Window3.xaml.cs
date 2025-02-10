using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace RollCall_Windows
{
    /// <summary>
    /// Window3.xaml 的交互逻辑
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
            // 获取屏幕工作区域的高度
            double workingAreaHeight = SystemParameters.WorkArea.Height;
            // 设置窗口位置到屏幕左下角（任务栏上方）
            // 左边界：0
            // 上边界：屏幕工作区域的高度减去窗口的高度
            this.Left = 0;
            this.Top = workingAreaHeight - this.Height;
            // 确保窗口不会超出屏幕范围
            if (this.Top < 0)
            {
                this.Top = 0;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window4 window4 = new Window4();
            window4.Show();
        }

        // 打开程序的点击事件
        private void OpenProgram_Click(object sender, RoutedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.Visibility = Visibility.Collapsed;
            }
            else if (this.Visibility == Visibility.Collapsed)
            {
                this.Visibility = Visibility.Visible;
            }
        }

        // 退出的点击事件
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();
            window2.Show();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://www.example.com"; // 替换为你的目标链接
            try
            {
                // 使用默认浏览器打开链接
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                // 如果打开链接失败，显示错误信息
                MessageBox.Show($"无法打开链接：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
