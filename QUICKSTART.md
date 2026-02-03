# 🚀 快速开始 - 配置你的 LLM

## 第一步：运行配置脚本

在项目根目录打开 PowerShell 终端，运行：

```powershell
.\setup-custom-llm.ps1
```

## 第二步：选择模型并输入 API Key

脚本会提示你选择：

```
1. Qwen2.5-72B (推荐用于复杂 UI 生成)
2. DeepSeek-R1 (推理模型)
```

**推荐选择 Qwen2.5-72B**，它更适合生成 XAML UI 代码。

然后输入你的 API Key。

## 第三步：启动应用

按 **F5** 或在终端运行：

```bash
cd src/GhostForge.Console
dotnet run
```

## 第四步：测试生成功能

1. 浏览器会自动打开 `https://localhost:5001/ui-generation`
2. 在文本框中输入（示例）：
   ```
   创建一个登录表单，包含用户名输入框、密码输入框和登录按钮
   ```
3. 点击 **"⚙ 生成 XAML ⚙"**
4. 等待 3-5 秒，查看生成的机械教风格 XAML 代码！

---

## ✅ 验证配置

运行以下命令查看当前配置：

```bash
cd src/GhostForge.Console
dotnet user-secrets list
```

应该看到：
```
SemanticKernel:ApiKey = your-key-here
SemanticKernel:Endpoint = https://qwen25.szjieruisi.com:9000  (或 deepseek 端点)
SemanticKernel:ModelId = Qwen2.5-72B  (或 deepseek-r1)
```

---

## 🎯 更多测试案例

成功后可以尝试更复杂的描述：

### 示例 1：数据统计面板
```
创建一个数据统计面板，包含三个指标卡片：CPU使用率、内存占用、网络流量，
每个卡片显示百分比进度条
```

### 示例 2：设置面板
```
创建一个设置页面，包含主题切换开关、语言选择下拉框、通知开关，
使用垂直布局
```

### 示例 3：用户信息卡片
```
创建一个用户信息卡片，显示头像（圆形）、用户名、邮箱、
以及编辑和删除按钮
```

---

## ❓ 遇到问题？

### 问题 1："生成失败"
- 检查 API Key 是否正确
- 确认网络能访问对应的端点
- 查看控制台是否有详细错误信息

### 问题 2：生成的 XAML 格式错误
- 不同模型生成质量不同，Qwen2.5-72B 通常效果更好
- 可以尝试调整描述更具体

### 问题 3：启动应用时看到黄色警告
- 说明 API Key 未配置，重新运行 `.\setup-custom-llm.ps1`

---

**就这么简单！现在开始使用 GhostForge 生成你的 UI 吧！** 🎉
