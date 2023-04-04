using Bandwagon.Web.Services;
using Bandwagon.Web.Services.TruffleSDK;
using Bandwagon.Web.Services.TruffleSDK.Mocks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddTransient<GroupedMessageCollector>();
builder.Services.AddTransient<MessageCorrelator>();

builder.Services.AddSingleton<SharedCircuitRepository>();
builder.Services.AddTransient<OrgCircuit>();
builder.Services.AddScoped<OrgCircuitProvider>();
builder.Services.AddTransient<UserCircuit>();
builder.Services.AddScoped<UserCircuitProvider>();
builder.Services.AddTransient<ILiveChatCircuit, PrerecordedLiveChatCircuit>();
builder.Services.AddScoped<LiveChatCircuitProvider>();
builder.Services.AddScoped<VideoPlayer>();

var useMocks = true;
if (useMocks)
{
    builder.Services.AddScoped<IEmbed, MockEmbed>();
    builder.Services.AddScoped<IUserClient, MockUserClient>();
    builder.Services.AddScoped<IOrgClient, MockOrgClient>();
}
else
{
    builder.Services.AddScoped<IEmbed, Embed>();
    builder.Services.AddScoped<IUserClient, UserClient>();
    builder.Services.AddScoped<IOrgClient, OrgClient>();
}

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
