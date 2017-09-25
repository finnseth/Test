import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http, Jsonp, Response, URLSearchParams } from '@angular/http';

import { ConfigurationReader } from 'infrastructure/services/configuration-reader.service';
import { SearchProvider } from '../../services/search/searchprovider';
import { SearchParameters } from '../../services/search/searchparameters';
import {
    SearchResult,
    SearchResultElement
} from '../../services/search/searchresult';
import { User } from './user';

import { ApiService } from './../../services/api-base.service';
import { AuthenticationService } from './../../services/authentication.service';
import { SessionService } from './../../services/session.service';

@Injectable()
export class UserSearchProvider extends ApiService
    implements SearchProvider {
    constructor(
        http: Http,
        authenticationService: AuthenticationService,
        sessionService: SessionService,
        configurationReader: ConfigurationReader
    ) {
        super(http, authenticationService, sessionService, configurationReader);
    }

    public search(query: string): Observable<SearchResult> {
        // console.log('User.search [' + query + ']');

        return super
            .Get<any>('/organization/shipping/user', new SearchParameters(query).toURLSearchParams())
            .map(response =>
                this.responseToSearchResult(response)
            );
    }

    private responseToSearchResult(response: any): SearchResult {

        const result = new SearchResult({provider: 'User'});

        if (response.value.length > 0) {
            response.value.forEach(user => {
                result.elements.push(
                    new SearchResultElement({ name: user.name })
                );
            });
            result.category =
                result.elements.length > 1 ? 'Users' : 'User';
        }
        return result;
    }
}
