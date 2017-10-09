using System;
using System.Linq;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Dualog.PortalService.Controllers.Organization.Shipping.User.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Company.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Company;
using Dualog.PortalService.Controllers.Organization.Shipping.User;

namespace Dualog.PortalService.Controllers
{
    public static class DataCreation
    {
        public static async Task<(long companyId, long userId)> RegisterRegularUserIntoCompany( ControllerTests controllerTests )
        {
            var user = controllerTests.Fixture.Create<UserDetailModel>();
            var company = controllerTests.Fixture.Create<CompanyModel>();

            await CompanyRepository.InternalAddCompany( controllerTests.DataContextFactory.CreateContext(), company );
            await UserRepository.InternalCreateUser( controllerTests.DataContextFactory.CreateContext(), user, company.Id );

            return (company.Id, user.Id);
        }
    }
}
