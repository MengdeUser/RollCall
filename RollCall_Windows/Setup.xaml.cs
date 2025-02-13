using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;

namespace RollCall_Windows
{
    /// <summary>
    /// Setup.xaml 的交互逻辑
    /// </summary>
    public partial class Setup : Window
    {
        readonly string versions = "v1.0.1";
        public Setup()
        {
            InitializeComponent();
            // 创建 WebClient 实例
            using (WebClient client = new WebClient())
            {
                // 设置下载进度事件
                client.DownloadProgressChanged += Client_DownloadProgressChanged;

                // 设置下载完成事件
                client.DownloadFileCompleted += Client_DownloadFileCompleted;

                // 开始下载文件
                string fileUrl = "https://gh-proxy.com/github.com/MengdeUser/RollCall/releases/download/0.0.1/RollCall_Setup.exe"; // 替换为实际文件 URL
                string destinationPath = "RollCall_Setup.exe"; // 替换为本地保存路径
                client.DownloadFileAsync(new Uri(fileUrl), destinationPath);
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // 更新进度条
            InfoText.Content = "下载附带文件中...%";
            DownloadProgressBar.Value = e.ProgressPercentage;
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("下载已取消！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (e.Error != null)
            {
                MessageBox.Show($"下载失败：{e.Error.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Setup_Windows();
            }
        }

        private void Setup_Windows()
        {
            InfoText.Content = "校验当前版本号...%";
            DownloadProgressBar.Value = 0;
            string destinationPath = "RollCall_Setup.exe"; // 替换为本地保存路径
                                                           // 启动外部程序
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = destinationPath,
                    UseShellExecute = false, // 设置为false以允许WaitForExit
                }
            };
            process.Start(); // 启动程序
            process.WaitForExit(); // 等待程序结束
            DownloadProgressBar.Value = 50;
            string filePath = "version.txt";
            // 读取文件内容
            string fileContent = File.ReadAllText(filePath);
            DownloadProgressBar.Value = 80;
            // 比较文件内容与变量内容
            if (fileContent == versions)
            {
                // 删除文件
                File.Delete(destinationPath);
                File.Delete(filePath);
                DownloadProgressBar.Value = 100;
                // 检查注册表中的FirstRun键
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\RollCall"))
                {
                    object firstRun = key.GetValue("FirstRun");

                    if (firstRun == null)
                    {
                        // 如果没有FirstRun键，运行MainWindow
                        new MainWindow().Show();
                        this.Close();
                    }
                    else
                    {
                        // 如果有FirstRun键，运行Window3
                        new Window3().Show();
                        this.Close();
                    }
                }
            }
            else
            {
                DownloadProgressBar.Value = 100;
                this.Visibility = Visibility.Hidden;
                // 获取应用程序启动时的路径
                string appStartupPath = AppDomain.CurrentDomain.BaseDirectory;
                MessageBox.Show($"需运行更新后的程序，打开资源管理器查看路径：\n{appStartupPath}", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                // 关闭当前应用程序
                Application.Current.Shutdown();
            }
        }
    }
}
