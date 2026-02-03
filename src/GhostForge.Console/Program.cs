using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using GhostForge.Console.Data;
using GhostForge.Core.Services;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// 配置 Semantic Kernel
var kernelBuilder = Kernel.CreateBuilder();

// 从配置中读取 API 设置
var endpoint = builder.Configuration["SemanticKernel:Endpoint"];
var apiKey = builder.Configuration["SemanticKernel:ApiKey"];
var modelId = builder.Configuration["SemanticKernel:ModelId"] ?? "gpt-4";

if (!string.IsNullOrEmpty(apiKey))
{
    if (!string.IsNullOrEmpty(endpoint))
    {
        // 自定义端点（包括 Azure OpenAI、Qwen、DeepSeek 等）
        if (endpoint.Contains("azure", StringComparison.OrdinalIgnoreCase))
        {
            // Azure OpenAI
            kernelBuilder.AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);
            Console.WriteLine($"✅ 已配置 Azure OpenAI: {endpoint}");
        }
        else
        {
            // 自定义 OpenAI 兼容服务（Qwen、DeepSeek、OneAPI 等）
            // Semantic Kernel 1.70+ 支持通过自定义 HttpClient 配置端点
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(endpoint)
            };
            
            // 注意：对于自定义端点，需要确保端点包含完整路径（如 /v1）
            kernelBuilder.AddOpenAIChatCompletion(
                modelId: modelId,
                apiKey: apiKey,
                httpClient: httpClient
            );
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ 已配置自定义 LLM 服务:");
            Console.WriteLine($"   端点: {endpoint}");
            Console.WriteLine($"   模型: {modelId}");
            Console.ResetColor();
        }
    }
    else
    {
        // OpenAI 官方 API（无自定义端点）
        kernelBuilder.AddOpenAIChatCompletion(modelId, apiKey);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ 已配置 OpenAI 官方 API，模型: {modelId}");
        Console.ResetColor();
    }
}
else
{
    // 如果未配置，记录警告（演示模式）
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("⚠️  WARNING: Semantic Kernel 未配置 API 密钥。UIService 将无法正常工作。");
    Console.WriteLine("📝 配置方法:");
    Console.WriteLine("   1. 运行配置脚本: .\\setup-custom-llm.ps1");
    Console.WriteLine("   2. 或手动运行:");
    Console.WriteLine("      cd src/GhostForge.Console");
    Console.WriteLine("      dotnet user-secrets set \"SemanticKernel:ApiKey\" \"YOUR_KEY\"");
    Console.ResetColor();
}

var kernel = kernelBuilder.Build();
builder.Services.AddSingleton(kernel);

// 注册核心服务
builder.Services.AddSingleton<UIService>();
builder.Services.AddSingleton<RoslynCompiler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
