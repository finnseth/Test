export class SearchResult {
  public category = '';
  public elements: SearchResultElement[] = [];

  public constructor(init?: Partial<SearchResult>) {
    Object.assign(this, init);
  }
}

export class SearchResultElement {
  public name = '';
  public route = '';

  public constructor(init?: Partial<SearchResultElement>) {
    Object.assign(this, init);
  }
}
