import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';


import { BreadcrumbComponent } from './breadcrumb.component';


@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        RouterModule
    ],

    declarations: [
        BreadcrumbComponent
    ],

    exports: [
        BreadcrumbComponent
    ]

})
export class BreadcrumbModule {}
