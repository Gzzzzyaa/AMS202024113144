using AMS202024113144.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace AMS202024113144.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ManageDbContext _context;
        private IList<Category> categories;
        private string _path; //图片路径变项
        public CategoryController(ManageDbContext context, IHostEnvironment environment)
        {
            _context = context;
            //设置相片的文件夹路径,透过构造方法取得
            _path = environment.ContentRootPath + "//wwwroot//images";
        }
        public IActionResult Query(string choice, string keyword)
        {

            if (choice == "0")
            {
                categories = _context.Categories.OrderBy(b => b.CategoryId)
                .ToList();
                return View(categories);
            }
            else if (choice == "1")
            {
                categories = _context.Categories.OrderBy(b => b.CategoryId)
                .Where(b => (b.CategoryId).ToString().Contains(keyword))
                .OrderBy(b => b.CategoryId)
                .ToList();
            }
            else if (choice == "2")
            {
                categories = _context.Categories.OrderBy(b => b.CategoryId)
                .Where(b => b.CategoryName.Contains(keyword))
                .OrderBy(b => b.CategoryId)
                .ToList();
            }
            //使用LINQ扩充方法


            return View(categories);
        }
        //PhotoAdmin Action:相片管理页面
        public IActionResult CategoryIndex()
        {
            categories = _context.Categories.OrderBy(p => p.CategoryId)
            .ToList();
            return View(categories);
        }
        //删除相片

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(b => b.CategoryId == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("CategoryIndex"); //重定向到相片管理页
        }

        //添加图书表单
        public IActionResult Create()
        {
            return View();
        }

        //添加图书
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    TempData["Result"] = "资源类别添加成功!";
                    //添加成功则重定向到Index动作方法,显示首页
                    return RedirectToAction("CategoryIndex");

                }
                catch (Exception ex)
                {
                    TempData["Result"] = "资源类别添加失败!";
                }
            }
            return View(category);
        }
        //删除图书

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(b => b.CategoryId == id);
            return View(category);
        }

        //修改图书
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int categoryId = category.CategoryId;
                    var temp = _context.Categories.FirstOrDefault(b => b.CategoryId == categoryId);
                    temp.CategoryName = category.CategoryName;
                    temp.Description = category.Description;
                    _context.SaveChanges();
                    TempData["Result"] = "资源类别修改成功!";
                    return RedirectToAction("CategoryIndex");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = "资源类别修改失败!";
                }
            }
            return View(category);
        }

    }
}