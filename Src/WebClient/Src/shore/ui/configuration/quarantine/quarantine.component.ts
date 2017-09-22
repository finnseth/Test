import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { number } from 'ng2-validation/dist/number';

import * as assert from 'assert';

import { Observable } from 'rxjs/Rx';

import { SelectItem } from 'primeng/primeng';

import { JsonSchema, SchemaFormBuilder } from './../../../../infrastructure/services/schema';
import { PatchGraphDocument } from './../../../../infrastructure/services/patchGraphDocument';

import { ICopyField } from './../../../../common/ui/copy/copy.component';
import { Ship } from './../../../../common/domain/ship/interfaces';

import { CacheType, CardType, DualogController, FormType } from '../dualog.controller';
import { QuarantineCompanyConfig, QuarantineService, QuarantineVesselConfig } from './quarantine.service';
import { CurrentShipService } from './../../../services/currentship.service';

@Component({
    selector: 'app-quarantine',
    templateUrl: './quarantine.component.html',
    styleUrls: ['./quarantine.component.scss']
})
export class QuarantineComponent extends DualogController implements OnInit {

    constructor(
        private quarantineService: QuarantineService,
        private fb: SchemaFormBuilder,
        private current: CurrentShipService) {

        super(fb, current);
    }

    ngOnInit() {

        this.registerCardForm('companyform', FormType.SingleRow,
            () => this.quarantineService.getVesselConfigSchema(),
            () => this.quarantineService.getCompanyConfigList(),
            CacheType.All, CardType.Company,
            (quarantineid: number, json: PatchGraphDocument) => this.quarantineService.PatchCompanyQuarantine(quarantineid, json));

        this.registerCardForm('compareform', FormType.SingleRow,
            () => this.quarantineService.getVesselConfigSchema(),
            (shipid: number) => this.quarantineService.getVesselConfig(shipid),
            CacheType.No, CardType.Compare);

        this.registerCardForm('shipform', FormType.SingleRow, 
            () => this.quarantineService.getVesselConfigSchema(),
            (shipid: number) => this.quarantineService.getVesselConfig(shipid),
            CacheType.No, CardType.Ship,
            (quarantineid: number, json: PatchGraphDocument) => this.quarantineService.PatchVesselQuarantine(quarantineid, json));

        this.registerCopy('shipform', 'compareform', [
            { key: 'useThisLevel', prettyname: 'Override fleet configuration'},
            {key: 'onHoldStationaryUser', prettyname: 'Stationary users'},
            {key: 'onHoldCrew', prettyname: 'Crew (personal) users'},
            {key: 'notificationOnHoldOriginal', prettyname: 'Message originator'},
            {key: 'notificationOnHoldPostmaster', prettyname: 'Ship administrator'},
            {key: 'notificationOnHoldRecipient', prettyname: 'Message recipient(s)'},
            {key: 'notificationOnHoldAdmins', prettyname: 'Shore administrator(s)n'},
            {key: 'maxBodyLength', prettyname: 'Max body length'},
            {key: 'onHoldDuration', prettyname: 'Hold for duration'},
            {key: 'notificationSender', prettyname: 'Custom notification sender'}
        ])

        this.init();
    }
}
