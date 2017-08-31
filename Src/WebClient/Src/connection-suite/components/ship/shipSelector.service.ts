import { ApiService, ConfigurationReader, JsonSchema } from 'dualog-common';

import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { SessionService } from 'connection-suite-shore/services/session.service';
import { Ship } from './interfaces';

@Injectable()
export class ShipSelectorService extends ApiService {

    constructor(
         http: Http,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         configurationReader: ConfigurationReader ) {

        super(http, authenticationService, sessionService, configurationReader);
    }

    public getVessel(vesselname: string): Observable<Ship[]> {
        return super.Get<Ship[]>( `/vessels`); // /${vesselname}
    }

    public getShips(): Observable<Ship[]> {
        return super.Get<any[]>( `/vessels`); // /${vesselname}
    }

    /*
     *
     * All advanced search request are gathered here
     *
     */

    public getShipInComputerRuleContext(): Observable<Ship[]> {
        return super.Get<Ship[]>( `/vessels`);
    }

    public getShipInUserContext(): Observable<Ship[]> {
        return super.Get<Ship[]>( `/vessels`);
    }

    public getShipInQuarantineContext(): Observable<Ship[]> {
        return super.Get<Ship[]>( `/vessels`);
    }
}

