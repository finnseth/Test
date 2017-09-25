import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';

import { ConfigurationReader } from 'infrastructure/services/configuration-reader.service';

import { SessionService } from './../../services/session.service';
import { AuthenticationService } from './../../services/authentication.service';

import { ApiService, PaginationInfo } from './../../services/api-base.service';


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
