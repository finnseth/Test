import { Observable } from 'rxjs/Observable';
import { SearchResult } from './searchresult';

export interface ISearchProvider {
  search(query: string) : Observable<SearchResult>;
}
