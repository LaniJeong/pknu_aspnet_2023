using System;
using System.Collections.Generic;

namespace bookRentalShopApi.Models;

public partial class Rentaltbl
{
    public int RentalIdx { get; set; }

    public int? MemberIdx { get; set; }

    public int? BookIdx { get; set; }

    public DateTime? RentalDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public virtual Bookstbl? BookIdxNavigation { get; set; }

    public virtual Membertbl? MemberIdxNavigation { get; set; }
}
