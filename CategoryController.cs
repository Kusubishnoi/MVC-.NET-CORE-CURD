using Identity.Models;
using Identity.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Identity.Controllers
{
    public class ViewModel
    {
        public IQueryable<Category> Category { get; set; }
        public int totalPage { get; set; }
        public int pageSize { get; set; }
        public string nameSortParm { get; set; }
        public string dateSortParm { get; set; }
        public string currentFilter { get; set; }
        public string currentSortOrder { get; set; }
    }
    [Authorize]
    public class CategoryController : Controller
    {
        private IGenericRepository<Category> _categoryRepository;
        public CategoryController(IGenericRepository<Category> category)
        {
            _categoryRepository = category;
        }
        //public IActionResult CategoryList()
        //{
        //    var category = _categoryRepository.GetAll();
        //    return View(category);
        //}
        public IActionResult CategoryList(string sortOrder, string searchString, int? pageNo, int? pageSize)
        {
            var vm = new ViewModel();
            int pgNo = (pageNo ?? 1);
            int pgSize = vm.pageSize = (pageSize ?? 5);
            IQueryable<Category> categories = null;
            int? totalPage = null;
            vm.currentSortOrder = sortOrder;

            if (!String.IsNullOrEmpty(searchString))
            {
                vm.currentFilter = searchString;
                categories = _categoryRepository.Search((x => x.IsDeleted == false && x.CategoryName.ToLower().Contains(searchString.ToLower())));
                totalPage = (int)Math.Ceiling((decimal)categories.Count() / pgSize);
            }
            else
            {
                categories = _categoryRepository.Search(x => x.IsDeleted == false);
                totalPage = (int)Math.Ceiling((decimal)categories.Count() / pgSize);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    categories = categories.OrderByDescending(s => s.CategoryName);
                    break;
                case "Date":
                    categories = categories.OrderBy(s => s.CreatedOn);
                    break;
                case "date_desc":
                    categories = categories.OrderByDescending(s => s.CreatedOn);
                    break;
                default:
                    categories = categories.OrderBy(s => s.CategoryName);
                    break;
            }

            categories = categories.Skip((pgNo - 1) * pgSize).Take(pgSize);

            vm.Category = categories;
            vm.totalPage = (int)totalPage;
            vm.nameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.dateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            return View("CategoryList", vm);
        }

       
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            category.CreatedOn = DateTime.Now;
            _categoryRepository.Insert(category);
            _categoryRepository.Save();

            return RedirectToAction("CategoryList");

        }
        public IActionResult Edit(int id)
        {
            var category = _categoryRepository.GetById(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
           
            _categoryRepository.Update(category);
            _categoryRepository.Save();

            return RedirectToAction("CategoryList");
        }
        public IActionResult Delete(int Id)
        {
            var category = _categoryRepository.GetById(Id);
           
            _categoryRepository.Delete(category);
            _categoryRepository.Save();

            return RedirectToAction("CategoryList");
        }
    }
}
