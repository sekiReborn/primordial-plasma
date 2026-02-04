# GhostForge 项目结构与使用指南

> 🚀 **快速开始**: 查看 [QUICKSTART.md](QUICKSTART.md) 快速配置并运行！  
> 🐛 **遇到问题?** 查看 [BUG_FIX_REPORT.md](BUG_FIX_REPORT.md) 获取完整的故障排查指南  
> 🔧 **调试指南**: [DEBUGGING_GUIDE.md](DEBUGGING_GUIDE.md) | [DEBUG_STEPS.md](DEBUG_STEPS.md)

## 项目概览

GhostForge 是一个 AI 驱动的动态 UI 生成系统，展示了如何结合 Blazor Server、Semantic Kernel、Roslyn 和 WPF 创建创新的解决方案。

## 解决方案结构

````
GhostForge/
├── src/
│   ├── GhostForge.Console/          # Blazor Server 控制台
│   ├── GhostForge.Core/             # 核心服务类库
│   └── GhostForge.WpfClient/        # WPF 展示端
└── GhostForge.slnx                  # 解决方案文件
````

## 核心组件

### 1. GhostForge.Core - 核心服务

#### `UIService.cs`
AI 驱动的 XAML 生成服务：
- 集成 Semantic Kernel 调用 LLM
- 使用自定义 Prompt Template 生成符合机械教风格的 XAML
- 自动验证生成的 XAML 语法
- 支持 Azure OpenAI 和 OpenAI

**关键方法**：
```csharp
Task<UIGenerationResult> GenerateXamlAsync(string userDescription)
bool ValidateXaml(string xaml, out string? errorMessage)
```

#### `RoslynCompiler.cs`
动态 C# 代码编译服务：
- 使用 Microsoft.CodeAnalysis.CSharp 进行编译
- 编译代码到内存程序集
- 支持创建 ViewModel 实例
- 提供详细的错误诊断

**关键方法**：
```csharp
CompilationResult CompileToAssembly(string csharpCode)
object? CreateInstance(Assembly assembly, string typeName)
```

