import { Component, OnInit, Renderer, ElementRef } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

import { MainMenuService } from './../../../services/mainmenu.service';
import { MainMenuItem } from './../../../services/mainmenu.service';

@Component({
    selector: 'dua-gridmenu',
    templateUrl: './gridmenu.component.html',
    styleUrls: ['./gridmenu.component.scss']
})
export class GridMenuComponent implements OnInit {

    menuItems: MainMenuItem[];

    constructor(
        private menuService: MainMenuService,
        private router: Router,
        private el: ElementRef,
        private renderer: Renderer) {
    }

    ngOnInit() {
        this.menuItems = this.menuService.GetMenuItemsByRoute(this.router.url);
        console.log(this.menuItems);
    }

    onClick($event, item: MainMenuItem) {
        if (item.route) {
            // force horizontal menus to close by sending a mouseleave event
            const newEvent = new MouseEvent('mouseleave', { bubbles: true });
            this.renderer.invokeElementMethod(
                this.el.nativeElement,
                'dispatchEvent',
                [newEvent]
            );
            this.router.navigate([item.route]);
        }
    }
}
