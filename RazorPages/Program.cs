using DataAccessLayer;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc().AddRazorPagesOptions(options => options.Conventions.AddPageRoute("/Login", ""));
builder.Services.AddDbContext<FuminiHotelManagementContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionStringDB"));
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

builder.Services.AddScoped<IGenericDAO<Customer>, GenericDAO<Customer>>();
builder.Services.AddScoped<IGenericDAO<BookingReservation>, GenericDAO<BookingReservation>>();
builder.Services.AddScoped<IGenericDAO<RoomInformation>, GenericDAO<RoomInformation>>();
builder.Services.AddScoped<IGenericDAO<RoomType>, GenericDAO<RoomType>>();
builder.Services.AddScoped<IGenericDAO<BookingDetail>, GenericDAO<BookingDetail>>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBookingReservationRepository, BookingReservationRepository>();
builder.Services.AddScoped<IRoomInformationRepository, RoomInformationRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IBookingReservationRepository, BookingReservationRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();