using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS202024113144.Models;

public partial class Asset
{
    [Display(Name = "资产编号")]
    [Required(ErrorMessage = "必须填写")]
    public int AssetId { get; set; }
    [Display(Name = "资产名称")]
    [Required(ErrorMessage = "必须填写")]
    public string AssetTitle { get; set; } = null!;
    [Display(Name = "资产规格")]
    [Required(ErrorMessage = "必须填写")]
    public string AssetSpecification { get; set; } = null!;
    [Display(Name = "价格")]
    [Required(ErrorMessage = "必须填写")]
    public string AssetPrice { get; set; } = null!;
    [Display(Name = "购入日期")]
    [Required(ErrorMessage = "必须填写")]
    public DateTime PurchaseTime { get; set; }
    [Display(Name = "存放位置")]
    [Required(ErrorMessage = "必须填写")]
    public string Location { get; set; } = null!;
    [Display(Name = "资产类别")]
    [Required(ErrorMessage = "必须填写")]
    public int CategoryId { get; set; }
    [Display(Name = "资产图片")]
    public string? ImgName { get; set; }
    [Display(Name = "资产保管人")]
    [Required(ErrorMessage = "必须填写")]
    public int Mid { get; set; }
    [ValidateNever]
    public virtual Category Category { get; set; } = null!;
    [ValidateNever]
    public virtual Member MidNavigation { get; set; } = null!;
}
