import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { Observable } from 'rxjs/Rx';

import * as assert from 'assert';

import { SelectItem } from 'primeng/primeng';

// Infrastructure
import { PatchGraphDocument } from './../../../../infrastructure/services/patchGraphDocument';
import { JsonSchema, SchemaFormBuilder } from './../../../../infrastructure/services/schema';

// Common
import { Ship } from './../../../../common/domain/ship/interfaces';

// Shore
import { CacheType, CardType, DualogController, FormType } from './../dualog.controller';
import { CurrentShipService } from './../../../services/currentship.service';
import { CompanyService } from './company.service';


@Component({
    selector: 'org-company',
    templateUrl: './company.component.html',
    styleUrls: ['./company.component.scss']
})
export class CompanyComponent extends DualogController implements OnInit {

    constructor(
        private companyService: CompanyService,
        private fb: SchemaFormBuilder,
        private current: CurrentShipService) {

        super(fb, current);
    }

    ngOnInit() {

        /*this.registerCardForm('companyform', FormType.SingleRow,
            () => this.companyService.getCompanySchema(),
            () => this.companyService.getCompany(),
            CacheType.All, CardType.Company,
            (companyid: number, json: PatchGraphDocument) => this.companyService.PatchCompany(companyid, json));

        this.init();*/
    }
}
