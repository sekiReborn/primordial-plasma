using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using GhostForge.Core.Models;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace GhostForge.Core.Services;

/// <summary>
/// 使用 Semantic Kernel 生成 XAML UI 的服务
/// </summary>
public class UIService
{
    private readonly Kernel _kernel;
    private readonly KernelFunction _xamlGenerationFunction;
    private readonly ILogger<UIService>? _logger;

    public UIService(Kernel kernel, ILogger<UIService>? logger = null)
    {
        _kernel = kernel;
        _logger = logger;
        
        _logger?.LogInformation("🔧 UIService 初始化中...");
        
        // 加载 Prompt 模板
        var promptTemplate = LoadPromptTemplate();
        
        // 创建 Kernel Function
        _xamlGenerationFunction = _kernel.CreateFunctionFromPrompt(
            promptTemplate,
            new OpenAIPromptExecutionSettings
            {
                MaxTokens = 2000,
                Temperature = 0.7,
                TopP = 0.9
            }
        );
        
        _logger?.LogInformation("✅ UIService 已就绪");
    }

    /// <summary>
    /// 根据自然语言描述生成 XAML
    /// </summary>
    /// <param name="request">生成请求</param>
    /// <returns>生成结果</returns>
    public async Task<UIGenerationResult> GenerateXamlAsync(UIGenerationRequest request)
    {
        try
        {
            // 调用 Semantic Kernel
            var kernelArguments = new KernelArguments
            {
                ["userDescription"] = request.Description,
                ["constraints"] = request.Constraints ?? "No additional constraints"
            };
            
            var result = await _xamlGenerationFunction.InvokeAsync(_kernel, kernelArguments);
            var xamlCode = result.ToString().Trim();
            
            // 清理可能的 markdown 代码块标记
            xamlCode = CleanXamlOutput(xamlCode);
            
            // 验证 XAML
            var isValid = ValidateXaml(xamlCode, out string? validationError);
            
            return new UIGenerationResult
            {
                XamlCode = xamlCode,
                IsValid = isValid,
                ValidationError = validationError
            };
        }
        catch (Exception ex)
        {
            return new UIGenerationResult
            {
                XamlCode = string.Empty,
                IsValid = false,
                ValidationError = $"生成失败: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// 简化的生成方法，直接接受描述字符串
    /// </summary>
    public async Task<UIGenerationResult> GenerateXamlAsync(string userDescription)
    {
        return await GenerateXamlAsync(new UIGenerationRequest
        {
            Description = userDescription
        });
    }

    /// <summary>
    /// 验证 XAML 是否为有效的 XML
    /// </summary>
    public bool ValidateXaml(string xaml, out string? errorMessage)
    {
        errorMessage = null;
        
        if (string.IsNullOrWhiteSpace(xaml))
        {
            errorMessage = "XAML 代码为空";
            _logger?.LogWarning("[VALIDATION] 失败：XAML 为空");
            return false;
        }
        
        try
        {
            // 尝试解析为 XML
            var doc = XDocument.Parse(xaml);
            
            // 检查根元素是否合法（不应该是 Window）
            var rootName = doc.Root?.Name.LocalName;
            _logger?.LogInformation("[VALIDATION] 根元素: {RootName}", rootName);
            
            if (rootName == "Window")
            {
                errorMessage = "XAML 不应包含 Window 根元素，应使用容器控件如 Grid 或 StackPanel";
                _logger?.LogWarning("[VALIDATION] 失败：包含 Window 根元素");
                return false;
            }
            
            // 检查是否是有效的 UI 元素
            var validRootElements = new[] { "Grid", "StackPanel", "DockPanel", "WrapPanel", "Canvas", "Border", "UserControl", "Page" };
            if (rootName != null && !validRootElements.Contains(rootName) && !rootName.Contains(":"))
            {
                // 警告但不阻止：可能是自定义控件
                _logger?.LogWarning("[VALIDATION] 警告：非标准根元素 '{RootName}'，但继续验证", rootName);
            }
            
            _logger?.LogInformation("✅ [VALIDATION] 成功：XAML 语法有效");
            return true;
        }
        catch (Exception ex)
        {
            // 提取更有用的错误信息
            var message = ex.Message;
            
            // 尝试显示位置信息
            if (ex is System.Xml.XmlException xmlEx)
            {
                message = $"第 {xmlEx.LineNumber} 行, 第 {xmlEx.LinePosition} 列: {xmlEx.Message}";
            }
            
            errorMessage = $"XAML 解析错误: {message}";
            _logger?.LogError("❌ [VALIDATION] 失败：{Message}", message);
            
            // 打印问题 XAML 的一部分用于调试
            if (xaml.Length > 200)
            {
                _logger?.LogDebug("[DEBUG] XAML 内容预览:\n{Preview}", xaml.Substring(0, 200));
            }
            else
            {
                _logger?.LogDebug("[DEBUG] XAML 完整内容:\n{Xaml}", xaml);
            }
            
            return false;
        }
    }

    /// <summary>
    /// 清理 AI 输出中可能包含的 markdown 标记
    /// </summary>
    private string CleanXamlOutput(string xaml)
    {
        if (string.IsNullOrWhiteSpace(xaml))
            return string.Empty;
            
        // 移除可能的 markdown 代码块（支持多种变体）
        xaml = xaml.Replace("```xaml", "")
                   .Replace("```xml", "")
                   .Replace("```XAML", "")
                   .Replace("```XML", "")
                   .Replace("```", "");
        
        // 移除可能的 HTML 转义字符
        xaml = xaml.Replace("&lt;", "<")
                   .Replace("&gt;", ">")
                   .Replace("&amp;", "&")
                   .Replace("&quot;", "\"");
        
        // 尝试提取 XML 内容（如果有额外的文本）
        var firstTagIndex = xaml.IndexOf('<');
        var lastTagIndex = xaml.LastIndexOf('>');
        
        if (firstTagIndex >= 0 && lastTagIndex > firstTagIndex)
        {
            xaml = xaml.Substring(firstTagIndex, lastTagIndex - firstTagIndex + 1);
        }
        
        // 移除前后空白
        xaml = xaml.Trim();
        
        // 调试日志（开发时可以看到生成的内容）
        _logger?.LogInformation("📝 [DEBUG] 清理后的 XAML 长度: {Length} 字符", xaml.Length);
        if (xaml.Length > 0)
        {
            var preview = xaml.Substring(0, Math.Min(100, xaml.Length));
            _logger?.LogInformation("[DEBUG] XAML 开头: {Preview}...", preview);
        }
        
        return xaml;
    }

    /// <summary>
    /// 加载 Prompt 模板文件
    /// </summary>
    private string LoadPromptTemplate()
    {
        // 尝试从嵌入资源或文件系统加载
        var promptPath = Path.Combine(AppContext.BaseDirectory, "Prompts", "XamlGenerationPrompt.txt");
        
        if (File.Exists(promptPath))
        {
            return File.ReadAllText(promptPath);
        }
        
        // 如果文件不存在，使用内联的默认模板
        return GetDefaultPromptTemplate();
    }

    /// <summary>
    /// 获取默认的 Prompt 模板
    /// </summary>
    private string GetDefaultPromptTemplate()
    {
        return @"You are a WPF XAML expert specializing in the Adeptus Mechanicus aesthetic from Warhammer 40K.

## Task
Generate a valid WPF XAML snippet based on the user's description. The XAML must:
1. Use HandyControls library controls where possible (namespace: hc=""https://handyorg.github.io/handycontrol"")
2. Apply the Mechanicus dark theme color palette
3. Be parseable by XamlReader.Parse() without errors
4. Follow WPF best practices for layout and data binding

## Color Palette (MUST USE)
- Background: #1a0f0a (dark brown-black)
- Secondary Background: #2a1f1a (lighter brown)
- Accent: #8b2e0b (rust red)
- Highlight: #d4af37 (brass gold)
- Text: #e8d4b0 (parchment white)
- Text Secondary: #9a8a7a (muted parchment)
- Border: #5a4a3a (dark bronze)

## Style Guidelines
- Use semi-transparent overlays (#cc1a0f0a for 80% opacity backgrounds)
- Add subtle glow effects using DropShadowEffect with brass gold color
- Use metallic gradients for backgrounds and borders
- Apply geometric patterns (hexagons, gears) when appropriate
- Include hover effects for interactive elements

## Constraints
- DO NOT include <Window> root element - start with a container like <Grid>, <StackPanel>, or <DockPanel>
- DO include xmlns declarations if using HandyControls: xmlns:hc=""https://handyorg.github.io/handycontrol""
- DO use data binding syntax {Binding PropertyName} for dynamic content
- DO NOT use x:Class or code-behind references
- x:Name is allowed for element references within the XAML

## User Request
{{$userDescription}}

## Additional Context
{{$constraints}}

## Output Format
Return ONLY the XAML code without markdown code blocks, explanations, or additional text. The output must be ready to parse directly.";
    }
}
