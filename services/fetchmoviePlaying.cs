using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;

namespace COMMON_PROJECT_STRUCTURE_API.services
{
    public class fetchmoviePlaying
    {
        dbServices ds = new dbServices();
        
    
       public async Task<responseData> FetchMoviePlaying(string details)
        {
            responseData resData = new responseData();
            try
            {
                var query = @"SELECT * FROM pc_student.showTimez_movies_playing  ";

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
                            movie_time = rowData[4],
                           castName1 = rowData[5],
                           castName2 = rowData[6],
                           castName3 = rowData[7],
                           castName4 = rowData[8],
                           castName5 = rowData[9],
                           castImage1 = rowData[10],
                           castImage2 = rowData[11],
                           castImage3 = rowData[12],
                           castImage4 = rowData[13],
                           castImage5 = rowData[14],
                           crewName1 = rowData[15],
                           crewName2 = rowData[16],
                           crewName3 = rowData[17],
                           aboutMovie = rowData[18],
                          
                           
                            
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
