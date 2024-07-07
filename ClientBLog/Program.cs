using ClientBLog;
using ClientBLog.Controller;
using ClientBLog.Data;
using ClientBLog.Models;
using ClientBLog.Notifications;
using ClientBLog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.WebSockets;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

public class Client
{
    static async Task Main(string[] args)
    {     
          
           var host = CreateHostBuilder(args).Build();
            

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    await context.Database.EnsureCreatedAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao criar o banco de dados: {ex.Message}");
                }

                try
                {
                    var notificationService = services.GetRequiredService<INotificationService>();
                    

                    // Resolver a PostController
                    var postController = services.GetRequiredService<PostController>();

                    // Chamar o método GetPosts e obter os posts
                    var posts = await postController.GetPosts();

                    Console.WriteLine("\n############ LISTA DE CONTEÚDOS ######################");
                    foreach (var post in posts)
                    {
                        Console.WriteLine($" {post.Id} - {post.UserId} - {post.Title} - {post.Content}");
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
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\nBem vindo(a) {username}, seu id de usuario é {user.Id}. Se atente a este número para interagir no blog.");
                            Console.WriteLine("Exemplo de utilização:\n");
                            Console.WriteLine("\n############ LISTA DE CONTEÚDOS #############################");
                            Console.WriteLine("8 - 1 - websockets - WebSocket é um protocolo de comunicação\n");
                            Console.WriteLine("\n#############################################################");
                            Console.WriteLine("8 - ID DA POSTAGEM\n");
                            Console.WriteLine("1 - ID DO USUARIO \n");
                            Console.WriteLine("websockets - TITULO \n");
                            Console.WriteLine("WebSocket é um protocolo de comunicação- CONTEUDO \n");
                            
                           

                            User usr = new User { Id = user.Id, Username = user.Username, Password = user.Password };
                               AcoesUsuario(usr, services, string.Empty,args);
                               Util.user = usr;
                               Util.services = services;
                               Util.usuarioLogado = true;
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

            await host.RunAsync();      
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>();
               webBuilder.UseUrls("http://localhost:5001"); 
           });

    //inicio
    public static async Task AcoesUsuario(User usuario, IServiceProvider services, string? option, string[] args)
    {
       
        while (true) 
        {
            Console.ResetColor();
            Console.WriteLine("\n");
            Console.WriteLine("1 - CRIAR NOVA POSTAGEM");
            Console.WriteLine("2 - ALTERAR POSTAGEM");
            Console.WriteLine("3 - DELETAR POSTAGEM");
            Console.WriteLine("4 - LISTAR POSTAGENS");
            Console.WriteLine("\n");

            Post post = new Post();
            string id;
            string digitoUsuario = Console.ReadLine();
            string message = string.Empty;

            NotificationService ns = new NotificationService();

            if (string.IsNullOrEmpty(digitoUsuario))
                digitoUsuario = option;

            if (digitoUsuario == "1")
            {
                post.UserId = usuario.Id;
                post.User = usuario;
                Console.WriteLine("DIGITE O TITULO DA POSTAGEM");
                post.Title = Console.ReadLine();
                Console.WriteLine("DIGITE O CONTEUDO DA POSTAGEM");
                post.Content = Console.ReadLine();
                CriarPostagem(post, usuario, services);
                message = $"{usuario.Username} criou uma postagem - {post.Title}".ToUpper();
                await ns.NotificationServiceMessage(message);
               
            }
            else if (digitoUsuario == "2")
            {
                Console.WriteLine("DIGITE ID DA POSTAGEM ");
                id = Console.ReadLine();
                Console.WriteLine("DIGITE O TITULO DA POSTAGEM");
                post.Title = Console.ReadLine();
                Console.WriteLine("DIGITE O CONTEUDO DA POSTAGEM");
                post.Content = Console.ReadLine();
                post.User = usuario;
                post.Id = Convert.ToInt16(id);
                AlterarPostagem(post, post.User, services, Convert.ToInt16(id));

                message = $"{post.User.Username} - alterou uma postagem - titulo : {post.Title}".ToUpper();

                await ns.NotificationServiceMessage(message);
            }
            else if (digitoUsuario == "3")
            {
                Console.WriteLine("DIGITE ID ");
                id = Console.ReadLine();
                ExcluirPostagem(usuario, services, Convert.ToInt16(id));

                message = $"{usuario.Username} - deletou uma postagem.".ToUpper();

                await ns.NotificationServiceMessage(message);
            }
            else if (digitoUsuario == "4")
            {
                // ListarPostsAll(services);
                listarTodosPosts(args);
            }
            else
            {
                Console.WriteLine("\nOpção inválida.");
            }
        }
    }

