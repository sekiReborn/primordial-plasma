using System.Reflection;

namespace GhostForge.Core.Models;

/// <summary>
/// Roslyn 编译结果
/// </summary>
public class CompilationResult
{
    /// <summary>
    /// 编译是否成功
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// 编译后的程序集（成功时）
    /// </summary>
    public Assembly? Assembly { get; set; }
    
    /// <summary>
    /// 编译错误信息集合
    /// </summary>
    public List<string> Errors { get; set; } = new();
    
    /// <summary>
    /// 编译警告信息集合
    /// </summary>
    public List<string> Warnings { get; set; } = new();
}
