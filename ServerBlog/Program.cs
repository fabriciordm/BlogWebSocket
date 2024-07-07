using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServeBlog.Models;
using ServerBlog.Controller;
using ServerBlog.Data;
using ServerBlog.Models;
using ServerBlog.Notifications;
using ServerBlog.Repositories;
using ServerBlog.Services;
using System;
using System.IO;
using System.Threading.Tasks;

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        // Create the host builder
//        var host = CreateHostBuilder(args).Build();

//        // Create a scope to resolve services
//        using (var scope = host.Services.CreateScope())
//        {
//            var services = scope.ServiceProvider;

//            try
//            {
//                var notificationService = services.GetRequiredService<INotificationService>();
//                await notificationService.NotificationServiceMessage("Mensagem inicial");

//                await ExecuteApplication(services);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Erro: {ex.Message}");
//            }
//        }
//    }

//    public static IHostBuilder CreateHostBuilder(string[] args) =>
//        Host.CreateDefaultBuilder(args)
//            .ConfigureAppConfiguration((hostingContext, config) =>
//            {
//                config.SetBasePath(Directory.GetCurrentDirectory());
//                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//                config.AddEnvironmentVariables();
//                config.AddCommandLine(args);
//            })
//            .ConfigureServices((context, services) =>
//            {
//                var startup = new Startup(context.Configuration);
//                startup.ConfigureServices(services);
//            });

//    static async Task ExecuteApplication(IServiceProvider services)
//    {
//        var postController = services.GetRequiredService<PostController>();

//        var posts = await postController.GetPosts();

//        Console.WriteLine("\n############ LISTA DE CONTEÚDOS ######################");
//        foreach (var post in posts)
//        {
//            Console.WriteLine($" {post.Id} - {post.Title} - {post.Content}");
//        }
//        Console.WriteLine("########################################################\n");

//        Console.WriteLine("Bem-vindo ao Blog! ;)\n");
//        Console.WriteLine("1. Registrar");
//        Console.WriteLine("2. Login");

//        var option = Console.ReadLine();

//        if (option == "1")
//        {
//            await RegisterUser(services);
//        }
//        else if (option == "2")
//        {
//            await LoginUser(services);
//        }
//    }

//    static async Task RegisterUser(IServiceProvider services)
//    {
//        var authService = services.GetRequiredService<IAuthService>();

//        Console.Write("Username: ");
//        var username = Console.ReadLine();
//        Console.Write("Password: ");
//        var password = Console.ReadLine();

//        try
//        {
//            var user = await authService.RegisterAsync(username, password);

//            if (user)
//            {
//                Console.WriteLine("Usuário registrado com sucesso!");
//            }
//            else
//            {
//                Console.WriteLine($"Usuario {username} já está em uso.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Erro: {ex.Message}");
//        }
//    }

//    static async Task LoginUser(IServiceProvider services)
//    {
//        var authService = services.GetRequiredService<IAuthService>();

//        Console.Write("Username: ");
//        var username = Console.ReadLine();
//        Console.Write("Password: ");
//        var password = Console.ReadLine();

//        try
//        {
//            var user = await authService.LoginAsync(username, password);

//            if (user != null)
//            {
//                Console.WriteLine($"\nBem vindo(a) {username}, seu id de usuario é {user.Id}. Se atente a este número para interagir no blog.");
//                User usr = new User { Id = user.Id, Username = user.Username, Password = user.Password };
//                await AcoesUsuario(usr, services);
//            }
//            else
//            {
//                Console.WriteLine("Login ou senha inválidos.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Erro: {ex.Message}");
//        }
//    }

//    static async Task AcoesUsuario(User usuario, IServiceProvider services)
//    {
//        Console.WriteLine("\n");

//        Console.WriteLine("1 - CRIAR NOVA POSTAGEM");
//        Console.WriteLine("2 - ALTERAR POSTAGEM");
//        Console.WriteLine("3 - DELETAR POSTAGEM");
//        Console.WriteLine("4 - LISTAR POSTAGENS");

//        Console.WriteLine("\n");

//        Post post = new Post();
//        string id;
//        string digitoUsuario = Console.ReadLine();

//        switch (digitoUsuario)
//        {
//            case "1":
//                post.UserId = usuario.Id;
//                post.User = usuario;
//                Console.WriteLine("DIGITE O TITULO DA POSTAGEM");
//                post.Title = Console.ReadLine();
//                Console.WriteLine("DIGITE O CONTEUDO DA POSTAGEM");
//                post.Content = Console.ReadLine();
//                await CriarPostagem(post, usuario, services);
//                break;

