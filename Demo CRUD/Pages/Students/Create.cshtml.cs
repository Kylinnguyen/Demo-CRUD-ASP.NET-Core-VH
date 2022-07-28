using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
namespace Demo_CRUD.Pages.StudentsPage
{
    public class CreateModel : PageModel
    {
        public InforStudents inforStudents = new InforStudents();
        public String errorMessage = "";
        public String successMessage = "";

        // function to check valid phone number
        public bool isPhone(string number)
        {   
            const string pattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            if (number != null) 
                return Regex.IsMatch(number, pattern);
            else 
                return false;
        }
        // function to check valid email
        public bool isEmail(string mail)
        {
            const string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            if (mail != null)
                return Regex.IsMatch(mail, pattern);
            else
                return false;
        }

        public void OnGet()
        {

        }
        public void OnPost()
        {
            inforStudents.fullname = Request.Form["fullname"];
            inforStudents.email = Request.Form["email"];
            inforStudents.phone = Request.Form["phone"];
            inforStudents.faculty = Request.Form["faculty"];

            // Check condition of input
            if(inforStudents.fullname.Length == 0 || inforStudents.email.Length == 0
                || inforStudents.phone.Length == 0 || inforStudents.faculty.Length == 0)
            {
                errorMessage = "Please fill all fields!!!";
                return;
            }

            if (!isEmail(inforStudents.email))
            {
                errorMessage = "Invalid mail!!!";
                return;
            }

            if (!isPhone(inforStudents.phone))
            {
                errorMessage = "Invalid phone number!!!";
                return;
            }

            // save new student into db
            try 
            {
                String conString = "Data Source=DESKTOP-14TK5R3\\KYLINNGUYEN;Initial Catalog=DemoCRUD;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();
                    String insert_query = "INSERT INTO Students(fullname, email, phone, faculty) VALUES(@fullname, @email, @phone, @faculty);";
                    using (SqlCommand cmd = new SqlCommand(insert_query, connection))
                    {
                        cmd.Parameters.AddWithValue("fullname", inforStudents.fullname);
                        cmd.Parameters.AddWithValue("email", inforStudents.email);
                        cmd.Parameters.AddWithValue("phone", inforStudents.phone);
                        cmd.Parameters.AddWithValue("faculty", inforStudents.faculty);

                        cmd.ExecuteNonQuery();
                    }

                }


            } 
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            // after save to db, clear input field
            inforStudents.fullname = "";
            inforStudents.email = "";
            inforStudents.phone = "";
            inforStudents.faculty = "";
            successMessage = "Add new student successfully!";

            // Redirect to Students page after add successfully
            Response.Redirect("/Students/Index");

        }
    }
}
