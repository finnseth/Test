import { Component, OnInit } from '@angular/core';
import { JsonSchema, SchemaFormBuilder } from 'dualog-common';

import { CommunicationService } from './communication.service';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'dua-communication',
  templateUrl: './communication.component.html',
  styleUrls: ['./communication.component.css']
})

export class CommunicationComponent implements OnInit {

    constructor( private communicationService: CommunicationService, private fb: SchemaFormBuilder ) {
    }

    ngOnInit() {
    }
}
