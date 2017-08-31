import {
    ButtonModule,
    DropdownModule,
    GrowlModule,
    InputTextModule,
    PanelModule,
    SelectButtonModule,
    SpinnerModule,
    TabViewModule,
} from 'primeng/primeng';
import {ChartModule, DataTableModule, SharedModule} from 'primeng/primeng';

import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { DashboardEditWidgetComponent } from './dashboard-editwidget.component';
import { DashboardService } from './dashboard.service';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { dashboardRouting } from './dashboard.routing';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        dashboardRouting,
        DropdownModule,
        InputTextModule,
        SpinnerModule,
        PanelModule,
        SelectButtonModule,
        GrowlModule,
        ButtonModule,
        ChartModule,
        DataTableModule,
        TabViewModule
    ],

    declarations: [
        DashboardComponent,
        DashboardEditWidgetComponent
    ],

    providers: [
        DashboardService
    ]
})

export class DashboardModule { }
