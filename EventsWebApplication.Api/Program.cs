using EventsWebApplication.Api.Extensions;
using EventsWebApplication.Application.Services;
using EventsWebApplication.Core;
using EventsWebApplication.Core.Contracts;
using EventsWebApplication.Core.Mappers;
using EventsWebApplication.DataAccess;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Добавляем конвертер для перечислений
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiAuthentication(builder.Configuration);


builder.Services.AddDbContext<EventsApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(EventsApplicationDbContext)));
});
builder.Services.AddAutoMapper(typeof(EventMapper),typeof(MemberMapper),typeof(UserMapper));

builder.Services.Configure<AuthorizationOptions>(builder.Configuration.GetSection(nameof(AuthorizationOptions)));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddScoped<IEventRepository,EventRepository>();
builder.Services.AddScoped<IMemberRepository,MemberRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();

builder.Services.AddScoped<IEventService,EventService>();
builder.Services.AddScoped<IMemberService,MemberService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IValidator<EventsRequest>, EventValidator>();
builder.Services.AddScoped<IValidator<RegisterUserRequest>, UserValidator>();
builder.Services.AddScoped<IValidator<LoginUserRequest>, LoginUserValidator>();
builder.Services.AddScoped<IValidator<MemberRegistrationRequest>, MemberValidator>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider,JwtProvider>();
builder.Services.AddScoped<IPermissionService, PermissionService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});


app.Run();
