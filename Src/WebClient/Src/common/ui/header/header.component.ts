import { Component, OnInit } from '@angular/core';

import { ScreenService } from '../../../infrastructure/services/screen.service';
import { MainMenuService } from '../../../infrastructure/services/mainmenu.service';

@Component({
    selector: 'dua-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
    constructor(
        private screenService: ScreenService,
        private menuService: MainMenuService
    ) {}

    ngOnInit() {}
}
