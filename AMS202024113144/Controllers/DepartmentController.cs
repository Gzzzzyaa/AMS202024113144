using AMS202024113144.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace AMS202024113144.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly ManageDbContext _context;
        private IList<Department> departments;
        private string _path; //图片路径变项
        public DepartmentController(ManageDbContext context, IHostEnvironment environment)
        {
            _context = context;
            //设置相片的文件夹路径,透过构造方法取得
            _path = environment.ContentRootPath + "//wwwroot//images";
        }
        //PhotoAdmin Action:相片管理页面
        public IActionResult DepartmentIndex()
        {
            departments = _context.Departments.OrderBy(p => p.Did)
            .Include(b => b.MidNavigation).AsNoTracking()
            .ToList();
            return View(departments);
        }
        public IActionResult Query(string choice, string keyword)
        {

            if (choice == "0")
            {
                departments = _context.Departments.OrderBy(b => b.Did)
                .ToList();
                return View(departments);
            }
            else if (choice == "1")
            {
                departments = _context.Departments.OrderBy(b => b.Did)
                    .Include(b => b.MidNavigation).AsNoTracking()
                .Where(b => b.Dname.Contains(keyword))
                .OrderBy(b => b.Did)
                .ToList();
            }
            else if (choice == "2")
            {
                departments = _context.Departments.OrderBy(b => b.Did)
                    .Include(b => b.MidNavigation).AsNoTracking()
                .Where(b => b.MidNavigation.Name.Contains(keyword))
                .OrderBy(b => b.Did)
                .ToList();
            }
            //使用LINQ扩充方法


            return View(departments);
        }
        //删除相片

        public IActionResult Delete(int id)
        {
            var department = _context.Departments.FirstOrDefault(b => b.Did == id);
            _context.Departments.Remove(department);
            _context.SaveChanges();
            return RedirectToAction("DepartmentIndex"); //重定向到相片管理页
        }


        public IActionResult Create(Department department)
        {
            Console.WriteLine("123456");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Departments.Add(department);
                    _context.SaveChanges();
                    TempData["Result"] = "部门添加成功!";
                    //添加成功则重定向到Index动作方法,显示首页
                    return RedirectToAction("DepartmentIndex");

                }
                catch (Exception ex)
                {
                    TempData["Result"] = "部门添加失败!";
                }
            }
            return View(department);
        }
        //删除图书

        public IActionResult Edit(int id)
        {
            var department = _context.Departments.FirstOrDefault(b => b.Did == id);
            return View(department);
        }

        //修改图书
        [HttpPost]
        public IActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int did = department.Did;
                    var temp = _context.Departments.FirstOrDefault(b => b.Did == did);
                    temp.Dname = department.Dname;
                    temp.Mid = department.Mid;
                    _context.SaveChanges();
                    TempData["Result"] = "部门修改成功!";
                    return RedirectToAction("DepartmentIndex");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = "部门修改失败!";
                }
            }
            return View(department);
        }

    }
}
