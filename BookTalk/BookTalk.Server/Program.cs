using BookTalk.BusinessLogic.Services;
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
builder.Services.AddDbContext<BookTalkDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookTalkDb")));

var mongoDBConfig = builder.Configuration.GetSection("ConnectionStrings:MongoDB");
var connectionString = mongoDBConfig["ConnStr"];
var database = mongoDBConfig["Database"];
var collections = mongoDBConfig.GetSection("Collections").Get<string[]>();

// ========== ���� ����ϱ� START ==========
builder.Services.AddScoped<MongoDBService>(serviceProvider => new MongoDBService(connectionString, database, collections));
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<CommentService>();
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

// ========== ������ ���۵� �� MongoDB�� ����� ���� ��� ���� START ==========
using var scope = app.Services.CreateScope();
var mongoDBService = scope.ServiceProvider.GetRequiredService<MongoDBService>();
await mongoDBService.DeleteAllSessions();
// ==========  ������ ���۵� �� MongoDB�� ����� ���� ��� ���� END  ==========

app.Run();
