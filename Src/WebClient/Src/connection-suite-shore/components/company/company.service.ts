import { ApiService, ConfigurationReader, JsonSchema } from 'dualog-common';

import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { SessionService } from 'connection-suite-shore/services/session.service';

@Injectable()
export class CompanyService extends ApiService {

    constructor(
         http: Http,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         configurationReader: ConfigurationReader ) {

        super(http, authenticationService, sessionService, configurationReader);
    }

    public getCompanies(): Observable<Company[]> {
        return super.Get<Company[]>( '/companies');
    }
}

export interface Company {
    id: number;
    name: string;
}
