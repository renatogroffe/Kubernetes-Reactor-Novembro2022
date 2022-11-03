using SiteQuestaoEventHub.EventHubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<VotacaoProducer>();
builder.Services.AddControllersWithViews();

builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString =
        builder.Configuration.GetConnectionString("ApplicationInsights");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();