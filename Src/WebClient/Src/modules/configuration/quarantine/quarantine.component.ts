import { NumberFormatStyle } from '@angular/common/src/pipes/intl';
import { number } from 'ng2-validation/dist/number';
import * as assert from 'assert';

import { Component, OnInit } from '@angular/core';
import { JsonSchema, SchemaFormBuilder } from '../../../dualog-common';
import { QuarantineCompanyConfig, QuarantineService, QuarantineVesselConfig } from './quarantine.service';

import { CopyField } from './../../../connection-suite-shore/components/copy/copy.component';
import { CurrentShipService } from '../../../connection-suite-shore/services/currentship.service';
import { DualogController, cachetype, cardtype } from '../dualog.controller';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import { PatchGraphDocument } from '../../../dualog-common/services/patchGraphDocument';
import { SelectItem } from 'primeng/primeng';
import { Ship } from '../../../connection-suite/components/ship/interfaces'; // todo

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

        this.registerCardForm("companyform", () => this.quarantineService.getVesselConfigSchema(),
            () => this.quarantineService.getCompanyConfigList(),
            cachetype.All, cardtype.Company,
            (quarantineid: number, json: PatchGraphDocument) => this.quarantineService.PatchCompanyQuarantine(quarantineid, json));

        this.registerCardForm("compareform", () => this.quarantineService.getVesselConfigSchema(),
            (shipid: number) => this.quarantineService.getVesselConfig(shipid),
            cachetype.No, cardtype.Compare);

        this.registerCardForm("shipform", () => this.quarantineService.getVesselConfigSchema(),
            (shipid: number) => this.quarantineService.getVesselConfig(shipid),
            cachetype.No, cardtype.Ship,
            (quarantineid: number, json: PatchGraphDocument) => this.quarantineService.PatchVesselQuarantine(quarantineid, json));

        this.registerCompare("shipform", "compareform", [
            {key: 'useThisLevel',prettyname: 'Override fleet configuration'},
            {key: 'onHoldStationaryUser',prettyname: 'Stationary users'},
            {key: 'onHoldCrew',prettyname: 'Crew (personal) users'},
            {key: 'notificationOnHoldOriginal',prettyname: 'Message originator'},
            {key: 'notificationOnHoldPostmaster',prettyname: 'Ship administrator'},
            {key: 'notificationOnHoldRecipient',prettyname: 'Message recipient(s)'},
            {key: 'notificationOnHoldAdmins',prettyname: 'Shore administrator(s)n'},
            {key: 'maxBodyLength',prettyname: 'Max body length'},
            {key: 'onHoldDuration',prettyname: 'Hold for duration'},
            {key: 'notificationSender',prettyname: 'Custom notification sender'}
        ])

        this.init();

    }


}
