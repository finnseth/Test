import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { MenuItem } from 'primeng/primeng';
import { MenuService } from '../../shore/services/menu.service';
import { Subscription } from 'rxjs/Rx';

@Component({
  templateUrl: './information.component.html'
})
export class InformationComponent implements OnInit, OnDestroy {

    private informationItems: MenuItem[] = [];
    private showContent: boolean;

    constructor(private menuService: MenuService, private route: ActivatedRoute) {
    }

    public ngOnInit(): void {
        this.informationItems = this.menuService.BuildMenu(this.route.routeConfig.children);
    }

    private onThisPage(url: string) {
        if (url === '/information') {
            this.showContent = true;
        } else {
            this.showContent = false;
        }
    }

   public ngOnDestroy(): void {
   }
}
