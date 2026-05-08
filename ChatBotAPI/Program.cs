using ChatBotAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ChatDb;Trusted_Connection=True;"));

// Services
builder.Services.AddHttpClient<OllamaService>(client =>
{
	client.Timeout = TimeSpan.FromMinutes(10);
});
builder.Services.AddScoped<ChatService>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		p => p.AllowAnyOrigin()
			  .AllowAnyHeader()
			  .AllowAnyMethod());
});
builder.Services.AddSingleton<VectorDbService>();
builder.Services.AddHttpClient<EmbeddingService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var vectorDb = scope.ServiceProvider.GetRequiredService<VectorDbService>();
	await vectorDb.InitializeCollection();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.EnsureCreated();
}
app.Run();