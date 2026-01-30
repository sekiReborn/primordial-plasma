using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Runtime.Loader;
using GhostForge.Core.Models;

namespace GhostForge.Core.Services;

/// <summary>
/// 使用 Roslyn 动态编译 C# 代码的服务
/// </summary>
public class RoslynCompiler
{
    private readonly List<MetadataReference> _defaultReferences;

    public RoslynCompiler()
    {
        // 初始化默认引用的程序集
        _defaultReferences = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.ComponentModel.INotifyPropertyChanged).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.ObjectModel").Location),
            MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
        };
    }

    /// <summary>
    /// 编译 C# 代码到内存程序集
    /// </summary>
    /// <param name="csharpCode">要编译的 C# 源代码</param>
    /// <param name="additionalReferences">额外的程序集引用</param>
    /// <returns>编译结果</returns>
    public CompilationResult CompileToAssembly(string csharpCode, IEnumerable<MetadataReference>? additionalReferences = null)
    {
        var result = new CompilationResult();
        
        try
        {
            // 解析语法树
            var syntaxTree = CSharpSyntaxTree.ParseText(csharpCode);
            
            // 合并引用
            var references = _defaultReferences.ToList();
            if (additionalReferences != null)
            {
                references.AddRange(additionalReferences);
            }
            
            // 创建编译选项
            var assemblyName = $"DynamicAssembly_{Guid.NewGuid():N}";
            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Debug)
            );
            
            // 编译到内存流
            using var ms = new MemoryStream();
            var emitResult = compilation.Emit(ms);
            
            if (!emitResult.Success)
            {
                // 收集错误和警告
                foreach (var diagnostic in emitResult.Diagnostics)
                {
                    var message = $"{diagnostic.Id}: {diagnostic.GetMessage()} at {diagnostic.Location.GetLineSpan()}";
                    
                    if (diagnostic.Severity == DiagnosticSeverity.Error)
                    {
                        result.Errors.Add(message);
                    }
                    else if (diagnostic.Severity == DiagnosticSeverity.Warning)
                    {
                        result.Warnings.Add(message);
                    }
                }
                
                result.Success = false;
                return result;
            }
            
            // 加载程序集
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
            
            result.Success = true;
            result.Assembly = assembly;
            
            // 添加警告信息
            foreach (var diagnostic in emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning))
            {
                result.Warnings.Add($"{diagnostic.Id}: {diagnostic.GetMessage()}");
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Errors.Add($"编译异常: {ex.Message}");
        }
        
        return result;
    }

    /// <summary>
    /// 从程序集创建类型实例
    /// </summary>
    /// <param name="assembly">编译后的程序集</param>
    /// <param name="typeName">完整类型名称（包含命名空间）</param>
    /// <param name="constructorArgs">构造函数参数</param>
    /// <returns>创建的实例，失败返回 null</returns>
    public object? CreateInstance(Assembly assembly, string typeName, params object[] constructorArgs)
    {
        try
        {
            var type = assembly.GetType(typeName);
            if (type == null)
            {
                // 尝试从所有导出类型中查找
                type = assembly.GetExportedTypes().FirstOrDefault(t => t.Name == typeName || t.FullName == typeName);
            }
            
            if (type == null)
            {
                return null;
            }
            
            return Activator.CreateInstance(type, constructorArgs);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 获取程序集中的所有公共类型
    /// </summary>
    public IEnumerable<Type> GetExportedTypes(Assembly assembly)
    {
        return assembly.GetExportedTypes();
    }
}
