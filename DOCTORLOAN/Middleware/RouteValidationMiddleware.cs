// <copyright file="RouteValidationMiddleware.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DOCTORLOAN.Middleware
{
    /// <summary>
    /// Middleware để validate routes và redirect về 404 nếu route không hợp lệ
    /// </summary>
    public class RouteValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RouteValidationMiddleware> _logger;
        
        // Danh sách các controllers và actions hợp lệ
        private static readonly HashSet<string> ValidControllers = new()
        {
            "Home", "Products", "News", "About", "Auth", "Contact", 
            "Clinic", "Cart", "ShowRoom", "Common", "Sitemap"
        };

        private static readonly Dictionary<string, HashSet<string>> ValidActions = new()
        {
            { "Home", new HashSet<string> { "Index", "Privacy", "Error", "NotFound", "ProcessPayooPayment" } },
            { "Products", new HashSet<string> { "Index", "ProductDetail" } },
            { "News", new HashSet<string> { "Index", "NewsDetail" } },
            { "About", new HashSet<string> { "Index" } },
            { "Auth", new HashSet<string> { "Login", "LoginPost" } },
            { "Contact", new HashSet<string> { "Index" } },
            { "Clinic", new HashSet<string> { "Index", "Booking" } },
            { "Cart", new HashSet<string> { "Index" } },
            { "ShowRoom", new HashSet<string> { "Index" } },
            { "Common", new HashSet<string> { "Index", "PolicyDetails", "OrderSuccess" } },
            { "Sitemap", new HashSet<string> { "Index" } }
        };

        public RouteValidationMiddleware(RequestDelegate next, ILogger<RouteValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;
            
            // Bỏ qua các static files và API endpoints
            if (IsStaticFile(path) || IsApiEndpoint(path) || IsHealthCheck(path))
            {
                await _next(context);
                return;
            }

            // Bỏ qua trang NotFound để tránh redirect loop
            if (path.StartsWith("/home/notfound", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // Parse path để lấy controller và action
            var pathParts = path.TrimStart('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
            
            if (pathParts.Length > 0)
            {
                var controllerName = pathParts[0];
                var actionName = pathParts.Length > 1 ? pathParts[1] : "Index";

                // Validate controller
                if (!ValidControllers.Contains(controllerName, StringComparer.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Invalid controller: {Controller} for path: {Path}", controllerName, path);
                    context.Response.Redirect("/Home/NotFound", permanent: false);
                    return;
                }

                // Validate action
                if (ValidActions.TryGetValue(controllerName, out var validActions))
                {
                    if (!validActions.Contains(actionName, StringComparer.OrdinalIgnoreCase))
                    {
                        _logger.LogWarning("Invalid action: {Action} for controller: {Controller} and path: {Path}", 
                            actionName, controllerName, path);
                        context.Response.Redirect("/Home/NotFound", permanent: false);
                        return;
                    }
                }
            }

            // Tiếp tục pipeline
            await _next(context);

            // Nếu sau khi routing mà vẫn là 404, redirect về NotFound
            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                _logger.LogWarning("404 Not Found for path: {Path}", path);
                context.Response.Clear();
                context.Response.Redirect("/Home/NotFound", permanent: false);
            }
        }

        private static bool IsStaticFile(string path)
        {
            var staticExtensions = new[] { ".css", ".js", ".jpg", ".jpeg", ".png", ".gif", ".svg", ".ico", 
                ".woff", ".woff2", ".ttf", ".eot", ".map", ".json", ".pdf", ".zip", ".mp4", ".webp" };
            return staticExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) ||
                   path.StartsWith("/libs/", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("/css/", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("/js/", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("/img/", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("/font/", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsApiEndpoint(string path)
        {
            return path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsHealthCheck(string path)
        {
            return path.Equals("/health", StringComparison.OrdinalIgnoreCase) ||
                   path.Equals("/healthz", StringComparison.OrdinalIgnoreCase);
        }
    }
}

