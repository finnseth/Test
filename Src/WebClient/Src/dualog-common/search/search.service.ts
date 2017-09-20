import { Injectable, Injector } from '@angular/core';
import { Jsonp, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/merge';
import 'rxjs/add/operator/concat';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/forkJoin';

import { ISearchProvider } from './searchprovider';
import { SearchResult } from './searchresult';
import { CdnjsSearchProvider } from './cdnjssearchprovider';
import { WikipediaSearchProvider } from './wikipediasearchprovider';
import { ShipSearchProvider } from './shipsearchprovider';

@Injectable()
export class SearchService {
    private searchProviders: Array<ISearchProvider> = [];

    constructor(private jsonp: Jsonp, private injector: Injector) {
      this.searchProviders.push(new WikipediaSearchProvider(jsonp));
      this.searchProviders.push(new CdnjsSearchProvider(jsonp));
      this.searchProviders.push(injector.get(ShipSearchProvider));
    }

    public searchChanged:Observable<boolean> = new Subject<boolean>();

    public search(query: Observable<string>) : Observable<SearchResult> {
      const searchStream = query.debounceTime(400) .distinctUntilChanged().share();
      searchStream.subscribe( change => {
        (<Subject<boolean>>this.searchChanged).next(true);
	    });
      return searchStream.switchMap(queryString => queryString.length > 2 ? Observable.merge(...this.parallelSearch(queryString)) : Observable.of([]));
  }

  parallelSearch(query: string) {
    let observableProviders = [];
    this.searchProviders.forEach(provider => observableProviders.push(provider.search(query)));
    return observableProviders;
  }

}
