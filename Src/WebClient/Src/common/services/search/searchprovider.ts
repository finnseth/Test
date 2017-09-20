import { Observable } from 'rxjs/Observable';
import { SearchResult } from './searchresult';

export interface SearchProvider {
  search(query: string): Observable<SearchResult>;
}
