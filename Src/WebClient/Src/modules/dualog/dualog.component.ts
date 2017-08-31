import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { MenuItem } from 'primeng/primeng';
import { MenuService } from 'connection-suite-shore/services/menu.service';
import { Subscription } from 'rxjs/Rx';

@Component({
  templateUrl: './dualog.component.html'
})
export class DualogComponent implements OnInit, OnDestroy {

    private dualogItems: MenuItem[] = [];

    constructor(private menuService: MenuService, private route: ActivatedRoute) {
    }

    public ngOnInit(): void {
        this.dualogItems = this.menuService.BuildMenu(this.route.routeConfig.children);
    }

    public ngOnDestroy(): void {
    }
}
