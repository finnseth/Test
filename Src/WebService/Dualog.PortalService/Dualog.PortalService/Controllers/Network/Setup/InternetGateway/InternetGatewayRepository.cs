using System;
using System.Collections.Generic;
using System.Linq;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Network.Setup.InternetGateway.Model;
using System.Threading.Tasks;
using Dualog.Data.Oracle.Shore.Model;
using System.Data.Entity;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Network.Setup.InternetGateway
{
    public class InternetGatewayRepository
    {
        IDataContextFactory _dcFactory;

        public InternetGatewayRepository( IDataContextFactory dcFactory )
        {
            _dcFactory = dcFactory;
        }

        public async Task<GenericDataModel<IEnumerable<CarrierTypeModel>>> GetMethods()
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var q = from m in dc.GetSet<DsCarrier>()
                        where (m.RowStatus ?? 0) == 0
                        select new CarrierTypeModel {
                            Id = m.Id,
                            ModemHandShake = m.ModemHandShake ?? 0,
                            Name = m.Name,
                            Priority = m.Priority ?? 0,
                        };

                return new GenericDataModel<IEnumerable<CarrierTypeModel>>()
                {
                    Value = await q.ToListAsync(),
                };

            }
        }

        public async Task<GenericDataModel<IEnumerable<CommunicationMethodModel>>> GetCommMethods(long companyId, long vesselId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var q = from e in dc.GetSet<DsCarrierVesselConfig>()
                        where e.VesselEquipment.Vessel.Id == vesselId && e.VesselEquipment.RowStatus <= 1
                        select new CommunicationMethodModel
                        {
                            Id = e.Id,
                            Description = e.Description,
                            CarrierName = e.VesselEquipment.Carrier.Name,
                            //Enabled = e.RowStatus == 0
                        };

                return new GenericDataModel<IEnumerable<CommunicationMethodModel>>()
                {
                    Value = await q.ToListAsync(),
                };
            }
        }

        public async Task<GenericDataModel<CommunicationMethodDetailsModel>> GetSingleComMethod(long companyId, long vesselId, long id)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var q = from e in dc.GetSet<DsCarrierVesselConfig>()
                            //where e.Vessel.Id == vesselId && e.RowStatus <= 1
                        let ve = e.VesselEquipment
                        let mv = ve.ModemVessel
                        select new CommunicationMethodDetailsModel
                        {
                            Id = e.Id,

                            AgreementId = e.Agreement.Id,
                            Description = e.Description,
                            AlwaysCloseFbb = e.AlwaysCloseFbb ?? false,
                            AsyncRas = e.AsyncRas ?? false,
                            BTDelay = (e.BTDelay ?? false) ? 0 : 1,
                            ChainId = e.ChainId == 0,
                            ChangeStandardGateway = e.ChangeStandardGateway ?? false,
                            ConfigEnabled = e.Enabled ?? false,
                            EnablePing = e.EnablePing ?? false,
                            InmcDelay = e.InmcDelay ?? 0,
                            IpRasProfile = e.IpRasProfile,
                            Outbound = e.Outbound ?? false,
                            Priority = e.Priority ?? 0,
                            SchedulingAllowed = e.SchedulingAllowed ?? false,
                            Service = e.Service,
                            Skip = e.Skip ?? 0,
                            StaticRoute = e.StaticRoute ?? false,
                            Threshold = e.Threshold ?? 0,
                            Web4SeaHighPriBW = e.Web4SeaHighPriBW ?? 0,
                            Web4SeaLowPriBW = e.Web4SeaLowPriBW ?? 0,
                            Web4SeaMediumPriBW = e.Web4SeaMediumPriBW ?? 0,

                            Enabled = ve.RowStatus == 0 && e.Rowstatus == 0,
                            ComBitRate = ve.ComBitRate ?? 0,
                            ComDataBits = ve.ComDataBits ?? 0,
                            ComFlowControl = ve.ComFlowControl ?? 0,
                            ComParity = ve.ComParity ?? 0,
                            ComPort = ve.ComPort ?? 0,
                            ComStopBits = ve.ComStopBits ?? 0,
                            ComTimeout = ve.ComTimeout ?? 0,
                            EquipmentIp = ve.EquipmentIp,
                            GatewayIp = ve.GatewayIp,

                            BitRate = mv.BitRate ?? 0,
                            BtMaxSize = mv.BtMaxSize ?? 0,
                            BTNumber = mv.BTNumber,
                            BTNumberRead = mv.BTNumberRead,
                            CheckPin = mv.CheckPin,
                            CloseComDelay = mv.CloseComDelay ?? 0,
                            CommandTimeout = mv.CommandTimeout ?? 0,
                            ConnectionDelay = mv.ConnectionDelay ?? 0,
                            ConnectionRetries = mv.ConnectionRetries ?? 0,
                            CurrentAction = mv.CurrentAction,
                            DbSignalRule = mv.DbSignalRule,
                            DialPrefix = mv.DialPrefix,
                            DualogEmail = mv.DualogEmail,
                            EmaPassword = mv.EmaPassword,
                            EmaUsername = mv.EmaUsername,
                            ExtInitString1 = mv.ExtInitString1,
                            ExtInitString2 = mv.ExtInitString2,
                            ExtInitString3 = mv.ExtInitString3,
                            ExtInitString4 = mv.ExtInitString4,
                            ExtInitString5 = mv.ExtInitString5,
                            ExtInitString6 = mv.ExtInitString6,
                            FbbCloseService = mv.FbbCloseService,
                            FbbCloseServiceOk = mv.FbbCloseServiceOk,
                            FbbConnectService = mv.FbbConnectService,
                            FbbConnectServiceOk = mv.FbbConnectServiceOk,
                            FbbDefineService = mv.FbbDefineService,
                            FbbDefineServiceOk = mv.FbbDefineServiceOk,
                            FbbServiceClose = mv.FbbServiceClosed,
                            FbbServiceCloseOk = mv.FbbServiceClosedOk,
                            FbbServiceOpen = mv.FbbServiceOpen,
                            FbbServiceOpenOk = mv.FbbServiceOpenOk,
                            FleetEndLineFeed = mv.FleetEndLineFeed,
                            FleetNewLine = mv.FleetNewLine,
                            GatewayNumber = mv.GatewayNumber,
                            IgnoreAtCommands = mv.IgnoreAtCommands ?? false,
                            InitSilenceTmo = mv.InitSilenceTmo ?? 0,
                            InitString1 = mv.InitString1,
                            InitString2 = mv.InitString2,
                            InitString3 = mv.InitString3,
                            InitString4 = mv.InitString4,
                            InitString5 = mv.InitString5,
                            InitString6 = mv.InitString6,
                            InmcDefaultRouting = mv.InmcDefaultRouting ?? false,
                            InmcMaxSize = mv.InmcMaxSize ?? 0,
                            InmcNumber = mv.InmcNumber,
                            InmcNumberRead = mv.InmcNumberRead,
                            InmcPrintTelexMessage = mv.InmcPrintTelexMessage ?? false,
                            InmcProgram = mv.InmcProgram,
                            IpHostName = mv.IpHostName,
                            IpPort = mv.IpPort ?? 0,
                            LoadPin = mv.LoadPin,
                            Manufacturer = mv.Manufacturer,
                            MinimumSignal = mv.MinimumSignal ?? 0,
                            Model = mv.Model,
                            ModemType = mv.ModemType,
                            NoCarrierIsOk = mv.NoCarrierIsOk ?? 0,
                            NumberMidFix = mv.NumberMidfix,
                            NumberPrefix = mv.NumberPrefix,
                            NumberSuffix = mv.NumberSuffix,
                            OpenComDelay = mv.OpenComDelay ?? 0,
                            PacketSizeDplx = mv.PacketSizeDplx ?? 0,
                            PauseBetweenRetries = mv.PauseBetweenRetries ?? 0,
                            PowerOn = mv.PowerOn,
                            PreInitAtCommands = mv.PreInitAtCommands ?? 0,
                            ProviderFormat = mv.ProviderFormat,
                            ServicePort = mv.ServicePort ?? 0,
                            ServiceProvider = mv.ServiceProvider,
                            Signal = mv.Signal,
                            SignalRangeMax = mv.SignalRangeMax ?? 0,
                            SignalRangeMin = mv.SignalRangeMin ?? 0,
                            SlidingWindowDplx = mv.SlidingWindowDplx ?? 0,
                            SlidingWindowZmdm = mv.SlidingWindowZmdm ?? 0,
                            StallTime = mv.StallTime ?? 0,
                            ToggleDtr = mv.ToggleDtr ?? false,
                            Version = mv.Version,
                            WaitForCarrier = mv.WaitForCarrier ?? 0,
                            WaitForUser = mv.WaitForUser ?? 0
                        };

                CommunicationMethodDetailsModel communicationMethodDetails = await q.FirstOrDefaultAsync();
                if (communicationMethodDetails == null)
                    throw new NotFoundException();

                return new GenericDataModel<CommunicationMethodDetailsModel>()
                {
                    Value = communicationMethodDetails,
                };
            }
        }

    }
}
