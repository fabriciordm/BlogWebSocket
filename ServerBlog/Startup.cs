using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerBlog.Data;

using ServerBlog.Controller;
using ServerBlog.Notifications;
using ServerBlog.Repositories;
using ServerBlog.Services;




//public class Startup
//{
//    public IConfiguration Configuration { get; }

//    public Startup(IConfiguration configuration)
//    {
//        Configuration = configuration;
//    }

//    public void ConfigureServices(IServiceCollection services)
//    {
//        services.AddDbContext<DataContext>(options =>
//            options.UseSqlServer(Configuration.GetConnectionString("ServerBlogDb")));

//        // Adicione outros serviços necessários
//    }

//    public void Configure(IHostBuilder app, IHostEnvironment env)
//    {
//        // Configurações adicionais
//    }
//}





public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configurar o contexto do banco de dados
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ServerBlogDb")));

        // Registrar serviços e controladores
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPostService, PostService>();
        //services.AddTransient<INotificationService, NotificationService>();

        services.AddSingleton<INotificationService, NotificationService>();

        // Registrar controllers
        services.AddScoped<PostController>();
    }

    //public void ConfigureServices(IServiceCollection services)
    //{
    //    // Configuração do banco de dados
    //    services.AddDbContext<DataContext>(options =>
    //        options.UseSqlServer(Configuration.GetConnectionString("ServerBlogDb")));

    //    // Configuração do container de dependências
    //    services.AddScoped<IUserRepository, UserRepository>();
    //    services.AddScoped<IPostRepository, PostRepository>();
    //    services.AddScoped<IAuthService, AuthService>();
    //    services.AddScoped<IPostService, PostService>();
    //    services.AddSingleton<INotificationService, NotificationService>();
    //    services.AddScoped<PostController>();
    //}

}




