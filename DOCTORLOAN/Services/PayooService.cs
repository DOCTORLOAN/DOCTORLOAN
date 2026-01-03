// <copyright file="PayooService.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using System.Security.Cryptography;
using System.Text;
using DOCTORLOAN.Models.Payoo;
using Newtonsoft.Json;

namespace DOCTORLOAN.Services;

public class PayooService
{
    private readonly PayooConfig config;
    private readonly IHttpClientFactory httpClientFactory;

    public PayooService(PayooConfig config, IHttpClientFactory httpClientFactory)
    {
        this.config = config;
        this.httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Escape XML để tránh XML injection.
    /// </summary>
    private string EscapeXml(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return input
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");
    }

    /// <summary>
    /// Tạo chuỗi checksum SHA-512.
    /// </summary>
    private string GenerateChecksum(string key, string data)
    {
        using (var sha512 = SHA512.Create())
        {
            var hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(key + data));
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();
        }
    }

    /// <summary>
    /// Lấy ngày giờ hiện tại theo định dạng.
    /// </summary>
    private string GetFormattedDate(string type)
    {
        var now = DateTime.Now;

        if (type == "datetime")
        {
            // Format: YYYYMMDDHHmmss
            return $"{now:yyyyMMddHHmmss}";
        }
        else
        {
            // Format: DD/MM/YYYY
            return $"{now:dd/MM/yyyy}";
        }
    }

    /// <summary>
    /// Xử lý thanh toán Payoo.
    /// </summary>
    public async Task<PayooPaymentResponse> CreatePaymentAsync(PayooPaymentRequest request)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.DataRes) ||
                string.IsNullOrWhiteSpace(request.Phone) ||
                string.IsNullOrWhiteSpace(request.CustomerName) ||
                string.IsNullOrWhiteSpace(request.AddressLine))
            {
                return new PayooPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Thông tin thanh toán không đầy đủ",
                };
            }

            // Parse order data từ DataRes
            var orderData = JsonConvert.DeserializeObject<dynamic>(request.DataRes);
            if (orderData?.data == null)
            {
                return new PayooPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Dữ liệu đơn hàng không hợp lệ",
                };
            }

            var validityTime = this.GetFormattedDate("datetime");
            var orderDate = this.GetFormattedDate("date");
            var orderNo = $"ORD_{this.config.ShopID}_{orderData.data.orderNo}";
            var totalPrice = (decimal)orderData.data.totalPrice;

            // Xây dựng dữ liệu XML với XML escaping
            var xmlData = $@"<shops><shop>
    <username>{this.EscapeXml(this.config.BusinessUsername)}</username>
    <shop_id>{this.EscapeXml(this.config.ShopID)}</shop_id>
    <shop_title>{this.EscapeXml(this.config.ShopTitle)}</shop_title>
    <shop_domain>{this.EscapeXml(this.config.Domain)}</shop_domain>
    <order_no>{this.EscapeXml(orderNo)}</order_no>
    <order_cash_amount>{totalPrice}</order_cash_amount>
    <order_ship_date>{this.EscapeXml(orderDate)}</order_ship_date>
    <order_ship_days>0</order_ship_days>
    <order_description>Mã đơn hàng: {this.EscapeXml(orderData.data.orderNo?.ToString() ?? string.Empty)} Tổng tiền: {totalPrice:N0} VND</order_description>
    <shop_back_url>{this.EscapeXml(this.config.ShopBackUrl)}</shop_back_url>
    <notify_url>{this.EscapeXml(this.config.NotifyUrl)}</notify_url>
    <validity_time>{this.EscapeXml(validityTime)}</validity_time>
    <customer>
        <name>{this.EscapeXml(request.CustomerName)}</name>
        <phone>{this.EscapeXml(request.Phone)}</phone>
        <address>{this.EscapeXml(request.AddressLine)}</address>
        <email>{this.EscapeXml(request.Email ?? string.Empty)}</email>
    </customer>
    <JsonResponse>true</JsonResponse>
    <direct_return_time>5</direct_return_time>
</shop></shops>";

            // Xóa khoảng trắng thừa
            xmlData = System.Text.RegularExpressions.Regex.Replace(xmlData, @"\s+", " ");

            // Tạo checksum
            var checksum = this.GenerateChecksum(this.config.ChecksumKey, xmlData);

            // Gửi request đến Payoo API
            var httpClient = this.httpClientFactory.CreateClient();
            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("data", xmlData),
                new KeyValuePair<string, string>("checksum", checksum),
                new KeyValuePair<string, string>("refer", this.config.Domain),
                new KeyValuePair<string, string>("payment_group", string.Empty),
            };

            var formContent = new FormUrlEncodedContent(formData);
            var response = await httpClient.PostAsync(config.BaseUrl + "create-preorder", formContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var payooResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

                if (payooResponse?.order?.payment_url != null)
                {
                    return new PayooPaymentResponse
                    {
                        Success = true,
                        PaymentUrl = payooResponse.order.payment_url.ToString(),
                    };
                }
            }

            return new PayooPaymentResponse
            {
                Success = false,
                ErrorMessage = "Không thể tạo đơn hàng thanh toán. Vui lòng thử lại.",
            };
        }
        catch (Exception ex)
        {
            return new PayooPaymentResponse
            {
                Success = false,
                ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}",
            };
        }
    }
}
