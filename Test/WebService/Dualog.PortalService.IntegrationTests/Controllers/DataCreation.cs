using System;
using System.Linq;
using Dualog.PortalService.Controllers.Users;
using Dualog.PortalService.Controllers.Users.Model;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Dualog.PortalService.Controllers.Companies;
using Dualog.PortalService.Controllers.Companies.Model;
using Dualog.Data.Entity;

namespace Dualog.PortalService.Controllers
{
    public static class DataCreation
    {
        public static async Task<(long companyId, long userId)> RegisterRegularUserIntoCompany( ControllerTests controllerTests )
        {
            var user = controllerTests.Fixture.Create<UserDetailsModel>();
            var company = controllerTests.Fixture.Create<CompanyInformation>();

            await CompanyRepository.InternalAddCompany( controllerTests.DataContextFactory.CreateContext(), company );
            await UserRepository.InternalCreateUser( controllerTests.DataContextFactory.CreateContext(), user, company.Id );

            return (company.Id, user.Id);
        }
    }
}
