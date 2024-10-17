using EventsWebApplication.Api;
using EventsWebApplication.Api.Extensions;
using EventsWebApplication.Application.Events.Services.GetAllEvents;
using EventsWebApplication.Application.Events.UseCases.CreateEvent;
using EventsWebApplication.Application.Events.UseCases.DeleteEvent;
using EventsWebApplication.Application.Events.UseCases.GetAllEvents;
using EventsWebApplication.Application.Events.UseCases.GetEventById;
using EventsWebApplication.Application.Events.UseCases.GetEventByName;
using EventsWebApplication.Application.Events.UseCases.GetEventsByFilter;
using EventsWebApplication.Application.Events.UseCases.UpdateEvent;
using EventsWebApplication.Application.Events.UseCases.UploadImage;
using EventsWebApplication.Application.Mappers.Events;
using EventsWebApplication.Application.Mappers.Users;
using EventsWebApplication.Application.Members.UseCases.AddUserToEvent;
using EventsWebApplication.Application.Members.UseCases.DeleteMemberFromEvent;
using EventsWebApplication.Application.Members.UseCases.GetMemberByEvent;
using EventsWebApplication.Application.Members.UseCases.GetMemberById;
using EventsWebApplication.Application.Services;
using EventsWebApplication.Application.Users.Login;
using EventsWebApplication.Application.Users.RefreshToken;
using EventsWebApplication.Application.Users.Registration;
using EventsWebApplication.Core;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using EventsWebApplication.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiAuthentication(builder.Configuration);


builder.Services.AddDbContext<EventsApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(EventsApplicationDbContext)));
});
builder.Services.AddAutoMapper(typeof(CreateEventRequestToEventEntity),typeof(UpdateEventRequestToEventEntity),
    typeof(EventEntityToEventDto), typeof(UserRegistrationRequestToUserEntity));

builder.Services.Configure<AuthorizationOptions>(builder.Configuration.GetSection(nameof(AuthorizationOptions)));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IEventRepository,EventRepository>();
builder.Services.AddScoped<IMemberRepository,MemberRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IRepository<UserEntity>, UserRepository>();
builder.Services.AddScoped<IRepository<MemberEntity>, MemberRepository>();
builder.Services.AddScoped<IRepository<EventEntity>, EventRepository>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IValidator, CreateEventRequestValidator>();
builder.Services.AddScoped<IValidator, UpdateEventRequestValidator>();
builder.Services.AddScoped<IValidator, DeleteEventRequestValidator>();
builder.Services.AddScoped<IValidator, GetAllEventsRequestValidator>();
builder.Services.AddScoped<IValidator, GetEventByIdRequestValidator>();
builder.Services.AddScoped<IValidator, GetEventByNameRequestValidator>();
builder.Services.AddScoped<IValidator, GetEventsByFilterRequestValidator>();
builder.Services.AddScoped<IValidator, UploadImageRequestValidator>();

builder.Services.AddScoped<IValidator, UserLoginRequestValidator>();
builder.Services.AddScoped<IValidator, UserRegistrationRequestValidator>();
builder.Services.AddScoped<IValidator, RefreshTokenRequestValidator>();
builder.Services.AddScoped<ValidationService>();

builder.Services.AddScoped<UserRegistrationUseCase>();
builder.Services.AddScoped<UserLoginUseCase>();
builder.Services.AddScoped<RefreshTokenUseCase>();

builder.Services.AddScoped<GetAllEventsUseCase>();
builder.Services.AddScoped<CreateEventUseCase>();
builder.Services.AddScoped<GetEventByIdUseCase>();
builder.Services.AddScoped<GetEventByNameUseCase>();
builder.Services.AddScoped<GetEventsByFilterUseCase>();
builder.Services.AddScoped<UpdateEventUseCase>();
builder.Services.AddScoped<UploadImageUseCase>();
builder.Services.AddScoped<DeleteEventUseCase>();

builder.Services.AddScoped<AddUserToEventUseCase>();
builder.Services.AddScoped<DeleteMemberFromEventUseCase>();
builder.Services.AddScoped<GetMemberByEventUseCase>();
builder.Services.AddScoped<GetMemberByIdUseCase>();




builder.Services.AddScoped<IValidator, AddUserToEventRequestValidator>();
builder.Services.AddScoped<IValidator, DeleteMemberFromEventRequestValidator>();
builder.Services.AddScoped<IValidator, GetMemberByEventRequestValidator>();
builder.Services.AddScoped<IValidator, GetMemberByIdRequestValidator>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
