# GhostForge 运行与调试指南

## 📋 前提条件

### 1. 安装必要的扩展
在 VS Code 中安装以下扩展：
- **C# Dev Kit** (Microsoft) - 必需
- **C#** (Microsoft) - 通常随 C# Dev Kit 自动安装

安装命令：
```bash
code --install-extension ms-dotnettools.csdevkit
```

### 2. 安装 .NET 8 SDK
确保已安装 .NET 8 SDK：
```bash
dotnet --version
# 应显示 8.x.x
```

### 3. 配置 API 密钥（重要！）
Blazor Console 需要连接到 LLM 服务，有两种配置方法：

#### 方法 1: User Secrets（推荐，不会泄露到 Git）
```bash
cd src/GhostForge.Console
dotnet user-secrets set "SemanticKernel:Endpoint" "YOUR_ENDPOINT_HERE"
dotnet user-secrets set "SemanticKernel:ApiKey" "YOUR_API_KEY_HERE"
dotnet user-secrets set "SemanticKernel:ModelId" "gpt-4"
```

#### 方法 2: appsettings.json（仅用于测试）
编辑 `src/GhostForge.Console/appsettings.json`：
```json
{
  "SemanticKernel": {
    "Endpoint": "https://your-openai-endpoint.com",
    "ApiKey": "sk-your-api-key",
    "ModelId": "gpt-4"
  }
}
```

---

## 🚀 快速启动

### 方式 1: 使用 VS Code 调试器（推荐）

1. **按 F5** 或点击"运行和调试"面板
2. 选择以下配置之一：
   - **🌐 Blazor Console (Web UI)** - 仅运行 Web 界面
   - **🌐 Blazor Console + Browser** - 运行并自动打开浏览器调试
   - **🖥 WPF Client** - 运行 WPF 桌面应用
   - **🚀 Full Stack (Blazor + WPF)** - 同时运行两个应用

**访问地址**：
- Web UI: https://localhost:5001/ui-generation
- 或: http://localhost:5000/ui-generation

### 方式 2: 使用命令行

#### 启动 Blazor Console
```bash
cd src/GhostForge.Console
dotnet run
# 然后访问 https://localhost:5001/ui-generation
```

#### 启动 WPF Client
```bash
cd src/GhostForge.WpfClient
dotnet run
```

---

## 🐛 调试技巧

### 设置断点
1. 在代码行号左侧点击设置红色断点
2. 按 **F5** 启动调试
3. 程序会在断点处暂停

### 常用调试快捷键
- **F5** - 继续执行
- **F10** - 单步跳过（Step Over）
- **F11** - 单步进入（Step Into）
- **Shift+F11** - 单步跳出（Step Out）
- **Shift+F5** - 停止调试

### 调试 Blazor 页面
如果需要调试 Blazor 前端代码（.razor 文件）：
1. 使用 **🌐 Blazor Console + Browser** 配置
2. 按 **F12** 打开浏览器开发者工具
3. 在浏览器中设置断点

---

## 🧪 测试流程

### 场景 1: 测试 XAML 生成（推荐从这里开始）

1. 启动 **🌐 Blazor Console (Web UI)**
2. 访问 UI Generation 页面
3. 输入测试描述：
   ```
   创建一个登录表单，包含用户名输入框、密码输入框和登录按钮
   ```
4. 点击"⚙ 生成 XAML ⚙"
5. 查看生成的 XAML 代码

### 场景 2: 测试 WPF 动态加载

1. 启动 **🖥 WPF Client**
2. 点击"开启动态UI锻造窗口"按钮
3. 查看渲染效果

### 场景 3: 完整流程（Blazor 生成 → WPF 加载）

1. 启动 **🚀 Full Stack (Blazor + WPF)**
2. 在 Blazor 中生成 XAML
3. 复制生成的 XAML 代码
4. 在 WPF Client 的 `MainWindow.xaml.cs` 中修改示例 XAML
5. 重新运行 WPF 查看效果

**提示**：当前版本需要手动复制粘贴，因为 Blazor ↔ WPF 通信功能尚未实现

---

## ⚠️ 常见问题

### 问题 1: "WARNING: Semantic Kernel 未配置 API 密钥"
**解决方法**：请按照"配置 API 密钥"部分设置 User Secrets 或 appsettings.json

### 问题 2: WPF 无法启动
**可能原因**：
- 检查是否在 Windows 系统上运行（WPF 仅支持 Windows）
- 确认项目目标框架为 `net8.0-windows`

### 问题 3: 调试器无法附加
**解决方法**：
1. 检查是否安装了 **C# Dev Kit** 扩展
2. 重启 VS Code
3. 运行 `dotnet build` 确保项目可以正常编译

### 问题 4: XAML 生成失败
**排查步骤**：
1. 检查控制台日志是否有错误信息
2. 确认 API 密钥有效且有使用额度
3. 检查网络连接是否正常

### 问题 5: "The debug type is not recognized" 警告
**解决方法**：安装 C# Dev Kit 扩展后重启 VS Code

---

## 📁 项目结构提示

```
src/
├── GhostForge.Console/       # Blazor Server 控制台
│   ├── Pages/UIGeneration.razor  ← 主要交互页面
│   └── Program.cs                ← 服务配置
├── GhostForge.Core/          # 核心类库
│   ├── Services/
│   │   ├── UIService.cs      ← AI 生成 XAML
│   │   └── RoslynCompiler.cs ← 动态编译
│   └── Prompts/              ← Prompt 模板
└── GhostForge.WpfClient/     # WPF 客户端
    ├── MainWindow.xaml       ← 主窗口
    └── Views/DynamicViewWindow.xaml ← 动态视图容器
```

---

## 🎯 开发建议

### 添加新功能时
1. 修改 Core 类库中的服务
2. 在 Console 项目中添加 UI 交互
3. 在 WpfClient 中实现视觉呈现

### 调试 Semantic Kernel 相关问题
1. 在 `UIService.GenerateXamlAsync` 方法设置断点
2. 查看 `kernelArguments` 和 `result` 内容
3. 检查生成的 XAML 是否符合预期

### 调试 XAML 解析问题
1. 在 `DynamicViewWindow.LoadDynamicUI` 方法设置断点
2. 查看 `xaml` 字符串内容
3. 检查 `XamlReader.Load` 是否抛出异常

---

## 📖 更多资源

- [README.md](../README.md) - 项目架构详细说明
- [Semantic Kernel 文档](https://learn.microsoft.com/semantic-kernel/)
- [WPF 动态 XAML 加载](https://learn.microsoft.com/dotnet/desktop/wpf/advanced/xamlreader-load-method)
