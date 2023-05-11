using Infrastructures.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Product.API.Extensions
{
    public static class ApplicationExtentions
    {
        public static void UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.OAuthClientId("tedu_microservices_swagger");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API");
                c.DisplayRequestDuration();
            });
            app.UseMiddleware<ErrorWrappingMiddleware>();
            app.UseAuthentication();

            app.UseRouting();
            //app.UseHttpsRedirection();

             app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                //{
                //    Predicate = _ => true,
                //    // ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                //});
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
