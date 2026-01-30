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

if (!string.IsNullOrEmpty(endpoint) && !string.IsNullOrEmpty(apiKey))
{
    // 使用 Azure OpenAI 或 OpenAI
    if (endpoint.Contains("azure", StringComparison.OrdinalIgnoreCase))
    {
        kernelBuilder.AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);
    }
    else
    {
        kernelBuilder.AddOpenAIChatCompletion(modelId, apiKey);
    }
}
else
{
    // 如果未配置，记录警告（演示模式）
    Console.WriteLine("WARNING: Semantic Kernel 未配置 API 密钥。UIService 将无法正常工作。");
    Console.WriteLine("请在 User Secrets 或 appsettings.json 中配置 SemanticKernel:Endpoint 和 SemanticKernel:ApiKey");
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
