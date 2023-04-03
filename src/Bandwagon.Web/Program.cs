using Bandwagon.Web.Services;
using Bandwagon.Web.Services.TruffleSDK;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<GroupedMessageCollector>();
builder.Services.AddScoped<MessageCorrelator>();
builder.Services.AddTransient<MessageCorrelator>();

builder.Services.AddSingleton<SharedCircuitRepository>();
builder.Services.AddTransient<OrgCircuit>();
builder.Services.AddScoped<OrgCircuitProvider>();
builder.Services.AddTransient<UserCircuit>();
builder.Services.AddScoped<UserCircuitProvider>();
builder.Services.AddTransient<ILiveChatCircuit, PrerecordedLiveChatCircuit>();
builder.Services.AddScoped<LiveChatCircuitProvider>();
builder.Services.AddScoped<VideoPlayer>();
builder.Services.AddScoped<Embed>();
builder.Services.AddScoped<UserClient>();
builder.Services.AddScoped<OrgClient>();

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
