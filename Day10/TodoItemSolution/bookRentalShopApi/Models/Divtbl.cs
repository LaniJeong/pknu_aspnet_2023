using System;
using System.Collections.Generic;

namespace bookRentalShopApi.Models;

public partial class Divtbl
{
    public string Division { get; set; } = null!;

    public string? Names { get; set; }

    public virtual ICollection<Bookstbl> Bookstbls { get; set; } = new List<Bookstbl>();
}
