using GenAI.API.Services;

var builder = WebApplication.CreateBuilder(args);


// Configure Kestrel server to set the maximum request body size to 100 MB (104857600 bytes)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // Set max request body size to 100 MB
});



builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped<IWhisperService, WhisperService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IOpenAiService, OpenAiService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Allow only the 4200 port
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin() // Allow all origins
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();
