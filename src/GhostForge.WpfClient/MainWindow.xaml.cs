using System.Windows;
using GhostForge.WpfClient.Views;

namespace GhostForge.WpfClient;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenDynamicView_Click(object sender, RoutedEventArgs e)
    {
        // 创建演示 XAML - 使用完整的 Brush 语法
        var demoXaml = @"
<Grid xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
    <Grid.RowDefinitions>
        <RowDefinition Height=""Auto""/>
        <RowDefinition Height=""*""/>
    </Grid.RowDefinitions>
    
    <TextBlock Grid.Row=""0""
               Text=""⚙ 演示界面 ⚙""
               FontSize=""24""
               FontWeight=""Bold""
               HorizontalAlignment=""Center""
               Margin=""0,20,0,20"">
        <TextBlock.Foreground>
            <SolidColorBrush Color=""#d4af37""/>
        </TextBlock.Foreground>
    </TextBlock>
    
    <Border Grid.Row=""1""
            BorderThickness=""2""
            Margin=""20""
            Padding=""20"">
        <Border.Background>
            <SolidColorBrush Color=""#2a1f1a""/>
        </Border.Background>
        <Border.BorderBrush>
            <SolidColorBrush Color=""#d4af37""/>
        </Border.BorderBrush>
        <StackPanel>
            <TextBlock Text=""欢迎使用 GhostForge""
                       FontSize=""18""
                       Margin=""0,0,0,15"">
                <TextBlock.Foreground>
                    <SolidColorBrush Color=""#e8d4b0""/>
                </TextBlock.Foreground>
            </TextBlock>
            <TextBlock Text=""此界面由 AI 动态生成并通过 XamlReader.Load() 加载""
                       FontSize=""14""
                       TextWrapping=""Wrap"">
                <TextBlock.Foreground>
                    <SolidColorBrush Color=""#9a8a7a""/>
                </TextBlock.Foreground>
            </TextBlock>
        </StackPanel>
    </Border>
</Grid>";

        var dynamicWindow = new DynamicViewWindow();
        dynamicWindow.LoadDynamicUI(demoXaml);
        dynamicWindow.Show();
    }
}