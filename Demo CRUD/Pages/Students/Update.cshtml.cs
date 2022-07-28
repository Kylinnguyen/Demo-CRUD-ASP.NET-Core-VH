using Demo_CRUD.Pages.StudentsPage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
namespace Demo_CRUD.Pages.Students
{
    public class UpdateModel : PageModel
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
            String id = Request.Query["id"];
            try
            {
                String conString = "Data Source=DESKTOP-14TK5R3\\KYLINNGUYEN;Initial Catalog=DemoCRUD;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();
                    String query = "SELECT * FROM Students WHERE id=@id";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {   
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {

                                inforStudents.id = "" + reader.GetInt32(0); //convert integer into str
                                inforStudents.fullname = reader.GetString(1);
                                inforStudents.email = reader.GetString(2);
                                inforStudents.phone = reader.GetString(3);
                                inforStudents.faculty = reader.GetString(4);
                            }
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

        }

        public void OnPost()
        {
            inforStudents.id = Request.Form["id"];
            inforStudents.fullname = Request.Form["fullname"];
            inforStudents.email = Request.Form["email"];
            inforStudents.phone = Request.Form["phone"];
            inforStudents.faculty = Request.Form["faculty"];

            // Check condition of input
            if (inforStudents.fullname.Length == 0 || inforStudents.email.Length == 0
                || inforStudents.phone.Length == 0 || inforStudents.faculty.Length == 0)
            {
                errorMessage = "Please fill all fields!!!";
                return;
            }

            // validate mail
            if (!isEmail(inforStudents.email))
            {
                errorMessage = "Invalid mail!!!";
                return;
            }

            // validate phone
            if (!isPhone(inforStudents.phone))
            {
                errorMessage = "Invalid phone number!!!";
                return;
            }

            // update and save into db
            try
            {
                String conString = "Data Source=DESKTOP-14TK5R3\\KYLINNGUYEN;Initial Catalog=DemoCRUD;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();
                    String insert_query = "UPDATE Students SET fullname = @fullname, email=@email, phone=@phone, faculty=@faculty WHERE id=@id;";
                    using (SqlCommand cmd = new SqlCommand(insert_query, connection))
                    {
                        cmd.Parameters.AddWithValue("id", inforStudents.id);
                        cmd.Parameters.AddWithValue("fullname", inforStudents.fullname);
                        cmd.Parameters.AddWithValue("email", inforStudents.email);
                        cmd.Parameters.AddWithValue("phone", inforStudents.phone);
                        cmd.Parameters.AddWithValue("faculty", inforStudents.faculty);

                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch(Exception ex)
            {
                errorMessage=ex.Message;
                return;
            }

            // redirect to students page after update infor successfully
            Response.Redirect("/Students/Index");
        }
    }
}
