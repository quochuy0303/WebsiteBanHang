using System;
using System.Collections.Generic;

namespace WebsiteBanHang.Models;

public partial class Blog
{
    public int IdBlog { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Img { get; set; }

    public string? Detail { get; set; }

    public DateTime? Datebegin { get; set; }

    public int? Order { get; set; }

    public string? Link { get; set; }

    public int? Hide { get; set; }

    public int? IdUsers { get; set; }

    public virtual User? IdUsersNavigation { get; set; }
}