#### Prompt Template
位于 [Prompts/XamlGenerationPrompt.txt](file:///c:/Users/marti/.gemini/antigravity/playground/primordial-plasma/src/GhostForge.Core/Prompts/XamlGenerationPrompt.txt)：
- 指定机械教配色方案（《命运2》/《战锤40K》风格）
- 确保生成的 XAML 可被 `XamlReader.Parse()` 解析
- 优先使用 HandyControls 控件

### 2. GhostForge.Console - Blazor Server 控制台

#### `UIGeneration.razor`
Web UI 交互界面：
- 自然语言输入框
- 实时调用 `UIService` 生成 XAML
- 显示生成结果和验证状态
- 机械教主题样式（`mechanicus.css`）

#### `Program.cs` 配置
依赖注入配置（已完成）：
```csharp
// Semantic Kernel 配置
var kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.AddOpenAIChatCompletion(modelId, apiKey);

// 注册核心服务
builder.Services.AddSingleton<UIService>();
builder.Services.AddSingleton<RoslynCompiler>();
```

### 3. GhostForge.WpfClient - WPF 展示端

#### `MechanicusTheme.xaml`
完整的机械教风格资源字典：
- 配色方案（深褐黑、铁锈红、黄铜金、羊皮纸白）
- 金属渐变画刷
- 发光效果（DropShadowEffect）
- 按钮/文本框样式

#### `DynamicViewWindow.xaml`
动态 UI 容器：
- 使用 `XamlReader.Load()` 解析生成的 XAML
- 支持 DataContext 绑定
- 机械教风格窗口框架

**核心逻辑**（`DynamicViewWindow.xaml.cs`）：
```csharp
public void LoadDynamicUI(string xaml, object? viewModel = null)
{
    var element = XamlReader.Load(xmlReader) as FrameworkElement;
    element.DataContext = viewModel;
    DynamicContentPresenter.Content = element;
}
```

## 使用流程

### 配置 API 密钥

**方法 1: User Secrets（推荐）**
```bash
cd src/GhostForge.Console
dotnet user-secrets set "SemanticKernel:Endpoint" "YOUR_ENDPOINT"
dotnet user-secrets set "SemanticKernel:ApiKey" "YOUR_API_KEY"
dotnet user-secrets set "SemanticKernel:ModelId" "gpt-4"
```

**方法 2: appsettings.json**
编辑 [appsettings.json](file:///c:/Users/marti/.gemini/antigravity/playground/primordial-plasma/src/GhostForge.Console/appsettings.json) 文件。

### 运行项目

**启动 Blazor 控制台**：
```bash
cd src/GhostForge.Console
dotnet run
```
访问：`https://localhost:5001/ui-generation`

**启动 WPF 客户端**：
```bash
cd src/GhostForge.WpfClient
dotnet run
```

## 端到端演示场景

### 场景 1: 通过 Blazor 生成 XAML

1. 打开 Blazor UI Generation 页面
2. 输入描述：
   > "创建一个登录表单，包含用户名、密码输入框和登录按钮"
3. 点击"生成 XAML"
4. 系统调用 Semantic Kernel，返回符合机械教风格的 XAML
5. XAML 自动验证，显示结果

### 场景 2: WPF 动态加载

1. 运行 WPF 客户端
2. 点击"开启动态UI锻造窗口"
3. 窗口加载演示 XAML（可替换为 Blazor 生成的结果）
4. 查看机械教风格的渲染效果

### 场景 3: Roslyn 动态编译（可扩展）

创建 ViewModel 示例代码：
```csharp
using System.ComponentModel;

namespace DynamicViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username = "";
        private string _password = "";

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
```

使用 `RoslynCompiler` 编译并创建实例：
```csharp
var compiler = new RoslynCompiler();
var result = compiler.CompileToAssembly(csharpCode);

if (result.Success)
{
    var instance = compiler.CreateInstance(
        result.Assembly!, 
        "DynamicViewModels.LoginViewModel"
    );
    dynamicWindow.LoadDynamicUI(generatedXaml, instance);
}
```

## 关键技术点

### Semantic Kernel Prompt 设计
- 明确约束条件（不包含 `<Window>` 根元素）
- 指定配色常量
- 要求输出纯 XAML（无 markdown 包装）

### XAML 验证
- 使用 `XDocument.Parse()` 进行 XML 语法检查
- 检测不允许的根元素
- 提供友好的错误提示

### Roslyn 编译注意事项
- 必须引用必要的程序集（System.Runtime、System.ComponentModel 等）
- 使用 `AssemblyLoadContext.Default.LoadFromStream()` 加载
- 错误诊断包含完整的位置信息

## 扩展建议

1. **双向通信**：WPF 客户端通过 HTTP 或 SignalR 与 Blazor 服务通信
2. **模板库**：预定义常用 UI 模板（登录、仪表板、表单等）
3. **代码生成增强**：Semantic Kernel 同时生成 XAML 和对应的 C# ViewModel
4. **实时预览**：Blazor 页面嵌入 WPF 预览（通过屏幕捕获）
5. **版本管理**：保存历史生成的 XAML/代码，支持回滚

## 故障排查

### UIService 不工作
- 检查 API 密钥配置
- 查看控制台警告信息
- 确认 LLM 端点可访问

### XAML 解析失败
- 检查生成的 XAML 是否包含 `<Window>` 根元素
- 确认 HandyControls 命名空间声明正确

### Roslyn 编译错误
- 检查是否缺少必要的 `using` 语句
- 查看 `CompilationResult.Errors` 获取详细信息

## 项目亮点

✅ 完整的 AI 驱动工作流  
✅ 机械教主题美学（独特的视觉风格）  
✅ 动态代码生成与编译  
✅ WPF 与 Blazor 混合架构  
✅ 可扩展的 Prompt 系统  
✅ 企业级错误处理与验证
