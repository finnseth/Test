import { Component, OnInit, VERSION } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ClientEnvironment } from '../../../environments/version';

@Component({
    selector: 'dua-status-bar',
    templateUrl: './dua-status-bar.component.html',
    styleUrls: ['./dua-status-bar.component.scss']
})
export class DuaStatusBarComponent implements OnInit {
    isProduction = environment.production;
    productVersion = ClientEnvironment.version;
    productLocation = ClientEnvironment.location;
    angularVersion = VERSION.full;

    constructor() {}

    ngOnInit() {}
}
