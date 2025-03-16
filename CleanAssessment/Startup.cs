namespace CleanAssessment
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<MyDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString(nameof(MyDbContext))));

            services.AddMediatR(config => { });

            //services.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));
            //services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            services.AddControllers();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseWebAssemblyDebugging();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //}

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
