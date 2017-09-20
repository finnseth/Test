import { Injectable, Injector } from '@angular/core';
import { Jsonp, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/merge';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/forkJoin';

import { SearchProvider } from './searchprovider';
import { SearchResult } from './searchresult';
import { CdnjsSearchProvider } from './cdnjssearchprovider';
import { WikipediaSearchProvider } from './wikipediasearchprovider';
import { ShipSearchProvider } from './shipsearchprovider';

@Injectable()
export class SearchService {
    private searchProviders: Array<SearchProvider> = [];
    public searchChanged: Observable<boolean> = new Subject<boolean>();

    constructor(private jsonp: Jsonp, private injector: Injector) {
        this.searchProviders.push(new WikipediaSearchProvider(jsonp));
        this.searchProviders.push(new CdnjsSearchProvider(jsonp));
        this.searchProviders.push(injector.get(ShipSearchProvider));
    }


    public search(query: Observable<string>): Observable<SearchResult> {
        const searchStream = query.debounceTime(400).distinctUntilChanged().share();
        searchStream.subscribe(change => {
            (<Subject<boolean>>this.searchChanged).next(true);
        });

        return searchStream.switchMap(queryString => queryString.length > 2 ?
                    Observable.merge(...this.searchProviders.map(provider => provider.search(queryString))) :
                    Observable.of(new SearchResult()));
    }

}
