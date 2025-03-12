namespace AuditSystem.Auth.Configuration
{
    public static class ApplicationBuilderExtentions
    {
        public static IApplicationBuilder ConfigureMiddlewares(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();

            return app;
        }
    }
}
