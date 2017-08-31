using Dualog.PortalService.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dualog.PortalService.Controllers.CommunicationMethods.Model
{
    public class CommunicationMethodDetailsModel : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        [Required( AllowEmptyStrings = true, ErrorMessage = "Email is required." )]
        [NoWhitespace]
        [StringLength( 100, ErrorMessage = ValidationStrings.StringToLong )]
        public string Description { get; set; }

        public bool AlwaysCloseFbb { get; set; }

        public bool AsyncRas { get; set; }

        [Range(0, 9999)]
        public int BTDelay { get; set; }

        public bool ChainId { get; set; }

        public bool ChangeStandardGateway { get; set; }

        public bool ConfigEnabled { get; set; }

        public bool EnablePing { get; set; }

        [Range( 0, 9999 )]
        public int InmcDelay { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string IpRasProfile { get; set; }

        public bool Outbound { get; set; }

        [Range( 0, 99 )]
        public int Priority { get; set; }

        public bool SchedulingAllowed { get; set; }

        [Range( 0, 99 )]
        public int Service { get; set; }

        [Range( 0, 9999 )]
        public int Skip { get; set; }

        public bool StaticRoute { get; set; }

        [Range( 0, 999999999999 )]
        public int Threshold { get; set; }

        [Range( 0, 99999999 )]
        public int Web4SeaHighPriBW { get; set; }

        [Range( 0, 99999999 )]
        public int Web4SeaLowPriBW { get; set; }

        [Range( 0, 99999999 )]
        public int Web4SeaMediumPriBW { get; set; }

        public string CarrierName { get; private set; }

        public bool Enabled { get; set; }

        [Range( 0, 99999999 )]
        public int ComBitRate { get; set; }

        [Range( 0, 999999 )]
        public int ComDataBits { get; set; }

        [Range( 0, 999999 )]
        public int ComFlowControl { get; set; }

        [Range( 0, 999999 )]
        public int ComParity { get; set; }

        [Range( 0, 99999999 )]
        public int ComPort { get; set; }

        [Range( 0, 999999 )]
        public int ComStopBits { get; set; }

        [Range( 0, 999999 )]
        public int ComTimeout { get; set; }

        [StringLength( 100, ErrorMessage = ValidationStrings.StringToLong )]
        public string EquipmentIp { get; set; }

        [StringLength( 100, ErrorMessage = ValidationStrings.StringToLong )]
        public string GatewayIp { get; set; }

        [Range( 0, 99999999 )]
        public int BitRate { get; set; }

        [Range( 0, 9999999999 )]
        public int BtMaxSize { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string BTNumber { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string BTNumberRead { get; set; }

        [StringLength( 100, ErrorMessage = ValidationStrings.StringToLong )]
        public string CheckPin { get; set; }

        [Range( 0, 999999 )]
        public int CloseComDelay { get; set; }

        [Range( 0, 999999 )]
        public int CommandTimeout { get; set; }

        [Range( 0, 999999 )]
        public int ConnectionDelay { get; set; }

        [Range( 0, 999 )]
        public int ConnectionRetries { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string CurrentAction { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string DbSignalRule { get; set; }

        [StringLength( 10, ErrorMessage = ValidationStrings.StringToLong )]
        public string DialPrefix { get; set; }

        [StringLength( 100, ErrorMessage = ValidationStrings.StringToLong )]
        [EmailAddress( ErrorMessage = "Invalid email address." )]
        public string DualogEmail { get; set; }

        [StringLength( 255, ErrorMessage = ValidationStrings.StringToLong )]
        public string EmaPassword { get; set; }

        [StringLength( 255, ErrorMessage = ValidationStrings.StringToLong )]
        public string EmaUsername { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string ExtInitString1 { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string ExtInitString2 { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string ExtInitString3 { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string ExtInitString4 { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string ExtInitString5 { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string ExtInitString6 { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string FbbCloseService { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string FbbCloseServiceOk { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string FbbConnectService { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string FbbConnectServiceOk { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string FbbDefineService { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string FbbDefineServiceOk { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string FbbServiceClose { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string FbbServiceCloseOk { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string FbbServiceOpen { get; set; }

        [StringLength( 300, ErrorMessage = ValidationStrings.StringToLong )]
        public string FbbServiceOpenOk { get; set; }

        [StringLength( 10, ErrorMessage = ValidationStrings.StringToLong )]
        public string FleetEndLineFeed { get; set; }

        [StringLength( 10, ErrorMessage = ValidationStrings.StringToLong )]
        public string FleetNewLine { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string GatewayNumber { get; set; }

        public bool IgnoreAtCommands { get; set; }

        [Range(0, 99999999)]
        public int InitSilenceTmo { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string InitString1 { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string InitString2 { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string InitString3 { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string InitString4 { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string InitString5 { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string InitString6 { get; set; }

        public bool InmcDefaultRouting { get; set; }

        [Range( 0, 999999 )]
        public int InmcMaxSize { get; set; }

        [StringLength( 20, ErrorMessage = ValidationStrings.StringToLong )]
        public string InmcNumber { get; set; }

        [StringLength( 20, ErrorMessage = ValidationStrings.StringToLong )]
        public string InmcNumberRead { get; set; }

        public bool InmcPrintTelexMessage { get; set; }

        [StringLength( 200, ErrorMessage = ValidationStrings.StringToLong )]
        public string InmcProgram { get; set; }

        [StringLength( 200, ErrorMessage = ValidationStrings.StringToLong )]
        public string IpHostName { get; set; }

        [Range(0, ushort.MaxValue)]
        public int IpPort { get; set; }

        [StringLength( 100, ErrorMessage = ValidationStrings.StringToLong )]
        public string LoadPin { get; set; }

        [StringLength( 30, ErrorMessage = ValidationStrings.StringToLong )]
        public string Manufacturer { get; set; }

        [Range(0, 99999999)]
        public int MinimumSignal { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string Model { get; set; }

        [StringLength( 100, ErrorMessage = ValidationStrings.StringToLong )]
        public string ModemType { get; set; }

        [Range( 0, 99999999 )]
        public int NoCarrierIsOk { get; set; }

        [StringLength( 10, ErrorMessage = ValidationStrings.StringToLong )]
        public string NumberMidFix { get; set; }

        [StringLength( 10, ErrorMessage = ValidationStrings.StringToLong )]
        public string NumberPrefix { get; set; }

        [StringLength( 10, ErrorMessage = ValidationStrings.StringToLong )]
        public string NumberSuffix { get; set; }

        [Range( 0, 999999 )]
        public int OpenComDelay { get; set; }

        [Range( 0, 99999999 )]
        public int PacketSizeDplx { get; set; }

        [Range( 0, 999 )]
        public int PauseBetweenRetries { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string PowerOn { get; set; }

        [Range( 0, 9999 )]
        public int PreInitAtCommands { get; set; }

        [StringLength( 100, ErrorMessage = ValidationStrings.StringToLong )]
        public string ProviderFormat { get; set; }

        [Range( 0, ushort.MaxValue )]
        public int ServicePort { get; set; }

        [StringLength( 100, ErrorMessage = ValidationStrings.StringToLong )]
        public string ServiceProvider { get; set; }

        [StringLength( 50, ErrorMessage = ValidationStrings.StringToLong )]
        public string Signal { get; set; }

        [Range( 0, 9999 )]
        public int SignalRangeMax { get; set; }

        [Range( 0, 9999 )]
        public int SignalRangeMin { get; set; }

        [Range( 0, 999999 )]
        public int SlidingWindowDplx { get; set; }

        [Range( 0, 999999 )]
        public int SlidingWindowZmdm { get; set; }

        [Range( 0, 999999 )]
        public int StallTime { get; set; }

        [Range( 0, 99 )]
        public bool ToggleDtr { get; set; }

        [StringLength( 30, ErrorMessage = ValidationStrings.StringToLong )]
        public string Version { get; set; }

        [Range( 0, 999999 )]
        public int WaitForCarrier { get; set; }

        [Range( 0, 999999 )]
        public int WaitForUser { get; set; }

        public long AgreementId { get; internal set; }
    }
}
