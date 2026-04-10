using ShelfManager.Application;
using ShelfManager.Infrastructure;
using ShelfManager.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();// gelen isteðin header'ýndaki token'ý okur, doðrular, kullanýcýyý tanýmlar. Biz sadece "JWT kullanacaðýz" dedik (AddJwtBearer), gerisini framework hallediyor.
app.UseAuthorization();//kullanýcýnýn tanýmlandýktan sonra o endpoint'e eriþim yetkisi var mý kontrol eder. [Authorize] attribute'u bunu kullanýr.
app.UseHttpsRedirection();


app.MapControllers();

app.Run();
