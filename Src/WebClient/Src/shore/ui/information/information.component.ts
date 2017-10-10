import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subscription } from 'rxjs/Rx';

import { MenuItem } from 'primeng/primeng';


@Component({
  templateUrl: './information.component.html'
})
export class InformationComponent implements OnInit, OnDestroy {

    private informationItems: MenuItem[] = [];
    private showContent: boolean;

    constructor(private route: ActivatedRoute) {
    }

    public ngOnInit(): void {
        this.informationItems = [];
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
