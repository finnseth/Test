import {
    AutoCompleteModule
} from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RoutesearchComponent } from './routesearch.component';

@NgModule({
    imports: [
        CommonModule,
        AutoCompleteModule
    ],

    declarations: [
        RoutesearchComponent
    ],

    providers: [
    ]
})
export class RoutesearchModule {}
