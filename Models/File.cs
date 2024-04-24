﻿using System;
using System.ComponentModel.DataAnnotations;
using myFirstProject.Data;

namespace myFirstProject.Models;

public partial class File
{
    public int Id { get; set; }

    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool? IsDelete { get; set; }
}
