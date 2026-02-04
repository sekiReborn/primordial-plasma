# 🎨 如何查看生成的 XAML 效果

## 问题：生成了 XAML 但看不到效果？

好消息！我为你准备了 **3种方法** 来预览 XAML：

---

## 方法 1: 使用内置的 WPF 预览窗口（最简单）⭐

### 步骤：
1. 在 Blazor UI 生成 XAML 后
2. 点击 **"📋 复制代码"** 按钮
3. 打开 `src/GhostForge.Wpf` 项目
4. 运行 WPF 应用：
   ```powershell
   cd src/GhostForge.Wpf
   dotnet run
   ```
5. XAML 会自动加载并显示

**即将实现**: 一键预览按钮 🔜

---

## 方法 2: 在 Visual Studio 中预览

### 步骤：
1. 复制生成的 XAML 代码
2. 打开 Visual Studio
3. 创建新的 WPF 应用或打开现有项目
4. 在 `MainWindow.xaml` 中粘贴代码
5. 查看设计器预览

**优点**: 可以即时编辑和调整

---

## 方法 3: 使用在线 XAML 预览工具

虽然 XAML 主要用于桌面应用，但可以转换为图片预览：

### XamlPad 替代工具
1. 安装 **Kaxaml** - 免费 XAML 编辑器
2. 下载: http://www.kaxaml.com/
3. 粘贴 XAML 代码
4. 实时预览

---

## 🔮 我正在开发的功能

### 即将推出：一键预览功能

我会在下个版本添加 **"👁 预览 XAML"** 按钮：

```
生成结果
├── 📋 复制代码
├── 💾 下载 XAML
├── 👁 预览 XAML  ← NEW!
└── 🗑 清除结果
```

点击后会：
1. 自动启动 WPF 预览窗口
2. 加载生成的 XAML
3. 实时显示界面效果

---

## 🚀 快速预览（当前最佳方案）

### 使用 GhostForge.Wpf 项目

#### 1. 修改 MainWindow.xaml

打开 `src/GhostForge.Wpf/MainWindow.xaml`，替换 `<Grid>` 内容：

```xml
<Window x:Class="GhostForge.Wpf.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="XAML 预览" Height="600" Width="800">
    
    <!-- 将生成的 XAML 粘贴到这里 -->
    <Grid Background="#1a0f0a">
        <!-- 你的 XAML 内容 -->
    </Grid>
    
</Window>
```

#### 2. 运行预览
```powershell
cd src/GhostForge.Wpf
dotnet run
```

---

## 💡 提示

### 调试 XAML 问题
如果 XAML 无法显示：

1. **检查验证状态**：
   - 看到 "✓ 验证通过" 才可预览
   - "✗ 验证失败" 说明有语法错误

2. **查看错误信息**：
   - 红色警告框会显示具体错误
   - 根据提示修复 XAML

3. **常见问题**：
   - 缺少 xmlns 命名空间
   - 标签未闭合
   - 属性值不正确

---

## 📋 完整工作流程

```
1. 在 Blazor UI 描述界面
   ↓
2. 点击"生成 XAML"
   ↓  
3. 查看生成的代码
   ↓
4. 点击"复制代码"
   ↓
5. 粘贴到 WPF 项目
   ↓
6. 运行 WPF 应用
   ↓
7. 查看实际效果！ 🎉
```

---

## 🔧 我会为你创建自动预览功能

要不要我现在就实现 **一键预览** 功能？

这样你就可以：
- 点击"预览"按钮
- 自动打开预览窗口
- 无需手动复制粘贴

回复 "是" 我立即开发这个功能！
