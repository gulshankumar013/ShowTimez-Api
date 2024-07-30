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
		public dbServices ds = new dbServices();
		public async Task<responseData> otpverify(requestData rData)
		{
			responseData resData = new responseData();

			string connectionString = "server=210.210.210.50;user=test_user;password=test*123;port=2020;database=pc_student;";

			try
			{

				string otp = rData.addInfo["otp"].ToString();

				// Validate OTP
				var validateOTPQuery = @"SELECT COUNT(*) FROM pc_student.OTP_generate WHERE otp=@otp";
				using (var connection = new MySqlConnection(connectionString))
				{
					await connection.OpenAsync();

					using (var cmd = new MySqlCommand(validateOTPQuery, connection))
					{

						cmd.Parameters.AddWithValue("@otp", otp);

						object countObj = await cmd.ExecuteScalarAsync();

						if (countObj != null && countObj != DBNull.Value)
						{
							if (int.TryParse(countObj.ToString(), out int count) && count > 0)
							{
								// Delete used OTP from OTP4_USER table
								var deleteOTPQuery = @"DELETE FROM pc_student.OTP_generate WHERE otp=@otp";
								using (var deleteCmd = new MySqlCommand(deleteOTPQuery, connection))
								{

									deleteCmd.Parameters.AddWithValue("@otp", otp);
									await deleteCmd.ExecuteNonQueryAsync();
								}

								resData.rData["success"] = true;
								resData.rData["message"] = "OTP verified successfully.";
							}
							else
							{
								resData.rData["message"] = "Invalid OTP.";
							}
						}
						else
						{
							resData.rData["message"] = "Invalid OTP.";
						}
					}
				}
			}
			catch (Exception ex)
			{
				resData.rData["message"] = "Exception occurred: " + ex.Message;
				Console.WriteLine("Exception in VerifyOTP: " + ex.Message);
			}

			return resData;
		}

		public async Task<responseData> Updatepassword(requestData rData)
		{
			responseData resData = new responseData();

			string connectionString = "server=210.210.210.50;user=test_user;password=test*123;port=2020;database=pc_student;";

			try
			{
				string email = rData.addInfo["email"].ToString();
				string newPassword = rData.addInfo["password"].ToString();

				// Update password in the database
				var updatePasswordQuery = @"UPDATE pc_student.Army_soldiers SET password = @password WHERE email = @email";

				using (var connection = new MySqlConnection(connectionString))
				{
					await connection.OpenAsync();

					using (var cmd = new MySqlCommand(updatePasswordQuery, connection))
					{
						// Implement your password hashing logic here if necessary
						cmd.Parameters.AddWithValue("@password", HashPassword(newPassword));
						cmd.Parameters.AddWithValue("@email", email);

						int rowsAffected = await cmd.ExecuteNonQueryAsync();

						if (rowsAffected > 0)
						{
							resData.rData["success"] = true;
							resData.rData["message"] = "Password updated successfully.";
						}
						else
						{
							resData.rData["message"] = "Failed to update password.";
						}
					}
				}
			}
			catch (Exception ex)
			{
				resData.rData["message"] = "Exception occurred: " + ex.Message;
				Console.WriteLine("Exception in UpdatePassword: " + ex.Message);
			}

			return resData;
		}


		private string HashPassword(string password)
		{

			return password;
		}

		public async Task<responseData> GenerateOtp(requestData rData)
		{
			responseData resData = new responseData();

			string connectionString = "server=210.210.210.50;user=test_user;password=test*123;port=2020;database=pc_student;";
			string gmailUsername = "gulshankumar013013@gmail.com"; // My Gmail address
			string gmailPassword = "8981236636g"; // My Gmail password

			try
			{
				string email = rData.addInfo["email"].ToString();

				// Check if the email exists in the database
				var checkEmailQuery = @"SELECT COUNT(*) FROM pc_student.showTimez_user WHERE email=@email";
				using (var connection = new MySqlConnection(connectionString))
				{
					await connection.OpenAsync();

					using (var cmd = new MySqlCommand(checkEmailQuery, connection))
					{
						cmd.Parameters.AddWithValue("@email", email);

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

									// // Save OTP to the database
									var insertQuery = @"INSERT INTO pc_student.OTP_generate (otp) VALUES (@otp)";
									using (var insertCmd = new MySqlCommand(insertQuery, connection))
									{

										insertCmd.Parameters.AddWithValue("@otp", otp);
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


									// resData.rData["rMessage"] = "OTP generated successfully and sent to " + email + " - otp - " + otp;
									resData.rData["rMessage"] = "Your OTP is:- " + otp;
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


