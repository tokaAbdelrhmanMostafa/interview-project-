using interview2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; // ✅ استخدم المكتبة الحديثة بدل System.Data.SqlClient
using System.Collections.Generic;

namespace interview2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _connectionString;

        public EmployeeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EmployeeDBConnection") ?? throw new ArgumentNullException("Connection string not found!");
        }

        // ✅ READ - عرض جميع الموظفين
        public IActionResult Index()
        {
            List<Employee> EMPLOYEE = new List<Employee>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM EMPLOYEE", con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    EMPLOYEE.Add(new Employee
                    {
                        ID = (int)reader["ID"],
                       NAME = reader["NAME"].ToString(),
                        POSITION = reader["POSITION"].ToString(),
                        SALARY = (decimal)reader["SALARY"],
                        DEPARTMENT = reader["DEPARTMENT"].ToString()
                    });
                }
            }

            return View(EMPLOYEE);
        }

      
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ✅ CREATE - تنفيذ الإضافة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO EMPLOYEE (NAME, POSITION, SALARY, DEPARTMENT) VALUES (@NAME, @POSITION, @SALARY, @DEPARTMENT)",
                        con);

                    cmd.Parameters.AddWithValue("@NAME", emp.NAME);
                    cmd.Parameters.AddWithValue("@POSITION", emp.POSITION);
                    cmd.Parameters.AddWithValue("@SALARY", emp.SALARY);
                    cmd.Parameters.AddWithValue("@DEPARTMENT", emp.DEPARTMENT);
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        // ✅ UPDATE - عرض صفحة تعديل موظف
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee emp = new Employee();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM EMPLOYEE WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    emp.ID = (int)reader["ID"];
                    emp.NAME = reader["NAME"].ToString();
                    emp.POSITION = reader["POSITION"].ToString();
                    emp.SALARY = (decimal)reader["SALARY"];
                    emp.DEPARTMENT = reader["DEPARTMENT"].ToString();
                }
            }

            return View(emp);
        }

        // ✅ UPDATE - تنفيذ التعديل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE EMPLOYEE SET NAME = @NAME, POSITION = @POSITION, SALARY = @SALARY, DEPARTMENT = @DEPARTMENT WHERE ID = @ID",
                        con);

                    cmd.Parameters.AddWithValue("@ID", emp.ID);
                    cmd.Parameters.AddWithValue("@NAME", emp.NAME);
                    cmd.Parameters.AddWithValue("@POSITION", emp.POSITION);
                    cmd.Parameters.AddWithValue("@SALARY", emp.SALARY);
                    cmd.Parameters.AddWithValue("@DEPARTMENT", emp.DEPARTMENT);
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            return View(emp);
        }

        // ✅ DELETE - تأكيد الحذف (عرض)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Employee emp = new Employee();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM EMPLOYEE WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    emp.ID = (int)reader["ID"];
                    emp.NAME = reader["NAME"].ToString();
                    emp.POSITION = reader["POSITION"].ToString();
                    emp.SALARY = (decimal)reader["SALARY"];
                    emp.DEPARTMENT = reader["DEPARTMENT"].ToString();
                }
            }

            return View(emp);
        }

        // ✅ DELETE - تنفيذ الحذف
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM EMPLOYEE WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
