using System.IO;
using System.Windows;

namespace RollCall_Windows
{
    /// <summary>
    /// Window4.xaml 的交互逻辑
    /// </summary>
    public partial class Window4 : Window
    {
        private List<string> names; // 存储名字的列表
        private HashSet<string> selectedNames; // 存储已抽选的名字
        private Random random; // 随机数生成器
        public Window4()
        {
            InitializeComponent();
            LoadNames(); // 加载名字
            random = new Random(); // 初始化随机数生成器
            selectedNames = new HashSet<string>(); // 初始化已抽选名字集合
            // 清空当前抽选结果
            NameLabel.Content = string.Empty;
        }

        // 从文件加载名字
        private void LoadNames()
        {
            try
            {
                // 从文件中按行读取名字
                names = File.ReadAllLines("name.txt").ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载名字时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonStart_Click(object sender, object e)
        {
            if (names.Count == 0)
            {
                MessageBox.Show("没有可抽选的名字！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 抽选名字
            string selectedName = SelectRandomName();
            if (selectedName != null)
            {
                NameLabel.Content = selectedName; // 显示抽中的名字
            }
            else
            {
                MessageBox.Show("所有名字都已抽选完毕！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // 清空已抽选的名字集合，准备下一轮
                selectedNames.Clear();
            }
        }

        // 随机选择一个名字
        private string SelectRandomName()
        {
            // 过滤掉已抽选的名字
            var availableNames = names.Except(selectedNames).ToList();
            if (availableNames.Count == 0) return null; // 如果没有可用名字，返回null

            // 随机选择一个名字
            string selectedName = availableNames[random.Next(availableNames.Count)];
            selectedNames.Add(selectedName); // 将抽选的名字添加到已抽选集合
            return selectedName;
        }

        private void ButtonEnd_Click(object sender, object e)
        {
            // 清空当前抽选结果
            NameLabel.Content = string.Empty;

            // 清空已抽选的名字集合，准备下一轮
            selectedNames.Clear();
        }

        private void Button_Click(object sender, object e)
        {
            Window2 window2 = new Window2();
            window2.Show();
        }
    }
}
