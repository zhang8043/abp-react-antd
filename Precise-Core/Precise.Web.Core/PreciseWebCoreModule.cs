using System;
using System.IO;
using System.Text;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Configuration.Startup;
using Abp.Hangfire;
using Abp.IO;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Precise.Configuration;
using Precise.EntityFrameworkCore;
using Precise.Web.Authentication.JwtBearer;
using Precise.Web.Authentication.TwoFactor;
using Precise.Web.Configuration;

namespace Precise
{
    [DependsOn(
         typeof(PreciseApplicationModule),
         typeof(PreciseWeChatModule),
         typeof(PreciseEntityFrameworkCoreModule),
         typeof(AbpAspNetCoreSignalRModule),
         typeof(AbpAspNetCoreModule),
         typeof(AbpRedisCacheModule), //如果不使用ReIDS缓存，可以删除AppDestCache模块依赖性。
         typeof(AbpHangfireAspNetCoreModule) //如果不使用Hangfire，可以删除Abp.Hangfire.AspNetCore依赖性
     )]
    public class PreciseWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public PreciseWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            //设置默认连接字符串
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                PreciseConsts.ConnectionStringName
            );

            //使用数据库进行语言管理
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Caching.Configure(TwoFactorCodeCacheItem.CacheName, cache =>
            {
                cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(2);
            });


            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(PreciseApplicationModule).GetAssembly()
                );

            Configuration.Modules.AbpAspNetCore()
              .CreateControllersForAppServices(
                  typeof(PreciseWeChatModule).GetAssembly()
              );

            if (_appConfiguration["Authentication:JwtBearer:IsEnabled"] != null && bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"]))
            {
                ConfigureTokenAuth();
            }

            Configuration.ReplaceService<IAppConfigurationAccessor, AppConfigurationAccessor>();

            //取消注释此行以使用Hangfire而不是默认的背景作业管理器（还记得取消注释Startup.cs文件中的相关行）。
            //Configuration.BackgroundJobs.UseHangfire();

            //取消此行使用ReIS-Cache代替内存缓存。
            //有关ReIIS配置和连接字符串的app.config 
            //Configuration.Caching.UseRedis(options =>
            //{
            //    options.ConnectionString = _appConfiguration["Abp:RedisCache:ConnectionString"];
            //    options.DatabaseId = _appConfiguration.GetValue<int>("Abp:RedisCache:DatabaseId");
            //});
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PreciseWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            SetAppFolders();
        }

        private void SetAppFolders()
        {
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = Path.Combine(_env.WebRootPath, $"Common{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}SampleProfilePics");
            appFolders.TempFileDownloadFolder = Path.Combine(_env.WebRootPath, $"Temp{Path.DirectorySeparatorChar}Downloads");
            appFolders.WebLogsFolder = Path.Combine(_env.ContentRootPath, $"App_Data{Path.DirectorySeparatorChar}Logs");

#if NET461
            if (_env.IsDevelopment())
            {
                var currentAssemblyDirectoryPath = typeof(PreciseWebCoreModule).GetAssembly().GetDirectoryPathOrNull();
                if (currentAssemblyDirectoryPath != null)
                {
                    appFolders.WebLogsFolder = Path.Combine(currentAssemblyDirectoryPath, $"App_Data{Path.DirectorySeparatorChar}Logs");
                }
            }
#endif

            try
            {
                DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder);
            }
            catch { }
        }
    }
}
