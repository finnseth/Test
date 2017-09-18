import { AccessRights, Availability } from 'connection-suite-shore/services/permission.service';
import { ApiService, ConfigurationReader, JsonSchema } from 'dualog-common';

import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { MenuService } from 'connection-suite-shore/services/menu.service';
import { Observable } from 'rxjs/Rx';
import { SessionService } from 'connection-suite-shore/services/session.service';
import { Ship } from 'connection-suite/components/ship/interfaces';

@Injectable()
export class QuarantineService extends ApiService {

    constructor(
         http: Http,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         configurationReader: ConfigurationReader,
         private menuService: MenuService ) {

        super(http, authenticationService, sessionService, configurationReader);
    }

    public getVesselConfigList(): Observable<QuarantineVesselConfig[]> {
        return super.Get<QuarantineVesselConfig[]>( '/email/setup/quarantine/shipquarantine');
    }

    public getVesselConfig(shipid:number): Observable<QuarantineVesselConfig> {
        return super.Get<QuarantineVesselConfig>( `/email/setup/quarantine/shipquarantine/${shipid}`);
    }

    public getCompanyConfigList(): Observable<QuarantineCompanyConfig[]> {
        return super.Get<QuarantineCompanyConfig[]>( '/email/setup/quarantine/companyquarantine');
    }

    public getVesselConfigSchema(): Observable<JsonSchema> {
        return super.GetSchema(`/email/setup/quarantine/shipquarantine/0`);
    }

    public PatchVesselQuarantine(id: number, payload: any ): Observable<QuarantineVesselConfig> {
        return super.Patch<QuarantineVesselConfig[]>( `/email/setup/quarantine/shipquarantine/${id}`, payload );
    }

    public PatchCompanyQuarantine(id: number, payload: any ): Observable<QuarantineCompanyConfig> {
        return super.Patch<QuarantineCompanyConfig[]>( `/email/setup/quarantine/companyquarantine/${id}`, payload );
    }

}

export interface QuarantineCompanyConfig {
    quarantineId: number;
    onHoldStationaryUser: boolean;
    onHoldCrew: boolean;
    notificationOnHoldOriginal: boolean;
    notificationOnHoldRecipient: boolean;
    notificationOnHoldPostmaster: boolean;
    notificationOnHoldAdmins: string;
    notificationSender: string;
    maxBodyLength: number;
    onHoldDuration: number;
}

export interface QuarantineVesselConfig extends QuarantineCompanyConfig {
    vesselId: number;
    vesselName: string;
    useThisLevel: boolean;
}
