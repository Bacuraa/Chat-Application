using ChatWebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<ChatWebAPIContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatWebAPIContext") ?? throw new InvalidOperationException("Connection string 'ChatWebAPIContext' not found.")));

builder.Services.AddSignalR();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow All", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Allow All");

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/Hubs/Chat");

app.Run();
