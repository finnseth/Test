import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ScreenLargeDirective } from './directives/screen-large.directive';
import { ScreenBelowLargeDirective } from './directives/screen-below-large.directive';
import { DuaCheckboxModule } from './ui/dua-checkbox/dua-checkbox.module';
import { DuaLabelModule } from './ui/dua-label/dua-label.module';
import { ScreenService } from './services/screen.service';
import { SchemaFormBuilder } from './services/schema';
import { DuaVerticalBarModule } from './ui/dua-vertical-bar/dua-vertical-bar.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        DuaCheckboxModule,
        DuaVerticalBarModule
    ],

    providers: [
        ScreenService,
        SchemaFormBuilder
    ],

    declarations: [
        ScreenBelowLargeDirective,
        ScreenLargeDirective
    ],

    exports: [
        ScreenBelowLargeDirective,
        ScreenLargeDirective
    ]
})
export class InfrastructureModule {}
