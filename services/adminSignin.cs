
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using MySql.Data.MySqlClient;
// using System;

// namespace COMMON_PROJECT_STRUCTURE_API.services
// {
//     public class adminSignin
//     {
//         dbServices ds = new dbServices();

//         public async Task<responseData>AdminSignin(requestData rData)
//         {
//             responseData resData = new responseData();
//             try
//             {
//                 var query = @"SELECT * FROM pc_student.giganexus_admin WHERE email=@email AND password=@password";
//                 MySqlParameter[] myParam = new MySqlParameter[]
//                 {
//                     new MySqlParameter("@EMAIL", rData.addInfo["email"]),
//                     new MySqlParameter("@PASSWORD", rData.addInfo["password"])
//                 };
//                 var dbData = ds.executeSQL(query, myParam);
                
//                 if (dbData[0].Count() > 0)
//                 {
//                     resData.rData["rMessage"] = "Signin Successful";
//                     // Add any additional data to response if needed, e.g., user details
//                 }
//                 else
//                 {
//                     resData.rData["rMessage"] = "Invalid Email id And Password ";
//                 }
//             }
//             catch (Exception ex)
//             {
//                 resData.rData["rMessage"] = "An error occurred: " + ex.Message;
//             }
//             return resData;
//         }

        
//     }
// }

// using System;
// using System.Collections.Generic;
// using System.IdentityModel.Tokens.Jwt;
// using System.Linq;
// using System.Security.Claims;
// using System.Text;
// using System.Text.RegularExpressions;
// using System.Threading.Tasks;
// using Microsoft.IdentityModel.Tokens;
// using MySql.Data.MySqlClient;


// namespace COMMON_PROJECT_STRUCTURE_API.services
// {
//     public class adminSignin
//     {
//         dbServices ds = new dbServices();
//         decryptService cm = new decryptService();

//         private readonly Dictionary<string, string> jwt_config = new Dictionary<string, string>();
//         private readonly Dictionary<string, string> _service_config = new Dictionary<string, string>();

      
//         public async Task<responseData> AdminSignin(requestData req)
//         {
//             responseData resData = new responseData();
//             resData.rData["rCode"] = 0;
//             try
//             {

//                 string input = req.addInfo["email"].ToString();
//                 bool isEmail_Id = IsValidEmail_Id(input);

//                 string columnName;


//                 if (isEmail_Id)
//                 {
//                     columnName = "email";
//                 }



//                 MySqlParameter[] para = new MySqlParameter[]
//                 {
//                     new MySqlParameter("@email", req.addInfo["email"].ToString()),
//                     new MySqlParameter("@password", req.addInfo["password"].ToString())

//                  };

//                 var checkSql = $"SELECT * FROM pc_student.showTimez_admin WHERE email=@email and password =@password";
//                 var checkResult = ds.ExecuteSQLName(checkSql, para);
//                 if (checkResult[0].Count() != 0)
//                 {
//                     var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt_config["Key"]));
//                     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
//                     var Sectoken = new JwtSecurityToken(jwt_config["Issuer"],
//                       jwt_config["Issuer"],
//                       null,
//                       expires: DateTime.Now.AddMinutes(120),
//                       signingCredentials: credentials);
//                     var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

//                     resData.eventID = req.eventID;
//                     resData.rData["rCode"] = 0;
//                     resData.rData["token"] = token;
//                     resData.rData["rMessage"] = "Login Successfully, Welcome!";
//                 }
//                 else
//                 {


//                     resData.rData["rCode"] = 1;
//                     resData.rData["rMessage"] = "Invalid Credentials";
//                 }
//                 // resData.rData["rMessage"] = "LogIn Successfully";



//             }
//             catch (Exception ex)
//             {
//                 resData.rData["rCode"] = 1;
//                 resData.rData["rMessage"] = ex.Message;
//             }
//             return resData;
//         }

//         public static bool IsValidEmail_Id(string email)
//         {
//             string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
//             return Regex.IsMatch(email, pattern);
//         }
//     }
// }           



using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace COMMON_PROJECT_STRUCTURE_API.services
{
    public class adminSignin
    {
        private readonly Dictionary<string, string> jwt_config = new Dictionary<string, string>
        {
            { "Key", "your_secret_key_here" },
            { "Issuer", "your_issuer_here" }
        };

        public async Task<responseData> AdminSignin(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"] = 0;
            try
            {
                // Validate email format
                string email = req.addInfo["email"].ToString();
                string password = req.addInfo["password"].ToString();
                bool isEmail_Id = IsValidEmail_Id(email);

                if (isEmail_Id && email == "gulshan@gmail.com" && password == "202cb962ac59075b964b07152d234b70")
                {
                    // Static credentials matched
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt_config["Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var tokenDescriptor = new JwtSecurityToken(
                        jwt_config["Issuer"],
                        jwt_config["Issuer"],
                        null,
                        expires: DateTime.Now.AddMinutes(120),
                        signingCredentials: credentials);
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

                    resData.eventID = req.eventID;
                    resData.rData["rCode"] = 0;
                    resData.rData["token"] = token;
                    resData.rData["rMessage"] = "Login Successfully, Welcome!";
                }
                else
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Invalid Credentials";
                }
            }
            catch (Exception ex)
            {
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = ex.Message;
            }
            return resData;
        }

        public static bool IsValidEmail_Id(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
        
    }
}
