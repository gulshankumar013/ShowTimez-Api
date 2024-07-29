using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace COMMON_PROJECT_STRUCTURE_API.services
{
    public class booking
    {
        
    dbServices ds = new dbServices();

        public async Task<responseData> Booking(requestData rData)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showTimez_booking  WHERE name=@name";
                MySqlParameter[] myParam = new MySqlParameter[]
                {
                    new MySqlParameter("@name", rData.addInfo["name"])
                };
                var dbData = ds.executeSQL(query, myParam);
                
                  if (dbData[0].Count() > 0)
                {
                    resData.rData["rMessage"] = "Duplicate Credentials";
                }
                else
                {
                    var sq = @"INSERT INTO pc_student.showTimez_booking  (name,image,selectedSeats,totalAmount,discription,showTime,user_id,theaterName) 
                               VALUES (@name, @image,@selectedSeats,@totalAmount,@discription,@showTime,@user_id,@theaterName)";
                    MySqlParameter[] insertParams = new MySqlParameter[]
                    {
                         new MySqlParameter("@name", rData.addInfo["name"]),
                         new MySqlParameter("@image", rData.addInfo["image"]),
                         new MySqlParameter("@selectedSeats", rData.addInfo["selectedSeats"]),
                        new MySqlParameter("@totalAmount", rData.addInfo["totalAmount"]),
                        new MySqlParameter("@discription", rData.addInfo["discription"]),
                        new MySqlParameter("@showTime", rData.addInfo["showTime"]),
                        new MySqlParameter("@user_id", rData.addInfo["user_id"]),
                        new MySqlParameter("@theaterName", rData.addInfo["theaterName"])

                        
                        
                    };
                    var insertResult = ds.executeSQL(sq, insertParams);

                    resData.rData["rMessage"] = "Movie ticket Booked";
                }
            }
            catch (Exception ex)
            {
                resData.rData["rMessage"] = "An error occurred: " + ex.Message;
            }
            return resData;
        }
         public async Task<responseData> FetchBookingById(requestData rData)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showTimez_booking WHERE user_id=@user_id";

                MySqlParameter[] myParam = new MySqlParameter[]
                {
                    new MySqlParameter("@user_id", rData.addInfo["user_id"]) // Ensure rData contains the id field
                };

                var dbData = ds.executeSQL(query, myParam);

                if (dbData == null)
                {
                    resData.rData["rMessage"] = "Database query returned null";
                    resData.rStatus = 1; // Indicate error
                    return resData;
                }

                List<object> usersList = new List<object>();

                foreach (var rowSet in dbData)
                {
                    if (rowSet != null)
                    {
                        foreach (var row in rowSet)
                        {
                            if (row != null)
                            {
                                List<string> rowData = new List<string>();

                                foreach (var column in row)
                                {
                                    if (column != null)
                                    {
                                        rowData.Add(column.ToString());
                                    }
                                }

                                var user = new
                                {
                                    id = rowData.ElementAtOrDefault(0),
                                    name = rowData.ElementAtOrDefault(1),
                                    image = rowData.ElementAtOrDefault(2),
                                    selectedSeats = rowData.ElementAtOrDefault(3),
                                    totalAmount = rowData.ElementAtOrDefault(4),
                                    discription = rowData.ElementAtOrDefault(5),
                                    showTime = rowData.ElementAtOrDefault(6),
                                    user_id = rowData.ElementAtOrDefault(7),
                                    theaterName = rowData.ElementAtOrDefault(8),
                                

                                };

                                usersList.Add(user);
                            }
                        }
                    }
                }

                resData.rData["users"] = usersList;
                resData.rData["rMessage"] = "Successful";
                resData.rStatus = 0; // Indicate success
            }
            catch (Exception ex)
            {
                resData.rData["rMessage"] = "Exception occurred: " + ex.Message;
                resData.rStatus = 1; // Indicate error
            }

            return resData;
        }

         public async Task<responseData> GetMovieTicketDistribution(requestData rData)
    {
        responseData resData = new responseData();
        try
        {
            var query = @"SELECT name, COUNT(*) as ticket_count FROM pc_student.showTimez_booking GROUP BY name";
            var dbData = ds.executeSQL(query, null);
            
            if (dbData[0].Count() > 0)
            {
                resData.rData["movies"] = dbData;
            }
            else
            {
                resData.rData["rMessage"] = "No data available";
            }
        }
        catch (Exception ex)
        {
            resData.rData["rMessage"] = "An error occurred: " + ex.Message;
        }
        return resData;
      }
       public async Task<responseData> FetchAllTicket(string details)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showtimez_booking  ";

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
                            image = rowData[2],
                            selectedSeats = rowData[3],
                            totalAmount = rowData[4],
                            discription = rowData[5],
                            showTime = rowData[6],
                            theaterName = rowData[8],
                            
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

        public async Task<responseData> DeleteTicketId(requestData rData)
        {
            responseData resData = new responseData();

            try
            {
                // Your delete query
                var query = @"DELETE FROM pc_student.showtimez_booking  WHERE id = @Id;";

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
                resData.rData["rMessage"] = "Exception occurred: " + ex.Message;
            }
            return resData;
        }
    }
}