using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS202024113144.Models;

public partial class Member
{
    [Display(Name = "员工编号")]
    [Required(ErrorMessage = "必须填写")]
    public int Mid { get; set; }
    [Display(Name = "密码")]
    [Required(ErrorMessage = "必须填写")]
    public string Password { get; set; } = null!;
    [Display(Name = "姓名")]
    public string? Name { get; set; }
    [Display(Name = "联络电话")]
    public string? Phone { get; set; }
    [Display(Name = "角色")]
    [Required(ErrorMessage = "必须填写")]
    public string Role { get; set; } = null!;
    [Display(Name = "部门代号")]
    [Required(ErrorMessage = "必须填写")]
    public int Did { get; set; }
    [ValidateNever]
    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
    [ValidateNever]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
    [ValidateNever]
    public virtual Department DidNavigation { get; set; } = null!;
}
