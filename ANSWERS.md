# 📖 问题解答总结

## 你的两个问题

### ❓ 问题 1: 如何设置 Gemini 的 API Key？

**答案**：使用命令行快速设置

```powershell
cd src/GhostForge.Console

# 设置 Gemini API 配置
dotnet user-secrets set "SemanticKernel:Endpoint" "https://generativelanguage.googleapis.com/v1beta"
dotnet user-secrets set "SemanticKernel:ModelId" "gemini-2.0-flash"
dotnet user-secrets set "SemanticKernel:ApiKey" "你的API密钥"
```

**获取 Gemini API Key**:
1. 访问: https://aistudio.google.com/app/apikey
2. 登录 Google 账号
3. 创建并复制 API Key

**详细指南**: `GEMINI_API_SETUP.md`

---

### ❓ 问题 2: 如何查看生成的 XAML 效果？

**答案**：有3种方法，最简单的是使用现有 WPF 项目

#### 方法 1: 快速预览（推荐）⭐

1. **复制 XAML**：在 Blazor UI 点击 "📋 复制代码"

2. **运行 WPF 应用**：
   ```powershell
   cd src/GhostForge.WpfClient
   dotnet run
   ```

3. **使用动态窗口**：
   - 点击 "⚙ 开启动态UI锻造窗口 ⚙"
   - 粘贴 XAML 或输入描述
   - 查看效果

#### 方法 2: Visual Studio XAML 设计器

1. 打开 Visual Studio
2. 创建或打开 WPF 项目
3. 粘贴 XAML 到 `MainWindow.xaml`
4. 查看设计器预览

#### 方法 3: 创建独立预览窗口

创建 `PreviewWindow.xaml` 专门用于预览生成的 XAML

**详细指南**: `QUICK_PREVIEW_GUIDE.md`

---

## 🎯 完整工作流

```
1️⃣  配置 API Key
    ↓
    cd src/GhostForge.Console
    dotnet user-secrets set "SemanticKernel:ApiKey" "你的密钥"

2️⃣  启动 Blazor UI
    ↓
    dotnet run
    访问 http://localhost:5001/ui-generation

3️⃣  生成 XAML
    ↓
    选择模型 (DeepSeek/Qwen/Gemini)
    输入描述或选择模板
    点击生成

4️⃣  预览效果
    ↓
    点击"复制代码"
    运行 WPF 应用
    cd ../GhostForge.WpfClient && dotnet run
    粘贴查看效果 🎉
```

---

## 📚 相关文档

| 文档 | 用途 |
|------|------|
| `GEMINI_API_SETUP.md` | Gemini API Key 设置详解 |
| `QUICK_PREVIEW_GUIDE.md` | XAML 预览完整指南 |
| `HOW_TO_PREVIEW_XAML.md` | 多种预览方法说明 |
| `UPGRADE_V2.1_NOTES.md` | v2.1 版本升级说明 |

---

## 💡 重要提示

### 当前限制
⚠️ **UI 模型选择器只是前端显示**
- 实际使用的模型由 `user-secrets` 配置决定
- 切换模型需要：
  1. 修改 user-secrets 配置
  2. 重启应用

### 即将改进
🔮 **计划中的功能**:
- ✨ UI 模型切换直接生效（无需重启）
- 👁 一键预览按钮
- 📊 模型性能对比
- 💾 历史生成记录

---

## 🚀 立即开始

### 快速测试流程

```powershell
# 1. 设置 Gemini API Key
cd src/GhostForge.Console
dotnet user-secrets set "SemanticKernel:ApiKey" "你的密钥"

# 2. 启动 Blazor UI
dotnet run &

# 3. 在另一个终端启动 WPF 预览
cd ../GhostForge.WpfClient
dotnet run
```

然后：
1. 打开浏览器访问 http://localhost:5001/ui-generation
2. 选择 "📝 登录表单" 模板
3. 点击生成
4. 复制 XAML
5. 在 WPF 应用中粘贴预览

---

## ❓ 还有问题？

常见问题：

**Q: 为什么 UI 切换模型后还是用旧模型？**  
A: UI 模型选择器当前只是前端功能，需要通过 user-secrets 真正切换

**Q: XAML 验证失败怎么办？**  
A: 查看红色错误框的具体信息，通常是缺少命名空间或标签未闭合

**Q: Gemini API 报错？**  
A: 检查 API Key 是否有效，访问 https://aistudio.google.com 确认

**Q: WPF 应用打不开？**  
A: 确保安装了 .NET 8 SDK 和 WPF 工作负载

---

祝你使用愉快！如有其他问题随时告诉我 🎉
