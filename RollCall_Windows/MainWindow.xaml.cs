using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace RollCall_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int page;
        public MainWindow()
        {
            InitializeComponent();
            page = 1;
            Check.Click += CheckBox_Checked;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (Check.IsChecked == true)
            {
                Next.IsEnabled = true;
            }
            else if (Check.IsChecked == false)
            {
                Next.IsEnabled = false;
            }
        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {
            if (page == 2)
            {
                page = 1;
                Next.Content = "下一步→";
                Last.Visibility = Visibility.Collapsed;
                privacy.Visibility = Visibility.Visible;
                privacy1.Visibility = Visibility.Collapsed;
                privacy2.Visibility = Visibility.Collapsed;
            }
            else if (page == 3)
            {
                page = 2;
                Next.Content = "下一步→";
                Last.Visibility = Visibility.Visible;
                privacy.Visibility = Visibility.Collapsed;
                privacy1.Visibility = Visibility.Visible;
                privacy2.Visibility = Visibility.Collapsed;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (page == 1)
            {
                page = 2;
                Next.Content = "下一步→";
                Last.Visibility = Visibility.Visible;
                privacy.Visibility = Visibility.Collapsed;
                privacy1.Visibility = Visibility.Visible;
                privacy2.Visibility = Visibility.Collapsed;
            }
            else if (page == 2)
            {
                page = 3;
                Next.Content = "    完成↵  ";
                Last.Visibility = Visibility.Visible;
                privacy.Visibility = Visibility.Collapsed;
                privacy1.Visibility = Visibility.Collapsed;
                privacy2.Visibility = Visibility.Visible;
            }
            else if (page == 3)
            {
                Last.Visibility = Visibility.Visible;
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\RollCall"))
                {
                    // 如果没有FirstRun键，运行Window3
                    key.SetValue("FirstRun", "true"); // 创建FirstRun键
                    new Window3().Show();
                    this.Close();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();
            window2.Show();
        }
    }
}