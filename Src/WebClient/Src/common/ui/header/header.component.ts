import { Component, OnInit, Injector } from '@angular/core';

import { ScreenService } from '../../../infrastructure/services/screen.service';
import { MainMenuService } from '../../../infrastructure/services/mainmenu.service';
import { SearchService } from '../../../common/services/search/search.service';
import { UserSearchProvider } from '../../../common/domain/user/usersearchprovider';
import { ShipSearchProvider } from 'common/domain/ship/shipsearchprovider';
import { CompanySearchProvider } from 'common/domain/company/companysearchprovider';

@Component({
    selector: 'dua-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
    constructor(
            private screenService: ScreenService,
            private menuService: MainMenuService,
            private searchService: SearchService,
            private injector: Injector ) {

        // register the providers to be used for the global search box
        const providers = [
            injector.get(UserSearchProvider),
            injector.get(ShipSearchProvider),
            injector.get(CompanySearchProvider)
        ];

        searchService.addProviders( providers );
    }

    ngOnInit() {}
}
