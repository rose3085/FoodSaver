namespace FoodSaver
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Your service configurations
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
