import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subscription } from 'rxjs/Rx';

import { MainMenuService } from './../../../../infrastructure/services/mainmenu.service';

@Component({
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss']
})
export class OrganizationComponent implements OnInit, OnDestroy {

    constructor(private menuService: MainMenuService, private route: ActivatedRoute) {

    }

    public ngOnInit(): void {
    }


    public ngOnDestroy(): void {
    }
}
