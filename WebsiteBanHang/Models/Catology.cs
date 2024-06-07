using System;
using System.Collections.Generic;

namespace WebsiteBanHang.Models;

public partial class Catology
{
    public int IdCat { get; set; }

    public string? NameCat { get; set; }

    public int? Order { get; set; }

    public string? Link { get; set; }

    public int? Hide { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
