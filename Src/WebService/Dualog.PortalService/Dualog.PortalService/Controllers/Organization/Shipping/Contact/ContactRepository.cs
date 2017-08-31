using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact
{
    public class ContactRepository
    {

        IDataContextFactory _dcFactory;

        public ContactRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }

        public async Task<IEnumerable<CompanyAddressModel>> GetCompanyAddress( long companyId )
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from adr in dc.GetSet<DsAddress>()
                         where (adr.Company.Id == companyId || companyId == 0)
                            && adr.Vessel == null 
                            && adr.User == null
                         select new CompanyAddressModel
                         {
                             Id = adr.Id,
                             Name = adr.Name,
                             FirstName = adr.FirstName,
                             SurName = adr.SurName,
                             Phone = adr.PhoneNumber,
                             Status = adr.Status,
                             Company = adr.Company.Id,
                             Email = adr.Email,
                             
                             Fields = from adf in adr.Fields
                                select new AddressFieldModel
                                {
                                    Id = adf.Id,
                                    Field = adf.Field.Id,
                                    Value = adf.Value
                                },
                             AddressGroup = from adg in adr.AddressGroup
                                select new AddressGroupModel
                                {
                                    Id = adg.Id,
                                    Name = adg.Name
                                }
                         };

                return await qc.ToListAsync();                
            }
        }

        public async Task<IEnumerable<ShipAddressModel>> GetShipAddress(long companyId, long shipId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from adr in dc.GetSet<DsAddress>()
                         where (adr.Company.Id == companyId || companyId == 0)
                            && adr.Vessel.Id == shipId
                            && adr.User == null
                         select new ShipAddressModel
                         {
                             Id = adr.Id,
                             Name = adr.Name,
                             FirstName = adr.FirstName,
                             SurName = adr.SurName,
                             Phone = adr.PhoneNumber,
                             Status = adr.Status,
                             Company = adr.Company.Id,
                             Ship = adr.Vessel.Id,
                             Email = adr.Email,

                             Fields = from adf in adr.Fields
                                select new AddressFieldModel
                                {
                                    Id = adf.Id,
                                    Field = adf.Field.Id,
                                    Value = adf.Value
                                },
                             AddressGroup = from adg in adr.AddressGroup
                                select new AddressGroupModel
                                {
                                    Id = adg.Id,
                                    Name = adg.Name
                                }
                         };

                return await qc.ToListAsync();
            }
        }

        public async Task<IEnumerable<UserAddressModel>> GetUserAddress(long companyId, long userId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from adr in dc.GetSet<DsAddress>()
                         where (adr.Company.Id == companyId || companyId == 0)
                            && adr.Vessel == null
                            && adr.User.Id == userId
                         select new UserAddressModel
                         {
                             Id = adr.Id,
                             Name = adr.Name,
                             FirstName = adr.FirstName,
                             SurName = adr.SurName,
                             Phone = adr.PhoneNumber,
                             Status = adr.Status,
                             Company = adr.Company.Id,
                             User = adr.User.Id,
                             Email = adr.Email,

                             Fields = from adf in adr.Fields
                                select new AddressFieldModel
                                {
                                    Id = adf.Id,
                                    Field = adf.Field.Id,
                                    Value = adf.Value
                                },
                             AddressGroup = from adg in adr.AddressGroup
                                select new AddressGroupModel
                                {
                                    Id = adg.Id,
                                    Name = adg.Name
                                }
                         };

                return await qc.ToListAsync();
            }
        }

        public async Task<IEnumerable<CompanyAddressGroupModel>> GetCompanyAddressGroup(long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from adg in dc.GetSet<DsAddressGroup>()
                         where (adg.Company.Id == companyId || companyId == 0)
                            && adg.Vessel == null
                            && adg.User == null
                         select new CompanyAddressGroupModel
                         {
                             Id = adg.Id,
                             Name = adg.Name,
                             Company = adg.Company.Id,
                             Address = from adr in adg.Address
                                       select new AddressModel
                                       {
                                           Id = adr.Id,
                                           Name = adr.Name
                                       }
                         };

                return await qc.ToListAsync();
            }
        }

        public async Task<IEnumerable<ShipAddressGroupModel>> GetShipAddressGroup(long companyId, long shipId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from adg in dc.GetSet<DsAddressGroup>()
                         where (adg.Company.Id == companyId || companyId == 0)
                            && adg.Vessel.Id == shipId
                            && adg.User == null
                         select new ShipAddressGroupModel
                         {
                             Id = adg.Id,
                             Name = adg.Name,
                             Company = adg.Company.Id,
                             Ship = adg.Vessel.Id,
                             Address = from adr in adg.Address
                                       select new AddressModel
                                       {
                                           Id = adr.Id,
                                           Name = adr.Name
                                       }
                         };

                return await qc.ToListAsync();
            }
        }

        public async Task<IEnumerable<UserAddressGroupModel>> GetUserAddressGroup(long companyId, long userId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from adg in dc.GetSet<DsAddressGroup>()
                         where (adg.Company.Id == companyId || companyId == 0)
                            && adg.Vessel == null
                            && adg.User.Id == userId
                         select new UserAddressGroupModel
                         {
                             Id = adg.Id,
                             Name = adg.Name,
                             Company = adg.Company.Id,
                             User = adg.User.Id,
                             Address = from adr in adg.Address
                                       select new AddressModel
                                       {
                                           Id = adr.Id,
                                           Name = adr.Name
                                       }
                         };

                return await qc.ToListAsync();
            }
        }
        
        
        public async Task<IEnumerable<ImportAddressModel>> GetImportAddress(long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {

                var qc = from ia in GetImportAddressQuery(dc)
                         where (ia.Company == companyId || companyId == 0)
                         select ia;

                return await qc.ToListAsync();
            }
        }

        private static IQueryable<ImportAddressModel> GetImportAddressQuery(IDataContext dc)
        {
            return from ia in dc.GetSet<DsImportAddress>()
                   select new ImportAddressModel
                   {
                       Id = ia.Id,
                       Recipient = ia.Recipient,
                       ReturnRecipient = ia.ReturnRecipient,
                       Sender = ia.Sender,
                       Company = ia.Company.Id,
                       Ship = ia.Vessel.Id,
                       User = ia.User.Id
                   };
        }
    }
}
