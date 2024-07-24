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
                            crewImage1 = rowData[18],
                            crewImage2 = rowData[19],
                            crewImage3 = rowData[20],
                            aboutMovie = rowData[21]



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

        public async Task<responseData> FetchMoviePlayingById(requestData rData)
        {
            responseData resData = new responseData();
            try
            {

                MySqlParameter[] myParam = new MySqlParameter[]
                {
                    new MySqlParameter("@id", rData.addInfo["id"]) ,// Ensure rData contains the id field
                    new MySqlParameter("@name", rData.addInfo["name"]) // Ensure rData contains the id field
                };

                var query = @"SELECT * FROM pc_student.showTimez_movies_playing WHERE id=@id or name=@name";
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
                                    image = rowData.ElementAtOrDefault(1),
                                    name = rowData.ElementAtOrDefault(2),
                                    discription = rowData.ElementAtOrDefault(3),
                                    movie_time = rowData.ElementAtOrDefault(4),
                                    castName1 = rowData.ElementAtOrDefault(5),
                                    castName2 = rowData.ElementAtOrDefault(6),
                                    castName3 = rowData.ElementAtOrDefault(7),
                                    castName4 = rowData.ElementAtOrDefault(8),
                                    castName5 = rowData.ElementAtOrDefault(9),
                                    castImage1 = rowData.ElementAtOrDefault(10),
                                    castImage2 = rowData.ElementAtOrDefault(11),
                                    castImage3 = rowData.ElementAtOrDefault(12),
                                    castImage4 = rowData.ElementAtOrDefault(13),
                                    castImage5 = rowData.ElementAtOrDefault(14),
                                    crewName1 = rowData.ElementAtOrDefault(15),
                                    crewName2 = rowData.ElementAtOrDefault(16),
                                    crewName3 = rowData.ElementAtOrDefault(17),
                                    crewImage1 = rowData.ElementAtOrDefault(18),
                                    crewImage2 = rowData.ElementAtOrDefault(19),
                                    crewImage3 = rowData.ElementAtOrDefault(20),
                                    aboutMovie = rowData.ElementAtOrDefault(21)



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
           public async Task<responseData> TotaMoviePlaying(requestData req)
                {
                    responseData resData = new responseData();
                    try
                    {
                        var query = @"SELECT COUNT(*) FROM  pc_student.showTimez_movies_playing";
                        var dbData = ds.executeSQL(query, null); // Assuming ExecuteQueryAsync is your method to run queries
                        int userCount = Convert.ToInt32(dbData[0][0][0]);
                        resData.rData["total_movie_playing"] = userCount;
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
    



