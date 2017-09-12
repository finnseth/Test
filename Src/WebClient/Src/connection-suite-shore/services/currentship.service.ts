import { Injectable } from '@angular/core';
import { Ship } from 'connection-suite/components/ship/interfaces';

import { ApiService, ConfigurationReader, JsonSchema } from 'dualog-common';

import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { Http } from '@angular/http';
import { SessionService } from 'connection-suite-shore/services/session.service';


@Injectable()
export class CurrentShipService extends ApiService {
    
        constructor(
             http: Http,
             authenticationService: AuthenticationService,
             sessionService: SessionService,
             configurationReader: ConfigurationReader ) {
    
            super(http, authenticationService, sessionService, configurationReader);
        }
    
        public getSelectedShip(): Ship {
            return this.sessionService.GetSelectedShip();
        }
    
        public setSelectedShip(ship: Ship):void {
            this.sessionService.SetSelectedShip(ship);
        }
    
    }
    

