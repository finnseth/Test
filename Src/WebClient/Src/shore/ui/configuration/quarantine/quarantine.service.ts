import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Rx';

import { ConfigurationReader } from './../../../../infrastructure/services/configuration-reader.service';
import { JsonSchema } from './../../../../infrastructure/services/schema';

import { Ship } from './../../../../common/domain/ship/interfaces';
import { SessionService } from './../../../../common/services/session.service';
import { AuthenticationService } from './../../../../common/services/authentication.service';
import { ApiService } from './../../../../common/services/api-base.service';

import { AccessRights, Availability } from './../../../services/permission.service';
import { MenuService } from './../../../services/menu.service';


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
