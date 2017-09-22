import { URLSearchParams, Jsonp, Response } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { SearchProvider } from './searchprovider';
import { SearchResult, SearchResultElement } from './searchresult';

@Injectable()
export class WikipediaSearchProvider implements SearchProvider {

  constructor(private jsonp: Jsonp) { }

  public search(query: string): Observable<SearchResult> {
    // console.log('WikipediaSearchProvider.search [' + query + ']');
    const search = new URLSearchParams()
    search.set('action', 'opensearch');
    search.set('search', query);
    search.set('format', 'json');

    return this.jsonp
      .get('http://en.wikipedia.org/w/api.php?callback=JSONP_CALLBACK', { search })
      .map(response => this.responseToSearchResult(response));
  }

  private responseToSearchResult(response: Response) : SearchResult {
    const result = new SearchResult();
    result.category = 'Wikipedia';

    const responseArray = response.json()[1];
    if (responseArray){
      // console.log(responseArray);
      responseArray.forEach(element => {
        result.elements.push(new SearchResultElement ({ name: element }));
      });
    }
    return result;
  }

}
