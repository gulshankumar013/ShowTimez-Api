using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Collections;

namespace COMMON_PROJECT_STRUCTURE_API.services
{
    public class theaterList
    {
        dbServices ds = new dbServices();

        public async Task<responseData>TheaterList(requestData rData)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showTimez_TheaterList WHERE name=@name";
                MySqlParameter[] myParam = new MySqlParameter[]
                {
                    new MySqlParameter("@name", rData.addInfo["name"])
                };
                var dbData = ds.executeSQL(query, myParam);
                
                if (dbData[0].Count() > 0)
                {
                    resData.rData["rMessage"] = "Theater already added";
                }
                else
                {
                    var sq = @"INSERT INTO pc_student.showTimez_TheaterList(name, location) 
                               VALUES ( @name, @location)";
                    MySqlParameter[] insertParams = new MySqlParameter[]
                    {
                        
                        new MySqlParameter("@name", rData.addInfo["name"]),
                        new MySqlParameter("@location", rData.addInfo["location"]),
                        
                    };
                    var insertResult = ds.executeSQL(sq, insertParams);

                    resData.rData["rMessage"] = "theater added successfully";
                }
            }
            catch (Exception ex)
            {
                resData.rData["rMessage"] = "An error occurred: " + ex.Message;
            }
            return resData;
        }

        public async Task<responseData>DeleteTheaterList(requestData rData)
        {

            responseData resData = new responseData();
           try
            {
                // Your delete query
                var query = @"DELETE FROM pc_student.showTimez_TheaterList WHERE id = @Id;";

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

         public async Task<responseData>UpdateTheaterList(requestData rData)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"UPDATE pc_student.giganexus_wishlist
                              SET  name = @name, location = @location
                              WHERE id = @id";
                MySqlParameter[] myParam = new MySqlParameter[]
                {
                    new MySqlParameter("@id", rData.addInfo["id"]),
                   
                     new MySqlParameter("@name", rData.addInfo["name"]),
                    new MySqlParameter("@location", rData.addInfo["location"]),
                    
                    
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

           public async Task<responseData> FetchTheaterList(string details)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showTimez_TheaterList ";

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
                            location = rowData[2],
                            distance = rowData[3],
                            
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
