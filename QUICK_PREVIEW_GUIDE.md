# 🎯 快速预览 XAML - 完整指南

## 立即预览生成的 XAML

你已经成功生成了 XAML！现在跟着这个简单流程预览效果：

---

## ⚡ 最快方法（2分钟）

### 步骤 1: 复制 XAML 代码
在 Blazor UI 中点击 **"📋 复制代码"**

### 步骤 2: 创建临时预览文件
创建一个新文件 `XamlPreview.xaml`：
```powershell
cd src/GhostForge.WpfClient/Views
notepad PreviewWindow.xaml
```

### 步骤 3: 粘贴完整预览模板
```xml
<Window x:Class="GhostForge.WpfClient.Views.PreviewWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:hc="https://handyorg.github.io/handycontrol"
        Title="XAML 预览" 
        Height="700" Width="900"
        WindowStartupLocation="CenterScreen"
        Background="#1a0f0a">
    
    <Border BorderBrush="#5a4a3a" BorderThickness="2" Margin="10">
        <ScrollViewer>
            <!-- ⬇️ 将生成的 XAML 粘贴到这里 ⬇️ -->
            <Grid Background="#2a1f1a">
                <!-- 你的 XAML 内容 -->
            </Grid>
            <!-- ⬆️ 粘贴结束 ⬆️ -->
        </ScrollViewer>
    </Border>
</Window>
```

### 步骤 4: 添加后端代码
创建 `PreviewWindow.xaml.cs`：
```powershell
notepad PreviewWindow.xaml.cs
```

粘贴：
```csharp
using System.Windows;

namespace GhostForge.WpfClient.Views
{
    public partial class PreviewWindow : Window
    {
        public PreviewWindow()
        {
            InitializeComponent();
        }
    }
}
```

### 步骤 5: 从主窗口打开预览
编辑 `MainWindow.xaml.cs`，添加：
```csharp
private void PreviewXaml_Click(object sender, RoutedEventArgs e)
{
    var preview = new Views.PreviewWindow();
    preview.Show();
}
```

### 步骤 6: 运行！
```powershell
cd src/GhostForge.WpfClient
dotnet run
```

---

## 🎨 更简单的方法（无需改代码）

### 使用现有的 DynamicView

1. **运行 WPF 应用**：
   ```powershell
   cd src/GhostForge.WpfClient
   dotnet run
   ```

2. **点击** "⚙ 开启动态UI锻造窗口 ⚙"

3. **输入描述** 或 **粘贴 XAML**

4. **点击生成** 查看效果

---

## 📋 完整工作流

```
┌─────────────────────────────────────┐
│  Blazor Web UI (localhost:5001)     │
│  ├─ 输入描述                        │
│  ├─ 点击生成                        │
│  ├─ 查看 XAML 代码                  │
│  └─ 点击"复制代码"                  │
└─────────────────────────────────────┘
            ⬇️  复制
┌─────────────────────────────────────┐
│  WPF 应用 (GhostForge.WpfClient)    │
│  ├─ 创建 PreviewWindow.xaml         │
│  ├─ 粘贴 XAML 内容                  │
│  ├─ 运行 dotnet run                 │
│  └─ 查看实际UI效果！ 🎉             │
└─────────────────────────────────────┘
```

---

## 🔥 自动化预览（即将推出）

我会添加一个 **"👁 预览"** 按钮：

1. 在 Blazor UI 生成 XAML 后
2. 点击 "👁 预览" 按钮
3. 自动：
   - 启动 WPF 预览窗口
   - 加载 XAML
   - 显示效果

需要我现在实现这个功能吗？

---

## 💡 提示

### 如果 XAML 无法显示
1. ✅ 检查 "验证状态" - 必须是 "✓ 验证通过"
2. ✅ 查看红色错误框的具体错误信息
3. ✅ 确保使用了 HandyControl 命名空间（如果需要）
4. ✅ 检查是否有未闭合的标签

### 调试技巧
```powershell
# 在 WPF 项目运行时查看详细错误
cd src/GhostForge.WpfClient
dotnet run --verbosity detailed
```

---

## 📦 需要的 NuGet 包

确保 WPF 项目已安装 HandyControl：
```powershell
cd src/GhostForge.WpfClient
dotnet add package HandyControl
```

---

## 下一步

试试这个流程：
1. 在 Blazor UI 生成一个 "登录表单"
2. 复制 XAML
3. 粘贴到 WPF PreviewWindow
4. 运行查看效果
5. 调整参数重新生成

祝你使用愉快！🚀
