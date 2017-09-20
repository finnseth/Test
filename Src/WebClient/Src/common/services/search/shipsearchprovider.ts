import { Http, Jsonp, Response, URLSearchParams } from '@angular/http';
import { SearchResult, SearchResultElement } from './searchresult';

import { ApiService } from 'shore/services/api-base.service';
import { AuthenticationService } from 'shore/services/authentication.service';
import { ConfigurationReader } from 'infrastructure/services/configuration-reader.service';
import { SearchProvider } from './searchprovider';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { SessionService } from 'shore/services/session.service';
import { Ship } from 'common/domain/ship/interfaces';

@Injectable()
export class ShipSearchProvider extends ApiService implements SearchProvider {
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
            .Get<Ship[]>(`/vessels`)
            .map(response =>
                this.responseToSearchResult(query.toLowerCase(), response)
            );
    }

    private responseToSearchResult(
        query: string,
        response: Ship[]
    ): SearchResult {
        const result = new SearchResult();

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
