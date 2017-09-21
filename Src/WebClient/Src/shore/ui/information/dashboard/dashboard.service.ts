import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Rx';

import { ConfigurationReader } from './../../../../infrastructure/services/configuration-reader.service';
import { JsonSchema } from './../../../../infrastructure/services/schema/';

import { ApiService } from './../../../services/api-base.service';
import { AuthenticationService } from './../../../services/authentication.service';
import { SessionService } from './../../../services/session.service';

@Injectable()
export class DashboardService extends ApiService {

    constructor(
         http: Http,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         configurationReader: ConfigurationReader ) {

            super(http, authenticationService, sessionService, configurationReader);
         }

    public GetDashboards(): Observable<IDashboardInformation[]> {

        return super.Get<IDashboardInformation[]>( '/dashboards');
    }

    public GetDashboardById( id: number): Observable<IDashboardInformation> {

        return super.Get<IDashboardInformation>( `/dashboards/${id}` ) ;
    }

    public GetWidgetsByDashboardId( id: number): Observable<IWidgetInformation[]> {

        return super.Get<IWidgetInformation[]>( `/dashboards/${id}/widgets`) ;
    }

    public GetWidgetData( id: number): Observable<IWidgetResult> {

        return super.Get<IWidgetResult>( `/dashboards/widgets/${id}/data` );
    }

    public GetWidgetById( id: number ): Observable<IWidgetInformation> {

        return super.Get<IWidgetInformation>(`/dashboards/widgets/${id}`);
    }

    public PatchWidgetById( id: number, payload: string ): Observable<IWidgetInformation> {
        return super.Patch<IWidgetInformation>( `/dashboards/widgets/${id}`, payload );
    }

    public GetWidgetSchema(): Observable<JsonSchema> {
         return super.GetSchema(`/dashboards/widgets/0`);
    }
}

export interface IDashboardInformation {
    id: number;
    name: string;
}

export interface IWidgetInformation {
    id: number;
    height?: number;
    horizontalRank?: number;
    title: string;
    verticalRank?: number;
    width?: number;
    widgetType: string;
    data?: any;
    columns: any[];
}

export interface IWidgetResult {
    columns: IWidgetColumns[];
    data: IWidgetResultData[];
}

export interface IWidgetResultData {
    field: string;
    value: any;
}

export interface IWidgetColumns {
    id: number;
    parameterType: string;
    name: string;
}
