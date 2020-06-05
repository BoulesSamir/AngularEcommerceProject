using Microsoft.AspNet.Identity;
using SouqyApi.DTO;
using SouqyApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Deployment.Internal;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace SouqyApi.Controllers
{
    [Authorize]
    public class ProductController : ApiController
    {
        public ApplicationDbContext context=new ApplicationDbContext();
        [AllowAnonymous]
        public IHttpActionResult getAllProduct()
        {
            List<ProductDTO> products = new List<ProductDTO>();
            foreach (var item in context.Product.Include(c=>c.owner).ToList())
            {
                products.Add(new ProductDTO
                {
                    ID = item.ID,
                    Name = item.Name,
                    quantity = item.quantity,
                    price = item.price,
                    img = item.img,
                    OwnerName = item.owner.FullName,
                    Description=item.Description
                    
                });
            }
            return Ok(products);
        }
        [Route("Api/getProductUser")]
        public IHttpActionResult getProductByUser()
        {
            string id = ClaimsPrincipal.Current.Identity.GetUserId();
            var prod = context.Product.Where(p => p.ownerId ==id ).ToList();
            List<ProductDTO> products = new List<ProductDTO>();
            foreach (var item in prod)
            {
                products.Add(new ProductDTO
                {
                    ID = item.ID,
                    Name = item.Name,
                    quantity = item.quantity,
                    price = item.price,
                    img = item.img,
                    Description = item.Description,
                    OwnerName = item.owner.FullName,
                    

                });
            }
            return Ok(products);
        }
        [AllowAnonymous]
        public IHttpActionResult getProductById(int id)
        {
            var prod = context.Product.FirstOrDefault(s => s.ID == id);
            ProductDTO product = new ProductDTO()
            {
                ID = prod.ID,
                Name = prod.Name,
                quantity = prod.quantity,
                price = prod.price,
                img = prod.img,
                Description=prod.Description,
                OwnerName = prod.owner.FullName
            };
            return Ok(product);
        }
        public IHttpActionResult postProduct()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = new Product();
            string PathImage;
            var httpRequest = HttpContext.Current.Request;
            var postedFile = httpRequest.Files["Img"];
            //product.ID =int.Parse( httpRequest["ID"]);
            product.Name = httpRequest["Name"];
            product.price = double.Parse(httpRequest["Price"]);
            product.Description= httpRequest["Description"];
            product.quantity = int.Parse(httpRequest["Quantity"]);
           
            product.ownerId = ClaimsPrincipal.Current.Identity.GetUserId();
             PathImage = DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            string filePath = "";
            filePath = HttpContext.Current.Server.MapPath("~/Images/" + PathImage);
            product.img= PathImage;
            postedFile.SaveAs(filePath);
            context.Product.Add(product);

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = product.ID }, product);


        }
        public IHttpActionResult putProduct(int id)
        {
            Product product = context.Product.Find(id);
            string PathImage;
            var httpRequest = HttpContext.Current.Request;
            product.Name = httpRequest["Name"];
            product.price = double.Parse(httpRequest["Price"]);
            product.Description = httpRequest["Description"];
            product.quantity = int.Parse(httpRequest["Quantity"]);
            product.ownerId = ClaimsPrincipal.Current.Identity.GetUserId();
            if (httpRequest.Files["Img"]!=null) {
                var postedFile = httpRequest.Files["Img"];
                PathImage = DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                string filePath = "";
                filePath = HttpContext.Current.Server.MapPath("~/Images/" + PathImage);
                product.img = PathImage;
                postedFile.SaveAs(filePath);
            }
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {

                return BadRequest();
              
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
        [Route("Api/cart")]
        public IHttpActionResult putQuantity()
        {
            var httpRequest = HttpContext.Current.Request;
            
            int id = int.Parse(httpRequest["id"]);
            int quantity=int.Parse(httpRequest["quantity"]) ;
            var product = context.Product.FirstOrDefault(p => p.ID == id);
            product.quantity = product.quantity - quantity;
            if (product.quantity >= 0)
            {
                context.SaveChanges();
                return (Ok());
            }
            return BadRequest();

        }
        public IHttpActionResult deleteProduct(int id)
        {
            var product = context.Product.FirstOrDefault(c => c.ID == id);
            if(product==null)
            {
                return NotFound();
            }
            try
            {
                context.Product.Remove(product);
                context.SaveChanges();
                return Ok("DElete Sucess");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private bool ProductExists(int id)
        {
            return context.Product.Count(e => e.ID == id) > 0;
        }
    }
}
