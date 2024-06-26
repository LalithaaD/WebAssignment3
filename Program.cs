using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebAssignment3.Data;
using WebAssignment3.EndPoints;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WEBTask3Context")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapUserEndpoints();

app.MapProductEndpoints();

app.MapCartEndpoints();

app.MapCommentEndpoints();

app.MapOrderEndpoints();

// Call the MapUserEndpoints and MapProductEndpoints methods here


app.Run();
