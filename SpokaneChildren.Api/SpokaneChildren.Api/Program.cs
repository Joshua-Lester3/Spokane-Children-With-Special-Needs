using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SpokaneChildren.Api.Data;
using SpokaneChildren.Api.Identity;
using SpokaneChildren.Api.Models;
using SpokaneChildren.Api.Services;
using System.Text;

var AllOrigins = "AllOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllOrigins, policy =>
	{
		policy.WithOrigins("http://localhost:3000");
		policy.AllowAnyMethod();
		policy.AllowAnyHeader();
		policy.AllowCredentials();
	});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
	config.SwaggerDoc("v1", new OpenApiInfo { Title = "Spokane Children API", Version = "v1" });
	config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer"
	});
	config.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new List<string>()
		}
	});
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Unable to connect to 'DefaultConnection'");
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<AnnouncementService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<ResourceService>();
builder.Services.AddScoped<UserService>();

// Identity Services
builder.Services.AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>(); // Tell identity where to store things

// JWT Token Setup
JwtConfiguration jwtConfig = builder.Configuration
	.GetSection("Jwt").Get<JwtConfiguration>() ?? throw new InvalidOperationException("JWT config not specified");
builder.Services.AddSingleton(jwtConfig);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtConfig.Issuer,
			ValidAudience = jwtConfig.Audience,

			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
		}
	);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.Migrate();
	await IdentitySeed.SeedAsync(
		scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>(),
		scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(),
		db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }