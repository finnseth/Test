import { URLSearchParams } from '@angular/http';

import { DefaultSearchSettings } from './searchprovider';

export class SearchParameters {
    search: string;
    limit?: number;

    constructor(search: string, limit?: number) {
        this.search = search;
        this.limit = limit;
    }

    public toURLSearchParams(): URLSearchParams {
        const parameters = new URLSearchParams();

        parameters.set('search', this.search);
        if (this.limit === undefined) {
            this.limit = DefaultSearchSettings.SearchLimit;
        }
        parameters.set('count', this.limit.toString());
        return parameters;
    }
}
