using App.Admin;
using App.Admin.Util.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add<ExceptionAndToastFilter>();
}).AddNToastNotifyToastr();
builder.Services.AddAdminMvcServices(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
    app.UseDeveloperExceptionPage();

app.UseNToastNotify();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseNToastNotify();

app.Run();
