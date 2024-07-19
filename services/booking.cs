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

        public async Task<responseData>Booking(requestData rData)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showTimez_booking WHERE name=@name";
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
                    var sq = @"INSERT INTO pc_student.showTimez_booking  (name,image, seat_no, date,price,discription,movie_time,user_id) 
                               VALUES (@name, @image, @seat_no, @date,@price,@discription,@movie_time,@user_id)";
                    MySqlParameter[] insertParams = new MySqlParameter[]
                    {
                        new MySqlParameter("@name", rData.addInfo["name"]),
                         new MySqlParameter("@image", rData.addInfo["image"]),
                        new MySqlParameter("@seat_no", rData.addInfo["seat_no"]),
                        new MySqlParameter("@date", rData.addInfo["date"]),
                        new MySqlParameter("@price", rData.addInfo["price"]),
                        new MySqlParameter("@discription", rData.addInfo["discription"]),
                        new MySqlParameter("@movie_time", rData.addInfo["movie_time"]),
                        new MySqlParameter("@user_id", rData.addInfo["user_id"]),
                    };
                    var insertResult = ds.executeSQL(sq, insertParams);

                    resData.rData["rMessage"] = "oder added Successful";
                }
            }
            catch (Exception ex)
            {
                resData.rData["rMessage"] = "An error occurred: " + ex.Message;
            }
            return resData;
        }
    }
}