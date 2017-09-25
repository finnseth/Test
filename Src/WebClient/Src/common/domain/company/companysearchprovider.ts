import { Injectable } from '@angular/core';
import { Http, Jsonp, Response, URLSearchParams } from '@angular/http';

import { Observable } from 'rxjs/Observable';

import { ConfigurationReader } from 'infrastructure/services/configuration-reader.service';

import { SearchProvider } from '../../services/search/searchprovider';
import { SearchParameters } from '../../services/search/searchparameters';
import {
    SearchResult,
    SearchResultElement
} from '../../services/search/searchresult';
import { Company } from './company';

import { ApiService } from './../../services/api-base.service';
import { AuthenticationService } from './../../services/authentication.service';
import { SessionService } from './../../services/session.service';

@Injectable()
export class CompanySearchProvider extends ApiService
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
        // console.log('Company.search [' + query + ']');

        return super
            .Get<Company[]>('/companies', new SearchParameters(query).toURLSearchParams())
            .map(response =>
                this.responseToSearchResult(response)
            );
    }

    private responseToSearchResult(
        response: Company[]
    ): SearchResult {
        const result = new SearchResult({provider: 'Company'});

        if (response.length > 0) {
            response.forEach(company => {
                result.elements.push(
                    new SearchResultElement({ name: company.name })
                );
            });
            result.category =
                result.elements.length > 1 ? 'Companies' : 'Company';
        }
        return result;
    }
}
