using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text.Json;
using System.Net;
using System.Net.Mail;

namespace COMMON_PROJECT_STRUCTURE_API.services
{
   public class otp
{

    public  dbServices ds = new dbServices();
    
   public async Task<responseData> OtpGenerate(requestData rData)
		{
			responseData resData = new responseData();

			string connectionString = "server=210.210.210.50;user=test_user;password=test*123;port=2020;database=pc_student; Max Pool Size=200;";
			string gmailUsername = "gulshankumar013013@gmail.com"; // My Gmail address
			string gmailPassword = "phls uipe krkm wyt"; // My Gmail password

			try
			{
				string email = rData.addInfo["EMAIL"].ToString();

				// Check if the email exists in the database
				var checkEmailQuery = @"SELECT COUNT(*) FROM pc_student.student_data WHERE EMAIL=@EMAIL";
				using (var connection = new MySqlConnection(connectionString))
				{
					await connection.OpenAsync();

					using (var cmd = new MySqlCommand(checkEmailQuery, connection))
					{
						cmd.Parameters.AddWithValue("@EMAIL", email);

						// Execute the query and read the resultS
						object countObj = await cmd.ExecuteScalarAsync();

						if (countObj != null && countObj != DBNull.Value)
						{
							if (int.TryParse(countObj.ToString(), out int count))
							{
								if (count > 0)
								{
									// Generate OTP
									string otp = Generate();

									// Save OTP to the database
									var insertQuery = @"INSERT INTO OTP_gen (EMAIL, OTP) VALUES (@EMAIL, @OTP)";
									using (var insertCmd = new MySqlCommand(insertQuery, connection))
									{
										insertCmd.Parameters.AddWithValue("@EMAIL", email);
										insertCmd.Parameters.AddWithValue("@OTP", otp);
										await insertCmd.ExecuteNonQueryAsync();
									}

									// Send OTP via email using Gmail SMTP
									using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
									{
										client.UseDefaultCredentials = false;
										client.Credentials = new NetworkCredential(gmailUsername, gmailPassword);
										client.EnableSsl = true;

										MailMessage mailMessage = new MailMessage();
										mailMessage.From = new MailAddress(gmailUsername);
										mailMessage.To.Add(email);
										mailMessage.Subject = "OTP Verification";
										mailMessage.Body = "Your OTP is: " + otp;

										await client.SendMailAsync(mailMessage);
									}


									resData.rData["rMessage"] = "OTP generated successfully and sent to " + email + " - otp - " + otp;
								}
								else
								{
									resData.rData["rMessage"] = "Email not found in our records.";
								}
							}
							else
							{
								resData.rData["rMessage"] = "Unable to convert count to integer.";
							}
						}
						else
						{
							resData.rData["rMessage"] = "Count value is null or empty.";
						}
					}
				}
			}
			catch (Exception ex)
			{
				resData.rData["rMessage"] = "Exception occurred: " + ex.Message + " --- " + ex;
				// Log exception here for troubleshooting
				Console.WriteLine("Exception in GenerateOtp: " + ex.Message);
			}

			return resData;
		}
		private string Generate()
		{
			Random random = new Random();
			return random.Next(100000, 999999).ToString(); // Generate 6-digit OTP
		}

    }
}