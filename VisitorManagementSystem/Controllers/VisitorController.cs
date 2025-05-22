using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using VisitorManagementSystem.Models;
using System.Configuration;
using System.IO;

namespace VisitorManagementSystem.Controllers
{
    public class VisitorController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult Index()
        {
            List<Visitor> visitors = new List<Visitor>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Visitors", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    visitors.Add(new Visitor
                    {
                        Id = (int)rdr["Id"],
                        FullName = rdr["FullName"].ToString(),
                        Email = rdr["Email"].ToString(),
                        ContactNumber = rdr["ContactNumber"].ToString(),
                        VisitDate = Convert.ToDateTime(rdr["VisitDate"]),
                        Purpose = rdr["Purpose"].ToString(),
                        PhotoPath = rdr["PhotoPath"].ToString()
                    });
                }
            }
            return View(visitors);
        }

        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Visitor visitor, HttpPostedFileBase photoFile)
        {
            if (!ModelState.IsValid)
            {
                return View(visitor);
            }

            if (photoFile != null && photoFile.ContentLength > 0)
            {
                string fileName = Path.GetFileName(photoFile.FileName);
                string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(fileName); // ✅ Unique filename
                string path = Path.Combine(Server.MapPath("~/Uploads/"), uniqueName);

                // Ensure Uploads folder exists
                if (!Directory.Exists(Server.MapPath("~/Uploads/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/"));
                }

                photoFile.SaveAs(path);
                visitor.PhotoPath = "~/Uploads/" + uniqueName;
            }
            else
            {
                ModelState.AddModelError("Photo", "Please upload a photo.");
                return View(visitor);
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Visitors 
                        (FullName, Email, ContactNumber, VisitDate, Purpose, PhotoPath) 
                         VALUES 
                        (@FullName, @Email, @ContactNumber, @VisitDate, @Purpose, @PhotoPath)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FullName", visitor.FullName);
                cmd.Parameters.AddWithValue("@Email", visitor.Email);
                cmd.Parameters.AddWithValue("@ContactNumber", visitor.ContactNumber);
                cmd.Parameters.AddWithValue("@VisitDate", visitor.VisitDate);
                cmd.Parameters.AddWithValue("@Purpose", visitor.Purpose);
                cmd.Parameters.AddWithValue("@PhotoPath", visitor.PhotoPath ?? (object)DBNull.Value); // ✅ Safe

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }



        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Visitors WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Visitor visitor = new Visitor();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Visitors WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    visitor.Id = Convert.ToInt32(reader["Id"]);
                    visitor.FullName = reader["FullName"].ToString();
                    visitor.Email = reader["Email"].ToString();
                    visitor.ContactNumber = reader["ContactNumber"].ToString();
                    visitor.VisitDate = Convert.ToDateTime(reader["VisitDate"]);
                    visitor.Purpose = reader["Purpose"].ToString();
                    visitor.PhotoPath = reader["PhotoPath"].ToString();
                }
            }
            return View(visitor);
        }

        [HttpPost]
        public ActionResult Edit(Visitor visitor, HttpPostedFileBase photoFile)
        {
            if (ModelState.IsValid)
            {
                // 1. Fetch existing visitor to retain original photo if no new one is uploaded
                Visitor existingVisitor = null;
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    string getQuery = "SELECT PhotoPath FROM Visitors WHERE Id = @Id";
                    SqlCommand getCmd = new SqlCommand(getQuery, conn);
                    getCmd.Parameters.AddWithValue("@Id", visitor.Id);
                    conn.Open();
                    SqlDataReader reader = getCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        existingVisitor = new Visitor
                        {
                            PhotoPath = reader["PhotoPath"].ToString()
                        };
                    }
                    conn.Close();
                }

                // 2. If a new photo is uploaded, update it. Else retain the existing.
                if (photoFile != null && photoFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(photoFile.FileName);
                    string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                    string path = Path.Combine(Server.MapPath("~/Uploads/"), uniqueName);

                    photoFile.SaveAs(path);
                    visitor.PhotoPath = "~/Uploads/" + uniqueName;
                }
                else
                {
                    visitor.PhotoPath = existingVisitor?.PhotoPath; // Retain old photo
                }

                // 3. Update DB
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    string query = @"UPDATE Visitors 
                             SET FullName = @FullName, Email = @Email, 
                                 ContactNumber = @ContactNumber, VisitDate = @VisitDate, 
                                 Purpose = @Purpose, PhotoPath = @PhotoPath
                             WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FullName", visitor.FullName);
                    cmd.Parameters.AddWithValue("@Email", visitor.Email);
                    cmd.Parameters.AddWithValue("@ContactNumber", visitor.ContactNumber);
                    cmd.Parameters.AddWithValue("@VisitDate", visitor.VisitDate);
                    cmd.Parameters.AddWithValue("@Purpose", visitor.Purpose);
                    cmd.Parameters.AddWithValue("@PhotoPath", visitor.PhotoPath ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", visitor.Id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            return View(visitor);
        }


        public ActionResult Details(int id)
        {
            Visitor visitor = new Visitor();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Visitors WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    visitor.Id = Convert.ToInt32(reader["Id"]);
                    visitor.FullName = reader["FullName"].ToString();
                    visitor.Email = reader["Email"].ToString();
                    visitor.ContactNumber = reader["ContactNumber"].ToString();
                    visitor.VisitDate = Convert.ToDateTime(reader["VisitDate"]);
                    visitor.Purpose = reader["Purpose"].ToString();
                    visitor.PhotoPath = reader["PhotoPath"].ToString();
                }
            }
            return View(visitor);
        }

    }
}
