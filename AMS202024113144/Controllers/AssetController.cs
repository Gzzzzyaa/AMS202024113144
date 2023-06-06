using AMS202024113144.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace AMS202024113144.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssetController : Controller
    {
        private readonly ManageDbContext _context;
        private IList<Asset> assets;
        private string _path; //图片路径变项
        public AssetController(ManageDbContext context, IHostEnvironment environment)
        {
            _context = context;
            //设置相片的文件夹路径,透过构造方法取得
            _path = environment.ContentRootPath + "//wwwroot//images";
        }
        //PhotoAdmin Action:相片管理页面
        public IActionResult AssetIndex()
        {
            assets = _context.Assets.OrderBy(p => p.AssetId)
            .Include(b => b.Category).AsNoTracking()
            .Include(b => b.MidNavigation).AsNoTracking()
            .ToList();
            return View(assets);
        }
        public IActionResult Query(string choice, string keyword)
        {

            if (choice == "0")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                    .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                    .ToList();
            }
            else if (choice == "1")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.AssetTitle.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                .Include(b => b.MidNavigation).AsNoTracking()
                .OrderBy(b => b.AssetId)
                .ToList();
            }
            else if (choice == "2")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.AssetSpecification.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            else if (choice == "3")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.AssetPrice.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            else if (choice == "4")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.Location.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            else if (choice == "5")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.Category.CategoryName.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            else if (choice == "6")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.MidNavigation.Name.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            //使用LINQ扩充方法


            return View(assets);
        }
        //删除相片

        public IActionResult Delete(int id)
        {
            var asset = _context.Assets.FirstOrDefault(b => b.AssetId == id);
            _context.Assets.Remove(asset);
            _context.SaveChanges();
            return RedirectToAction("AssetIndex"); //重定向到相片管理页
        }

        //添加图书表单
        public IActionResult Create()
        {
            return View();
        }

        //添加图书
        /*     [HttpPost]
             public IActionResult Create(Asset asset)
             {
                 if (ModelState.IsValid)
                 {
                     try
                     {
                         _context.Assets.Add(asset);
                         _context.SaveChanges();
                         TempData["Result"] = "资源添加成功!";
                         //添加成功则重定向到Index动作方法,显示首页
                         return RedirectToAction("AssetIndex");

                     }
                     catch (Exception ex)
                     {
                         TempData["Result"] = "资源添加失败!";
                     }
                 }
                 return View(asset);
             }
         */    //删除图书

        public IActionResult Edit(int id)
        {
            var asset = _context.Assets.FirstOrDefault(b => b.AssetId == id);
            
            return View(asset);
        }

        //修改图书
        [HttpPost]
        public async Task<IActionResult> Edit(Asset asset, IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                if (imgFile != null)
                {
                    if (imgFile.Length > 0)
                    {
                        //相片提交
                        string fileName =
                       $"{Guid.NewGuid().ToString()}.{Path.GetExtension(imgFile.FileName).Substring(1)}";
                        string savePath = $"{_path}\\{fileName}";
                        using (var steam = new FileStream(savePath, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(steam);
                        }
                        try
                        {
                            int assetId = asset.AssetId;
                            var temp = _context.Assets.FirstOrDefault(b => b.AssetId == assetId);
                            temp.AssetTitle = asset.AssetTitle;
                            temp.AssetSpecification = asset.AssetSpecification;
                            temp.AssetPrice = asset.AssetPrice;
                            temp.PurchaseTime = asset.PurchaseTime;
                            temp.Location = asset.Location;
                            temp.CategoryId = asset.CategoryId;
                            temp.ImgName = fileName;
                            temp.Mid = asset.Mid;

                            _context.SaveChanges();
                            TempData["Result"] = "资源修改成功!";
                            return RedirectToAction("AssetIndex");
                        }
                        catch (Exception ex)
                        {
                            TempData["Result"] = "资源修改失败!";
                        }
                    }
                }
            }
            return View(asset);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Asset asset, IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                if (imgFile != null)
                {
                    if (imgFile.Length > 0)
                    {
                        //相片提交
                        string fileName =
                       $"{Guid.NewGuid().ToString()}.{Path.GetExtension(imgFile.FileName).Substring(1)}";
                        string savePath = $"{_path}\\{fileName}";
                        using (var steam = new FileStream(savePath, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(steam);
                        }
                        //相片信息写入记录
                        asset.ImgName = fileName;
                        asset.PurchaseTime = DateTime.Now;
                        _context.Assets.Add(asset);
                        _context.SaveChanges();
                        return RedirectToAction("AssetIndex", "Asset"); //重定向到Home/Index
                    }
                }
            }
            return View(asset);
        }

    }
}