
using PR8.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace PR8.Controllers
{
    public class ProductController : Controller
    {
        string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=InventoryDB;Integrated Security=True";

        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Products", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Price = Convert.ToDecimal(dr["Price"])
                    });
                }
            }

            return View(products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product p)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Products
                    (ProductName, Quantity, Price)
                    VALUES (@ProductName, @Quantity, @Price)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Products WHERE ProductId=@ProductId", con);

                cmd.Parameters.AddWithValue("@ProductId", id);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        Product p = new Product
                        {
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = dr["ProductName"].ToString(),
                            Quantity = Convert.ToInt32(dr["Quantity"]),
                            Price = Convert.ToDecimal(dr["Price"])
                        };

                        return View(p);
                    }
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product p)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE Products SET
                    ProductName=@ProductName,
                    Quantity=@Quantity,
                    Price=@Price
                    WHERE ProductId=@ProductId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", p.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Products WHERE ProductId=@ProductId", con);

                cmd.Parameters.AddWithValue("@ProductId", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
