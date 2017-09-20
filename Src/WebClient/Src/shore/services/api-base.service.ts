import { Headers, Http, RequestOptions, Response, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';

import { AuthenticationService } from '../../shore/services/authentication.service';
import { ConfigurationReader } from '../../infrastructure/services/configuration-reader.service';
import { SessionService } from '../../shore/services/session.service';

export class PaginationInfo {
    offset?: number;
    count?: number;
    includeTotalCount: boolean;

    constructor( offset?: number, count?: number, includeTotalCount = false) {

        this.count = count;
        this.offset = offset;
        this.includeTotalCount = includeTotalCount;
    }

    public toURLSearchParams(): URLSearchParams {
        const qs = new URLSearchParams();

        if (this.offset != null) {
            qs.set('offset', this.offset.toString() );
        }
        if (this.count != null) {
            qs.set('count', this.count.toString());
        }
        qs.set('includeTotalCount', `${this.includeTotalCount}`);

        return qs;
    }
}

export abstract class ApiService {

    authHeaders: Headers;

    constructor(
         protected http: Http,
         protected authenticationService: AuthenticationService,
         protected sessionService: SessionService,
         protected configurationReader: ConfigurationReader) {}

    // Returns an  Observable with the data returned from the API
    public Get<T>( url: string, qs: URLSearchParams = null ): Observable<T> {

        const config = this.configurationReader.GetConfiguration();
        // const opt = new RequestOptions({
        //     params: qs,
        //     headers: new Headers({
        //         'content-type': 'application/json'
        //     })
        // });


        const opt = new RequestOptions();
        opt.params = qs;
        opt.headers = new Headers({
            'content-type': 'application/json'
        });


        // Send the GET request and return the response as a json object
        return this.AuthGet( config.dualogApi.server + '/api/v1' + url, opt  ).map( (response: Response) => {

            return response.json() || {};
        } ).catch(this.handleError);
    }

    private AuthGet(url: string, options: RequestOptions = undefined): Observable<Response> {
        return this.http.get(url, this._setRequestOptions(options));
    }

    public Patch<T>( url: string, patch: any ) {

        const config = this.configurationReader.GetConfiguration();
        const opt = new RequestOptions({

            headers: new Headers({

                'content-type': 'application/json'
            })
        });

        // Send the PATCH request and return the response as a json object
        // todo: Need a strategy for this configuration
        return this.AuthPatch(
            config.dualogApi.server + '/api/v1' + url,
            patch,
            opt ).map( (response: Response)  => {

            return response.json() || {};
        });
    }

    private AuthPatch(url: string, data: any, options: RequestOptions = undefined): Observable<Response> {

        // const body = JSON.stringify(data);
        return this.http.patch(url, data, this._setRequestOptions(options));
    }


    public GetSchema( url: string ) {

        const config = this.configurationReader.GetConfiguration();
        const opt = new RequestOptions({
            headers: new Headers({

                'accept': 'application/schema+json'
            })
        });

        // Send the GET request and return the response as a json object

        // Send the GET request and return the response as a json object
        return this.AuthGet( config.dualogApi.server + '/api/v1' + url, opt  ).map( (response: Response) => {

            return response.json() || {};
        } ).catch(this.handleError);
    }


    /**
     * @param options if options are not supplied the default content type is application/json
     */
    AuthPut(url: string, data: any, options: RequestOptions = undefined): Observable<Response> {

        const body = JSON.stringify(data);

        return this.http.put(url, body, this._setRequestOptions(options));
    }

    /**
     * @param options if options are not supplied the default content type is application/json
     */
    AuthDelete(url: string, options: RequestOptions = undefined): Observable<Response> {

        return this.http.delete(url, this._setRequestOptions(options));
    }

    /**
     * @param options if options are not supplied the default content type is application/json
     */
     AuthPost(url: string, data: any, options: RequestOptions = undefined): Observable<Response> {

        const body = JSON.stringify(data);

        return this.http.post(url, body, this._setRequestOptions(options));
    }



    private _setAuthHeaders(user: any): void {
        this.authHeaders = new Headers();
        this.authHeaders.append('Authorization', user.token_type + ' ' + user.access_token);
        if (this.authHeaders.get('Content-Type')) {
        } else {
            this.authHeaders.append('Content-Type', 'application/json');
        }
    }

    private _setRequestOptions(options: RequestOptions = undefined) {
        if (this.authenticationService.loggedIn) {
            this._setAuthHeaders(this.authenticationService.currentUser);
        }

        if ( options === undefined ) {
            return new RequestOptions({ headers: this.authHeaders });
        }

        const key = this.authHeaders.keys()[0];
        const value = this.authHeaders.values()[0][0];
        options.headers.append(key, value);

         if (this.sessionService.IsDualogAdmin) {
           if (this.sessionService.GetSelectedCompany() !== undefined) {
                options.headers.append('xdcid', this.sessionService.GetSelectedCompany().id.toString());
           }
        }
        return options;
    }


    private handleError (error: Response | any) {

        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}
