using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Common;
using BookTalk.Shared.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7202")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});

// DbContext ����ϱ�
// BookTalk.Server ������Ʈ�� API ��Ʈ�ѷ����� ������ �۾��� ���� �ʾƵ� DBContext Ŭ������ ���Ե� Service Ŭ������ �����ڿ��� ���Թ޴´�.
builder.Services.AddDbContext<BookTalkDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Server=127.0.0.1;Database=TestDb;User Id=sa;Password=1234;TrustServerCertificate=True")));
builder.Services.AddDbContext<BookTalkDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookTalkDb")));


// ========== ���� ����ϱ� START ==========
builder.Services.AddScoped<MenuService>();
// ==========  ���� ����ϱ� END  ==========

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
