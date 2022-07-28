using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace Demo_CRUD.Pages.StudentsPage;

public class IndexModel : PageModel
{   
    public List<InforStudents> student_list = new List<InforStudents>();
    public void OnGet()
    {
        try
        {
            String conString = "Data Source=DESKTOP-14TK5R3\\KYLINNGUYEN;Initial Catalog=DemoCRUD;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(conString))
            {   
                connection.Open();
                String query = "Select * from Students";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InforStudents student = new InforStudents();
                            student.id = "" + reader.GetInt32(0); //convert integer into str
                            student.fullname = reader.GetString(1);
                            student.email = reader.GetString(2);
                            student.phone = reader.GetString(3);
                            student.faculty = reader.GetString(4);
                            student.created_at = reader.GetDateTime(5).ToString();

                            //add obj to list
                            student_list.Add(student);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine("ex:" + ex.ToString());
        }
    }
}

public class InforStudents
{
    public String id;
    public String fullname;
    public String email;
    public String phone;
    public String faculty;
    public String created_at;
}
