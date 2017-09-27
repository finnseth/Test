import { Injectable } from '@angular/core';
import { Jsonp, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/merge';

import { SearchProvider } from './searchprovider';
import { SearchResult } from './searchresult';

@Injectable()
export class SearchService {
    private searchProviders: SearchProvider[] = [];
    public searchChanged: Observable<boolean> = new Subject<boolean>();

    constructor(private jsonp: Jsonp ) {
    }

    public addProviders( providers: SearchProvider[] ): void {
        const filtered = providers.filter( p =>
            !this.searchProviders.some( sp => sp.name === p.name )
         );

         this.searchProviders.push( ...filtered );
    }

    public search(query: Observable<string>): Observable<SearchResult> {
        const searchStream = query.debounceTime(400).distinctUntilChanged().share();
        searchStream.subscribe(change => {
            (<Subject<boolean>>this.searchChanged).next(true);
        });

        return searchStream.switchMap(queryString => queryString.length > 2 ?
                    Observable.merge(...this.searchProviders.map(provider =>
                        provider.search(queryString).catch(err => Observable.of(new SearchResult({error: err.toString()}))))) :
                    Observable.of(new SearchResult()));
    }
}
