using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

public class Startup : Datasilk.Startup {

    public override void Configured(IApplicationBuilder app, IHostingEnvironment env, IConfigurationRoot config)
    {
        base.Configured(app, env, config);
        Legendary.Query.QuerySql.connectionString = server.sqlConnectionString;
        var query = new Legendary.Query.Users();
        server.resetPass = query.HasPasswords();
        server.hasAdmin = query.HasAdmin();
    }
}
