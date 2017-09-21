import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DuaVerticalBarModule } from '../dua-vertical-bar/dua-vertical-bar.module';
import { DuaStatusBarComponent } from './dua-status-bar.component';

@NgModule({
    imports: [DuaVerticalBarModule, CommonModule],

    declarations: [DuaStatusBarComponent],

    exports: [DuaStatusBarComponent]
})
export class DuaStatusBarModule {}
