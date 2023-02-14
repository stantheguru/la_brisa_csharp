using System.Security.Cryptography;
using System.Text;
using la_brisa.DBOperations;
using la_brisa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace la_brisa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _iuser;
        public UserController(IUser iuser)
        {
            _iuser = iuser;
        }

        [HttpGet]

        public async Task<IEnumerable<User>> GetUsers()

        {

            return await _iuser.Get();

        }

        [HttpPost]
       

        public async Task<bool> Post([FromForm] User user)
        {
            //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);

            /*var fileName = Path.GetFileName(photo.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Media", fileName);
            var file_name = "/Media/" + fileName;
            using (var fileSrteam = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(fileSrteam);

            }*/
            string hashPassword = "";
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(user.Password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                hashPassword =  sb.ToString();
            }

            

            SqlConnection conn = new SqlConnection(@"Server=DESKTOP-OFRUC79;database=la_brisa;integrated security=true");
            string sqlExists = "select Email from dbo.Users where Email='" + user.Email + "'";

            SqlCommand sqlCommand = new SqlCommand(sqlExists, conn);
            conn.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            string exists = "NO";
            while (reader.Read())
            {
                JObject obj = new JObject();
                var message = reader["Email"].ToString();
                if (message == "")
                {
                    exists = "NO";
                }
                else
                {
                    exists = "YES";
                }
                

            }
            conn.Close();
            if (exists == "NO") {
                string query = "insert into Users values(@Name,@Email, @Password, @PhoneNumber, @ProfilePicture)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add(new SqlParameter("@Name", user.Name));
                cmd.Parameters.Add(new SqlParameter("@Email", user.Email));
                cmd.Parameters.Add(new SqlParameter("@Password", hashPassword));
                cmd.Parameters.Add(new SqlParameter("@PhoneNumber", user.PhoneNumber));
                cmd.Parameters.Add(new SqlParameter("@ProfilePicture", ""));
                conn.Open();
                int noOfRowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                return noOfRowsAffected > 0 ? true : false;
            }
            else
            {
                Console.WriteLine(exists);
                return false;
            }
        }
    }
}
