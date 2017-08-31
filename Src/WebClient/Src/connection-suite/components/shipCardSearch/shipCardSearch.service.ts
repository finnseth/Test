import { ApiService, ConfigurationReader, JsonSchema } from 'dualog-common';

import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { SessionService } from 'connection-suite-shore/services/session.service';
import { Ship } from 'connection-suite/components/ship/interfaces';

@Injectable()
export class ShipCardSearchService extends ApiService {

    constructor(
         http: Http,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         configurationReader: ConfigurationReader ) {

        super(http, authenticationService, sessionService, configurationReader);
    }

    public getQuarantineShips(): Observable<Ship[]> {
        return super.Get<any[]>( `/organization/shipping/shipwithquarantineinfo`);
    }

    public getShips(): Observable<Ship[]> {
        return super.Get<any[]>( `/vessels`); // /${vesselname}
    }

    public getSelectedShip(): Ship {
        return this.sessionService.GetSelectedShip();
    }

    public setSelectedShip(ship: Ship):void {
        this.sessionService.SetSelectedShip(ship);
    }

}
