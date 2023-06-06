using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS202024113144.Models;

public partial class Category
{
    [Display(Name = "类别代号")]
    [Required(ErrorMessage = "必须填写")]
    public int CategoryId { get; set; }
    [Display(Name = "资产名称")]
    [Required(ErrorMessage = "必须填写")]
    public string CategoryName { get; set; } = null!;
    [Display(Name = "资产说明")]
    public string? Description { get; set; }
    [ValidateNever]
    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
