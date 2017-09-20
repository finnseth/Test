import { URLSearchParams, Jsonp, Response } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ISearchProvider } from './searchprovider';
import { SearchResult, SearchResultElement } from './searchresult';

@Injectable()
export class CdnjsSearchProvider implements ISearchProvider {
  baseUrl: string = 'http://api.cdnjs.com/libraries?callback=JSONP_CALLBACK';

  constructor(private jsonp: Jsonp) { }

  public search(query: string): Observable<SearchResult> {
    //console.log('CdnjsSearchProvider.search [' + query + ']');

    var search = new URLSearchParams()
    search.set('search', query);

    return this.jsonp
      .get(this.baseUrl, { search })
      .map(response => this.responseToSearchResult(response));
  }

  private responseToSearchResult(response: Response) : SearchResult {
    var result = new SearchResult();
    result.category = "CDN";

    var responseArray = response.json().results;
    if (responseArray){
      //console.log(responseArray);
      responseArray.forEach(element => {
        result.elements.push(new SearchResultElement ({ name: element.name }));
      });
    }
    return result;
  }
}
