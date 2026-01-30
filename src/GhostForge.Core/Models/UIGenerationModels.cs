namespace GhostForge.Core.Models;

/// <summary>
/// 请求生成 UI 的输入模型
/// </summary>
public class UIGenerationRequest
{
    /// <summary>
    /// 用户对界面的自然语言描述
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// 可选的主题名称（默认为 Mechanicus）
    /// </summary>
    public string ThemeName { get; set; } = "Mechanicus";
    
    /// <summary>
    /// 可选的约束条件（如特定控件类型）
    /// </summary>
    public string? Constraints { get; set; }
}

/// <summary>
/// UI 生成结果
/// </summary>
public class UIGenerationResult
{
    /// <summary>
    /// 生成的 XAML 字符串
    /// </summary>
    public required string XamlCode { get; set; }
    
    /// <summary>
    /// 是否通过验证
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// 验证错误信息（如有）
    /// </summary>
    public string? ValidationError { get; set; }
}
