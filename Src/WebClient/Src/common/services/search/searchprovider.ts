import { Observable } from 'rxjs/Observable';
import { SearchResult } from './searchresult';

export interface SearchProvider {
    search(query: string): Observable<SearchResult>;
};

export class DefaultSearchSettings {
    public static readonly SearchLimit = 5;
}
