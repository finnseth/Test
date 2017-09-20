import { URLSearchParams, Jsonp, Response, Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ApiService } from '../../dualog-common/services/api-base.service';
import { ConfigurationReader } from '../../dualog-common/services/configuration-reader.service';

import { ISearchProvider } from './searchprovider';
import { SearchResult, SearchResultElement } from './searchresult';
import { AuthenticationService } from '../../connection-suite-shore/services/authentication.service';
import { SessionService } from '../../connection-suite-shore/services/session.service';
import { Ship } from '../../connection-suite/components/ship/interfaces';

@Injectable()
export class UserSearchProvider extends ApiService implements ISearchProvider {

  constructor(
    http: Http,
    authenticationService: AuthenticationService,
    sessionService: SessionService,
    configurationReader: ConfigurationReader) {
    super(http, authenticationService, sessionService, configurationReader);
  }

  public search(query: string): Observable<SearchResult> {
    //console.log('Ship.search [' + query + ']');

    return super.Get<Ship[]>(`/users`)
      .map(response => this.responseToSearchResult(query.toLowerCase(), response));
  }

  private responseToSearchResult(query: string, response: Ship[]): SearchResult {
    var result = new SearchResult();

    if (response.length > 0) {
      response.forEach(ship => {
        if (ship.name.toLowerCase().indexOf(query) != -1) {
          result.elements.push(new SearchResultElement({ name: ship.name }));
        }
      });
      result.category = (result.elements.length > 1) ? "Ships" : "Ship";
    }
    return result;
  }

}
