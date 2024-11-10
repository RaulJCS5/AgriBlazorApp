using AgriBlazorApp.Components;
using AgriBlazorApp.Components.Account;
using AgriBlazorApp.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
   // .AddGoogle(options =>
   //    {
   //        IConfigurationSection googleAuthNSection =
   //        builder.Configuration.GetSection("Authentication:Google");
   //        options.ClientId = googleAuthNSection["ClientId"];
   //        options.ClientSecret = googleAuthNSection["ClientSecret"];
   //    })
   //.AddFacebook(options =>
   //{
   //    IConfigurationSection FBAuthNSection =
   //    builder.Configuration.GetSection("Authentication:FB");
   //    options.ClientId = FBAuthNSection["ClientId"];
   //    options.ClientSecret = FBAuthNSection["ClientSecret"];
   //})
   //.AddMicrosoftAccount(microsoftOptions =>
   //{
   //    microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
   //    microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
   //})
   //.AddTwitter(twitterOptions =>
   //{
   //    twitterOptions.ConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerAPIKey"];
   //    twitterOptions.ConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"];
   //    twitterOptions.RetrieveUserDetails = true;
   //})
    .AddIdentityCookies();

// Load .env file
Env.Load();

// Retrieve environment variables
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
var dbUsername = Environment.GetEnvironmentVariable("DB_USERNAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

if (string.IsNullOrEmpty(dbHost) || string.IsNullOrEmpty(dbPort) || string.IsNullOrEmpty(dbDatabase) || string.IsNullOrEmpty(dbUsername) || string.IsNullOrEmpty(dbPassword))
{
    throw new InvalidOperationException($"One or more required environment variables are not set. DB_HOST: {dbHost}, DB_PORT: {dbPort}, DB_DATABASE: {dbDatabase}, DB_USERNAME: {dbUsername}, DB_PASSWORD: {dbPassword}");
}
else
{
    Console.WriteLine("Environment variables loaded successfully.");
    Console.WriteLine($"DB_HOST: {dbHost} + DB_PORT: {dbPort} + DB_DATABASE: {dbDatabase} + DB_USERNAME: {dbUsername} + DB_PASSWORD: {dbPassword}");
}

// Configure PostgreSQL connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
connectionString = connectionString.Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST"))
                                    .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT"))
                                    .Replace("${DB_DATABASE}", Environment.GetEnvironmentVariable("DB_DATABASE"))
                                    .Replace("${DB_USERNAME}", Environment.GetEnvironmentVariable("DB_USERNAME"))
                                    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD")); builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Test the database connection
TestDatabaseConnection(connectionString);

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();

void TestDatabaseConnection(string connectionString)
{
    Console.WriteLine("Testing database connection...");
    Console.WriteLine("connectionString: " + connectionString);
    using var connection = new NpgsqlConnection(connectionString);
    try
    {
        connection.Open();

        Console.WriteLine("Database connection successful.");
    }
    catch (NpgsqlException npgsqlEx)
    {
        Console.WriteLine($"PostgreSQL error: {npgsqlEx.Message}");
        throw new Exception($"Database connection failed due to PostgreSQL error: {npgsqlEx.Message}");
    }
    catch (InvalidOperationException invalidOpEx)
    {
        Console.WriteLine($"Invalid operation: {invalidOpEx.Message}");
        throw new Exception($"Database connection failed due to invalid operation: {invalidOpEx.Message}");
    }
    catch (ArgumentException argEx)
    {
        Console.WriteLine($"Argument error: {argEx.Message}");
        throw new Exception($"Database connection failed due to an argument error: {argEx.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"General error: {ex.Message}");
        throw new Exception($"Database connection failed due to an unexpected error: {ex.Message}");
    }
    finally
    {
        connection.Close();
        Console.WriteLine("Database connection test complete.");
    }
}