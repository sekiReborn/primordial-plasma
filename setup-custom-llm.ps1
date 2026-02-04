#!/usr/bin/env pwsh
# GhostForge 自定义 LLM 配置助手

Write-Host "🔧 GhostForge LLM 配置助手" -ForegroundColor Cyan
Write-Host "=================================" -ForegroundColor Cyan
Write-Host ""

# 切换到正确的目录
$consolePath = Join-Path $PSScriptRoot "src\GhostForge.Console"
Set-Location $consolePath

Write-Host "选择要使用的 LLM 模型:" -ForegroundColor Yellow
Write-Host "1. Qwen2.5-72B (内部部署 - 推荐用于复杂 UI 生成)"
Write-Host "2. DeepSeek-R1 (内部部署 - 推理模型)"
Write-Host "3. Gemini 2.0 Flash (Google AI - 快速高效)"
Write-Host "4. OpenAI GPT-4"
Write-Host "5. 其他自定义服务"
Write-Host ""

$choice = Read-Host "请输入选项 (1-5)"

$endpoint = ""
$modelId = ""
$apiKey = ""

switch ($choice) {
    "1" {
        Write-Host "`n✅ 配置 Qwen2.5-72B" -ForegroundColor Green
        $endpoint = "https://qwen25.szjieruisi.com:9000/v1"
        $modelId = "Qwen2.5-72B"
        
        Write-Host "端点: $endpoint" -ForegroundColor Gray
        Write-Host "模型: $modelId" -ForegroundColor Gray
        Write-Host ""
        
        $apiKey = Read-Host "请输入你的 API Key"
    }
    
    "2" {
        Write-Host "`n✅ 配置 DeepSeek-R1" -ForegroundColor Green
        $endpoint = "https://deepseekr1.szjieruisi.com:9000/v1"
        $modelId = "deepseek-r1"
        
        Write-Host "端点: $endpoint" -ForegroundColor Gray
        Write-Host "模型: $modelId" -ForegroundColor Gray
        Write-Host ""
        
        $apiKey = Read-Host "请输入你的 API Key"
    }
    
    "3" {
        Write-Host "`n✅ 配置 Gemini 2.0 Flash (Google AI)" -ForegroundColor Green
        $endpoint = "https://generativelanguage.googleapis.com/v1beta"
        $modelId = "gemini-2.0-flash"
        
        Write-Host "端点: $endpoint" -ForegroundColor Gray
        Write-Host "模型: $modelId" -ForegroundColor Gray
        Write-Host ""
        Write-Host "💡 获取 API Key: https://aistudio.google.com/app/apikey" -ForegroundColor Cyan
        Write-Host ""
        
        $apiKey = Read-Host "请输入你的 Google AI API Key"
    }
    
    "4" {
        Write-Host "`n✅ 配置 OpenAI GPT-4" -ForegroundColor Green
        $modelId = "gpt-4"
        # OpenAI官方API不需要自定义端点
        
        Write-Host "使用OpenAI官方API" -ForegroundColor Gray
        Write-Host "模型: $modelId" -ForegroundColor Gray
        Write-Host ""
        
        $apiKey = Read-Host "请输入你的 OpenAI API Key"
    }
    
    "5" {
        Write-Host "`n✅ 配置自定义服务" -ForegroundColor Green
        $endpoint = Read-Host "请输入 API 端点 (例如: https://api.example.com/v1)"
        $modelId = Read-Host "请输入模型ID (例如: gpt-4, qwen2.5-72b)"
        $apiKey = Read-Host "请输入你的 API Key"
    }
    
    default {
        Write-Host "❌ 无效选项" -ForegroundColor Red
        exit 1
    }
}

if ([string]::IsNullOrWhiteSpace($apiKey)) {
    Write-Host "❌ API Key 不能为空" -ForegroundColor Red
    exit 1
}

# 保存配置
Write-Host "`n💾 保存配置到 User Secrets..." -ForegroundColor Cyan

if (-not [string]::IsNullOrWhiteSpace($endpoint)) {
    dotnet user-secrets set "SemanticKernel:Endpoint" $endpoint
}
dotnet user-secrets set "SemanticKernel:ApiKey" $apiKey
dotnet user-secrets set "SemanticKernel:ModelId" $modelId

Write-Host "✅ 配置已保存！" -ForegroundColor Green

# 测试API连接
Write-Host "`n🧪 测试 API 连接..." -ForegroundColor Cyan

if ([string]::IsNullOrWhiteSpace($endpoint)) {
    # OpenAI官方API
    $testEndpoint = "https://api.openai.com/v1/chat/completions"
}
else {
    $testEndpoint = "$endpoint/chat/completions"
}

$headers = @{
    "Authorization" = "Bearer $apiKey"
    "Content-Type"  = "application/json"
}

$testBody = @{
    model      = $modelId
    messages   = @(
        @{
            role    = "user"
            content = "测试连接"
        }
    )
    max_tokens = 5
} | ConvertTo-Json

try {
    Write-Host "发送测试请求到: $testEndpoint" -ForegroundColor Gray
    $response = Invoke-RestMethod -Uri $testEndpoint -Method Post -Headers $headers -Body $testBody -TimeoutSec 10
    Write-Host "✅ API 连接成功！" -ForegroundColor Green
    Write-Host ""
}
catch {
    Write-Host "❌ API 连接失败" -ForegroundColor Red
    Write-Host "状态码: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    Write-Host "错误信息: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "⚠️  配置已保存，但API连接失败。请检查:" -ForegroundColor Yellow
    Write-Host "  - API Key 是否正确" -ForegroundColor Gray
    Write-Host "  - API Key 是否已过期" -ForegroundColor Gray
    Write-Host "  - 网络连接是否正常" -ForegroundColor Gray
    Write-Host "  - 端点地址是否正确" -ForegroundColor Gray
    Write-Host ""
}

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
