using Microsoft.EntityFrameworkCore;
using relationshipOnetoone.context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("relationshiponetoone")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(
   options =>
   {
       options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
       options.RoutePrefix = string.Empty;
       options.DocumentTitle = "My Swagger";

   }
);


app.MapControllers();



app.Run();
