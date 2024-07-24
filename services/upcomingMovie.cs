using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace COMMON_PROJECT_STRUCTURE_API.services
{
    public class upcomingMovie
    {
        dbServices ds = new dbServices();

        public async Task<responseData> UpcomingMovie(requestData req)
        {
            responseData resData = new responseData();
            try
            {
                

                MySqlParameter[] insertParams = new MySqlParameter[]
              {
                        
                         new MySqlParameter("@image", req.addInfo["image"]),
                         new MySqlParameter("@name", req.addInfo["name"]),
                         new MySqlParameter("@discription", req.addInfo["discription"]),
                        
              };
                var sq = @"insert into  pc_student.showTimez_upcomingMovie(image,name,discription) values(@image,@name,@discription)";

                var insertResult = ds.executeSQL(sq, insertParams);
                if (insertResult[0].Count() == null)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Failed to create card";
                }
                else
                {
                    resData.rData["rCode"] = 0;
                    resData.rData["rMessage"] = "card created successfully";

                }
            }
            catch (Exception ex)
            {
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = "An error occurred: " + ex.Message;
            }
            return resData;
        }

         public async Task<responseData>FetchUpcomingMovie(string details)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showTimez_upcomingMovie  ";

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
                            image = rowData[1],
                            name = rowData[2],
                            discription = rowData[3],
                           
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

         public async Task<responseData>DeleteUpcomingMovie(requestData rData)
        {

            responseData resData = new responseData();
           try
            {
                // Your delete query
                var query = @"DELETE FROM pc_student.showTimez_upcomingMovie WHERE id = @Id;";

                // Your parameters
                MySqlParameter[] myParam = new MySqlParameter[]
                {
                    new MySqlParameter("@Id", rData.addInfo["id"])
                };

                // Condition to execute the delete query
                bool shouldExecuteDelete = true;

                if (shouldExecuteDelete)
                {
                    int rowsAffected = ds.ExecuteUpdateSQL(query, myParam);

                    if (rowsAffected > 0)
                    {
                        resData.rData["rMessage"] = "DELETE SUCCESSFULLY.";
                    }
                    else
                    {
                        resData.rData["rMessage"] = "No rows affected. Delete failed.";
                    }
                }
                else
                {
                    resData.rData["rMessage"] = "Condition not met. Delete query not executed.";
                }
            }
            catch (Exception ex)
            {
                resData.rData["rMessage"] = "Exception occurred: " + ex.Message  + ex;
            }
            return resData;
        }


         public async Task<responseData> TotalCommingPlaying(requestData req)
                {
                    responseData resData = new responseData();
                    try
                    {
                        var query = @"SELECT COUNT(*) FROM  pc_student.showTimez_upcomingMovie";
                        var dbData = ds.executeSQL(query, null); // Assuming ExecuteQueryAsync is your method to run queries
                        int userCount = Convert.ToInt32(dbData[0][0][0]);
                        resData.rData["total_coming_movie"] = userCount;
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