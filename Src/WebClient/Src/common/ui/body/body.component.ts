import { Component, OnInit } from '@angular/core';

import { MainMenuService } from './../../../infrastructure/services/mainmenu.service';
import { ScreenService } from './../../../infrastructure/services/screen.service';


@Component({
    selector: 'dua-body',
    templateUrl: './body.component.html',
    styleUrls: ['./body.component.scss']
})
export class BodyComponent implements OnInit {
    constructor(private screenService: ScreenService,
        private menuService: MainMenuService) {
    }

    ngOnInit() {}
}
