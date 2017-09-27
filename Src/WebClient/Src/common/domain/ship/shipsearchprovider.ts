import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http, Jsonp, Response, URLSearchParams } from '@angular/http';

import { ConfigurationReader } from 'infrastructure/services/configuration-reader.service';
import { SearchProvider } from '../../services/search/searchprovider';
import { SearchResult, SearchResultElement } from '../../services/search/searchresult';
import { SearchParameters } from '../../services/search/searchparameters';
import { Ship } from './interfaces';

import { ApiService } from './../../services/api-base.service';
import { AuthenticationService } from './../../services/authentication.service';
import { SessionService } from './../../services/session.service';

@Injectable()
export class ShipSearchProvider extends ApiService implements SearchProvider {

    public name = 'Ship';

    constructor(
        http: Http,
        authenticationService: AuthenticationService,
        sessionService: SessionService,
        configurationReader: ConfigurationReader
    ) {
        super(http, authenticationService, sessionService, configurationReader);
    }

    public search(query: string): Observable<SearchResult> {
        // console.log('Ship.search [' + query + ']');

        return super
            .Get<Ship[]>('/vessels', new SearchParameters(query).toURLSearchParams())
            .map(response =>
                this.responseToSearchResult(query.toLowerCase(), response)
            );
    }

    private responseToSearchResult(
        query: string,
        response: Ship[]
    ): SearchResult {
        const result = new SearchResult({provider: this.name});

        if (response.length > 0) {
            response.forEach(ship => {
                if (ship.name.toLowerCase().indexOf(query) !== -1) {
                    result.elements.push(
                        new SearchResultElement({ name: ship.name })
                    );
                }
            });
            result.category = result.elements.length > 1 ? 'Ships' : 'Ship';
        }
        return result;
    }
}
