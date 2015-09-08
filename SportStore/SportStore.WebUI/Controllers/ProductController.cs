using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportStore.Domain.Entities;
using SportStore.Domain.Abstract;
using SportStore.WebUI.Models;

namespace SportStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductsRepository reposiotory;
        public int pageSize = 4;

        public ProductController(IProductsRepository productRepository)
        {
            this.reposiotory = productRepository;
        }

        public ViewResult List(int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = reposiotory.Products
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = reposiotory.Products.Count()
                }
            };
            return View(model);
        }
    }
}