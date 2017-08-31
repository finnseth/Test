using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Dualog.Data.Configuration;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore;
using Dualog.PortalService.Authentication;
using Dualog.PortalService.Controllers.Dashboard;
using Dualog.PortalService.Controllers.Users.Model;
using Dualog.PortalService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Ploeh.AutoFixture;
using Serilog;
using Dualog.PortalService.Controllers.Users;
using Dualog.PortalService.Controllers.Companies.Model;
using Dualog.PortalService.Controllers.Companies;

namespace Dualog.PortalService.Controllers
{
    public class ControllerTests : IDisposable
    {
        Fixture _fixture = new Fixture();
        long _loggedInUserId;
        long _loggedInCompanyId;

        public ControllerTests( bool regularUser = true )
        {
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .Enrich.FromLogContext()
              .WriteTo.LiterateConsole()
              .CreateLogger();

            Fixture.Customize<UserDetailsModel>( c => c.With( p => p.Email, Fixture.Create<MailAddress>().Address ) );
            Fixture.Customize<CompanyInformation>( c => c.With( p => p.Email, Fixture.Create<MailAddress>().Address ) );

            Init();
        }

        public Fixture Fixture => _fixture;
        public long LoggedInUserId => _loggedInUserId;
        public long LoggedInCompanyId => _loggedInCompanyId;

        public IDataContextFactory DataContextFactory
        {
            get
            {
                return new OracleShoreDataContextFactory(
                    new ParameterizedConnectionStringResolver(
                        "192.168.5.161",
                        1521,
                        "devcsdb.dualog.no",
                        "g20dualog",
                        "g20dualog" ) );
            }
        }


        private async void Init()
        {
            (_loggedInCompanyId, _loggedInUserId) = await DataCreation.RegisterRegularUserIntoCompany( this );
        }


        protected async Task<ObjectLookup> SetupData( string fileName, TestServer server )
        {
            var tdr = new TestDataReader();
            var testData = tdr.ReadAsync(fileName);

            var tdw = new TestDataWorker(server);

            var ol = await tdw.Execute( testData );
            return ol;
        }

        public TestServer CreateServer()
        {
            var builder = new WebHostBuilder()

                .ConfigureServices(services =>
                {


                    services.AddTransient<IDataContextFactory>(sp => DataContextFactory );

                    services.AddAuthorization(options =>
                    {
                    });

                    services.Configure<TestAuthenticationOptions>( options =>
                    {
                        options.CompanyId = LoggedInCompanyId;
                        options.UserId = LoggedInUserId;
                    } );


                    services.AddMvc(options =>
                    {
                        options.RespectBrowserAcceptHeader = true;

                    }).AddApplicationPart(typeof(DashboardController).Assembly);
                })

                .Configure(app =>
                {
                    app.UseMiddleware<TestAuthenticationMiddleware>();
                    app.UseMvc();
                });

            return new TestServer( builder );
        }




        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void OnDispose()
        {
        }

        // This code added to correctly implement the disposable pattern.
        public async void Dispose()
        {
            if( !disposedValue )
            {
                OnDispose();

                using( var dc = DataContextFactory.CreateContext() )
                {
                    await UserRepository.InternalDeleteAllUsersInCompany( dc, LoggedInCompanyId );
                    await CompanyRepository.InternalRemoveCompany( dc, LoggedInCompanyId );
                }


                disposedValue = true;
            }
        }
        #endregion
    }
}
