namespace DOCTORLOAN.Helpers;

public static class LoadingStateHelper
{
    public const string IsLoading = "IsLoading";
    public const string LoadingMessage = "LoadingMessage";
    public const string HasError = "HasError";
    public const string ErrorMessage = "ErrorMessage";

    public static void SetLoadingState(this Microsoft.AspNetCore.Mvc.Controller controller, bool isLoading, string message = "Đang tải dữ liệu...")
    {
        controller.ViewBag.IsLoading = isLoading;
        controller.ViewBag.LoadingMessage = message;
    }

    public static void SetErrorState(this Microsoft.AspNetCore.Mvc.Controller controller, bool hasError, string message = "Đã xảy ra lỗi khi tải dữ liệu")
    {
        controller.ViewBag.HasError = hasError;
        controller.ViewBag.ErrorMessage = message;
    }
}

