import { Component, OnInit } from '@angular/core';

import { MenuItem } from 'primeng/primeng';
import { Observable } from 'rxjs/Rx';

@Component({
    templateUrl: './config.component.html',
    styleUrls: ['./config.component.css'],
})

export class ConfigComponent implements OnInit {

    private menuItems: MenuItem[];

    ngOnInit() {
        this.menuItems = [{
            label: 'Action',
            items: [
                {
                    label: 'Create new vessel',
                },
                {
                    label: 'Deactivate vessel',
                },
                {
                    label: 'Create new internet gateway',
                },

            ]
        },
        {
            label: 'Email',
        },
        {
            label: 'Internet'
        },
        {
            label: 'File Transfer'
        },
        {
            label: 'AntiVirus'
        }
        ]
    }
}
