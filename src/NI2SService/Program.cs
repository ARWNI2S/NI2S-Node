namespace NI2S.Node
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) => throw new NotImplementedException();
            //Host.CreateDefaultBuilder(args)
            //    .ConfigureWebHostDefaults(webBuilder =>
            //    {
            //        webBuilder.UseStartup<Startup>();
            //    }).AsWebSocketHostBuilder()
            //    .UseSession<ChatSession>()
            //    .ConfigureServices((context, services) =>
            //    {
            //        services.AddSingleton<RoomService>();
            //    })                
            //    .UseCommand<StringPackageInfo, StringPackageConverter>(commandOptions =>
            //        {
            //            commandOptions.AddCommand<CON>();
            //            commandOptions.AddCommand<MSG>();
            //        });
    }
}