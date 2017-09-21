import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Rx';

import { JsonSchema, SchemaFormBuilder } from './../../../../../infrastructure/services/schema';

import { CommunicationService } from '../communication.service';

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
