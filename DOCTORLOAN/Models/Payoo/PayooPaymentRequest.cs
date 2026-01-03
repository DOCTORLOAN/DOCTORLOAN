// <copyright file="PayooPaymentRequest.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

namespace DOCTORLOAN.Models.Payoo;

public class PayooPaymentRequest
{
    public string DataRes { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string AddressLine { get; set; } = string.Empty;

    public string? Email { get; set; }
}

public class PayooPaymentResponse
{
    public bool Success { get; set; }

    public string? PaymentUrl { get; set; }

    public string? ErrorMessage { get; set; }
}
