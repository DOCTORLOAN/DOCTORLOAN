using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using DOCTORLOAN.Models.Payoo;
using DOCTORLOAN.Models.NewsModal;
using DOCTORLOAN.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
}).AddRazorRuntimeCompilation();
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Auth/Login";
        option.AccessDeniedPath = "/Auth/Login";
        option.SlidingExpiration = true;
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        option.Cookie.Name = "DoctorLoan.Auth";
        option.Cookie.HttpOnly = true;
        option.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        option.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
    });
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("DoctorLoanApi", client =>
{
    //client.BaseAddress = new Uri("https://doctorloan-api.giathaidoctorloan.vn/");
    client.BaseAddress = new Uri("https://localhost:44333/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Configure Payoo settings
var payooConfig = builder.Configuration.GetSection("Payoo").Get<PayooConfig>()
    ?? throw new InvalidOperationException("Payoo configuration is missing");
builder.Services.AddSingleton(payooConfig);

// Configure NewsModal settings
var newsModalConfig = builder.Configuration.GetSection("NewsModal").Get<NewsModalConfig>()
    ?? new NewsModalConfig { Keyword = "Kinh doanh" };
builder.Services.AddSingleton(newsModalConfig);

// Register PayooService
builder.Services.AddScoped<PayooService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Security Headers (CSP report-only to avoid breaking while tuning)
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    
    // Permissions Policy: Note that 'unload' is not a standard Permissions Policy feature
    // The violations are from third-party scripts (Facebook SDK) and don't affect functionality
    context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
    
    // CSP Report-Only: allow required third-parties while tuning.
    var connectSrc = "'self' https://doctorloan-api.giathaidoctorloan.vn https://esgoo.net https://www.google-analytics.com https://www.googletagmanager.com https://www.facebook.com ws://localhost:* wss://localhost:*";
    
    // Add Browser Link support in Development (Visual Studio Browser Link)
    if (app.Environment.IsDevelopment())
    {
        connectSrc += " http://localhost:* http://127.0.0.1:*";
    }
    
    context.Response.Headers["Content-Security-Policy-Report-Only"] =
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline' https://www.googletagmanager.com https://www.google-analytics.com https://www.youtube.com https://s.ytimg.com https://za.zdn.vn https://cdn.amcharts.com; " +
        "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
        "img-src 'self' data: https://www.google-analytics.com https://doctorloan-api.giathaidoctorloan.vn; " +
        "font-src 'self' https://fonts.gstatic.com data:; " +
        "connect-src " + connectSrc + "; " +
        "frame-src https://www.youtube.com https://page.widget.zalo.me https://www.facebook.com https://www.google.com https://maps.google.com; " +
        "frame-ancestors 'none'";
    await next();
});

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