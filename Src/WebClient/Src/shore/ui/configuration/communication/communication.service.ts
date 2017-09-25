import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';

import { JsonSchema } from './../../../../infrastructure/services/schema';
import { ConfigurationReader } from './../../../../infrastructure/services/configuration-reader.service';

import { AuthenticationService } from './../../../../common/services/authentication.service';
import { SessionService } from './../../../../common/services/session.service';
import { ApiService } from './../../../../common/services/api-base.service';

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

