import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

import { ConfigurationReader } from '../../infrastructure/services/configuration-reader.service';
import { JsonSchema } from '../../infrastructure/services/schema';
import { SessionService } from '../../common/services/session.service';
import { Ship } from '../../common/domain/ship/interfaces';
import { AuthenticationService } from '../../common/services/authentication.service';

import { ApiService } from '../../common/services/api-base.service';

@Injectable()
export class CurrentShipService extends ApiService {

    constructor(
        http: Http,
        authenticationService: AuthenticationService,
        sessionService: SessionService,
        configurationReader: ConfigurationReader) {

        super(http, authenticationService, sessionService, configurationReader);
    }

    public getSelectedShip(): Ship {
        return this.sessionService.GetSelectedShip();
    }

    public setSelectedShip(ship: Ship): void {
        this.sessionService.SetSelectedShip(ship);
    }

}