//            case "2":
//                Console.WriteLine("DIGITE ID ");
//                id = Console.ReadLine();
//                Console.WriteLine("DIGITE O TITULO DA POSTAGEM");
//                post.Title = Console.ReadLine();
//                Console.WriteLine("DIGITE O CONTEUDO DA POSTAGEM");
//                post.Content = Console.ReadLine();
//                post.User = usuario;
//                post.Id = Convert.ToInt16(id);
//                await AlterarPostagem(post, post.User, services, Convert.ToInt16(id));
//                break;

//            case "3":
//                Console.WriteLine("DIGITE ID ");
//                id = Console.ReadLine();
//                await ExcluirPostagem(usuario, services, Convert.ToInt16(id));
//                break;

//            case "4":
//                await ListarPostsAll(services);
//                break;

//            default:
//                Console.WriteLine("\nOpção inválida.");
//                break;
//        }
//    }

//    static async Task CriarPostagem(Post postagem, User usr, IServiceProvider services)
//    {
//        var postController = services.GetRequiredService<PostController>();

//        await postController.CreatePost(postagem.Title, postagem.Content, postagem.UserId);

//        Console.WriteLine("\nPostagem criada com sucesso!\n");

//        var posts = await ListarPosts(services, usr);

//        foreach (var post in posts)
//        {
//            Console.WriteLine(post);
//        }
//    }

//    static async Task ExcluirPostagem(User usr, IServiceProvider services, int id)
//    {
//        var postController = services.GetRequiredService<PostController>();

//        bool deletado = await postController.DeletePost(id, usr);

//        if (!deletado)
//        {
//            Console.WriteLine("\nVocê não tem permissão para excluir postagens de outro usuário.");
//        }
//        else
//        {
//            Console.WriteLine("\nDeletado com sucesso.");
//        }
//    }

//    static async Task AlterarPostagem(Post postagem, User usr, IServiceProvider services, int id)
//    {
//        var postController = services.GetRequiredService<PostController>();

//        bool editavel = postController.VerificarOwnerPost(id, postagem.Title, postagem.Content, usr);

//        if (editavel)
//        {
//            using (var dbContext = services.GetRequiredService<DataContext>())
//            {
//                dbContext.Attach(postagem);
//                dbContext.Entry(postagem).State = EntityState.Modified;
//                await dbContext.SaveChangesAsync();
//            }
//        }
//        else
//        {
//            Console.WriteLine("Você não tem permissão para editar postagens de outro usuário.");
//        }

//        var posts = await ListarPosts(services, usr);

//        foreach (var post in posts)
//        {
//            Console.WriteLine(post);
//        }
//    }

//    static async Task<List<string>> ListarPosts(IServiceProvider services, User usr)
//    {
//        var postController = services.GetRequiredService<PostController>();

//        var posts = await postController.GetPosts();

//        var postList = new List<string>();

//        foreach (var post in posts)
//        {
//            postList.Add($" {post.Id} - {post.UserId} - {post.Title} - {post.Content}");
//        }

//        return postList;
//    }

//    static async Task ListarPostsAll(IServiceProvider services)
//    {
//        var postController = services.GetRequiredService<PostController>();

//        var posts = await postController.GetPosts();

//        foreach (var post in posts)
//        {
//            Console.WriteLine($"{post.Id} - {post.UserId} - {post.Title} - {post.Content}");
//        }
//    }
//}


