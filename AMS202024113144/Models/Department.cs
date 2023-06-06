using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS202024113144.Models;

public partial class Department
{
    [Display(Name = "部门代号")]
    [Required(ErrorMessage = "必须填写")]
    public int Did { get; set; }
    [Display(Name = "部门名称")]
    [Required(ErrorMessage = "必须填写")]
    public string? Dname { get; set; }
    [Display(Name = "部门主管")]
    [Required(ErrorMessage = "必须填写")]
    public int Mid { get; set; }
    [ValidateNever]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
    [ValidateNever]
    public virtual Member MidNavigation { get; set; } = null!;
}
