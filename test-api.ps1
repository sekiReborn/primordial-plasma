#!/usr/bin/env pwsh
# API 密钥快速测试工具

param(
    [string]$ApiKey,
    [string]$Endpoint,
    [string]$ModelId
)

Write-Host "🧪 API 连接测试工具" -ForegroundColor Cyan
Write-Host "===================" -ForegroundColor Cyan
Write-Host ""

# 如果未提供参数，尝试从 user secrets 读取
if ([string]::IsNullOrWhiteSpace($ApiKey)) {
    Write-Host "📋 从 User Secrets 读取配置..." -ForegroundColor Gray
    
    $consolePath = Join-Path $PSScriptRoot "src\GhostForge.Console"
    Push-Location $consolePath
    
    $secrets = dotnet user-secrets list | Out-String
    
    if ($secrets -match "SemanticKernel:ApiKey = (.+)") {
        $ApiKey = $Matches[1]
    }
    
    if ($secrets -match "SemanticKernel:Endpoint = (.+)") {
        $Endpoint = $Matches[1]
    }
    
    if ($secrets -match "SemanticKernel:ModelId = (.+)") {
        $ModelId = $Matches[1]
    }
    
    Pop-Location
}

# 验证必要参数
if ([string]::IsNullOrWhiteSpace($ApiKey)) {
    Write-Host "❌ 错误: 未找到 API Key" -ForegroundColor Red
    Write-Host ""
    Write-Host "用法:" -ForegroundColor Yellow
    Write-Host "  .\test-api.ps1 -ApiKey 'your-key' -Endpoint 'https://api.example.com/v1' -ModelId 'gpt-4'"
    Write-Host ""
    Write-Host "或者先配置 User Secrets:" -ForegroundColor Yellow
    Write-Host "  .\setup-custom-llm.ps1"
    exit 1
}

if ([string]::IsNullOrWhiteSpace($ModelId)) {
    $ModelId = "gpt-4"
    Write-Host "⚠️  未指定模型，使用默认: $ModelId" -ForegroundColor Yellow
}

# 构建测试端点
if ([string]::IsNullOrWhiteSpace($Endpoint)) {
    $testUrl = "https://api.openai.com/v1/chat/completions"
    Write-Host "📍 使用 OpenAI 官方 API" -ForegroundColor Cyan
}
else {
    $testUrl = "$Endpoint/chat/completions"
    Write-Host "📍 使用自定义端点: $Endpoint" -ForegroundColor Cyan
}

Write-Host "🎯 模型: $ModelId" -ForegroundColor Cyan
Write-Host "🔑 API Key: $($ApiKey.Substring(0, [Math]::Min(20, $ApiKey.Length)))..." -ForegroundColor Cyan
Write-Host ""

# 准备请求
$headers = @{
    "Authorization" = "Bearer $ApiKey"
    "Content-Type"  = "application/json"
}

$body = @{
    model      = $ModelId
    messages   = @(
        @{
            role    = "user"
            content = "请回复'连接成功'"
        }
    )
    max_tokens = 10
} | ConvertTo-Json

# 执行测试
Write-Host "⏳ 发送测试请求到: $testUrl" -ForegroundColor Gray
Write-Host ""

try {
    $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
    $response = Invoke-RestMethod -Uri $testUrl -Method Post -Headers $headers -Body $body -TimeoutSec 30
    $stopwatch.Stop()
    
    Write-Host "✅ API 连接成功！" -ForegroundColor Green
    Write-Host ""
    Write-Host "📊 响应信息:" -ForegroundColor Cyan
    Write-Host "  耗时: $($stopwatch.ElapsedMilliseconds) ms" -ForegroundColor Gray
    Write-Host "  模型: $($response.model)" -ForegroundColor Gray
    
    if ($response.choices -and $response.choices.Count -gt 0) {
        $content = $response.choices[0].message.content
        Write-Host "  响应: $content" -ForegroundColor Gray
    }
    
    Write-Host ""
    Write-Host "🎉 配置正确，可以开始使用 GhostForge！" -ForegroundColor Green
    Write-Host ""
    
    exit 0
    
}
catch {
    Write-Host "❌ API 连接失败" -ForegroundColor Red
    Write-Host ""
    
    $statusCode = "未知"
    if ($_.Exception.Response) {
        $statusCode = $_.Exception.Response.StatusCode.value__
    }
    
    Write-Host "📋 错误详情:" -ForegroundColor Yellow
    Write-Host "  HTTP 状态码: $statusCode" -ForegroundColor Gray
    Write-Host "  错误消息: $($_.Exception.Message)" -ForegroundColor Gray
    Write-Host ""
    
    # 根据状态码提供建议
    switch ($statusCode) {
        401 {
            Write-Host "💡 401 Unauthorized - 建议检查:" -ForegroundColor Yellow
            Write-Host "  ✓ API Key 是否正确" -ForegroundColor Gray
            Write-Host "  ✓ API Key 是否已过期或被禁用" -ForegroundColor Gray
            Write-Host "  ✓ API Key 的格式是否正确（通常以 'sk-' 开头）" -ForegroundColor Gray
        }
        403 {
            Write-Host "💡 403 Forbidden - 建议检查:" -ForegroundColor Yellow
            Write-Host "  ✓ 账户余额是否充足" -ForegroundColor Gray
            Write-Host "  ✓ API Key 是否有访问该模型的权限" -ForegroundColor Gray
        }
        404 {
            Write-Host "💡 404 Not Found - 建议检查:" -ForegroundColor Yellow
            Write-Host "  ✓ 端点地址是否正确" -ForegroundColor Gray
            Write-Host "  ✓ 端点是否包含 '/v1' 后缀" -ForegroundColor Gray
            Write-Host "  ✓ 模型ID是否正确" -ForegroundColor Gray
        }
        429 {
            Write-Host "💡 429 Too Many Requests - 建议:" -ForegroundColor Yellow
            Write-Host "  ✓ 请求频率过高，请稍后再试" -ForegroundColor Gray
        }
        500 {
            Write-Host "💡 500 Internal Server Error - 建议:" -ForegroundColor Yellow
            Write-Host "  ✓ 服务器端错误，请稍后再试" -ForegroundColor Gray
        }
        default {
            Write-Host "💡 常见问题排查:" -ForegroundColor Yellow
            Write-Host "  ✓ 网络连接是否正常" -ForegroundColor Gray
            Write-Host "  ✓ 防火墙是否阻止了连接" -ForegroundColor Gray
            Write-Host "  ✓ 端点地址是否可访问" -ForegroundColor Gray
        }
    }
    
    Write-Host ""
    Write-Host "🔧 修复建议:" -ForegroundColor Cyan
    Write-Host "  1. 运行配置脚本重新配置: .\setup-custom-llm.ps1" -ForegroundColor Gray
    Write-Host "  2. 检查 API 服务商的控制台确认密钥状态" -ForegroundColor Gray
    Write-Host "  3. 如需帮助，请查看: FIX_API_KEY.md" -ForegroundColor Gray
    Write-Host ""
    
    exit 1
}
