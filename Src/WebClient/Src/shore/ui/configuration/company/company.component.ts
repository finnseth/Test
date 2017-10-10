import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { Observable } from 'rxjs/Rx';

import * as assert from 'assert';

import { SelectItem } from 'primeng/primeng';

// Infrastructure
import { PatchGraphDocument } from './../../../../infrastructure/services/patchGraphDocument';
import { JsonSchema, SchemaFormBuilder } from './../../../../infrastructure/services/schema';
import { MainMenuService } from './../../../../infrastructure/services/mainmenu.service';

// Common
import { Ship } from './../../../../common/domain/ship/interfaces';
import { SessionService } from './../../../../common/services/session.service';

// Shore
import { CacheType, CardType, DualogController, FormType } from './../dualog.controller';
import { CurrentShipService } from './../../../services/currentship.service';
import { CompanyService } from './../../../services/company.service';


@Component({
    selector: 'org-company',
    templateUrl: './company.component.html',
    styleUrls: ['./company.component.scss']
})
export class CompanyComponent extends DualogController implements OnInit {

    isDualogAdmin = false;

    constructor(
        private companyService: CompanyService,
        private fb: SchemaFormBuilder,
        private current: CurrentShipService,
        private sessionService: SessionService,
        private menuService: MainMenuService,
        private router: Router
    ) {

        super(fb, current, menuService, router);
        this.isDualogAdmin = this.sessionService.IsDualogAdmin;
    }

    ngOnInit() {

        this.registerCardForm('companyform', FormType.SingleRow,
            () => this.companyService.getCompanySchema(),
            () => this.companyService.getCompany(),
            CacheType.All, CardType.Company,
            (companyid: number, json: PatchGraphDocument) => this.companyService.PatchCompany(companyid, json));

        this.init();
    }
}
