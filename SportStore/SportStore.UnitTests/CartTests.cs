using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportStore.Domain.Entities;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.WebUI.Controllers;
using System.Web.Mvc;
using SportStore.WebUI.Models;

namespace SportStore.UnitTests
{
    
    [TestClass]
    public class CartTests
    {
      [TestMethod]
        public void Can_Add_New_Lines()
        {
          //Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();
          
          // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            SportStore.Domain.Entities.Cart.CartLine[] results = target.Lines.ToArray(); 

          //Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

         [TestMethod]
      public void Can_Add_Quantity_For_Existing_Lines()
      {
             //Arrange
          Product p1 = new Product { ProductID = 1, Name = "P1" };
          Product p2 = new Product { ProductID = 2, Name = "P2" };

          Cart target = new Cart();

             //Act
          target.AddItem(p1, 1);
          target.AddItem(p2, 1);
          target.AddItem(p1, 10);
          SportStore.Domain.Entities.Cart.CartLine[] result = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

             //Assert
          Assert.AreEqual(result.Length, 2);
          Assert.AreEqual(result[0].Quantity, 11);
          Assert.AreEqual(result[1].Quantity, 1);
      }

        [TestMethod]

         public void Can_Remove_Line()
         {
             Product p1 = new Product { ProductID = 1, Name = "P1" };
             Product p2 = new Product { ProductID = 2, Name = "P2" };
             Product p3 = new Product { ProductID = 3, Name = "P3" };

             Cart target = new Cart();

             target.AddItem(p1, 1);
             target.AddItem(p2, 3);
             target.AddItem(p3, 5);
             target.AddItem(p2, 1);

            //Act

             target.RemoveLine(p2);

            //Assert
             Assert.AreEqual(target.Lines.Where(c => c.Product == p2).Count(), 0);
             Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestMethod]

        public void Calculate_Cart_Total()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M};
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();

            //Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            //Assert
            Assert.AreEqual(result, 450M);
        }

        [TestMethod]

        public void Can_Clear_Contents()
        {
            //Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 2);

            //Act

            target.Clear();

            //Assert

            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]

        public void Can_Add_To_Cart()
        {
            //Arrange

            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ProductID = 1, Name ="P1", Category = "Apples"},
            }.AsQueryable());

            Cart cart = new Cart();

            CartController target = new CartController(mock.Object);

            //Act
            target.AddToCart(cart, 1, null);

            //Assert

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]

        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ProductID = 1, Name = "P1", Category = "Apples"}
            }.AsQueryable());

            Cart cart = new Cart();
            CartController target = new CartController(mock.Object);

            //Act
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]

        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();

            CartController target = new CartController(null);

            //Act
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            //Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");

        }
    }
}
