using System;
using System.Collections.Generic;

namespace WebsiteBanHang.Models;

public partial class Cart
{
    public int IdCart { get; set; }

    public DateTime? Datebegin { get; set; }

    public int? Hide { get; set; }

    public int? IdUsers { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual User? IdUsersNavigation { get; set; }
}
