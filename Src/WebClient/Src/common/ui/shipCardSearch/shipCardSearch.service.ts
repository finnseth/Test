import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Rx';

import { JsonSchema } from './../../../infrastructure/services/schema';
import { ConfigurationReader } from './../../../infrastructure/services/configuration-reader.service';

import { Ship } from './../../domain/ship/interfaces';

import { AuthenticationService } from './../../services/authentication.service';
import { SessionService } from './../../services/session.service';

import { ApiService } from './../../services/api-base.service';


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
        return super.Get<any[]>( `/organization/shipping/ship`); // /${vesselname}
    }

    public getSelectedShip(): Ship {
        return this.sessionService.GetSelectedShip();
    }

    public setSelectedShip(ship: Ship):void {
        this.sessionService.SetSelectedShip(ship);
    }

}
