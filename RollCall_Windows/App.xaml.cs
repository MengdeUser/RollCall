using Microsoft.Win32;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;

namespace RollCall_Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private const string GitHubApiUrl = "https://api.github.com/repos/MengdeUser/your-repo-name/releases/latest"; // 替换为你的GitHub仓库的API地址
        private const string CurrentVersion = "1.0.0"; // 替换为当前程序的版本号

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 检查程序是否已经在运行
            if (IsApplicationAlreadyRunning())
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("程序已经在运行。是否要关闭正在运行的程序并重启当前程序？",
                                                          "程序已在运行",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // 获取当前进程的名称
                    string currentProcessName = Process.GetCurrentProcess().ProcessName;

                    // 查找所有同名的进程
                    Process[] processes = Process.GetProcessesByName(currentProcessName);

                    // 关闭所有其他同名进程（除了当前进程）
                    foreach (var process in processes)
                    {
                        if (process.Id != Process.GetCurrentProcess().Id)
                        {
                            process.Kill();
                            process.WaitForExit();
                        }
                    }
                }
                else
                {
                    // 用户选择不关闭正在运行的程序，直接退出当前程序
                    System.Windows.Application.Current.Shutdown();
                    return;
                }
            }

            // 检查注册表中的FirstRun键
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\RollCall"))
            {
                object firstRun = key.GetValue("FirstRun");

                if (firstRun == null)
                {
                    // 如果没有FirstRun键，运行MainWindow
                    key.SetValue("FirstRun", "true"); // 创建FirstRun键
                                                      // 检测最新版本
                    var latestVersion = await GetLatestVersionFromGitHub();
                    if (latestVersion != null)
                    {
                        // 比较版本号
                        var currentVersion = new Version(CurrentVersion);
                        var latestVersionObj = new Version(latestVersion);

                        if (latestVersionObj > currentVersion)
                        {
                            // 提示用户是否更新
                            MessageBoxResult result = MessageBox.Show($"发现新版本 {latestVersion}，当前版本为 {CurrentVersion}。是否更新到最新版本？", "更新提示", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                // 打开下载窗口并开始下载
                                DownloadWindow downloadWindow = new DownloadWindow(latestVersion);
                                downloadWindow.Show();
                            }
                        }
                    }
                    new MainWindow().Show();
                }
                else
                {
                    // 如果有FirstRun键，运行Window3
                    // 检测最新版本
                    var latestVersion = await GetLatestVersionFromGitHub();
                    if (latestVersion != null)
                    {
                        // 比较版本号
                        var currentVersion = new Version(CurrentVersion);
                        var latestVersionObj = new Version(latestVersion);

                        if (latestVersionObj > currentVersion)
                        {
                            // 提示用户是否更新
                            MessageBoxResult result = MessageBox.Show($"发现新版本 {latestVersion}，当前版本为 {CurrentVersion}。是否更新到最新版本？", "更新提示", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                // 打开下载窗口并开始下载
                                DownloadWindow downloadWindow = new DownloadWindow(latestVersion);
                                downloadWindow.Show();
                            }
                        }
                    }
                    new Window3().Show();
                }
            }
        }

        // 检查程序是否已经在运行
        private bool IsApplicationAlreadyRunning()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);

            // 如果有多个同名进程，则认为程序已经在运行
            return processes.Count() > 1;
        }

        // 从GitHub获取最新版本号
        private async Task<string> GetLatestVersionFromGitHub()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(GitHubApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                        return data.tag_name; // 获取最新版本号
                    }
                }
            }
            catch (Exception ex)
            {
                // 异常处理，可以记录日志等
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
