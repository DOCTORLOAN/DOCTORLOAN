using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers; // Thêm namespace này

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option => {
        option.LoginPath = "/Auth/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Kiểm tra xem đây có phải là môi trường Development không.
        // Trong môi trường Development, có thể không cần cache hoặc cache ngắn hơn
        // để dễ dàng xem các thay đổi ngay lập tức.
        // Nếu không phải Development, áp dụng cache dài hạn.
        if (!app.Environment.IsDevelopment())
        {
            const int durationInSeconds = 60 * 60 * 24 * 365; // 1 năm (31,536,000 giây)
            ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                "public,max-age=" + durationInSeconds;
            ctx.Context.Response.Headers[HeaderNames.Expires] =
                DateTime.UtcNow.AddYears(1).ToString("R"); // RFC1123 format
        }
        else
        {
            // Trong môi trường Development, có thể không cache hoặc cache rất ngắn
            // để đảm bảo bạn thấy các thay đổi ngay lập tức.
            ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                "no-cache, no-store, must-revalidate";
            ctx.Context.Response.Headers[HeaderNames.Pragma] = "no-cache";
            ctx.Context.Response.Headers[HeaderNames.Expires] = "0";
        }
    }
});

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();