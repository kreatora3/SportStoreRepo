using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportStore.Domain.Entities;
using SportStore.Domain.Abstract;

namespace SportStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductsRepository reposiotory;

        public ProductController(IProductsRepository productRepository)
        {
            this.reposiotory = productRepository;
        }

        public ViewResult List()
        {
            return View(reposiotory.Products);
        }
    }
}