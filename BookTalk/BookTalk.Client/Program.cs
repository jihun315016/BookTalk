var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".BookTalk.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.Use(async (context, next) =>
{
    var sessionConfig = builder.Configuration.GetSection("Session");
    string sessionId = sessionConfig["id"];

    var sessionIdCookie = context.Request.Cookies[sessionId];
    if (sessionIdCookie != null)
    {
        // 쿠키가 존재하는 경우 만료 시간을 갱신
        context.Response.Cookies.Append(
            sessionId,
            sessionIdCookie,
            new CookieOptions
            {
                HttpOnly = true
            }
        );
    }

    await next.Invoke();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
