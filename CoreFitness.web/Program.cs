using CoreFitness.Infrastructure;
using CoreFitness.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var authDb = services.GetRequiredService<AuthDbContext>();
    var coreDb = services.GetRequiredService<CoreFitnessDbContext>();

    await authDb.Database.EnsureCreatedAsync();
    await coreDb.Database.EnsureCreatedAsync();

    await DbSeeder.SeedRolesAsync(app.Services.CreateScope().ServiceProvider);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();