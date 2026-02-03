# ⚠️ API 配置指南

GhostForge 需要连接到 LLM 服务才能生成 XAML。请选择以下任一方法配置：

---

## 🚀 方法 1：使用配置脚本（推荐）

在项目根目录运行：

```powershell
.\setup-api.ps1
```

按照提示选择你的 LLM 服务提供商并输入相关信息。

---

## 🔧 方法 2：手动配置 User Secrets

### OpenAI 官方 API

```bash
cd src/GhostForge.Console
dotnet user-secrets set "SemanticKernel:ApiKey" "sk-你的密钥"
dotnet user-secrets set "SemanticKernel:ModelId" "gpt-4"
```

### Azure OpenAI

```bash
cd src/GhostForge.Console
dotnet user-secrets set "SemanticKernel:Endpoint" "https://你的资源.openai.azure.com"
dotnet user-secrets set "SemanticKernel:ApiKey" "你的密钥"
dotnet user-secrets set "SemanticKernel:ModelId" "你的部署名称"
```

### 其他兼容 OpenAI 的服务

```bash
cd src/GhostForge.Console
dotnet user-secrets set "SemanticKernel:Endpoint" "https://你的服务地址"
dotnet user-secrets set "SemanticKernel:ApiKey" "你的密钥"
dotnet user-secrets set "SemanticKernel:ModelId" "模型ID"
```

---

## 📝 方法 3：直接修改 appsettings.json（仅用于快速测试）

**警告**：此方法会将密钥明文保存在文件中，不推荐用于生产环境！

编辑 `src/GhostForge.Console/appsettings.json`：

```json
{
  "SemanticKernel": {
    "Endpoint": "你的端点（OpenAI 官方 API 可留空）",
    "ApiKey": "你的密钥",
    "ModelId": "gpt-4"
  }
}
```

---

## ✅ 验证配置

运行以下命令查看已配置的 secrets：

```bash
cd src/GhostForge.Console
dotnet user-secrets list
```

应该能看到类似输出：
```
SemanticKernel:ApiKey = sk-***
SemanticKernel:ModelId = gpt-4
```

---

## 🎯 测试生成功能

1. 按 **F5** 启动调试
2. 选择 **🌐 Blazor Console (Web UI)**
3. 在浏览器中访问 UI Generation 页面
4. 输入测试描述：
   ```
   创建一个登录表单，包含用户名输入框、密码输入框和登录按钮
   ```
5. 点击"⚙ 生成 XAML ⚙"

如果配置正确，应该会生成机械教风格的 XAML 代码！

---

## ❓ 常见问题

### "生成失败" 错误

**可能原因**：
1. API 密钥未配置或错误
2. API 额度不足
3. 网络连接问题
4. 模型 ID 错误

**解决方法**：
1. 运行 `dotnet user-secrets list` 确认配置
2. 检查 Blazor 应用控制台的错误日志
3. 确认 API 密钥有效且有余额

### 没有 OpenAI API Key？

**免费替代方案**：
- 使用 Ollama 本地运行 LLM（需要修改代码）
- 使用国内 AI 服务（如智谱 GLM、通义千问等，需要兼容层）
- 注册 OpenAI 免费试用

---

## 📞 需要帮助？

查看 [DEBUGGING_GUIDE.md](DEBUGGING_GUIDE.md) 获取更多调试信息。
