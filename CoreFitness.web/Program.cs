using CoreFitness.Application;
using CoreFitness.Infrastructure;
using CoreFitness.Infrastructure.Seeders;
using CoreFitness.Web;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var authDb = services.GetRequiredService<AuthDbContext>();
    var coreDb = services.GetRequiredService<CoreFitnessDbContext>();

    await authDb.Database.EnsureCreatedAsync();
    await coreDb.Database.EnsureCreatedAsync();

    await DbSeeder.SeedRolesAsync(services);
    await DbSeeder.SeedMembershipTypesAsync(services);
    await DbSeeder.SeedAdminAsync(services);
    await DbSeeder.SeedTrainingSessions(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseExceptionHandler();
app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();