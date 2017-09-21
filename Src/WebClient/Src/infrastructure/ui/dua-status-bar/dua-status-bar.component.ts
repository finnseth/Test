import { Component, OnInit } from '@angular/core';
//import { environment } from '../../../environments/environment';
import { version } from '../../../environments/version';

@Component({
    selector: 'dua-status-bar',
    templateUrl: './dua-status-bar.component.html',
    styleUrls: ['./dua-status-bar.component.scss']
})
export class DuaStatusBarComponent implements OnInit {
    //isProduction = environment.production;
    productVersion = version;

    constructor() {}

    ngOnInit() {}
}
