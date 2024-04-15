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

// DbContext 등록하기
// BookTalk.Server 프로젝트의 API 컨트롤러에서 별도의 작업을 하지 않아도 DBContext 클래스가 주입된 Service 클래스를 생성자에서 주입받는다.
builder.Services.AddDbContext<BookTalkDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookTalkDb")));

var mongoDBConfig = builder.Configuration.GetSection("ConnectionStrings:MongoDB");
var connectionString = mongoDBConfig["ConnStr"];
var database = mongoDBConfig["Database"];
var collections = mongoDBConfig.GetSection("Collections").Get<string[]>();

// ========== 서비스 등록하기 START ==========
builder.Services.AddScoped<MongoDBService>(serviceProvider => new MongoDBService(connectionString, database, collections));
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<CommentService>();
// ==========  서비스 등록하기 END  ==========

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

// ========== 서버가 시작될 때 MongoDB에 저장된 세션 모두 삭제 START ==========
using var scope = app.Services.CreateScope();
var mongoDBService = scope.ServiceProvider.GetRequiredService<MongoDBService>();
await mongoDBService.DeleteAllSessions();
// ==========  서버가 시작될 때 MongoDB에 저장된 세션 모두 삭제 END  ==========

app.Run();
