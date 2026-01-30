using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace GhostForge.WpfClient.Views;

/// <summary>
/// 动态视图窗口 - 用于加载和显示 AI 生成的 XAML
/// </summary>
public partial class DynamicViewWindow : Window
{
    public DynamicViewWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 加载动态 XAML 并绑定 ViewModel
    /// </summary>
    /// <param name="xaml">XAML 字符串</param>
    /// <param name="viewModel">可选的 ViewModel 实例</param>
    public void LoadDynamicUI(string xaml, object? viewModel = null)
    {
        try
        {
            ShowLoading(true);
            
            // 使用 XamlReader 解析 XAML
            using var stringReader = new StringReader(xaml);
            using var xmlReader = System.Xml.XmlReader.Create(stringReader);
            
            var element = XamlReader.Load(xmlReader) as FrameworkElement;
            
            if (element == null)
            {
                ShowError("无法解析 XAML：根元素不是 FrameworkElement");
                return;
            }
            
            // 绑定 DataContext
            if (viewModel != null)
            {
                element.DataContext = viewModel;
            }
            
            // 设置到容器
            DynamicContentPresenter.Content = element;
            
            ShowLoading(false);
        }
        catch (Exception ex)
        {
            ShowError($"加载 XAML 失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 显示/隐开加载提示
    /// </summary>
    private void ShowLoading(bool show)
    {
        LoadingOverlay.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// 显示错误信息
    /// </summary>
    private void ShowError(string message)
    {
        ShowLoading(false);
        MessageBox.Show(message, "GhostForge Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    // 窗口拖动
    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            WindowState = WindowState == WindowState.Maximized 
                ? WindowState.Normal 
                : WindowState.Maximized;
        }
        else
        {
            DragMove();
        }
    }

    // 最小化
    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    // 关闭
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
