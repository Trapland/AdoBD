using System;
using System.Collections.Generic;

namespace AdoBD.Scaffolded;

public partial class Sale
{
    public Guid Id { get; set; }

    public DateTime SaleDt { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public Guid ManagerId { get; set; }

    public DateTime? DeleteDt { get; set; }

    public virtual Manager Manager { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
