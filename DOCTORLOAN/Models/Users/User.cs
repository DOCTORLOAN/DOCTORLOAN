// <copyright file="User.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

namespace DOCTORLOAN.Models.Users;

public class User
{
    public Guid UUId { get; set; }

    public int RoleId { get; set; }

    public string Code { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName { get; set; }

    public string UserName { get; set; }

    private string email;

    public string Email
    {
        get { return string.IsNullOrEmpty(this.email) ? string.Empty : this.email.Trim(); }
        set { this.email = value; }
    }

    private string phone;

    public string Phone
    {
        get { return string.IsNullOrEmpty(this.phone) ? string.Empty : this.phone.Trim(); }
        set { this.phone = value; }
    }

    public int? ParentId { get; set; }

    public int Status { get; set; }

    public int Avatar { get; set; }

    public int Gender { get; set; }

    public DateTime? DOB { get; set; }

    public string Password { get; set; }

    public string PasswordHash { get; set; }

    public string Remarks { get; set; }

    public bool IsResetPassword { get; set; } = false;

    public bool IsSignOut { get; set; } = false;

    public int? ValidUnixTime { get; set; }

    public virtual User ParentUser { get; set; }
}
