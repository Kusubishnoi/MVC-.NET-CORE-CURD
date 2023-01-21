using Identity.Models;
using Identity.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Controllers
{
    public class ViewModel1
    {
        public IEnumerable<Category> Category { get; set; }
        public IQueryable<SubCategory> SubCategory { get; set; }
        public int totalPage { get; set; }
        public int pageSize { get; set; }
        public string nameSortParm { get; set; }
        public string expDateSortParm { get; set; }
        public string createDateSortParm { get; set; }
        public string currentFilter { get; set; }
        public string currentSortOrder { get; set; }
    }
    public class SubCategoryController : Controller
    {
        private readonly IGenericRepository<SubCategory> _subcategoryRepository;
        private readonly IGenericRepository<Category> _categoryRepository;

        public int CategoryId { get; private set; }

        public SubCategoryController(IGenericRepository<SubCategory> subcategory, IGenericRepository<Category> category)
        {
            _subcategoryRepository = subcategory;
            _categoryRepository = category;
        }
        //public IActionResult SubCategoryList(int id)
        //{
        //    CategoryId = id;

        //    var allSubCategory = _subcategoryRepository.GetAll();
        //    return View(allSubCategory);

        //}
        public IActionResult Index(string sortOrder, string searchString, int? pageNo, int? pageSize)
        {
            var vm = new ViewModel1();
            int pgNo = (pageNo ?? 1);
            int pgSize = vm.pageSize = (pageSize ?? 5);
            IQueryable<SubCategory> subcategories = null;
            int? totalPage = null;
            vm.currentSortOrder = sortOrder;
            if (!String.IsNullOrEmpty(searchString))
            {
                vm.currentFilter = searchString;
                subcategories = _subcategoryRepository.Search((x => x.IsDeleted == false && x.SubCategoryName.ToLower().Contains(searchString.ToLower())));
                totalPage = (int)Math.Ceiling((decimal)subcategories.Count() / pgSize);
            }
            else
            {
                subcategories = _subcategoryRepository.Search(x => x.IsDeleted == false);
                totalPage = (int)Math.Ceiling((decimal)subcategories.Count() / pgSize);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    subcategories = subcategories.OrderByDescending(p => p.SubCategoryName);
                    break;
                case "Date":
                    subcategories = subcategories.OrderBy(p => p.createdOn);
                    break;
                case "date_desc":
                    subcategories = subcategories.OrderByDescending(s => s.CategoryName);
                    break;
                case "createDate":
                    subcategories = subcategories.OrderBy(s => s.createdOn);
                    break;
                case "Cdate_desc":
                    subcategories = subcategories.OrderByDescending(s => s.createdOn);
                    break;
                default:
                    subcategories = subcategories.OrderBy(s => s.SubCategoryName);
                    break;
            }
            subcategories = subcategories.Skip((pgNo - 1) * pgSize).Take(pgSize);
            vm.SubCategory = subcategories;
            vm.totalPage = (int)totalPage;
            vm.nameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.expDateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            vm.createDateSortParm = sortOrder == "createDate" ? "Cdate_desc" : "createDate";
            vm.Category = _categoryRepository.Search(x => x.IsDeleted == false);
            return View("Index", vm);
        }

        public IActionResult Create()
        {
            var _categoryNames = _categoryRepository.GetAll().Select(o => o.CategoryName).ToList();
            ViewBag.CategoryNames = _categoryNames;
            return View();
        }
        [HttpPost]
        public IActionResult Create(SubCategory subcategory)
        {
            subcategory.createdOn = DateTime.Now;
            _subcategoryRepository.Insert(subcategory);
            _subcategoryRepository.Save();

            return RedirectToAction("SubCategoryList");
        }

        public IActionResult Edit(int id)
        {
            var _categoryNames = _categoryRepository.GetAll().Select(o => o.CategoryName).ToList();
            ViewBag.CategoryNames = _categoryNames;
            var subcategory = _subcategoryRepository.GetById(id);
            return View(subcategory);
        }

        [HttpPost]
        public IActionResult Edit(SubCategory subcategory)
        {
            if (!ModelState.IsValid)
            {
                return View(subcategory);
            }

            _subcategoryRepository.Update(subcategory);
            _subcategoryRepository.Save();

            return RedirectToAction("SubCategoryList");
        }
        public IActionResult Delete(int Id)
        {
            var subcategory = _subcategoryRepository.GetById(Id);

            _subcategoryRepository.Update(subcategory);
            _subcategoryRepository.Save();

            return RedirectToAction("subCategoryList");

        }

    }

}

