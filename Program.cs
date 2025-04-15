using Microsoft.EntityFrameworkCore;
using ONGManager.Controllers;
using ONGManager.Data;
using ONGManager.Services;
using Supabase;



var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<OngDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(provider =>
    new Client(
        supabaseUrl: "https://maiqixucwgpeecwgpqxu.supabase.co",
        supabaseKey: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1haXFpeHVjd2dwZWVjd2dwcXh1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDM4NjMzMjUsImV4cCI6MjA1OTQzOTMyNX0.ZGvOBUMCcCL4mf0IdbmMPfAXORRJadGmu5ekbbGBuFQ",
        new SupabaseOptions
        {
            AutoConnectRealtime = true,
        }
    ));
builder.Services.AddScoped<ImagemService>();
builder.Services.AddScoped<CadastroAnimaisController>();
builder.WebHost.UseStaticWebAssets();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CadastroAnimais}/{action=Index}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CadastroUsuarios}/{action=Index}")
    .WithStaticAssets();



app.Run();

