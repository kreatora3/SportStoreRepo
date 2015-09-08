using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Controllers;
using SportStore.WebUI.Models;
using SportStore.WebUI.HtmlHelpers;


namespace SportStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //arrange
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ProductID = 1, Name="P1"},
                new Product{ProductID = 2, Name="P2"},
                new Product{ProductID = 3, Name="P3"},
                new Product{ProductID = 4, Name="P4"},
                new Product{ProductID = 5, Name="P5"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;

            //assert 
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0], "P4");
            Assert.AreEqual(prodArray[1], "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML helper - we need to do this           
            // in order to apply the extension method          
            HtmlHelper myHelper = null;
            // Arrange - create PagingInfo data          
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            // Arrange - set up the delegate using a lambda expression          
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            // Act          
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            // Assert            
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Paginaton_View_Model()
        { 
            //arrange
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ProductID = 1, Name="P1"},
                new Product{ProductID = 2, Name="P2"},
                new Product{ProductID = 3, Name="P3"},
                new Product{ProductID = 4, Name="P4"},
                new Product{ProductID = 5, Name="P5"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;

            //assert 
            PagingInfo pageInfo = new PagingInfo();
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalItems, 2);
        }
    }

}
