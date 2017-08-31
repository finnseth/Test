import { Component, OnInit } from '@angular/core';
import { JsonSchema, SchemaFormBuilder } from 'dualog-common';

import { CommunicationService } from '../communication.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'dua-communication-method',
  templateUrl: './method.component.html',
  styleUrls: ['./method.component.css']
})
export class MethodComponent implements OnInit {

    constructor( private communicationService: CommunicationService, private fb: SchemaFormBuilder ) {
    }

    ngOnInit() {
    }
}
