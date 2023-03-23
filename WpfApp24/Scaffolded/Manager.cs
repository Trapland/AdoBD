using System;
using System.Collections.Generic;

namespace AdoBD.Scaffolded;

public partial class Manager
{
    public Guid Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Secname { get; set; } = null!;

    public Guid IdMainDep { get; set; }

    public Guid? IdSecDep { get; set; }

    public Guid? IdChief { get; set; }

    public DateTime? DeleteDt { get; set; }

    public virtual Department IdMainDepNavigation { get; set; } = null!;

    public virtual Department? IdSecDepNavigation { get; set; }

    public virtual ICollection<Sale> Sales { get; } = new List<Sale>();
}
