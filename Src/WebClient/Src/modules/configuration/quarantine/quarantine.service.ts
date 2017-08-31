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
        return super.Get<QuarantineVesselConfig[]>( '/email/settings/quarantine/vessels');
    }

    public getCompanyConfigList(): Observable<QuarantineCompanyConfig[]> {
        return super.Get<QuarantineCompanyConfig[]>( '/email/settings/quarantine');
    }

    public getVesselConfigSchema(): Observable<JsonSchema> {
        return super.GetSchema(`/email/settings/quarantine/vessels/0`);
    }
}

export interface QuarantineCompanyConfig {
    quarantineId: number;
    useThisLevel: boolean;
    onHoldStationaryUser: boolean;
    onHoldCrew: boolean;
    notificationOnHoldOriginal: boolean;
    notificationOnHoldRecipient: boolean;
    notificationOnHoldPostmaster: boolean;
    notificationOnHoldAdmins: boolean;
    notificationSender: boolean;
    maxBodyLength: number;
    onHoldDuration: number;
}

export interface QuarantineVesselConfig extends QuarantineCompanyConfig {
    vesselId: number;
    vesselName: string;
}
