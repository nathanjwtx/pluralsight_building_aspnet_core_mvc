using System.Collections.Generic;
using System.Linq;
using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
        }

        // public ViewResult List()
        // {
        //     PiesListViewModel piesListViewModel = new PiesListViewModel();
        //     piesListViewModel.Pies = _pieRepository.AllPies;
        //     piesListViewModel.CurrentCategory = "Cheese Cakes";
        //     
        //     return View(piesListViewModel);
        // }

        public ViewResult List(string category)
        {
            IEnumerable<Pie> pies;
            string currentCategory;

            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
            }
            else
            {
                pies = _pieRepository.AllPies.Where(p => p.Category.CategoryName == category).OrderBy(p => p.PieId);
                /* finds a category, or type Category, in AllCategories IEnumerable. The null-operator '?.' either returns null if
                FirstOrDefault returns the default value of null. CategoryName is the string name property from the Category class */
                currentCategory = _categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryName == category)
                    ?.CategoryName;
            }

            return View(new PiesListViewModel
            {
                Pies = pies,
                CurrentCategory = currentCategory
            });
        }
        
        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
            {
                return NotFound();
            }

            return View(pie);
        }
    }
}