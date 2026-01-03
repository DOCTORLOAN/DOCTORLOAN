// <copyright file="Signin.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

namespace DOCTORLOAN.Models.VMAuth
{
    public class Signin
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool KeepLoggedIn { get; set; }
    }
}