    public async static Task listarTodosPosts(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var postController = services.GetRequiredService<PostController>();

            // Chamar o método GetPosts e obter os posts
            var posts = await postController.GetPosts();

            Console.WriteLine("\n############ LISTA DE CONTEÚDOS ######################");
            foreach (var post in posts)
            {
                Console.WriteLine($" {post.Id} - {post.UserId} - {post.Title} - {post.Content}");
            }
            Console.WriteLine("########################################################\n");
        }
    }

    //fin
    static async Task AcoesUsuario_(User usuario, IServiceProvider services , string?option)
    {
       
        using (ClientWebSocket client = new ClientWebSocket())
        {
            Console.ResetColor();
            Console.WriteLine("\n");
            Console.WriteLine("1 - CRIAR NOVA POSTAGEM");
            Console.WriteLine("2 - ALTERAR POSTAGEM");
            Console.WriteLine("3 - DELETAR POSTAGEM");
            Console.WriteLine("4 - LISTAR POSTAGENS");
            Console.WriteLine("\n");

            Post post = new Post();
            string id;
            string digitoUsuario = Console.ReadLine();
            string message = string.Empty;

            NotificationService ns = new NotificationService();

            if (string.IsNullOrEmpty(digitoUsuario))
                digitoUsuario = option;

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
                    message = $"{usuario.Username} criou uma postagem - {post.Title}".ToUpper();

                    using (StringReader stringReader = new StringReader(message))
                    {                      
                        Console.SetIn(stringReader);
                        await ns.NotificationServiceMessage();
                    }
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

                    message = $"{post.User.Username} - alterou uma postagem - titulo : {post.Title}".ToUpper();  // Remoção de espaços desnecessários

                    using (StringReader stringReader = new StringReader(message))
                    {
                        Console.SetIn(stringReader);
                        await ns.NotificationServiceMessage();
                       
                    }
                   break;


                case "3":
                    Console.WriteLine("DIGITE ID ");
                    id = Console.ReadLine();
                    ExcluirPostagem(usuario, services, Convert.ToInt16(id));

                    message = $"{usuario.Username} - deletou uma postagem.".ToUpper();  // Remoção de espaços desnecessários

                    using (StringReader stringReader = new StringReader(message))
                    {
                        Console.SetIn(stringReader);
                        await ns.NotificationServiceMessage();
                    }
                    break;


                case "4":
                    
                    ListarPostsAll(services);
                    
                    
                    break;

                default:
                    Console.WriteLine("\nOpção inválida.");
                    break;
            }
        }
    }

    //static async Task CriarPostagem(Post postagem, User usr, IServiceProvider services)
    //{  
    //        var postController = services.GetRequiredService<PostController>();
    //        await postController.CreatePost(postagem.Title, postagem.Content, postagem.UserId);
    //        string[] args = new string[1024];
           
    //}

    public static void CriarPostagem(Post postagem, User usr, IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var postController = services.GetRequiredService<PostController>();
             postController.CreatePost(postagem.Title, postagem.Content, postagem.UserId);
            //Console.WriteLine("\nPostagem criada com sucesso!\n");
        }
    }

   

    public static async Task ExcluirPostagem(User usr, IServiceProvider services, int id)
    {
        using (var scope = services.CreateScope())
        {

            var postController = services.GetRequiredService<PostController>();

            bool deletado = await postController.DeletePost(id, usr);

            if (!deletado)
            {
                Console.WriteLine("\nVocê não tem permissão para excluir postagens de outro usuário.");
            }
           
        }
       
    }

    public static async Task AlterarPostagem(Post postagem, User usr, IServiceProvider services, int id)
    {
        using (var scope = services.CreateScope())
        {
            var postController = scope.ServiceProvider.GetRequiredService<PostController>();

            bool editavel = postController.VerificarOwnerPost(id, postagem.Title, postagem.Content, usr);

            if (editavel)
                await postController.Update(postagem);
            else
            {
                Console.WriteLine("Você não tem permissão para editar postagens de outro usuário.");
            }
        }
    }

    //static async Task AlterarPostagem(Post postagem, User usr, IServiceProvider services, int id)
    //{
    //    var postController = services.GetRequiredService<PostController>();

    //    bool editavel = postController.VerificarOwnerPost(id, postagem.Title, postagem.Content, usr);

    //    if (editavel)      
    //      await postController.Update(postagem);

    //    else
    //    {
    //        Console.WriteLine("Você não tem permissão para editar postagens de outro usuário.");
    //    }

    //    //var posts = await ListarPosts(services, usr);

    //    //foreach (var post in posts)
    //    //{
    //    //    Console.WriteLine(post);
    //    //}
    //}

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
        using (var scope = services.CreateScope())
        {
            var postController = services.GetRequiredService<PostController>();

            var posts = await postController.GetPosts();


            foreach (var post in posts)
            {
                Console.WriteLine("\n#########################################################################################");
                Console.WriteLine($"{post.Id} - {post.UserId} - {post.Title} - {post.Content}".ToUpper());
                Console.WriteLine("###########################################################################################\n");
            }
        }
    }
       


 
}

