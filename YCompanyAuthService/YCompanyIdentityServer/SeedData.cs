﻿using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YCompanyIdentityServer.Data;
using YCompanyIdentityServer.Models;

namespace YCompanyIdentityServer;

public class SeedData
{
    public static async Task<IServiceScope> EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
        await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
        await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

        var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        if (await userManger.FindByNameAsync("amit.chawla") == null)
        {
            await userManger.CreateAsync(DevelopmentSeedData.DefaultUser, DevelopmentSeedData.DefaultPassword);
        }
        var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

        /*
         * Api resources
         *
         */
        if (!await configurationDbContext.ApiResources.AnyAsync())
        {
            await configurationDbContext.ApiResources.AddAsync(DevelopmentSeedData.ApiResource.ToEntity());
            await configurationDbContext.SaveChangesAsync();
        }

        /*
         * Api Scopes
         *
         */
        if (!await configurationDbContext.ApiScopes.AnyAsync())
        {
            await configurationDbContext.ApiScopes.AddAsync(DevelopmentSeedData.ApiScope.ToEntity());

            await configurationDbContext.SaveChangesAsync();
        }

        /*
        * Clients
        *
        */
        if (!await configurationDbContext.Clients.AnyAsync())
        {
            await configurationDbContext.Clients.AddRangeAsync(DevelopmentSeedData.ClientEntities);

            await configurationDbContext.SaveChangesAsync();

            /*
             * Identity Resources.
             * 
             */
            if (!await configurationDbContext.IdentityResources.AnyAsync())
            {
                await configurationDbContext.IdentityResources.AddRangeAsync(DevelopmentSeedData.IdentityResourceEntities);

                await configurationDbContext.SaveChangesAsync();
            }
        }

        return scope;
    }
}
