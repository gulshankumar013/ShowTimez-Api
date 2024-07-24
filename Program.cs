
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Text.Json;
using COMMON_PROJECT_STRUCTURE_API.services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebHost.CreateDefaultBuilder(args)
    .ConfigureServices(s =>
    {
        IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        s.AddSingleton<login>();
        s.AddSingleton<signup>();
        s.AddSingleton<update>();
        s.AddSingleton<signin>();
        s.AddSingleton<forgot_password>();
        s.AddSingleton<moviePlaying>();
        s.AddSingleton<fetchmoviePlaying>();
        s.AddSingleton<giganexusCatCard>();
        s.AddSingleton<fetchgiganexusCatCard>();
        s.AddSingleton<theaterList>();
        s.AddSingleton<contactUs>();
        s.AddSingleton<showTimezAdmin>();
        s.AddSingleton<adminSignin>();
        s.AddSingleton<fetchAllMessage>();
        s.AddSingleton<upcomingMovie>();
        s.AddSingleton<offerOnBrands>();
        s.AddSingleton<paymentService>();
        s.AddSingleton<orderlist>();
        s.AddSingleton<otp>();
        s.AddSingleton<booking>();


        s.AddAuthorization();
        s.AddControllers();
        s.AddCors();
        s.AddAuthentication("SourceJWT").AddScheme<SourceJwtAuthenticationSchemeOptions, SourceJwtAuthenticationHandler>("SourceJWT", options =>
        {
            options.SecretKey = appsettings["jwt_config:Key"].ToString();
            options.ValidIssuer = appsettings["jwt_config:Issuer"].ToString();
            options.ValidAudience = appsettings["jwt_config:Audience"].ToString();
            options.Subject = appsettings["jwt_config:Subject"].ToString();
        });
    })
    .Configure(app =>
    {
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors(options =>
            options.WithOrigins("https://localhost:5002", "http://localhost:5001")
            .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

        app.UseAuthentication();
        app.UseAuthorization();



        app.UseEndpoints(e =>
        {
            var login = e.ServiceProvider.GetRequiredService<login>();
            var signup = e.ServiceProvider.GetRequiredService<signup>();
            var update = e.ServiceProvider.GetRequiredService<update>();
            var signin = e.ServiceProvider.GetRequiredService<signin>();
            var forgot_password = e.ServiceProvider.GetRequiredService<forgot_password>();
            var moviePlaying = e.ServiceProvider.GetRequiredService<moviePlaying>();
            var fetchmoviePlaying = e.ServiceProvider.GetRequiredService<fetchmoviePlaying>();
            var giganexusCatCard = e.ServiceProvider.GetRequiredService<giganexusCatCard>();
            var fetchgiganexusCatCard = e.ServiceProvider.GetRequiredService<fetchgiganexusCatCard>();
            var theaterList = e.ServiceProvider.GetRequiredService<theaterList>();
            var contactUs = e.ServiceProvider.GetRequiredService<contactUs>();
            var showTimezAdmin = e.ServiceProvider.GetRequiredService<showTimezAdmin>();
            var adminSignin = e.ServiceProvider.GetRequiredService<adminSignin>();
            var fetchAllMessage = e.ServiceProvider.GetRequiredService<fetchAllMessage>();
            var upcomingMovie = e.ServiceProvider.GetRequiredService<upcomingMovie>();
            var offerOnBrands = e.ServiceProvider.GetRequiredService<offerOnBrands>();
            var paymentService = e.ServiceProvider.GetRequiredService<paymentService>();
            var orderlist = e.ServiceProvider.GetRequiredService<orderlist>();
            var otp = e.ServiceProvider.GetRequiredService<otp>();
            var booking = e.ServiceProvider.GetRequiredService<booking>();


            e.MapPost("otpGenerate",
           [AllowAnonymous] async (HttpContext http) =>
           {
               var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
               requestData rData = JsonSerializer.Deserialize<requestData>(body);
               if (rData.eventID == "1001") // update
                   await http.Response.WriteAsJsonAsync(await otp.OtpGenerate(rData));
           });


            e.MapPost("login",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await login.Login(rData));
            });

            e.MapPost("signin",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await signin.Signin(rData));
            });


               e.MapPost("adminfetch",
             [AllowAnonymous] async (HttpContext http) =>
             {
                 var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                 requestData rData = JsonSerializer.Deserialize<requestData>(body);

                 if (rData.eventID == "1001") // getUserByEmail
                 {
                     var result = await signin.AdminFetch(body);
                     await http.Response.WriteAsJsonAsync(result);
                 }
             });


            e.MapPost("signup",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await signup.Signup(rData));
            });

           

            e.MapPost("booking",
     [AllowAnonymous] async (HttpContext http) =>
     {
         var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
         requestData rData = JsonSerializer.Deserialize<requestData>(body);
         if (rData.eventID == "1001") // update
             await http.Response.WriteAsJsonAsync(await booking.Booking(rData));
     });

            e.MapPost("bookingbyid",
                [AllowAnonymous] async (HttpContext http) =>
                {
                    var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                    requestData rData = JsonSerializer.Deserialize<requestData>(body);
                    if (rData.eventID == "1001") // update
                        await http.Response.WriteAsJsonAsync(await booking.FetchBookingById(rData));
                });



            e.MapPost("fetchUser",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await signup.FetchUser(rData));
            });


            e.MapPost("fetchAllUser",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);

                if (rData.eventID == "1001") // getUserByEmail
                {
                    var result = await signup.FetchAllUser(body);
                    await http.Response.WriteAsJsonAsync(result);
                }
            });

           
            e.MapPost("totaluser",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await signup.Totaluser(rData));
            });





            e.MapPost("forgot_password",
          [AllowAnonymous] async (HttpContext http) =>
          {
              var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
              requestData rData = JsonSerializer.Deserialize<requestData>(body);
              if (rData.eventID == "1001") // update
                  await http.Response.WriteAsJsonAsync(await forgot_password.Forgot_password(rData));
          });

            e.MapPost("update",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await update.Update(rData));
            });

            e.MapPost("delete",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // delete
                    await http.Response.WriteAsJsonAsync(await update.Delete(rData));
            });

            e.MapPost("updateProfileImage",
           [AllowAnonymous] async (HttpContext http) =>
           {
               var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
               requestData rData = JsonSerializer.Deserialize<requestData>(body);
               if (rData.eventID == "1001") // delete
                   await http.Response.WriteAsJsonAsync(await update.UpdateProfileImage(rData));
           });

            e.MapPost("fetchProfileImage",
      [AllowAnonymous] async (HttpContext http) =>
      {
          var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
          requestData rData = JsonSerializer.Deserialize<requestData>(body);
          if (rData.eventID == "1001") // delete
              await http.Response.WriteAsJsonAsync(await update.FetchProfileImage(rData));
      });


            e.MapPost("moviePlaying",
           [AllowAnonymous] async (HttpContext http) =>
           {
               var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
               requestData rData = JsonSerializer.Deserialize<requestData>(body);
               if (rData.eventID == "1001") // update
                   await http.Response.WriteAsJsonAsync(await moviePlaying.MoviePlaying(rData));
           });


            _ = e.MapPost("deleteMoviePlaying",
           [AllowAnonymous] async (HttpContext http) =>
           {
               var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
               requestData rData = JsonSerializer.Deserialize<requestData>(body);
               if (rData.eventID == "1001") // update
                   await http.Response.WriteAsJsonAsync(await moviePlaying.DeleteMoviePlaying(rData));
           });

            e.MapPost("updateMoviePlaying",
           [AllowAnonymous] async (HttpContext http) =>
           {
               var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
               requestData rData = JsonSerializer.Deserialize<requestData>(body);
               if (rData.eventID == "1001") // update
                   await http.Response.WriteAsJsonAsync(await moviePlaying.UpdateMoviePlaying(rData));
           });


            e.MapPost("fetchmoviePlaying",
             [AllowAnonymous] async (HttpContext http) =>
             {
                 var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                 requestData rData = JsonSerializer.Deserialize<requestData>(body);

                 if (rData.eventID == "1001") // getUserByEmail
                 {
                     var result = await fetchmoviePlaying.FetchMoviePlaying(body);
                     await http.Response.WriteAsJsonAsync(result);
                 }
             });

            e.MapPost("fetchMoviePlayingById",
               [AllowAnonymous] async (HttpContext http) =>
               {
                   var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                   requestData rData = JsonSerializer.Deserialize<requestData>(body);
                   if (rData.eventID == "1001") // update
                       await http.Response.WriteAsJsonAsync(await fetchmoviePlaying.FetchMoviePlayingById(rData));
               });

                 e.MapPost("totalmovieplaying",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await fetchmoviePlaying.TotaMoviePlaying(rData));
            });



            e.MapPost("giganexusCatCard",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await giganexusCatCard.GiganexusCatCard(rData));
            });

            e.MapPost("deleteGiganexusCatCard",
           [AllowAnonymous] async (HttpContext http) =>
           {
               var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
               requestData rData = JsonSerializer.Deserialize<requestData>(body);
               if (rData.eventID == "1001") // update
                   await http.Response.WriteAsJsonAsync(await giganexusCatCard.DeleteGiganexusCatCard(rData));
           });

            e.MapPost("updateGiganexusCatCard",
         [AllowAnonymous] async (HttpContext http) =>
         {
             var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
             requestData rData = JsonSerializer.Deserialize<requestData>(body);
             if (rData.eventID == "1001") // update
                 await http.Response.WriteAsJsonAsync(await giganexusCatCard.UpdateGiganexusCatCard(rData));
         });

            e.MapPost("fetchgiganexusCatCard",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await fetchgiganexusCatCard.FetchgiganexusCatCard(rData));
            });



            e.MapPost("theaterList",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await theaterList.TheaterList(rData));
            });

            e.MapPost("deletetheaterList",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await theaterList.DeleteTheaterList(rData));
            });

            e.MapPost("updatetheaterList",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await theaterList.UpdateTheaterList(rData));
            });

            e.MapPost("fetchtheaterList",
           [AllowAnonymous] async (HttpContext http) =>
           {
               var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
               requestData rData = JsonSerializer.Deserialize<requestData>(body);

               if (rData.eventID == "1001") // getUserByEmail
               {
                   var result = await theaterList.FetchTheaterList(body);
                   await http.Response.WriteAsJsonAsync(result);
               }
           });


            e.MapPost("contactUs",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await contactUs.ContactUs(rData));
            });


            e.MapPost("fetchAllMessage",
             [AllowAnonymous] async (HttpContext http) =>
             {
                 var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                 requestData rData = JsonSerializer.Deserialize<requestData>(body);

                 if (rData.eventID == "1001") // getUserByEmail
                 {
                     var result = await fetchAllMessage.FetchAllMessage(body);
                     await http.Response.WriteAsJsonAsync(result);
                 }
             });


            e.MapPost("deleteMessageId",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await fetchAllMessage.DeleteMessageId(rData));
            });


            e.MapPost("showTimezAdmin",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await showTimezAdmin.ShowTimezAdmin(rData));
            });

            e.MapPost("deleteShowTimezAdmin",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await showTimezAdmin.DeleteShowTimezAdmin(rData));
            });

            e.MapPost("updateShowTimezAdmin",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await showTimezAdmin.UpdateShowTimezAdmin(rData));
            });

            e.MapPost("fetchShowTimezAdmin",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await showTimezAdmin.FetchShowTimezAdmin(rData));
            });


            e.MapPost("fetchAllAdmin",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);

                if (rData.eventID == "1001") // getUserByEmail
                {
                    var result = await showTimezAdmin.FetchAllAdmin(body);
                    await http.Response.WriteAsJsonAsync(result);
                }
            });

            e.MapPost("adminSignin",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await adminSignin.AdminSignin(rData));
            });


            e.MapPost("upcomingMovie",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                    await http.Response.WriteAsJsonAsync(await upcomingMovie.UpcomingMovie(rData));
            });

            e.MapPost("deleteupcomingMovie",
           [AllowAnonymous] async (HttpContext http) =>
           {
               var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
               requestData rData = JsonSerializer.Deserialize<requestData>(body);
               if (rData.eventID == "1001") // update
                   await http.Response.WriteAsJsonAsync(await upcomingMovie.DeleteUpcomingMovie(rData));
           });


            e.MapPost("fetchUpcomingMovie",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);

                if (rData.eventID == "1001") // getUserByEmail
                {
                    var result = await upcomingMovie.FetchUpcomingMovie(body);
                    await http.Response.WriteAsJsonAsync(result);
                }
            });

             e.MapPost("totalcommingplaying",
           [AllowAnonymous] async (HttpContext http) =>
           {
               var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
               requestData rData = JsonSerializer.Deserialize<requestData>(body);
               if (rData.eventID == "1001") // update
                   await http.Response.WriteAsJsonAsync(await upcomingMovie.TotalCommingPlaying(rData));
           });

            e.MapPost("offerOnBrands",
          [AllowAnonymous] async (HttpContext http) =>
          {
              var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
              requestData rData = JsonSerializer.Deserialize<requestData>(body);
              if (rData.eventID == "1001") // update
                  await http.Response.WriteAsJsonAsync(await offerOnBrands.OfferOnBrands(rData));
          });

            e.MapPost("fetchTopBrandProduct",
          [AllowAnonymous] async (HttpContext http) =>
          {
              var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
              requestData rData = JsonSerializer.Deserialize<requestData>(body);

              if (rData.eventID == "1001") // getUserByEmail
              {
                  var result = await offerOnBrands.FetchTopBrandProduct(body);
                  await http.Response.WriteAsJsonAsync(result);
              }
          });

            e.MapPost("createOrder",
            [AllowAnonymous] async (HttpContext http) =>
            {
                try
                {
                    var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                    requestData rData = JsonSerializer.Deserialize<requestData>(body);
                    var result = await paymentService.CreateOrder(rData);
                    await http.Response.WriteAsJsonAsync(result);
                }
                catch (Exception ex)
                {
                    await http.Response.WriteAsJsonAsync(new { error = ex.Message });
                }
            });

            e.MapPost("capturePayment",
                [AllowAnonymous] async (HttpContext http) =>
                {
                    try
                    {
                        var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                        requestData rData = JsonSerializer.Deserialize<requestData>(body);
                        var result = await paymentService.CapturePayment(rData);
                        await http.Response.WriteAsJsonAsync(result);
                    }
                    catch (Exception ex)
                    {
                        await http.Response.WriteAsJsonAsync(new { error = ex.Message });
                    }
                });


            e.MapPost("orderlist",
                    [AllowAnonymous] async (HttpContext http) =>
                    {
                        var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                        requestData rData = JsonSerializer.Deserialize<requestData>(body);
                        if (rData.eventID == "1001") // update
                            await http.Response.WriteAsJsonAsync(await orderlist.Orderlist(rData));
                    });








            IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            e.MapGet("/dbstring",
                async c =>
                {
                    dbServices dspoly = new();
                    await c.Response.WriteAsJsonAsync("{'mongoDatabase':" + appsettings["mongodb:connStr"] + "," + " " + "MYSQLDatabase" + " =>" + appsettings["db:connStrPrimary"]);
                });

            e.MapGet("/bing",
                async c => await c.Response.WriteAsJsonAsync("{'Name':'Anish','Age':'26','Project':'COMMON_PROJECT_STRUCTURE_API'}"));
        });
    });

builder.Build().Run();

public record requestData
{
    [Required]
    public string eventID { get; set; }
    [Required]
    public IDictionary<string, object> addInfo { get; set; }
}

public record responseData
{
    public responseData()
    {
        eventID = "";
        rStatus = 0;
        rData = new Dictionary<string, object>();
    }
    [Required]
    public int rStatus { get; set; } = 0;
    public string eventID { get; set; }
    public IDictionary<string, object> addInfo { get; set; }
    public IDictionary<string, object> rData { get; set; }
}
