using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PortalVioo.Context;
using PortalVioo.Interface;
using PortalVioo.Models;
using PortalVioo.ModelsApp;
using PortalVioo.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("premiereappdb"), buider =>
        buider.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null)));
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//        options.UseSqlServer(builder.Configuration.GetConnectionString("premiereappdb")));
builder.Services.AddScoped<IRepositoryGenericApp<ApplicationUser>, RepositoryGenericApp<ApplicationUser>>();
builder.Services.AddScoped<IRepositoryGenericApp<ParamStatus>, RepositoryGenericApp<ParamStatus>>();
builder.Services.AddScoped<IRepositoryGenericApp<ParamPriorite>, RepositoryGenericApp<ParamPriorite>>();
builder.Services.AddScoped<IRepositoryGenericApp<ParamType>, RepositoryGenericApp<ParamType>>();
builder.Services.AddScoped<IRepositoryGenericApp<Projet>, RepositoryGenericApp<Projet>>();
builder.Services.AddScoped<IRepositoryGenericApp<MembreProjet>, RepositoryGenericApp<MembreProjet>>();
builder.Services.AddScoped<IRepositoryGenericApp<Tache>, RepositoryGenericApp<Tache>>();

builder.Services.Configure<IISServerOptions>(options =>
{
    options.AutomaticAuthentication = false;
    
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// For Identity  
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseAuthorization();

app.MapControllers();

app.Run();
