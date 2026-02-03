# 🐛 已知问题和解决方案

## 问题：自定义端点可能不工作

### 症状
启动应用后，可能会遇到以下错误之一：
1. `AddOpenAIChatCompletion` 方法不接受 `httpClient` 参数
2. 连接超时或无法访问端点
3. 其他 Semantic Kernel 相关错误

### 根本原因
Semantic Kernel 1.70.0 对于自定义 OpenAI 兼容端点的支持可能需要不同的配置方式。

---

## ✅ 解决方案

### 方案 1：端点路径修正（推荐先尝试）

确保端点配置包含完整的 API 路径。修改配置：

```bash
cd src/GhostForge.Console

# 对于 Qwen2.5-72B，确保包含 /v1
dotnet user-secrets set "SemanticKernel:Endpoint" "https://qwen25.szjieruisi.com:9000/v1"

# 对于 DeepSeek-R1
dotnet user-secrets set "SemanticKernel:Endpoint" "https://deepseekr1.szjieruisi.com:9000/v1"
```

### 方案 2：使用代理转换

如果方案 1 不工作，可以通过环境变量设置 HTTP 代理：

```bash
# Windows PowerShell
$env:HTTPS_PROXY = "https://qwen25.szjieruisi.com:9000"
dotnet run
```

### 方案 3：直接修改代码使用官方 API

临时测试可以使用 OpenAI 官方 API：

```bash
cd src/GhostForge.Console
dotnet user-secrets remove "SemanticKernel:Endpoint"
dotnet user-secrets set "SemanticKernel:ApiKey" "你的OpenAI密钥"
dotnet user-secrets set "SemanticKernel:ModelId" "gpt-3.5-turbo"
```

---

## 🔍 诊断步骤

### 1. 查看启动日志

运行应用时注意控制台输出：

- **成功配置**：应该看到绿色的 `✅ 已配置自定义 LLM 服务` 消息
- **配置失败**：会看到黄色警告

### 2. 测试网络连接

在 PowerShell 中测试端点可达性：

```powershell
# 测试 Qwen 端点
Invoke-WebRequest -Uri "https://qwen25.szjieruisi.com:9000/v1/models" -Method GET -Headers @{"Authorization"="Bearer sk-你的密钥"}

# 测试 DeepSeek 端点  
Invoke-WebRequest -Uri "https://deepseekr1.szjieruisi.com:9000/v1/models" -Method GET -Headers @{"Authorization"="Bearer sk-你的密钥"}
```

### 3. 检查 Semantic Kernel 版本

确认使用的版本：

```bash
cd src/GhostForge.Core
dotnet list package | findstr SemanticKernel
```

应该显示：`Microsoft.SemanticKernel  1.70.0`

---

## 📝 如果错误仍然存在

请提供以下信息：

1. **完整的错误消息**（从控制台或调试器）
2. **启动日志**（特别是配置部分）
3. **网络测试结果**

我会根据具体错误提供更精确的解决方案。

---

## 🎯 预期行为

正常启动后，控制台应该显示：

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
✅ 已配置自定义 LLM 服务:
   端点: https://qwen25.szjieruisi.com:9000
   模型: Qwen2.5-72B
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

浏览器打开后，在 UI Generation 页面输入描述，应该能成功生成 XAML。
