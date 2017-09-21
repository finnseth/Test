import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';

import { JsonSchema } from './../../../../infrastructure/services/schema';
import { ConfigurationReader } from './../../../../infrastructure/services/configuration-reader.service';

import { ApiService } from './../../../services/api-base.service';
import { AuthenticationService } from './../../../services/authentication.service';
import { SessionService } from './../../../services/session.service';

@Injectable()
export class CommunicationService extends ApiService {

    constructor(
         http: Http,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         configurationReader: ConfigurationReader ) {

        super(http, authenticationService, sessionService, configurationReader);
    }
}

