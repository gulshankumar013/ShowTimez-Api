using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;


namespace COMMON_PROJECT_STRUCTURE_API.services
{
    public class signup
    {
        dbServices ds = new dbServices();

        public async Task<responseData> Signup(requestData rData)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showTimez_user WHERE email=@email";
                MySqlParameter[] myParam = new MySqlParameter[]
                {
                    new MySqlParameter("@email", rData.addInfo["email"])
                };
                var dbData = ds.executeSQL(query, myParam);
                
                  if (dbData[0].Count() > 0)
                {
                    resData.rData["rMessage"] = "Duplicate Credentials";
                }
                else
                {
                    var sq = @"INSERT INTO pc_student.showTimez_user (name,mobile, email, password,gender,date_of_Birth) 
                               VALUES (@name, @mobile, @email, @password,@gender,@date_of_Birth)";
                    MySqlParameter[] insertParams = new MySqlParameter[]
                    {
                        new MySqlParameter("@NAME", rData.addInfo["name"]),
                         new MySqlParameter("@mobile", rData.addInfo["mobile"]),
                        new MySqlParameter("@EMAIL", rData.addInfo["email"]),
                        new MySqlParameter("@PASSWORD", rData.addInfo["password"]),
                        new MySqlParameter("@Gender", rData.addInfo["gender"]),
                        new MySqlParameter("@Date_of_Birth", rData.addInfo["date_of_Birth"]),

                        
                        
                    };
                    var insertResult = ds.executeSQL(sq, insertParams);

                    resData.rData["rMessage"] = "Signup Successful";
                }
            }
            catch (Exception ex)
            {
                resData.rData["rMessage"] = "An error occurred: " + ex.Message;
            }
            return resData;
        }


         public async Task<responseData> FetchAllUser(string details)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showTimez_user ORDER BY id DESC ";

                var dbData = ds.executeSQL(query, null);

                List<object> usersList = new List<object>();

                foreach (var rowSet in dbData)
                {
                    foreach (var row in rowSet)
                    {
                        List<string> rowData = new List<string>();

                        foreach (var column in row)
                        {
                            rowData.Add(column.ToString());
                        }

                        var user = new
                        {
                            id = rowData[0],
                            name = rowData[1],
                             mobile = rowData[2],
                            email = rowData[3],
                            password = rowData[4],
                            profile = rowData[5],
                            gender = rowData[6],
                            date_of_Birth = rowData[7],
                            
                           
                            
                        };

                        usersList.Add(user);
                    }
                }

                resData.rData["users"] = usersList;
                resData.rData["rMessage"] = "Successful";
            }
            catch (Exception ex)
            {
                resData.rData["rMessage"] = "Exception occurred: " + ex.Message;
            }

            return resData;
        }

          public async Task<responseData>FetchUser(requestData req)

 {
            responseData resData = new responseData();
            resData.rData["rCode"] = 0;
            try
            {
                var list = new ArrayList();
                MySqlParameter[] myParams = new MySqlParameter[] {
                new MySqlParameter("@email", req.addInfo["email"]),
                };
                var sq = $"SELECT * FROM pc_student.showTimez_user WHERE email=@email;";
                var data = ds.ExecuteSQLName(sq, myParams);

                if (data == null || data[0].Count() == 0)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "No user is present...";
                }
                else
                {

                    resData.rData["id"] = data[0][0]["id"];
                    resData.rData["name"] = data[0][0]["name"];
                    resData.rData["mobile"] = data[0][0]["mobile"];
                     resData.rData["email"] = data[0][0]["email"];
                    resData.rData["password"] = data[0][0]["password"];
                    resData.rData["gender"] = data[0][0]["gender"];
                    resData.rData["date_of_Birth"] = data[0][0]["date_of_Birth"];
                    
                    
                }

            }
            catch (Exception ex)
            {
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = ex.Message;

            }
            return resData;
        }
        public async Task<responseData> Totaluser(requestData req)
                {
                    responseData resData = new responseData();
                    try
                    {
                        var query = @"SELECT COUNT(*) FROM pc_student.showTimez_user";
                        var dbData = ds.executeSQL(query, null); // Assuming ExecuteQueryAsync is your method to run queries
                        int userCount = Convert.ToInt32(dbData[0][0][0]);
                        resData.rData["total_users"] = userCount;
                        resData.rData["rMessage"] = "Successful";
                    }
                    catch (Exception ex)
                    {
                        resData.rData["rMessage"] = "Exception occurred: " + ex.Message;
                    }
                    return resData;
                }

    }
}
