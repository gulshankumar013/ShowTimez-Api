using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace COMMON_PROJECT_STRUCTURE_API.services
{
    public class moviePlaying
    {
        dbServices ds = new dbServices();
        public async Task<responseData> MoviePlaying(requestData req)
        {
            responseData resData = new responseData();
            try
            {
                

                MySqlParameter[] insertParams = new MySqlParameter[]
              {
                        
                         new MySqlParameter("@image", req.addInfo["image"]),
                         new MySqlParameter("@name", req.addInfo["name"]),
                         new MySqlParameter("@discription", req.addInfo["discription"]),
                         new MySqlParameter("@movie_time", req.addInfo["movie_time"]),
                         new MySqlParameter("@castName1", req.addInfo["castName1"]),
                         new MySqlParameter("@castName2", req.addInfo["castName2"]),
                         new MySqlParameter("@castName3", req.addInfo["castName3"]),
                         new MySqlParameter("@castName4", req.addInfo["castName4"]),
                         new MySqlParameter("@castName5", req.addInfo["castName5"]),
                         new MySqlParameter("@castImage1", req.addInfo["castImage1"]),
                         new MySqlParameter("@castImage2", req.addInfo["castImage2"]),
                         new MySqlParameter("@castImage3", req.addInfo["castImage3"]),
                         new MySqlParameter("@castImage4", req.addInfo["castImage4"]),
                         new MySqlParameter("@castImage5", req.addInfo["castImage5"]),
                         new MySqlParameter("@crewName1", req.addInfo["crewName1"]),
                         new MySqlParameter("@crewName2", req.addInfo["crewName2"]),
                         new MySqlParameter("@crewName3", req.addInfo["crewName3"]),
                         new MySqlParameter("@crewImage1", req.addInfo["crewImage1"]),
                         new MySqlParameter("@crewImage2", req.addInfo["crewImage2"]),
                         new MySqlParameter("@crewImage3", req.addInfo["crewImage3"]),
                         new MySqlParameter("@aboutMovie", req.addInfo["aboutMovie"]),
                         
              };
                var sq = @"insert into pc_student.showTimez_movies_playing(image,name,discription,movie_time,castName1,castName2,castName3,castName4,castName5,castImage1,castImage2,castImage3,castImage4,castImage5,crewName1,crewName2,crewName3,crewImage1,crewImage2,crewImage3,aboutMovie) 
                values(@image,@name,@discription,@movie_time,@castName1,@castName2,@castName3,@castName4,@castName5,@castImage1,@castImage2,@castImage3,@castImage4,@castImage5,@crewName1,@crewName2,@crewName3,@crewImage1,@crewImage2,@crewImage3,@aboutMovie)";

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
        

        public async Task<responseData>DeleteMoviePlaying(requestData rData)
        {

            responseData resData = new responseData();
           try
            {
                // Your delete query
                var query = @"DELETE FROM pc_student.showTimez_movies_playing WHERE id = @Id;";

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

         public async Task<responseData> UpdateMoviePlaying(requestData rData)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"UPDATE pc_student.showTimez_movies_playing
                              SET image = @image, name = @name, discription = @discription, price = @price
                              WHERE id = @id";
                MySqlParameter[] myParam = new MySqlParameter[]
                {
                    new MySqlParameter("@image", rData.addInfo["image"]),
                    new MySqlParameter("@name", rData.addInfo["name"]),
                    new MySqlParameter("@discription", rData.addInfo["discription"]),
                    new MySqlParameter("@price", rData.addInfo["price"]),
                    new MySqlParameter("@id", rData.addInfo["id"])
                };

                int rowsAffected = ds.ExecuteUpdateSQL(query, myParam);

                if (rowsAffected > 0)
                {
                    resData.rData["rMessage"] = "Update successful.";
                }
                else
                {
                    resData.rData["rMessage"] = "No rows affected. Update failed.";
                }
            }
            catch (Exception ex)
            {
                resData.rData["rMessage"] = "Exception occurred: " + ex.Message + ex;
            }
            return resData;
        }

         public async Task<responseData> FetchAllMoviePlaying(string details)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showTimez_movies_playing ORDER BY id DESC ";

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
                            price = rowData[4],
                            brand = rowData[5],
                            about = rowData[6],
                            specifications = rowData[7]
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
    }
}
