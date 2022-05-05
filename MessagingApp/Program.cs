
using MessagingApp.Hubs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ChatHistorydbContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnStr"));
    }
    );
builder.Services.AddDbContext<MessageAppSecurityContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SecurityConnStr") );
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<MessageAppSecurityContext>().AddDefaultUI();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EncrytDecryptService>();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseSession();
app.UseAuthorization();
app.MapHub<ChatHub>("/Chat/Index");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
