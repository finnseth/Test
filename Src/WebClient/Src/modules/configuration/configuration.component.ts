import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { MenuItem } from 'primeng/primeng';
import { MenuService } from 'connection-suite-shore/services/menu.service';
import { SessionService } from 'connection-suite-shore/services/session.service';
import { Subscription } from 'rxjs/Rx';

@Component({
    templateUrl: './configuration.component.html',
    styleUrls: ['./configuration.component.scss']
})
export class ConfigurationComponent implements OnInit, OnDestroy {

    private isDualogAdmin = false;
    private isReady = false;
    private module: string;
    
    constructor(
        private menuService: MenuService,
        private sessionService: SessionService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    public ngOnInit(): void {
        this.isDualogAdmin = this.sessionService.IsDualogAdmin;

        this.checkIfReady();

        // Update the path when navigation around
        this.router.events.filter(event => event instanceof NavigationEnd).subscribe(event => {
            this.updatePathInfo();
        });
    }

    /**
     * Check if the site is ready for the current logged on user
     * 
     * If the logged on user is a Dualog Admin, he must have selected a company before
     * the configuration is visible
     * 
     * @private
     * @memberof ConfigurationComponent
     */
    private checkIfReady() {
        if (this.isDualogAdmin) {
            const selectedCompany = this.sessionService.GetSelectedCompany();
            if ( selectedCompany !== undefined ) {
                this.isReady = true;
            } else {
                this.updatePathInfo();
            }
        } else {
            this.isReady = true;
        }
    }

    private updatePathInfo() {
        const currentUrl: string[] = this.router.url.split('/');
        this.module = currentUrl[currentUrl.length - 1];
    }

    public ngOnDestroy(): void {
    }
}
