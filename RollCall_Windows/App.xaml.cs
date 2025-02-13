using System.Diagnostics;
using System.Windows;

namespace RollCall_Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
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
                    new Setup().Show();
                }
                else
                {
                    // 用户选择不关闭正在运行的程序，直接退出当前程序
                    System.Windows.Application.Current.Shutdown();
                    return;
                }
            }
            else
            {
                new Setup().Show();
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
    }
}
