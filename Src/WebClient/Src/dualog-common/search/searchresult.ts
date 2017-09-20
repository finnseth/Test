export class SearchResult {
  public category: string = "";
  public elements: SearchResultElement[] = [];

  public constructor(init?:Partial<SearchResult>) {
    Object.assign(this, init);
  }
}

export class SearchResultElement {
  public name: string = "";
  public route: string = "";

  public constructor(init?:Partial<SearchResultElement>) {
    Object.assign(this, init);
  }
}
