using System.Security.Cryptography;
using System.Text;
using la_brisa.DBOperations;
using la_brisa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Security.Policy;
using System.ComponentModel;

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
        [Route("signup")]
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
                hashPassword = sb.ToString();
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

        [Route("login")]
        [HttpPost]
        public async Task<string> Login([FromForm] Login user)
        {



            SqlConnection conn = new SqlConnection(@"Server=DESKTOP-OFRUC79;database=la_brisa;integrated security=true");
            string sqlExists = "select Email, Password, Name, ProfilePicture, PhoneNumber from dbo.Users where Email='" + user.Email + "'";

            SqlCommand sqlCommand = new SqlCommand(sqlExists, conn);
            conn.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            string exists = "NO";
            JObject job = new JObject();
            while (reader.Read())
            {

                var password = reader["Password"].ToString();
                var mobile = reader["PhoneNumber"].ToString();
                var Name = reader["Name"].ToString();
                var Picture = reader["ProfilePicture"].ToString();
                Console.WriteLine(Name);
                job.Add("Mobile", mobile);
                job.Add("Name", Name);
                job.Add("Picture", Picture);


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
                    hashPassword = sb.ToString();
                }
                Console.WriteLine(job);
                if (password == hashPassword)
                {
                    exists = "YES";
                    job.Add("exists", "YES");
                }
                else
                {
                    exists = "NO";
                    job.Add("exists", "NO");
                }





            }



            conn.Close();
            if (exists == "YES")
            {

                return job.ToString();
            }
            else
            {
                return job.ToString();
            }
        }


        //fetch holidays
        [Route("holidays")]
        [HttpGet]
        public async Task<string> Holidays()
        {
             
            SqlConnection conn = new SqlConnection(@"Server=DESKTOP-OFRUC79;database=la_brisa;integrated security=true");
            string sqlHolidays = "select * from dbo.Holidays";

            SqlCommand sqlCommand = new SqlCommand(sqlHolidays, conn);
            conn.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            JArray jarray = new JArray();
            while (reader.Read())
            {

                JObject job = new JObject();
                job.Add("HolidayName", reader["HolidayName"].ToString());
                job.Add("Location", reader["Location"].ToString());
                job.Add("StartDate", reader["StartDate"].ToString());
                job.Add("EndDate", reader["EndDate"].ToString());
                job.Add("Price", reader["Price"].ToString());
                job.Add("Image", reader["Image"].ToString());
                jarray.Add(job);

            }

            Console.WriteLine(jarray);

            conn.Close();
            

                return jarray.ToString();
            
        }


        [Route("add_holiday")]
        [HttpPost]
        public async Task<bool> AddHoliday([FromForm] Holiday holiday, IFormFile file)
        {

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            Console.WriteLine(filePath);
            var file_name = "/images/" + fileName;
            using (var fileSrteam = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileSrteam);

            }


            SqlConnection conn = new SqlConnection(@"Server=DESKTOP-OFRUC79;database=la_brisa;integrated security=true");
            string query = "insert into Holidays values(@HolidayName,@Location, @StartDate, @EndDate, @Price, @Image)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(new SqlParameter("@HolidayName", holiday.HolidayName));
            cmd.Parameters.Add(new SqlParameter("@Location", holiday.Location));
            cmd.Parameters.Add(new SqlParameter("@StartDate", holiday.StartDate));
            cmd.Parameters.Add(new SqlParameter("@EndDate", holiday.EndDate));
            cmd.Parameters.Add(new SqlParameter("@Price", holiday.Price));
            cmd.Parameters.Add(new SqlParameter("@Image", file_name));

            conn.Open();
            int noOfRowsAffected = cmd.ExecuteNonQuery();
            conn.Close();
            return noOfRowsAffected > 0 ? true : false;

        
    
        }

    }
}
