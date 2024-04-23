using System;
using System.Collections.Generic;

namespace myFirstProject.Models;

public partial class Activity
{
    public int Id { get; set; }

    public int? ProjectId { get; set; }

    public int? ActivityHeaderId { get; set; }

    public string? Name { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }
}
