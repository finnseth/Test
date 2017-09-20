import { ApiService } from 'shore/services/api-base.service';
import { AuthenticationService } from 'shore/services/authentication.service';
import { ConfigurationReader } from 'infrastructure/services/configuration-reader.service';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { JsonSchema } from 'infrastructure/services/schema/';
import { Observable } from 'rxjs/Rx';
import { SessionService } from 'shore/services/session.service';

@Injectable()
export class NetworkControlService extends ApiService {

    constructor(
         http: Http,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         configurationReader: ConfigurationReader ) {

        super(http, authenticationService, sessionService, configurationReader);
    }

    public getFleetRules(): Observable<ComputerRule[]> {
        return super.Get<ComputerRule[]>( '/internet/networkcontrol/rules');
    }

    public getShipRules(id: number): Observable<ComputerRule[]> {
        return super.Get<ComputerRule[]>( `/internet/networkcontrol/rules/vessels/${id}`);
    }

    public GetComputerRuleSchema(): Observable<JsonSchema> {
         return super.GetSchema(`/internet/networkcontrol/rules/vessels/0`);
    }
}

export interface ComputerRule {
    id?: number;
    method?: Method;
    description?: string;
    sourceComputer?: string;
    priority: number;
    service?: Service,
    destinations?: string[];
    Active?: boolean;
    isCompanyRule?: boolean;
}

export interface Method {
    id: number,
    name: string
}

export interface Service {
    id: number;
    name: string;
}
