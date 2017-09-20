import { ApiService, PaginationInfo } from 'shore/services/api-base.service';

import { AuthenticationService } from 'shore/services/authentication.service';
import { ConfigurationReader } from 'infrastructure/services/configuration-reader.service';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { SessionService } from 'shore/services/session.service';

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
