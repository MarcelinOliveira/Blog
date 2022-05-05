using BlogEF.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddDbContext<VSBlogDataContext>();
var app = builder.Build();

app.MapControllers();
app.Run();