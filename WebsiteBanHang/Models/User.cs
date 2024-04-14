using System;
using System.Collections.Generic;

namespace WebsiteBanHang.Models;

public partial class User
{
    public int IdUsers { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? Permission { get; set; }

    public int? Hide { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
