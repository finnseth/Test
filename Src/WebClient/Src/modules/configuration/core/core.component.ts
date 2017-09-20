import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { MenuService } from 'shore/services/menu.service';
import { Subscription } from 'rxjs/Rx';

@Component({
  templateUrl: './core.component.html'
})
export class CoreComponent implements OnInit, OnDestroy {

    gridItems: IGridItem[] = [];
    selectedGridItem: IGridItem;

    constructor(private menuService: MenuService, private route: ActivatedRoute) {
    }

    public ngOnInit(): void {
/*
            { label: 'Core', icon: 'fa-circle-o-notch', items: [
                { label: 'Company', icon: 'fa-building' },
                { label: 'Ship', icon: 'fa-ship' },
                { label: 'User Group', icon: 'fa-users' },
                { label: 'User', icon: 'fa-user', routerLink: 'users' },
            ]},
        ];
*/
        this.buildGrid();
    }

    private buildGrid(): void {
        // Loop throug each configured routed to build up the menu
        for (const r of this.route.routeConfig.children) {
            if (r.data !== undefined) {
                if (r.data.permissions !== undefined) {
                    if (this.menuService.GetMenuAccess(r.data.permissions)) {
                        const item = {
                            label: r.data.label,
                            icon: r.data.icon,
                            routerLink: r.path
                        };
                        this.gridItems.push(item);
                    };
                }
            }
        }
    }

    public ngOnDestroy(): void {
    }
}

interface IGridItem {
    label: string;
    icon: string;
}