class Program
{
    static async Task Main(string[] args)
    {
       // var host = CreateHostBuilder(args).Build();
       var host = CreateHostBuilder(args).UseConsoleLifetime().Build();


        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var notificationService = services.GetRequiredService<INotificationService>();
                await notificationService.NotificationServiceMessage("Mensagem inicial");


                // Resolver a PostController
                var postController = services.GetRequiredService<PostController>();

                // Chamar o método GetPosts e obter os posts
                var posts = await postController.GetPosts();

                Console.WriteLine("\n############ LISTA DE CONTEÚDOS ######################");
                foreach (var post in posts)
                {
                    Console.WriteLine($" {post.Id} - {post.Title} - {post.Content}");
                }
                Console.WriteLine("########################################################\n");

                Console.WriteLine("Bem-vindo ao Blog! ;)\n");
                Console.WriteLine("1. Registrar");
                Console.WriteLine("2. Login");

                var option = Console.ReadLine();

                if (option == "1")
                {
                    var authService = services.GetRequiredService<IAuthService>();

                    Console.Write("Username: ");
                    var username = Console.ReadLine();
                    Console.Write("Password: ");
                    var password = Console.ReadLine();

                    try
                    {
                        var user = await authService.RegisterAsync(username, password);

                        if (user)
                        {
                            Console.WriteLine("Usuário registrado com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine($"Usuario {username} já está em uso.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                }
                else if (option == "2")
                {
                    var authService = services.GetRequiredService<IAuthService>();

                    Console.Write("Username: ");
                    var username = Console.ReadLine();
                    Console.Write("Password: ");
                    var password = Console.ReadLine();

                    try
                    {
                        var user = await authService.LoginAsync(username, password);

                        if (user != null)
                        {
                            Console.WriteLine($"\nBem vindo(a) {username}, seu id de usuario é {user.Id}. Se atente a este número para interagir no blog.");
                            User usr = new User { Id = user.Id, Username = user.Username, Password = user.Password };
                            AcoesUsuario(usr, services);
                        }
                        else
                        {
                            Console.WriteLine("Login ou senha inválidos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
        if (host is IAsyncDisposable d) await d.DisposeAsync();
        // await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                var startup = new Startup(context.Configuration);
                startup.ConfigureServices(services);
            });

    static void AcoesUsuario(User usuario, IServiceProvider services)
    {
        // Função atualizada para usar IServiceProvider em vez de DependencyContainer
        Console.WriteLine("\n");

        Console.WriteLine("1 - CRIAR NOVA POSTAGEM");
        Console.WriteLine("2 - ALTERAR POSTAGEM");
        Console.WriteLine("3 - DELETAR POSTAGEM");
        Console.WriteLine("4 - LISTAR POSTAGENS");

        Console.WriteLine("\n");

        Post post = new Post();
        string id;
        string digitoUsuario = Console.ReadLine();

        switch (digitoUsuario)
        {
            case "1":
                post.UserId = usuario.Id;
                post.User = usuario;
                Console.WriteLine("DIGITE O TITULO DA POSTAGEM");
                post.Title = Console.ReadLine();
                Console.WriteLine("DIGITE O CONTEUDO DA POSTAGEM");
                post.Content = Console.ReadLine();
                CriarPostagem(post, usuario, services);
                break;

            case "2":
                Console.WriteLine("DIGITE ID ");
                id = Console.ReadLine();
                Console.WriteLine("DIGITE O TITULO DA POSTAGEM");
                post.Title = Console.ReadLine();
                Console.WriteLine("DIGITE O CONTEUDO DA POSTAGEM");
                post.Content = Console.ReadLine();
                post.User = usuario;
                post.Id = Convert.ToInt16(id);
                AlterarPostagem(post, post.User, services, Convert.ToInt16(id));
                break;

            case "3":
                Console.WriteLine("DIGITE ID ");
                id = Console.ReadLine();
                ExcluirPostagem(usuario, services, Convert.ToInt16(id));
                break;

            case "4":
                ListarPostsAll(services);
                break;

            default:
                Console.WriteLine("\nOpção inválida.");
                break;
        }
    }

    static async Task CriarPostagem(Post postagem, User usr, IServiceProvider services)
    {
        var postController = services.GetRequiredService<PostController>();

        await postController.CreatePost(postagem.Title, postagem.Content, postagem.UserId);

        Console.WriteLine("\nPostagem criada com sucesso!\n");

        var posts = await ListarPosts(services, usr);

        foreach (var post in posts)
        {
            Console.WriteLine(post);
        }
    }

    static async Task ExcluirPostagem(User usr, IServiceProvider services, int id)
    {
        var postController = services.GetRequiredService<PostController>();

        bool deletado = await postController.DeletePost(id, usr);

        if (!deletado)
        {
            Console.WriteLine("\nVocê não tem permissão para excluir postagens de outro usuário.");
        }
        else
        {
            Console.WriteLine("\nDeletado com sucesso.");
        }
    }

    static async Task AlterarPostagem(Post postagem, User usr, IServiceProvider services, int id)
    {
        var postController = services.GetRequiredService<PostController>();

        bool editavel = postController.VerificarOwnerPost(id, postagem.Title, postagem.Content, usr);

        if (editavel)
        {
            await postController.Update(postagem);
        }
        else
        {
            Console.WriteLine("Você não tem permissão para editar postagens de outro usuário.");
        }

        var posts = await ListarPosts(services, usr);

        foreach (var post in posts)
        {
            Console.WriteLine(post);
        }
    }

    static async Task<List<string>> ListarPosts(IServiceProvider services, User usr)
    {
        var postController = services.GetRequiredService<PostController>();

        var posts = await postController.GetPosts();

        var postList = new List<string>();

        foreach (var post in posts)
        {
            postList.Add($" {post.Id} - {post.UserId} - {post.Title} - {post.Content}");
        }

        return postList;
    }

    static async Task ListarPostsAll(IServiceProvider services)
    {
        var postController = services.GetRequiredService<PostController>();

        var posts = await postController.GetPosts();

        foreach (var post in posts)
        {
            Console.WriteLine($"{post.Id} - {post.UserId} - {post.Title} - {post.Content}");
        }
    }
}
