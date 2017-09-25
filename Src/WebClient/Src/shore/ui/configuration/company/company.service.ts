import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';

// Infrastructure
import { ConfigurationReader } from './../../../../infrastructure/services/configuration-reader.service';
import { JsonSchema } from './../../../../infrastructure/services/schema';

// Common
import { AuthenticationService } from './../../../../common/services/authentication.service';
import { SessionService } from './../../../../common/services/session.service';
import { ApiService } from './../../../../common/services/api-base.service';

// Shore
import { MenuService } from './../../../services/menu.service';
import { ShoreCompany } from './../../../domain/company/company';

@Injectable()
export class CompanyService extends ApiService {

    constructor(
         http: Http,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         configurationReader: ConfigurationReader,
         private menuService: MenuService ) {

        super(http, authenticationService, sessionService, configurationReader);
    }

    public getCompanySchema(): Observable<JsonSchema> {
        return super.GetSchema(`/companies/0`);
    }

    public getCompany(): Observable<ShoreCompany> {
        return super.Get<ShoreCompany>( '/companies/');
    }

    public PatchCompany(id: number, payload: any ): Observable<ShoreCompany> {
        return super.Patch<ShoreCompany[]>( `/companies/${id}`, payload );
    }

}
