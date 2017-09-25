import { ApiService } from 'shore/services/api-base.service';
import { AuthenticationService } from 'shore/services/authentication.service';
import { ConfigurationReader } from 'infrastructure/services/configuration-reader.service';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { JsonSchema } from 'infrastructure/services/schema';
import { Observable } from 'rxjs/Rx';
import { SessionService } from 'shore/services/session.service';
import { Ship } from 'common/domain/ship/interfaces';

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
