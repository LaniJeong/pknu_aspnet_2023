using System;
using System.Collections.Generic;

namespace bookRentalShopApi.Models;

public partial class Usertbl
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Password { get; set; } = null!;
}
