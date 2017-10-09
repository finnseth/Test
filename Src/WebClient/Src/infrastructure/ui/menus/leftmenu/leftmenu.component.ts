import {
    ActivatedRoute,
    NavigationEnd,
    PRIMARY_OUTLET,
    Params,
    Router
} from '@angular/router';
import { Component, OnInit } from '@angular/core';

import {
    MainMenuItem,
    MainMenuService
} from './../../../services/mainmenu.service';

@Component({
    selector: 'dua-leftmenu',
    templateUrl: './leftmenu.component.html',
    styleUrls: ['./leftmenu.component.scss']
})
export class LeftMenuComponent implements OnInit {

    items: MainMenuItem[];

    constructor(
        public menuService: MainMenuService,
        private router: Router,
        private activatedRoute: ActivatedRoute
    ) {}

    ngOnInit() {
        this.items = this.menuService.items;

        this.checkActiveRoute(this.router.url);

        this.router.events
            .subscribe((event) => {
            if (event instanceof NavigationEnd) {
                this.checkActiveRoute(this.router.url);
            }
        });
    }

    private checkActiveRoute(route: string): void {
        if (this.items !== undefined) {
            const item = this.findItemByRoute(this.items, route);
            if (item !== null) {
                this.menuService.SetTopItem(item);
                if (this.menuService.topItem.expanded) {
                    this.menuService.SetItemExpanded(item);
                }
            }
        }
    }

    openMenu(): void {
        const item = this.findItemByRoute(this.items, this.router.url);
        if (item !== null) {
            this.menuService.SetItemExpanded(item);
        }
    }

    private findItemByRoute(items: MainMenuItem[], url: string, topItem?: MainMenuItem): MainMenuItem {
        for (const item of items) {
            if (item.route === url) {
                return item;
            } else {
                if (this.checkSubMenu(item.submenu, url)) {
                    return item;
                }
            }
        }
        return null;
    }

    private checkSubMenu(items: MainMenuItem[], url: string): boolean {
        for (const item of items) {
            if (item.route === url) {
                return true;
            } else {
                if (item.submenu !== undefined) {
                    if (item.submenu !== null) {
                        if (this.checkSubMenu(item.submenu, url)) {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    handleClick(event, item: MainMenuItem) {
        if (item.route !== undefined && item.route !== null) {
            if (item === this.menuService.topItem) {
                item.expanded = !item.expanded;
            } else {
                this.menuService.SetTopItem(item);
                this.menuService.SetItemExpanded(item);
                this.router.navigate([item.route]);
            }
        }
    }
}
