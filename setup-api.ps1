#!/usr/bin/env pwsh
# GhostForge API 配置助手
# 用于快速配置 Semantic Kernel 所需的 API 密钥

Write-Host "🔧 GhostForge API 配置助手" -ForegroundColor Cyan
Write-Host "=================================" -ForegroundColor Cyan
Write-Host ""

# 切换到正确的目录
$consolePath = Join-Path $PSScriptRoot "src\GhostForge.Console"
Set-Location $consolePath

Write-Host "选择你的 LLM 服务提供商:" -ForegroundColor Yellow
Write-Host "1. OpenAI 官方 API (api.openai.com)"
Write-Host "2. Azure OpenAI"
Write-Host "3. 其他兼容 OpenAI 的服务 (如 OneAPI、国内代理)"
Write-Host ""

$choice = Read-Host "请输入选项 (1-3)"

switch ($choice) {
    "1" {
        Write-Host "`n✅ 配置 OpenAI 官方 API" -ForegroundColor Green
        $apiKey = Read-Host "请输入你的 OpenAI API Key (sk-...)"
        $modelId = Read-Host "模型 ID (默认: gpt-4，可选 gpt-3.5-turbo)"
        
        if ([string]::IsNullOrWhiteSpace($modelId)) {
            $modelId = "gpt-4"
        }
        
        dotnet user-secrets set "SemanticKernel:ApiKey" $apiKey
        dotnet user-secrets set "SemanticKernel:ModelId" $modelId
        # OpenAI 官方不需要 Endpoint
        
        Write-Host "`n✅ 配置完成！" -ForegroundColor Green
        Write-Host "使用模型: $modelId" -ForegroundColor Gray
    }
    
    "2" {
        Write-Host "`n✅ 配置 Azure OpenAI" -ForegroundColor Green
        $endpoint = Read-Host "Azure OpenAI Endpoint (https://xxx.openai.azure.com)"
        $apiKey = Read-Host "API Key"
        $modelId = Read-Host "部署名称 (Deployment Name)"
        
        dotnet user-secrets set "SemanticKernel:Endpoint" $endpoint
        dotnet user-secrets set "SemanticKernel:ApiKey" $apiKey
        dotnet user-secrets set "SemanticKernel:ModelId" $modelId
        
        Write-Host "`n✅ 配置完成！" -ForegroundColor Green
        Write-Host "Endpoint: $endpoint" -ForegroundColor Gray
        Write-Host "部署名称: $modelId" -ForegroundColor Gray
    }
    
    "3" {
        Write-Host "`n✅ 配置第三方服务" -ForegroundColor Green
        $endpoint = Read-Host "服务 Endpoint (例如: https://api.example.com)"
        $apiKey = Read-Host "API Key"
        $modelId = Read-Host "模型 ID (例如: gpt-4)"
        
        dotnet user-secrets set "SemanticKernel:Endpoint" $endpoint
        dotnet user-secrets set "SemanticKernel:ApiKey" $apiKey
        dotnet user-secrets set "SemanticKernel:ModelId" $modelId
        
        Write-Host "`n✅ 配置完成！" -ForegroundColor Green
        Write-Host "Endpoint: $endpoint" -ForegroundColor Gray
        Write-Host "模型: $modelId" -ForegroundColor Gray
    }
    
    default {
        Write-Host "❌ 无效选项" -ForegroundColor Red
        exit 1
    }
}

Write-Host ""
Write-Host "📋 当前配置的 Secrets:" -ForegroundColor Cyan
dotnet user-secrets list

Write-Host ""
Write-Host "🚀 下一步:" -ForegroundColor Yellow
Write-Host "  1. 按 F5 启动调试"
Write-Host "  2. 选择 '🌐 Blazor Console (Web UI)'"
Write-Host "  3. 在浏览器中访问 UI Generation 页面测试"
Write-Host ""
