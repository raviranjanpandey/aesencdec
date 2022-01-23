using AESEncryptDecrypt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.MapPost("/encrypt", (string value) =>
{
    string key = app.Configuration["AES:Key"];
    string iv = app.Configuration["AES:AES_IV"];
    return Results.Ok(AESEncDec.AESEncryption(value, key, iv));
});
app.MapPost("/decrypt", (string value) =>
{
    string key = app.Configuration["AES:Key"];
    string iv = app.Configuration["AES:AES_IV"];
    return Results.Ok(AESEncDec.AESDecryption(value, key, iv));
});

app.Run();