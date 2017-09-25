import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subscription } from 'rxjs/Rx';

import { MenuItem } from 'primeng/primeng';

import { MenuService } from './../../services/menu.service';
import { SessionService } from './../../../common/services/session.service';


@Component({
    templateUrl: './configuration.component.html',
    styleUrls: ['./configuration.component.scss']
})
export class ConfigurationComponent implements OnInit, OnDestroy {

    private isDualogAdmin = false;
    private isReady = false;
    private module: string;
    private isMenuEnabled = false;

    constructor(
        private menuService: MenuService,
        private sessionService: SessionService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    public ngOnInit(): void {
        this.isDualogAdmin = this.sessionService.IsDualogAdmin;

        this.isMenuEnabled = (this.router.url === '/configuration') ? true : false;

        this.checkIfReady();

        // Update the path when navigation around
        this.router.events.filter(event => event instanceof NavigationEnd).subscribe(event => {
            this.updatePathInfo();
            this.enableGridMenu(event);
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

    private enableGridMenu(event) {
        this.isMenuEnabled = (event.url === '/configuration') ? true : false;
    }

    public ngOnDestroy(): void {
    }
}
