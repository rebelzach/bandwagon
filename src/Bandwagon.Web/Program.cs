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
builder.Services.AddTransient<OrgSession>();
builder.Services.AddSingleton<OrgSessionProvider>();
builder.Services.AddTransient<UserSession>();
builder.Services.AddSingleton<UserSessionProvider>();
builder.Services.AddTransient<ILiveStream, SavedLiveStream>();
builder.Services.AddSingleton<LiveStreamProvider>();
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
