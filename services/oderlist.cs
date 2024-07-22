using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace COMMON_PROJECT_STRUCTURE_API.services
{
    public class orderlist
    {
        dbServices ds = new dbServices();
        public async Task<responseData> Orderlist(requestData req)
        {
            responseData resData = new responseData();
            try
            {
                
                MySqlParameter[] insertParams = new MySqlParameter[]
              {
                         new MySqlParameter("@name", req.addInfo["name"]),
                         new MySqlParameter("@image", req.addInfo["image"]),
                         new MySqlParameter("@selectedSeats", req.addInfo["selectedSeats"]),
                         new MySqlParameter("@totalAmount", req.addInfo["totalAmount"]),
                         new MySqlParameter("@discription", req.addInfo["discription"]),
                         new MySqlParameter("@showTime", req.addInfo["showTime"]),
                          new MySqlParameter("@user_id", req.addInfo["user_id"]),
                           new MySqlParameter("@theaterName", req.addInfo["theaterName"]),
                        
              };
                var sq = @"insert into pc_student.showTimez_booking(name,image,selectedSeats,totalAmount,discription,showTime,user_id,theaterName) 
                values(@name,@image,@selectedSeats,@totalAmount,@discription,@showTime,@user_id,@theaterName)";

                var insertResult = ds.executeSQL(sq, insertParams);
                if (insertResult[0].Count() == null)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Failed to receive oder ";
                }
                else
                {
                    resData.rData["rCode"] = 0;
                    resData.rData["rMessage"] = "Order receive successfully";

                }
            }
            catch (Exception ex)
            {
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = "An error occurred: " + ex.Message;
            }
            return resData;
        }
    }
}   