import { Injectable  } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';

export interface IConfiguration {
    authentication: {
        server: string;
        clientId: string;
        secret: string;
    };
    dualogApi: {
        server: string;
    };
}


@Injectable()
export class ConfigurationReader {

    config: IConfiguration = null;

    constructor (private http: Http) {}

    public GetConfiguration(): IConfiguration {

          return this.config;
    }

    public load(): Promise<void> {
                // Load the configuration
        const url = window.location.origin + '/configuration.json';
        return this.http.get( url ).map( (response: Response) => {

            this.config = <IConfiguration> response.json();
        } ).toPromise();

    };
}
