#!/usr/bin/env pwsh
# GhostForge 自定义 LLM 配置助手

Write-Host "🔧 GhostForge LLM 配置助手" -ForegroundColor Cyan
Write-Host "=================================" -ForegroundColor Cyan
Write-Host ""

# 切换到正确的目录
$consolePath = Join-Path $PSScriptRoot "src\GhostForge.Console"
Set-Location $consolePath

Write-Host "选择要使用的 LLM 模型:" -ForegroundColor Yellow
Write-Host "1. Qwen2.5-72B (推荐用于复杂 UI 生成)"
Write-Host "2. DeepSeek-R1 (推理模型)"
Write-Host ""

$choice = Read-Host "请输入选项 (1-2)"

switch ($choice) {
    "1" {
        Write-Host "`n✅ 配置 Qwen2.5-72B" -ForegroundColor Green
        $endpoint = "https://qwen25.szjieruisi.com:9000"
        $modelId = "Qwen2.5-72B"
        
        Write-Host "端点: $endpoint/v1" -ForegroundColor Gray
        Write-Host "模型: $modelId" -ForegroundColor Gray
        Write-Host ""
        
        $apiKey = Read-Host "sk-75RWj585jKIa364rWvwM5g59sxIm1FBDYZlQNmKAEhOAIYsO"
        
        dotnet user-secrets set "SemanticKernel:Endpoint" $endpoint
        dotnet user-secrets set "SemanticKernel:ApiKey" $apiKey
        dotnet user-secrets set "SemanticKernel:ModelId" $modelId
        
        Write-Host "`n✅ 配置完成！" -ForegroundColor Green
    }
    
    "2" {
        Write-Host "`n✅ 配置 DeepSeek-R1" -ForegroundColor Green
        $endpoint = "https://deepseekr1.szjieruisi.com:9000"
        $modelId = "deepseek-r1"
        
        Write-Host "端点: $endpoint/v1" -ForegroundColor Gray
        Write-Host "模型: $modelId" -ForegroundColor Gray
        Write-Host ""
        
        $apiKey = Read-Host "请输入你的 API Key"
        
        dotnet user-secrets set "SemanticKernel:Endpoint" $endpoint
        dotnet user-secrets set "SemanticKernel:ApiKey" $apiKey
        dotnet user-secrets set "SemanticKernel:ModelId" $modelId
        
        Write-Host "`n✅ 配置完成！" -ForegroundColor Green
    }
    
    default {
        Write-Host "❌ 无效选项" -ForegroundColor Red
        exit 1
    }
}

Write-Host ""
Write-Host "📋 当前配置的 User Secrets:" -ForegroundColor Cyan
dotnet user-secrets list

Write-Host ""
Write-Host "🚀 下一步:" -ForegroundColor Yellow
Write-Host "  1. 按 F5 启动调试"
Write-Host "  2. 选择 '🌐 Blazor Console (Web UI)'"
Write-Host "  3. 在 UI Generation 页面输入描述测试生成"
Write-Host ""
Write-Host "💡 测试建议:" -ForegroundColor Cyan
Write-Host "  输入: 创建一个登录表单，包含用户名、密码输入框和登录按钮"
Write-Host ""
