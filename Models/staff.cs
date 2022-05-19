using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class staff
    {
        public string RFC { get; set; } = null!;
        public string CURP { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
